using EVOGAMI.Animations;
using EVOGAMI.Movement.CheckProvider.Ground;
using UnityEngine;

namespace EVOGAMI.Movement
{
    /// <summary>
    ///     Handles jumping.
    /// </summary>
    [RequireComponent(typeof(GroundCheckProviderBase))]
    public class Jump : 
        MovementBase,
        IAnimationHandler
    {
        [Header("Jump")]
        // The force applied to the player when jumping.
        [SerializeField] [Tooltip("The force applied to the player when jumping.")]
        private float jumpForce = 5f;
        
        [Header("Ground Check")]
        // The ground check provider.
        [SerializeField] [Tooltip("The ground check provider.")]
        private GroundCheckProviderBase groundCheckProvider;
        
        [Header("Animation")]
        // The animator component
        [SerializeField] [Tooltip("The animator component")]
        private Animator animator;

        // Cashed Property Indices
        private static readonly int Vertical = Animator.StringToHash("vertical");

        #region Animation

        public void SetAnimationParams(float delta)
        {
            if (!animator) return;

            var vertical = PlayerManager.PlayerRb.velocity.y; 
            
            animator.SetFloat(Vertical, vertical, 0.1f, delta);
        }

        #endregion

        #region Callbacks

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();
            
            InputManager.OnJumpPerformed += OnJumpPerformed;
        }
        
        protected override void UnregisterCallbacks()
        {
            base.UnregisterCallbacks();
            
            InputManager.OnJumpPerformed -= OnJumpPerformed;
        }

        #endregion
        
        #region Input Events
        
        private void OnJumpPerformed()
        {
            if (!groundCheckProvider.IsCheckTrue) return;
            
            // Perform jump
            PlayerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
        #endregion

        #region Unity Functions
        
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            
            // Set animation parameters
            SetAnimationParams(Time.fixedDeltaTime);
        }

        #endregion
    }
}