using EVOGAMI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace EVOGAMI.UI.PanelMenu
{
    /// <summary>
    ///     The pause menu.
    /// </summary>
    public class PauseMenu : SubMenuBase
    {
        public override void Enable(Selectable previousElement)
        {
            base.Enable(previousElement);

            // Pause the game
            Time.timeScale = Mathf.Epsilon;
            
            // Disable player input
            InputManager.Instance.Controls.UI.Cancel.performed += ctx => OnContinueClicked();
            InputManager.Instance.Controls.Player.Disable();
            InputManager.Instance.Controls.Origami.Disable();
        }

        public override void Disable()
        {
            base.Disable();

            // Unpause the game
            Time.timeScale = 1;
            
            // Enable player input
            InputManager.Instance.Controls.UI.Cancel.performed -= ctx => OnContinueClicked();
            InputManager.Instance.Controls.Player.Enable();
            InputManager.Instance.Controls.Origami.Enable();
        }

        #region Callbacks

        /// <summary>
        ///     Callback for when the continue button is clicked.
        /// </summary>
        public void OnContinueClicked()
        {
            Disable();
            if (UiManager.Instance.headsUpDisplay && !UiManager.Instance.headsUpDisplay.gameObject.activeSelf)
                UiManager.Instance.headsUpDisplay.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        ///     Callback for when the options button is clicked.
        /// </summary>
        public void OnOptionsClicked()
        {
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