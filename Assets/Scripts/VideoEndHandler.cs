using EVOGAMI.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoEndHandler : MonoBehaviour
{
    // Reference to the VideoPlayer component
    [SerializeField] [Tooltip("Reference to the VideoPlayer component")]
    private VideoPlayer videoPlayer;
    // Name of the scene to switch to
    [SerializeField] [Tooltip("Name of the scene to switch to")]
    public string nextSceneName;

    private void Start()
    {
        // Subscribe to the loopPointReached event
        if (videoPlayer) videoPlayer.loopPointReached += OnVideoEnd;
        
        InputManager.Instance.OnCancelPerformed += SkipVideo;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        LoadScene();
    }

    private void SkipVideo()
    {
        // Skip the video and load the scene
        if (videoPlayer) videoPlayer.Stop(); // Stop the video

        LoadScene();
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(nextSceneName); // Load the specified scene
    }
}
