using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.Movement
{
    /// <summary>
    ///     Base class for all movement scripts.
    /// </summary>
    public abstract class MovementBase : MonoBehaviour
    {
        // Managers
        protected InputManager InputManager;
        protected PlayerManager PlayerManager;

        // Player
        protected Rigidbody PlayerRb;
        protected Transform PlayerTransform;
        protected Transform CameraTransform;

        private bool _isCallbackRegistered = false;

        #region Callbacks

        protected virtual void RegisterCallbacks()
        {
            _isCallbackRegistered = true;
        }

        protected virtual void UnregisterCallbacks()
        {
            _isCallbackRegistered = false;
        }

        #endregion

        #region Unity Functions

        protected virtual void Start()
        {
            // Managers
            PlayerManager = PlayerManager.Instance;
            InputManager = InputManager.Instance;

            // Player
            PlayerTransform = PlayerManager.Player.transform;
            PlayerRb = PlayerManager.PlayerRb;
            CameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
            
            if (!_isCallbackRegistered) RegisterCallbacks();
        }

        protected virtual void OnEnable()
        {
            if (!InputManager) return;
            
            if (!_isCallbackRegistered) RegisterCallbacks();
        }
        
        protected virtual void OnDisable()
        {
            if (_isCallbackRegistered) UnregisterCallbacks();
        }

        protected virtual void Update() {}

        protected virtual void FixedUpdate() {}

        #endregion
    }
}