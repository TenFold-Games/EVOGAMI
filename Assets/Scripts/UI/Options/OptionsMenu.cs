using EVOGAMI.Core;
using EVOGAMI.UI.Options.Tabs;
using EVOGAMI.UI.PanelMenu;
using UnityEngine;

namespace EVOGAMI.UI.Options
{
    public class OptionsMenu : SubMenuBase
    {
        [Header("Tabs")]
        // The graphics tab
        [SerializeField] [Tooltip("The graphics tab")]
        private TabBase graphicsTab;
        // The audio tab
        [SerializeField] [Tooltip("The audio tab")]
        private TabBase audioTab;
        // The controls tab
        [SerializeField] [Tooltip("The controls tab")]
        private TabBase controlsTab;
        // The credits tab
        [SerializeField] [Tooltip("The credits tab")]
        private TabBase creditsTab;

        [HideInInspector] 
        public TabBase currentTab;

        [HideInInspector] 
        public SubMenuBase cameFrom;

        private void Reset()
        {
            // Set the graphics tab as the current tab
            graphicsTab.OnTabEnter();
            // Disable all other tabs
            audioTab.gameObject.SetActive(false);
            controlsTab.gameObject.SetActive(false);
            creditsTab.gameObject.SetActive(false);
        }

        public override void OnCancelPerformed()
        {
            base.OnCancelPerformed();
            
            controller.CloseOptionsMenu();
            
            controller.SetCancelPerformedFlag();
        }

        #region Interaction Callbacks

        public void OnBackClicked()
        {
            controller.CloseOptionsMenu();
        }

        #endregion

        #region Unity Functions

        public override void OnEnable()
        {
            // Hide the HUD
            controller.ToggleHUD(false);

            // Disable player input
            InputManager.Instance.Controls.Player.Disable();
            InputManager.Instance.Controls.Origami.Disable();

            base.OnEnable();
            Reset();

            // Pause the game
            Time.timeScale = Mathf.Epsilon;
            
            // Lock the cursor
            Cursor.lockState = CursorLockMode.None;
        }

        public override void OnDisable()
        {
            // Unlock the cursor
            Cursor.lockState = CursorLockMode.Locked;

            // Unpause the game
            Time.timeScale = 1.0f;

            base.OnDisable();

            // Enable player input
            InputManager.Instance.Controls.Player.Enable();
            InputManager.Instance.Controls.Origami.Enable();
            
            // Show the HUD
            controller.ToggleHUD(true);
        }

        #endregion
    }
}