using EVOGAMI.Core;
using EVOGAMI.UI.PanelMenu;
using EVOGAMI.UI.Transformation;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace EVOGAMI.UI
{
    public class SubMenuController : MonoBehaviour
    {
        [Header("Menus")]
        // The pause menu
        [SerializeField] [Tooltip("The pause menu")]
        private SubMenuBase pauseMenu;

        private GameObject _headsUpDisplay;
        
        [SerializeField] [Tooltip("The transformation panel")]
        private TransformPanel _transformationPanel;

        public void Start()
        {
            // Get the heads-up display
            _headsUpDisplay = UiManager.Instance.headsUpDisplay;

            // Subscribe to events
            InputManager.Instance.OnPausePerformed += OnPausePerformed;
            
            pauseMenu.EnabledCallback += OnPauseMenuEnabled;
            pauseMenu.DisabledCallback += OnPauseMenuDisabled;
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
        
        private void OnPausePerformed()
        {
            if (_transformationPanel.isPanelOpen)
                _transformationPanel.Toggle();
            else
                pauseMenu.gameObject.SetActive(!pauseMenu.IsActive);
        }
    }
}