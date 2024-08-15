using EVOGAMI.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace EVOGAMI.Media
{
    public class VideoEndHandler : MonoBehaviour
    {
        [Header("Video")]
        // Reference to the VideoPlayer component
        [SerializeField]
        [Tooltip("Reference to the VideoPlayer component")]
        private VideoPlayer videoPlayer;

        [Header("Scene Management")]
        // Name of the scene to switch to
        [SerializeField]
        [Tooltip("Name of the scene to switch to")]
        public string nextSceneName;

        // The BGM FMOD studio event emitter manager
        private FMODStudioEventManager _bgmEmitterManager;

        private void OnVideoEnd(VideoPlayer vp)
        {
            if (_bgmEmitterManager) _bgmEmitterManager.Stop();
            Destroy(_bgmEmitterManager.gameObject);

            LoadScene();
        }

        private void SkipVideo()
        {
            // Skip the video and load the scene
            if (videoPlayer) videoPlayer.Stop(); // Stop the video

            OnVideoEnd(videoPlayer);
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(nextSceneName); // Load the specified scene
        }

        #region Unity Functions

        private void Start()
        {
            // Subscribe to the loopPointReached event
            if (videoPlayer) videoPlayer.loopPointReached += OnVideoEnd;

            _bgmEmitterManager = FMODStudioEventManager.Instance;

            InputManager.Instance.OnCancelPerformed += SkipVideo;
        }

        private void OnDestroy()
        {
            // Unsubscribe from the loopPointReached event
            if (videoPlayer) videoPlayer.loopPointReached -= OnVideoEnd;

            InputManager.Instance.OnCancelPerformed -= SkipVideo;
        }

        #endregion
    }
}