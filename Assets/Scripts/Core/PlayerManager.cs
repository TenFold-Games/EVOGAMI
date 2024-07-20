using System.Collections.Generic;
using EVOGAMI.Origami;
using UnityEngine;

namespace EVOGAMI.Core
{
    public class PlayerManager : MonoBehaviour
    {
        /// <summary>
        ///     Singleton instance of the PlayerManager
        /// </summary>
        public static PlayerManager Instance { get; private set; }
        
        // Managers
        private GameManager _gameManager;

        // Player
        public GameObject Player { get; private set; }
        public Rigidbody PlayerRb { get; private set; }
        public OrigamiContainer PlayerOrigami { get; private set; }

        // Player forms
        public Dictionary<OrigamiContainer.OrigamiForm, bool> GainedForms { get; private set; }

        #region Unity Functions

        public void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);

            // Find player
            Player = GameObject.FindGameObjectWithTag("Player");
            Debug.Assert(Player != null, "Player not found"); // Should not happen
            // Get player components
            PlayerRb = Player.GetComponent<Rigidbody>();
            PlayerOrigami = Player.GetComponent<OrigamiContainer>();
            Debug.Assert(PlayerRb != null, "Player Rigidbody not found"); // Should not happen
            Debug.Assert(PlayerOrigami != null, "Player OrigamiContainer not found"); // Should not happen

            // Set gained forms
            GainedForms = new Dictionary<OrigamiContainer.OrigamiForm, bool>
            {
                { OrigamiContainer.OrigamiForm.NinjaStar, false },
                { OrigamiContainer.OrigamiForm.Frog, false },
                { OrigamiContainer.OrigamiForm.Crab, false },
                { OrigamiContainer.OrigamiForm.None, true }
            };

            // Set cranes collected
            CranesCollected = 0;

            _gameManager = GameManager.Instance;
            
            // Load form unlock states
            foreach (var (form, unlocked) in _gameManager.origamiSettings.formUnlockStates)
                GainedForms[form] = unlocked;
            if (!GainedForms[_gameManager.origamiSettings.initialForm])
                GainForm(_gameManager.origamiSettings.initialForm);
            
            // Reset player rotation on form change
            PlayerOrigami.OnFormChange += (_, _) =>
                Player.transform.rotation = Quaternion.Euler(0, Player.transform.rotation.eulerAngles.y, 0);
        }
        
        private void FixedUpdate()
        {
            // If flipped, flip back
            if (Player.transform.up.y < 0)
                Player.transform.rotation = Quaternion.Euler(0, Player.transform.rotation.eulerAngles.y, 0);
        }
        
        #endregion

        #region Spawning

        public void RespawnAtLastCheckpoint()
        {
            // Get last checkpoint
            var t = _gameManager.CurrentCheckpoint.transform;
            if (t == null) return; // Should not happen
            
            RespawnPlayer(t);
        }

        private void RespawnPlayer(Transform t)
        {
            // Reset player velocity
            PlayerRb.velocity = Vector3.zero;
            // Reset player position and rotation
            Player.transform.position = t.position;
            Player.transform.rotation = t.rotation;
        }

        #endregion

        #region Gaining Forms

        public delegate void GainFormCallback(OrigamiContainer.OrigamiForm form);

        public event GainFormCallback OnGainForm = delegate { };

        /// <summary>
        ///     Gain a new form
        /// </summary>
        /// <param name="form">The form to gain</param>
        public void GainForm(OrigamiContainer.OrigamiForm form)
        {
            // Form already gained, or form not recognized
            if (GainedForms[form] || !GainedForms.ContainsKey(form))
                return;

            GainedForms[form] = true;
            OnGainForm(form);
        }

        #endregion

        #region Scores

        public int CranesCollected { get; private set; }

        public delegate void CraneCollectedCallback();

        public event CraneCollectedCallback OnCraneCollected = delegate { };

        /// <summary>
        ///     Increase the player's score
        /// </summary>
        public void CraneCollected()
        {
            CranesCollected++;
            OnCraneCollected();

            // if (CranesCollected == _gameManager.origamiSettings.totalCranes)
            //     GameManager.Instance.GameComplete();
        }

        #endregion
    }
}