using System;
using UnityEngine;

namespace EVOGAMI.Custom.Scriptable
{
    [Serializable]
    [CreateAssetMenu(fileName = "UserPreferences", menuName = "Settings/UserPreferences", order = 0)]
    public class UserPreferences : ScriptableObject
    {
        [Header("Graphics")]
        // The quality level of the graphics.
        [SerializeField] [Tooltip("The quality level of the graphics.")]
        public int qualityLevel = 2;
        // Whether the game is in fullscreen mode.
        [SerializeField] [Tooltip("Whether the game is in fullscreen mode.")]
        public FullScreenMode fullscreenMode = FullScreenMode.FullScreenWindow;
        // The screen resolution.
        [SerializeField] [Tooltip("The screen resolution.")]
        public Vector2Int resolution = new(1920, 1080);
        // Whether VSync is enabled.
        [SerializeField] [Tooltip("Whether VSync is enabled.")]
        public bool vSync = true;
        
        [Header("Audio")]
        // The master volume.
        [SerializeField] [Tooltip("The master volume.")]
        public float masterVolume = 1f;
        // The music volume.
        [SerializeField] [Tooltip("The music volume.")]
        public float musicVolume = 1f;
        // The sound effects volume.
        [SerializeField] [Tooltip("The sound effects volume.")]
        public float sfxVolume = 1f;
        
        [Header("Controls")]
        // The sensitivity of the camera.
        [SerializeField] [Tooltip("The sensitivity of the camera.")]
        public float cameraSensitivity = 1f;
        // Invert the camera controls on the X-axis.
        [SerializeField] [Tooltip("Invert the camera controls on the X-axis.")]
        public bool invertX;
        // Invert the camera controls on the Y-axis.
        [SerializeField] [Tooltip("Invert the camera controls on the Y-axis.")]
        public bool invertY;
    }
}