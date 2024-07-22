using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.UI.PanelMenu
{
    /// <summary>
    ///     The pause menu.
    /// </summary>
    public class PauseMenu : SubMenuBase
    {
        [Header("References")]
        // The sub-menu controller
        [SerializeField] [Tooltip("The sub-menu controller")]
        private SubMenuController controller;

        public override void OnEnable()
        {
            // Disable player input
            InputManager.Instance.Controls.Player.Disable();
            InputManager.Instance.Controls.Origami.Disable();

            base.OnEnable();

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

        #region Callbacks

        /// <summary>
        ///     Callback for when the reset button is clicked.
        /// </summary>
        public void OnResetClicked()
        {
            GameManager.Instance.ResetGame();
            gameObject.SetActive(false);
        }

        /// <summary>
        ///     Callback for when the options button is clicked.
        /// </summary>
        public void OnOptionsClicked()
        {
            controller.OpenOptionsMenu(this);
        }

        /// <summary>
        ///     Callback for when the exit button is clicked.
        /// </summary>
        public void OnExitClicked()
        {
            GameManager.Instance.ExitGame();
        }

        #endregion
    }
}