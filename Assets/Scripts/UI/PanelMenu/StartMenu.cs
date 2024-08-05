using System.Collections;
using EVOGAMI.Core;
using EVOGAMI.UI.Common;
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
        
        [Header("Buttons")]
        // The start button
        [SerializeField] [Tooltip("The start button")]
        private MenuButton startButton;
        // The options button
        [SerializeField] [Tooltip("The options button")]
        private MenuButton optionsButton;
        // The exit button
        [SerializeField] [Tooltip("The exit button")]
        private MenuButton exitButton;

        #region Unity Functions

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

        #endregion

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
            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            yield return new WaitForSeconds(0.5f);
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
            Invoke(nameof(Quit), 0.5f);
        }

        private void Quit()
        {
            Application.Quit();
        }

        #endregion
    }
}