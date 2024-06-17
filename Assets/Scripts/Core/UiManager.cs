using System.Collections.Generic;
using EVOGAMI.Custom.Serializable;
using EVOGAMI.Origami;
using TMPro;
using UnityEngine;
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
        
        [Header("UI Sections")]
        // The heads-up display
        [SerializeField] [Tooltip("The heads-up display")]
        public GameObject headsUpDisplay;
        // The UI for when the player finishes the level
        [SerializeField] [Tooltip("The UI for when the player finishes the level")]
        public GameObject finishLevelUi;
        
        private Dictionary<OrigamiContainer.OrigamiForm, GameObject> _origamiMapping;

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
        }
        
        /// <summary>
        ///     Callback for when the player collects a crane
        /// </summary>
        private void OnCraneCollected()
        {
            scoreString.text = PlayerManager.Instance.CranesCollected + " / " + GameManager.Instance.origamiSettings.totalCranes;
        }
        
        #endregion
    }
}