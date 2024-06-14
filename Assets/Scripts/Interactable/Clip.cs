using EVOGAMI.Region;
using UnityEngine;

namespace EVOGAMI.Interactable
{
    public class Clip : MonoBehaviour
    {
        [Header("Callback Regions")] 
        // The regions that detect the ball
        [SerializeField] [Tooltip("The region that detects the ball")]
        private CallbackRegion ballDetector;
        // The region that detects the player
        [SerializeField] [Tooltip("The region that detects the player")]
        private CallbackRegion playerDetector;
        
        private ObjectLauncher _objectLauncher;
        
        // Flags
        private bool _ballInPlace;

        #region Unity Functions

        private void Awake()
        {
            // Components
            _objectLauncher = GetComponent<ObjectLauncher>();

            // Initialize flags
            _ballInPlace = false;
            
            // Subscribe to events
            ballDetector.RegionEnterCallback.AddListener(OnBallEnter);
            ballDetector.RegionExitCallback.AddListener(OnBallExit);
            playerDetector.RegionEnterCallback.AddListener(OnPlayerEnter);
        }

        #endregion

        /// <summary>
        ///     Callback for when the ball enters the BallDetector region
        /// </summary>
        /// <param name="other">The collider of the ball</param>
        private void OnBallEnter(Collider other)
        {
            _ballInPlace = true;
            _objectLauncher.SetObject(other.gameObject);
        }

        /// <summary>
        ///     Callback for when the ball exits the BallDetector region
        /// </summary>
        /// <param name="other">The collider of the ball</param>
        private void OnBallExit(Collider other)
        {
            _ballInPlace = false;
            _objectLauncher.SetObject(null);
        }

        /// <summary>
        ///     Callback for when the player enters the PlayerDetector region
        /// </summary>
        /// <param name="other">The collider of the player</param>
        private void OnPlayerEnter(Collider other)
        {
            // No ball to launch
            if (!_ballInPlace) return;

            _objectLauncher.Launch();
        }
    }
}