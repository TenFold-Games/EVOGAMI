using EVOGAMI.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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
        
        private void Start()
        {
            Enable(null);
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