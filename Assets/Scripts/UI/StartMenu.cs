using EVOGAMI.Core;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;
using UnityEngine.SceneManagement;

namespace EVOGAMI.UI
{
    
    public class StartMenu : SubMenuBase
    {
        [SerializeField] private string sceneName;

        #region Callbacks

        /// <summary>
        ///     Callback for when the continue button is clicked.
        /// </summary>
        public void OnStartClicked()
        {
            SceneManager.LoadScene(sceneName);
            
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