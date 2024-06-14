using System;
using EVOGAMI.Core;
using EVOGAMI.UI.PanelMenu;
using UnityEngine;
using UnityEngine.Serialization;

namespace EVOGAMI.UI
{
    public class SubMenuController : MonoBehaviour
    {
        // Managers
        private UiManager _uiManager;
        
        [Header("Menus")]
        // The pause menu
        [SerializeField] [Tooltip("The pause menu")]
        private SubMenuBase pauseMenu;
        
        private GameObject _headsUpDisplay;

        public void Start()
        {
            _uiManager = UiManager.Instance;
            
            // Get the heads-up display
            _headsUpDisplay = UiManager.Instance.headsUpDisplay;
            
            // Subscribe to events
            InputManager.Instance.OnPausePerformed += OnPausePerformed;
        }

        private void OnPausePerformed()
        {
            
            var isEnabled = pauseMenu.gameObject.activeInHierarchy;
            
            // Toggle menu
            _headsUpDisplay.SetActive(isEnabled);
            pauseMenu.gameObject.SetActive(!isEnabled);
            if (!isEnabled)
            {
                pauseMenu.Enable(null);
                // Unlock cursor
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                pauseMenu.Disable();
                // Lock cursor
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}