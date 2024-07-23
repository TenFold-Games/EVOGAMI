using UnityEngine;
using UnityEngine.SceneManagement;

namespace EVOGAMI.UI.PanelMenu
{
    /// <summary>
    ///     The start menu.
    /// </summary>
    public class StartMenu : SubMenuBase
    {
        [Header("Scene")]
        // The name of the scene to load
        [SerializeField] [Tooltip("The name of the scene to load")]
        private string sceneToLoad;

        public override void OnEnable()
        {
            base.OnEnable();

            Cursor.lockState = CursorLockMode.None;
        }

        public override void OnDisable()
        {
            base.OnDisable();

            Cursor.lockState = CursorLockMode.Locked;
        }

        public override void OnCancelPerformed(out bool isPanelClosed)
        {
            // Nothing to do
            isPanelClosed = false;
        }

        #region Button Callbacks

        /// <summary>
        ///     Callback for when the continue button is clicked.
        /// </summary>
        public void OnStartClicked()
        {
            SceneManager.LoadScene(sceneToLoad);
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
            // Application.Quit();

            Invoke(nameof(Quit), 1.0f);
        }

        private void Quit()
        {
            Application.Quit();
        }

        #endregion
    }
}