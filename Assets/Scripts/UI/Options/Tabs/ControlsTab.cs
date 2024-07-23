using EVOGAMI.Options;
using UnityEngine;
using UnityEngine.UI;

namespace EVOGAMI.UI.Options.Tabs
{
    public class ControlsTab : TabBase
    {
        [Header("Controls")]
        // Sensitivity Slider
        [SerializeField] [Tooltip("Sensitivity Slider")]
        private Slider sensitivitySlider;
        // Invert X Toggle
        [SerializeField] [Tooltip("Invert X Toggle")]
        private Toggle invertXToggle;
        // Invert Y Toggle
        [SerializeField] [Tooltip("Invert Y Toggle")]
        private Toggle invertYToggle;

        protected override void Initialize()
        {
            sensitivitySlider.value = OptionsManager.Instance.preferences.cameraSensitivity;
            invertXToggle.isOn = OptionsManager.Instance.preferences.invertX;
            invertYToggle.isOn = OptionsManager.Instance.preferences.invertY;
        }

        protected override void RegisterCallbacks()
        {
            // Update callbacks
            sensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderValueChanged);
            invertXToggle.onValueChanged.AddListener(OnInvertXToggleValueChanged);
            invertYToggle.onValueChanged.AddListener(OnInvertYToggleValueChanged);
        }

        #region Callbacks

        private void OnSensitivitySliderValueChanged(float value)
        {
            OptionsManager.Instance.SetSensitivity(value);
        }

        private void OnInvertXToggleValueChanged(bool value)
        {
            OptionsManager.Instance.SetInvertX(value);
        }

        private void OnInvertYToggleValueChanged(bool value)
        {
            OptionsManager.Instance.SetInvertY(value);
        }

        #endregion
    }
}