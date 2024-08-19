using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EVOGAMI.Custom.Serializable;
using EVOGAMI.Origami;
using EVOGAMI.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace EVOGAMI.Core
{
    public class UiManager : MonoBehaviour
    {
        /// <summary>
        ///     Singleton instance of the UiManager
        /// </summary>
        public static UiManager Instance { get; private set; }

        [Header("UI Elements")]
        // The score string
        [SerializeField] [Tooltip("The score string")]
        private TextMeshProUGUI scoreString;
        // The mappings between forms and icons
        [SerializeField] [Tooltip("The mappings between forms and icons")]
        public List<OriObjMapping> formIconMappings;
        [SerializeField] [Tooltip("A black screen for fading, covering the entire screen")]
        private Image blackScreen;
        
        [Header("UI Sections")]
        // The heads-up display
        [SerializeField] [Tooltip("The heads-up display")]
        public GameObject headsUpDisplay;
        // The UI for when the player finishes the level
        [SerializeField] [Tooltip("The UI for when the player finishes the level")]
        public GameObject finishLevelUi;
        // The UI for the game gained
        [FormerlySerializedAs("gameGainedUi")] [SerializeField] [Tooltip("The UI for the game gained")]
        public GameObject formGainedUi;
        
        private Dictionary<OrigamiContainer.OrigamiForm, GameObject> _origamiMapping;

        #region Fading

        private IEnumerator FadeToBlackCoroutine(float wait, float duration)
        {
            yield return new WaitForSeconds(wait);

            // Enable the black screen
            blackScreen.gameObject.SetActive(true);

            for (float i = 0; i <= 1; i += Time.deltaTime / duration)
            {
                blackScreen.color = new Color(0, 0, 0, i);
                yield return null;
            }

            blackScreen.color = new Color(0, 0, 0, 1); // Ensure the black screen is fully black
        }

        private IEnumerator FadeFromBlackCoroutine(float wait, float duration)
        {
            yield return new WaitForSeconds(wait);

            for (float i = 1; i >= 0; i -= Time.deltaTime / duration)
            {
                blackScreen.color = new Color(0, 0, 0, i);
                yield return null;
            }
            blackScreen.gameObject.SetActive(false);
        }

        public Task FadeToBlack(float wait, float duration)
        {
            return FadeToBlackCoroutine(wait, duration).AsTask(this);
        }

        public Task FadeFromBlack(float wait, float duration)
        {
            return FadeFromBlackCoroutine(wait, duration).AsTask(this);
        }

        #endregion

        #region Unity Functions

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }

        private void Start()
        {
            // Initialize dictionary
            _origamiMapping = new Dictionary<OrigamiContainer.OrigamiForm, GameObject>();
            foreach (var mapping in formIconMappings)
            {
                _origamiMapping.Add(mapping.form, mapping.gameObject);
                mapping.gameObject.SetActive(PlayerManager.Instance.GainedForms[mapping.form]);
            }
            
            scoreString.text = "0 / " + GameManager.Instance.origamiSettings.totalCranes;
            
            // Subscribe to events
            PlayerManager.Instance.OnGainForm += OnGainForm;
            PlayerManager.Instance.OnCraneCollected += OnCraneCollected;
        }

        #endregion
        
        #region Callbacks
        
        /// <summary>
        ///     Callback for when the player gains a new form
        /// </summary>
        /// <param name="form">The form that the player gained</param>
        private void OnGainForm(OrigamiContainer.OrigamiForm form)
        {
            if (!_origamiMapping.ContainsKey(form)) return; // Should not happen

            _origamiMapping[form].SetActive(true);
            
            formGainedUi.SetActive(true);
            StartCoroutine(WaitCloseFormGained());
        }
        
        private IEnumerator WaitCloseFormGained()
        {
            yield return new WaitForSeconds(3);
            formGainedUi.SetActive(false);
        }
        
        /// <summary>
        ///     Callback for when the player collects a crane
        /// </summary>
        private void OnCraneCollected()
        {
            if (PlayerManager.Instance.CranesCollected > GameManager.Instance.origamiSettings.totalCranes){
                GameManager.Instance.origamiSettings.totalCranes = PlayerManager.Instance.CranesCollected;
                scoreString.color = new Color(1f, 0.84f, 0f);


            }


            scoreString.text = PlayerManager.Instance.CranesCollected + " / " + GameManager.Instance.origamiSettings.totalCranes;
        }
        
        #endregion
    }
}