using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoEndHandler : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    public string sceneName; // Name of the scene to switch to

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd; // Subscribe to the loopPointReached event
        }
    }

    void Update()
    {
        // Check for "W" key press or "West" button on controller
        if (Input.GetKeyDown(KeyCode.W) || Input.GetButtonDown("West"))
        {
            SkipVideo();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        LoadScene();
    }

    void SkipVideo()
    {
        // Skip the video and load the scene
        if (videoPlayer != null)
        {
            videoPlayer.Stop(); // Stop the video
        }
        LoadScene();
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneName); // Load the specified scene
    }
}
