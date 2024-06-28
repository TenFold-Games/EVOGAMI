using System;
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

        private void Start()
        {
            OnEnable(); // Explicitly call OnEnable
        }

        #region Callbacks

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
        }

        /// <summary>
        ///     Callback for when the exit button is clicked.
        /// </summary>
        public void OnExitClicked()
        {
            Application.Quit();
        }

        #endregion
    }
}