using EVOGAMI.Core;
using EVOGAMI.UI.PanelMenu;
using EVOGAMI.UI.Transformation;
using UnityEngine;
using Cursor = UnityEngine.Cursor;

namespace EVOGAMI.UI
{
    public class SubMenuController : MonoBehaviour
    {
        [Header("Menus")]
        // The pause menu
        [SerializeField]
        [Tooltip("The pause menu")]
        private PauseMenu pauseMenu;
        // The options menu
        [SerializeField] [Tooltip("The options menu")]
        private OptionsMenu.OptionsMenu optionsMenu;
        // The transformation panel
        [SerializeField] [Tooltip("The transformation panel")]
        private TransformPanel transformationPanel;

        private GameObject _headsUpDisplay;

        public void Start()
        {
            // Get the heads-up display
            _headsUpDisplay = UiManager.Instance.headsUpDisplay;

            // Disable the menus
            pauseMenu.gameObject.SetActive(false);
            optionsMenu.gameObject.SetActive(false);

            // Subscribe to events
            InputManager.Instance.OnPausePerformed += OnPausePerformed;

            pauseMenu.EnabledCallback += OnPauseMenuEnabled;
            pauseMenu.DisabledCallback += OnPauseMenuDisabled;

            optionsMenu.EnabledCallback += OnOptionsMenuEnabled;
            optionsMenu.DisabledCallback += OnOptionsMenuDisabled;
        }

        private void OnPauseMenuEnabled()
        {
            _headsUpDisplay.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }

        private void OnPauseMenuDisabled()
        {
            _headsUpDisplay.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnOptionsMenuEnabled()
        {
            _headsUpDisplay.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }

        private void OnOptionsMenuDisabled()
        {
            _headsUpDisplay.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnPausePerformed()
        {
            if (transformationPanel.isPanelOpen)
                transformationPanel.Toggle();
            else
                pauseMenu.gameObject.SetActive(!pauseMenu.IsActive);
        }

        public void OpenOptionsMenu(SubMenuBase cameFrom)
        {
            // Options menu was opened from the pause menu
            optionsMenu.cameFrom = cameFrom;
            if (optionsMenu.cameFrom)
                optionsMenu.cameFrom.gameObject.SetActive(false);

            optionsMenu.gameObject.SetActive(true);
        }

        public void CloseOptionsMenu()
        {
            optionsMenu.gameObject.SetActive(false);

            // Options menu was opened from some other menu
            if (optionsMenu.cameFrom)
                optionsMenu.cameFrom.gameObject.SetActive(true);
        }
    }
}