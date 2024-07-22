using Cinemachine;
using EVOGAMI.Core;
using EVOGAMI.Custom.Scriptable;
using EVOGAMI.UI.OptionsMenu.Tabs;
using EVOGAMI.UI.PanelMenu;
using UnityEngine;

namespace EVOGAMI.UI.OptionsMenu
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

        [Header("Controller")]
        [SerializeField] [Tooltip("The sub menu controller")]
        private SubMenuController controller;

        [HideInInspector] public TabBase currentTab;

        [HideInInspector] public SubMenuBase cameFrom;

        #region Interaction Callbacks

        public void OnBackClicked()
        {
            controller.CloseOptionsMenu();
        }

        #endregion

        #region Unity Functions

        public override void OnEnable()
        {
            // Disable player input
            InputManager.Instance.Controls.Player.Disable();
            InputManager.Instance.Controls.Origami.Disable();

            base.OnEnable();
            Reset();

            // Pause the game
            Time.timeScale = Mathf.Epsilon;
        }

        public override void OnDisable()
        {
            // Unpause the game
            Time.timeScale = 1.0f;

            base.OnDisable();

            // Enable player input
            InputManager.Instance.Controls.Player.Enable();
            InputManager.Instance.Controls.Origami.Enable();
        }

        private void Reset()
        {
            // Set the graphics tab as the current tab
            graphicsTab.OnTabEnter();
            // Disable all other tabs
            audioTab.gameObject.SetActive(false);
            controlsTab.gameObject.SetActive(false);
            creditsTab.gameObject.SetActive(false);
        }

        #endregion
    }
}