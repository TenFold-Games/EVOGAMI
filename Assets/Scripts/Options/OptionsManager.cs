using System.Collections.Generic;
using EVOGAMI.Core;
using EVOGAMI.Custom.Scriptable;
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

        public readonly HashSet<ISensitivityController> SensitivityControllers = new();
        public readonly HashSet<IXInverter> XInverters = new();
        public readonly HashSet<IYInverter> YInverters = new();

        #region Unity Functions

        public void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
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

        #endregion

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "Alpha Build Scene") return;

            ApplyControlsSettings();
        }

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

        // TODO: Implement audio settings

        #region Audio

        public void ApplyAudioSettings()
        {
        }

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