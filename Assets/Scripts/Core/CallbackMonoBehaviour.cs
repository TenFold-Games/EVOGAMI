using UnityEngine;

namespace EVOGAMI.Core
{
    public abstract class CallbackMonoBehaviour : MonoBehaviour
    {
        // Managers
        protected InputManager InputManager;

        // Flags
        private bool _isCallbackRegistered;

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
            InputManager = InputManager.Instance;

            Debug.Assert(InputManager, "InputManager is null.");
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

        #endregion
    }
}