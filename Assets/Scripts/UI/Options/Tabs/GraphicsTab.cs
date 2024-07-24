using System.Collections.Generic;
using System.Linq;
using EVOGAMI.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EVOGAMI.UI.Options.Tabs
{
    public class GraphicsTab : TabBase
    {
        [Header("Graphics")]
        // Quality Settings Dropdown
        [SerializeField] [Tooltip("Quality Settings Dropdown")]
        private TMP_Dropdown qualityDropdown;
        // Fullscreen Settings Dropdown
        [SerializeField] [Tooltip("Fullscreen Settings Dropdown")]
        private TMP_Dropdown fullscreenDropdown;
        // Resolution Settings Dropdown
        [SerializeField] [Tooltip("Resolution Settings Dropdown")]
        private TMP_Dropdown resolutionDropdown;
        // VSync Setting Toggle
        [SerializeField] [Tooltip("VSync Settings Toggle")]
        private Toggle vSyncToggle;

        private readonly HashSet<Vector2Int> _resolutions = new();

        private void GetResolutions()
        {
            foreach (var resolution in Screen.resolutions)
            {
                // Get the width, calculate the height (16:9)
                var width = resolution.width;
                var height = (int)(resolution.width / 16f * 9f);
                
                // Add resolution to list
                _resolutions.Add(new Vector2Int(width, height));
            }
        }

        private void UpdateResolutionDropdown()
        {
            // Remove all resolutions from dropdown
            resolutionDropdown.ClearOptions();

            foreach (var resolution in _resolutions)
                // Add resolutions to dropdown
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData($"{resolution[0]}x{resolution[1]}"));

            // Get the index of the current screen resolution
            var index = Screen.resolutions.TakeWhile(resolution =>
                resolution.width != OptionsManager.Instance.preferences.resolution[0] ||
                resolution.height != OptionsManager.Instance.preferences.resolution[1]).Count();

            if (resolutionDropdown.value != index) resolutionDropdown.value = index;
        }

        private int FullscreenModeToInt()
        {
            return OptionsManager.Instance.preferences.fullscreenMode switch
            {
                FullScreenMode.ExclusiveFullScreen => 0,
                FullScreenMode.FullScreenWindow => 1,
                FullScreenMode.MaximizedWindow => 1,
                FullScreenMode.Windowed => 2,
                _ => 2
            };
        }

        protected override void Initialize()
        {
            GetResolutions();
            UpdateResolutionDropdown();

            qualityDropdown.value = OptionsManager.Instance.preferences.qualityLevel;
            fullscreenDropdown.value = FullscreenModeToInt();
            resolutionDropdown.value = _resolutions.ToList().IndexOf(OptionsManager.Instance.preferences.resolution);

            // Apply the settings
            OptionsManager.Instance.ApplyGraphicsSettings();
        }

        protected override void RegisterCallbacks()
        {
            qualityDropdown.onValueChanged.AddListener(OnQualityDropdownValueChanged);
            fullscreenDropdown.onValueChanged.AddListener(OnFullscreenDropdownValueChanged);
            resolutionDropdown.onValueChanged.AddListener(OnResolutionDropdownValueChanged);
            vSyncToggle.onValueChanged.AddListener(OnVSyncToggleValueChanged);
        }

        #region Callbacks

        private void OnQualityDropdownValueChanged(int index)
        {
            OptionsManager.Instance.SetQualityLevel(index);
        }

        private void OnFullscreenDropdownValueChanged(int index)
        {
            OptionsManager.Instance.SetFullscreen(index switch
            {
                0 => FullScreenMode.ExclusiveFullScreen,
                1 => FullScreenMode.FullScreenWindow,
                2 => FullScreenMode.Windowed,
                _ => FullScreenMode.Windowed
            });
        }

        private void OnResolutionDropdownValueChanged(int index)
        {
            OptionsManager.Instance.SetResolution(_resolutions.ElementAt(index));
        }

        private void OnVSyncToggleValueChanged(bool value)
        {
            OptionsManager.Instance.SetVSync(value);
        }

        #endregion
    }
}