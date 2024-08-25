using EVOGAMI.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace EVOGAMI.UI
{
    public class Outro : MonoBehaviour
    {
        [SerializeField] [Tooltip("The video played at the end of the game")]
        private VideoPlayer outroVideoPlayer;

        [SerializeField] [Tooltip("The panel shown after the video ends")]
        private GameObject thankYouPanel;

        private void LoadStartMenu()
        {
            SceneManager.LoadScene("Start Menu");
        }

        #region Callbacks

        private void OnVideoEnd(VideoPlayer source)
        {
            outroVideoPlayer.gameObject.SetActive(false);
            thankYouPanel.SetActive(true);
        }

        private void OnCancelPerformed()
        {
            Debug.Log("Cancel performed, is playing: " + outroVideoPlayer.isPlaying);
            if (outroVideoPlayer.isPlaying)
            {
                // Stop the video and show the thank you panel
                outroVideoPlayer.Stop();
                outroVideoPlayer.gameObject.SetActive(false);
                thankYouPanel.SetActive(true);
            }
            else
            {
                // Load the start menu
                LoadStartMenu();
            }
        }

        #endregion

        #region Unity Functions

        private void Awake()
        {
            outroVideoPlayer.loopPointReached += OnVideoEnd;
            InputManager.Instance.OnCancelPerformed += OnCancelPerformed;
        }

        private void OnDestroy()
        {
            outroVideoPlayer.loopPointReached -= OnVideoEnd;
            InputManager.Instance.OnCancelPerformed -= OnCancelPerformed;
        }

        private void Start()
        {
            thankYouPanel.SetActive(false);
            outroVideoPlayer.Play();
        }

        #endregion
    }
}