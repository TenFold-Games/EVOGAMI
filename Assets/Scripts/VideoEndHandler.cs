using EVOGAMI.Core;
using EVOGAMI.Utils;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Video;

public class VideoEndHandler : MonoBehaviour
{
    [Header("Video")]
    // Reference to the VideoPlayer component
    [SerializeField] [Tooltip("Reference to the VideoPlayer component")]
    private VideoPlayer videoPlayer;

    [Header("Scene Management")]
    // Name of the scene to switch to
    [SerializeField] [Tooltip("Name of the scene to switch to")]
    public string nextSceneName;

    // The BGM FMOD studio event emitter manager
    private FMODStudioEventManager _bgmEmitterManager;

    private void Start()
    {
        // Subscribe to the loopPointReached event
        if (videoPlayer) videoPlayer.loopPointReached += OnVideoEnd;

        _bgmEmitterManager = FMODStudioEventManager.Instance;

        InputManager.Instance.OnCancelPerformed += SkipVideo;
    }

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
}