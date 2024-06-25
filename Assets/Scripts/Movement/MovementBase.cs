using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.Movement
{
    /// <summary>
    ///     Base class for all movement scripts.
    /// </summary>
    public abstract class MovementBase : CallbackMonoBehaviour
    {
        // Managers
        protected PlayerManager PlayerManager;

        // Player
        protected Rigidbody PlayerRb;
        protected Transform PlayerTransform;
        protected Transform CameraTransform;

        #region Unity Functions

        protected override void Start()
        {
            // Managers
            PlayerManager = PlayerManager.Instance;

            base.Start();

            // Player
            PlayerTransform = PlayerManager.Player.transform;
            PlayerRb = PlayerManager.PlayerRb;
            CameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }

        protected virtual void Update() {}

        protected virtual void FixedUpdate() {}

        #endregion
    }
}