using System.Collections;
using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.UI.PanelMenu
{
    /// <summary>
    ///     The pause menu.
    /// </summary>
    public class PauseMenu : SubMenuBase
    {
        public override void OnEnable()
        {
            // Hide the HUD
            controller.ToggleHUD(false);

            // Disable player input
            InputManager.Instance.Controls.Player.Disable();
            InputManager.Instance.Controls.Origami.Disable();

            base.OnEnable();

            // Pause the game
            Time.timeScale = Mathf.Epsilon;

            // Unlock the cursor
            Cursor.lockState = CursorLockMode.None;
        }

        public override void OnDisable()
        {
            // Lock the cursor
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

        #region Callbacks

        public void OnPausePerformed()
        {
            if (controller.isCancelPerformedThisFrame) return; // Fix `esc` key having double role

            gameObject.SetActive(!gameObject.activeSelf);
        }

        public override void OnCancelPerformed(out bool isPanelClosed)
        {
            isPanelClosed = !gameObject.activeSelf;

            if (!gameObject.activeSelf) return; // Ignore if not active

            gameObject.SetActive(false);

            controller.SetCancelPerformedFlag();
        }

        #endregion

        #region Button Callbacks

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
            Debug.Log("OnExitClicked");
            StartCoroutine(Quit());
        }
        
        private IEnumerator Quit()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            Application.Quit();
        }

        #endregion
    }
}