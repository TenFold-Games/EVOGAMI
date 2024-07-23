using EVOGAMI.Options;
using UnityEngine;
using UnityEngine.UI;

namespace EVOGAMI.UI.OptionsMenu.Tabs
{
    public class AudioTab : TabBase
    {
        // Master Volume Slider
        [SerializeField] [Tooltip("Master Volume Slider")]
        private Slider masterVolumeSlider;
        // Music Volume Slider
        [SerializeField] [Tooltip("Music Volume Slider")]
        private Slider musicVolumeSlider;
        // SFX Volume Slider
        [SerializeField] [Tooltip("SFX Volume Slider")]
        private Slider sfxVolumeSlider;
        // Mute when inactive Toggle
        [SerializeField] [Tooltip("Mute when inactive Toggle")]
        private Toggle muteWhenInactiveToggle;

        protected override void Initialize()
        {
            masterVolumeSlider.value = OptionsManager.Instance.preferences.masterVolume;
            musicVolumeSlider.value = OptionsManager.Instance.preferences.musicVolume;
            sfxVolumeSlider.value = OptionsManager.Instance.preferences.sfxVolume;
            muteWhenInactiveToggle.isOn = OptionsManager.Instance.preferences.muteWhenInactive;
        }

        protected override void RegisterCallbacks()
        {
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeSliderValueChanged);
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderValueChanged);
            sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeSliderValueChanged);
            muteWhenInactiveToggle.onValueChanged.AddListener(OnMuteWhenInactiveToggleValueChanged);
        }
        
        #region Callbacks
        
        private void OnMasterVolumeSliderValueChanged(float value)
        {
            OptionsManager.Instance.SetMasterVolume(value);
        }
        
        private void OnMusicVolumeSliderValueChanged(float value)
        {
            OptionsManager.Instance.SetMusicVolume(value);
        }
        
        private void OnSfxVolumeSliderValueChanged(float value)
        {
            OptionsManager.Instance.SetSfxVolume(value);
        }
        
        private void OnMuteWhenInactiveToggleValueChanged(bool value)
        {
            OptionsManager.Instance.SetMuteWhenInactive(value);
        }
        
        #endregion
    }
}