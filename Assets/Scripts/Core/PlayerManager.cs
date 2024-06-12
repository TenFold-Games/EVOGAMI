using System.Collections.Generic;
using EVOGAMI.Custom.Scriptable;
using EVOGAMI.Origami;
using UnityEngine;
using UnityEngine.UIElements;

namespace EVOGAMI.Core
{
    public class PlayerManager : MonoBehaviour
    {
        // Singleton
        public static PlayerManager Instance { get; private set; }

        // Player
        public GameObject Player { get; private set; }
        public Rigidbody PlayerRb { get; private set; }
        public OrigamiContainer PlayerOrigami { get; private set; }
        
        // Settings
        [SerializeField] private OrigamiSettings origamiSettings;
        [SerializeField] private OrigamiUnlockSettings unlockSettings;

        public void Awake()
        {
            // Singleton
            if (Instance == null) Instance = this;
            else Destroy(this);

            // Keep between scenes
            // DontDestroyOnLoad(this);

            // Find player
            Player = GameObject.FindGameObjectWithTag("Player");
            PlayerRb = Player.GetComponent<Rigidbody>();

            // Set lives
            _lives = maxLives;

            // Set gained forms
            GainedForms = new Dictionary<OrigamiContainer.OrigamiForm, bool>
            {
                { OrigamiContainer.OrigamiForm.Human, false },
                { OrigamiContainer.OrigamiForm.Frog, false },
                { OrigamiContainer.OrigamiForm.Plane, false },
                { OrigamiContainer.OrigamiForm.Bug, false },
                { OrigamiContainer.OrigamiForm.None, true }
            };
            foreach (var (form, unlocked) in unlockSettings.formUnlockStates)
                if (GainedForms.ContainsKey(form))
                    GainedForms[form] = unlocked;
            GainedForms[origamiSettings.initialForm] = true;
            
            // Set scores
            _score = 0;
            
            // Set player origami
            PlayerOrigami = Player.GetComponent<OrigamiContainer>();
        }

        #region Spawning

        public void RespawnPlayer(Transform t)
        {
            // Reset player velocity
            PlayerRb.velocity = Vector3.zero;
            // Reset player position and rotation
            Player.transform.position = t.position;
            Player.transform.rotation = t.rotation;
        }

        #endregion

        #region Life

        [SerializeField] private int maxLives = 3;
        public int MaxLives => maxLives; // Read-only

        private int _lives;

        public delegate void LoseLifeCallback(int lives);

        public event LoseLifeCallback OnLoseLife = delegate { };

        public void DecreaseLife()
        {
            if (_lives > 0)
            {
                _lives--;
                OnLoseLife(_lives);
                
                if (_lives == 0)
                    GameManager.Instance.GameOver();
            }
        }

        #endregion

        #region Gaining Forms

        public Dictionary<OrigamiContainer.OrigamiForm, bool> GainedForms { get; private set; }

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

        private int _score;

        public delegate void GainScoresCallback(int scores);

        public event GainScoresCallback OnChangeScore = delegate { };

        /// <summary>
        ///     Increase the player's score
        /// </summary>
        public void IncreaseScores()
        {
            _score++;
            OnChangeScore(_score);
        }

        #endregion


    }
}