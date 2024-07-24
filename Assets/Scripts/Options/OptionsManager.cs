using System.Collections.Generic;
using EVOGAMI.Core;
using EVOGAMI.Custom.Scriptable;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EVOGAMI.Options
{
    public class OptionsManager : CallbackMonoBehaviour
    {
        public static OptionsManager Instance;

        [Header("Preferences")]
        // The scriptable object containing the user preferences
        [SerializeField] [Tooltip("The scriptable object containing the user preferences")]
        public UserPreferences preferences;

        [Header("FMOD Audio")]
        // The master vca for the audio
        [BankRef] [SerializeField] [Tooltip("The master vca for the audio")]
        private string masterVca = "vca:/Master";
        // The BGM vca for the audio
        [BankRef] [SerializeField] [Tooltip("The BGM vca for the audio")]
        private string bgmVca = "vca:/BGM";

        // The sfx vca for the audio
        [BankRef] [Tooltip("The SFX vca for the audio")]
        private readonly string sfxVca = "vca:/SFX";

        public readonly HashSet<ISensitivityController> SensitivityControllers = new();
        public readonly HashSet<IXInverter> XInverters = new();
        public readonly HashSet<IYInverter> YInverters = new();

        private VCA _bgmVca;
        private VCA _masterVca;
        private VCA _sfxVca;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "Alpha Build Scene") return;

            ApplyControlsSettings();
        }

        #region Unity Functions

        public void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                // DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            // Register the scene loaded callback
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Apply settings
            ApplyGraphicsSettings();
            ApplyAudioSettings();
            ApplyControlsSettings();
        }

        protected override void Start()
        {
            // Get the VCAs
            _masterVca = RuntimeManager.GetVCA(masterVca);
            _bgmVca = RuntimeManager.GetVCA(bgmVca);
            _sfxVca = RuntimeManager.GetVCA(sfxVca);
        }

        #endregion

        #region Options

        #region Graphics

        public void SetQualityLevel(int quality)
        {
            preferences.qualityLevel = quality;
        }

        public void SetFullscreen(FullScreenMode fullscreen)
        {
            preferences.fullscreenMode = fullscreen;
            Screen.fullScreenMode = fullscreen;
        }

        public void SetResolution(int width, int height)
        {
            preferences.resolution = new Vector2Int(width, height);
            Screen.SetResolution(width, height, preferences.fullscreenMode);
        }

        public void SetResolution(Vector2Int res)
        {
            preferences.resolution = res;
            Screen.SetResolution(res.x, res.y, preferences.fullscreenMode);
        }

        public void SetVSync(bool isVSync)
        {
            preferences.vSync = isVSync;

            QualitySettings.vSyncCount = isVSync ? 1 : 0;
        }

        public void ApplyGraphicsSettings()
        {
            SetQualityLevel(preferences.qualityLevel);
            SetFullscreen(preferences.fullscreenMode);
            SetResolution(preferences.resolution);
            SetVSync(preferences.vSync);
        }

        #endregion

        #region Audio

        public void SetMasterVolume(float volume)
        {
            _masterVca.setVolume(volume);
        }

        public void SetMusicVolume(float volume)
        {
            _bgmVca.setVolume(volume);
        }

        public void SetSfxVolume(float volume)
        {
            _sfxVca.setVolume(volume);
        }

        public void SetMuteWhenInactive(bool shouldMute)
        {
            preferences.muteWhenInactive = shouldMute;
        }

        public void ApplyAudioSettings()
        {
            SetMasterVolume(preferences.masterVolume);
            SetMusicVolume(preferences.musicVolume);
            SetSfxVolume(preferences.sfxVolume);
            SetMuteWhenInactive(preferences.muteWhenInactive);
        }

        #region Mute When Inactive

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!preferences.muteWhenInactive) return;

            _masterVca.setVolume(hasFocus ? preferences.masterVolume : 0f);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!preferences.muteWhenInactive) return;

            _masterVca.setVolume(pauseStatus ? 0f : preferences.masterVolume);
        }

        #endregion

        #endregion

        #region Controls

        public void SetSensitivity(float sensitivity)
        {
            preferences.cameraSensitivity = sensitivity;

            foreach (var controller in SensitivityControllers) controller.SetSensitivity(sensitivity);
        }

        public void SetInvertX(bool invert)
        {
            preferences.invertX = invert;

            foreach (var inverter in XInverters) inverter.InvertX(invert);
        }

        public void SetInvertY(bool invert)
        {
            preferences.invertY = invert;

            foreach (var inverter in YInverters) inverter.InvertY(invert);
        }

        public void ApplyControlsSettings()
        {
            SetSensitivity(preferences.cameraSensitivity);
            SetInvertX(preferences.invertX);
            SetInvertY(preferences.invertY);
        }

        #endregion

        #endregion
    }
}