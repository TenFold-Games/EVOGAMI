using System.Collections.Generic;
using Custom.Serializable;
using EVOGAMI.Origami;
using TMPro;
using UnityEngine;

namespace EVOGAMI.Core
{
    public class UiManager : MonoBehaviour
    {
        // Singleton
        public static UiManager Instance { get; private set; }
        
        [Header("Timer")]
        [SerializeField] private float timer;
        [SerializeField] private TextMeshProUGUI countdownString;

        [Header("Score")]
        [SerializeField] private TextMeshProUGUI scoreString;
        [SerializeField] private GameObject[] lifeIcons;

        [Header("Form Icons")]
        public List<OriObjMapping> formIconMappings;
        private Dictionary<OrigamiContainer.OrigamiForm, GameObject> _origamiMapping;

        // [SerializeField] private float userScore;
        // [SerializeField] private TextMeshProUGUI scoreText;
        // [SerializeField] private GameObject gameOverScreen;
        // [SerializeField] private bool closed;
        
        private void Awake()
        {
            // Singleton
            if (Instance == null) Instance = this;
            else Destroy(this);
        }

        private void Start()
        {
            PlayerManager.Instance.OnLoseLife += OnLoseLife;
            PlayerManager.Instance.OnGainForm += OnGainForm;
            PlayerManager.Instance.OnChangeScore += OnChangeScore;

            // Initialize dictionary
            _origamiMapping = new Dictionary<OrigamiContainer.OrigamiForm, GameObject>();
            foreach (var mapping in formIconMappings)
            {
                _origamiMapping.Add(mapping.form, mapping.gameObject);
                mapping.gameObject.SetActive(PlayerManager.Instance.GainedForms[mapping.form]);
            }
            
            timer = 0;
        }

        private void Update()
        {
            if (timer >= 0)
            {
                timer += Time.deltaTime;
                countdownString.text = "timer:" + (int)timer;
            }
        }

        /// <summary>
        ///     Callback for when the player loses a life
        /// </summary>
        /// <param name="lives"></param>
        private void OnLoseLife(int lives)
        {
            if (lives < 0 || lives > PlayerManager.Instance.MaxLives)
                return;

            lifeIcons[lives].SetActive(false);
        }

        /// <summary>
        ///     Callback for when the player gains a form
        /// </summary>
        /// <param name="form">The form that the player gained</param>
        private void OnGainForm(OrigamiContainer.OrigamiForm form)
        {
            if (!_origamiMapping.ContainsKey(form))
                return;

            _origamiMapping[form].SetActive(true);
        }
        
        /// <summary>
        ///     Callback for when the player score changes
        /// </summary>
        /// <param name="scores">The score that the player currently has</param>
        private void OnChangeScore(int scores)
        {
            scoreString.text = scores.ToString();
        }
    }
}