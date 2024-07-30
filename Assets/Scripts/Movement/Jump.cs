using System.Collections;
using EVOGAMI.Animations;
using EVOGAMI.Core;
using EVOGAMI.Movement.CheckProvider.Ground;
using UnityEngine;
using FMODUnity;

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
        // Whether there is a difference between big and small jumps.
        [SerializeField] [Tooltip("Whether there is a difference between big and small jumps.")]
        private bool isBigJump = false;
        // The minimal time the jump must perform before being cut off.
        [SerializeField] [Tooltip("The minimal time the jump must perform before being cut off.")]
        private float minJumpCutoff = 0.1f;
        
        [Header("Ground Check")]
        // The ground check provider.
        [SerializeField] [Tooltip("The ground check provider.")]
        private GroundCheckProviderBase groundCheckProvider;
        
        [Header("Animation")]
        // The animator component
        [SerializeField] [Tooltip("The animator component")]
        private Animator animator;

        // FMOD Studio Event Emitter
        [SerializeField] StudioEventEmitter frogjumpsfx;
        
        private float _jumpTimer = 0f;

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
            
            InputManager.OnJumpStarted += OnJumpStarted;
            InputManager.OnJumpCancelled += OnJumpCancelled;
        }
        
        protected override void UnregisterCallbacks()
        {
            base.UnregisterCallbacks();
            
            InputManager.OnJumpStarted -= OnJumpStarted;
            InputManager.OnJumpCancelled -= OnJumpCancelled;
        }

        #endregion
        
        #region Input Events
        
        private void OnJumpStarted()
        {
            if (!groundCheckProvider.IsCheckTrue) return;
            
            _jumpTimer = Time.time;
            
            // Add haptic feedback
            InputManager.VibrateController(0.05f, 0.05f, 0.025f);

            // Perform jump
            PlayerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // Play frog jump sfx
            frogjumpsfx?.Play();

        }

        private void OnJumpCancelled()
        {
            if (!isBigJump) return;
            
            var diff = Time.time - _jumpTimer;
            
            if (diff < minJumpCutoff)
                // Wait until the minimal jump time is reached
                StartCoroutine(StopJumpingAfterTime(minJumpCutoff - diff));
            else
                // Stop immediately
                StopJumping();
        }

        #endregion
        
        private IEnumerator StopJumpingAfterTime(float time)
        {
            yield return new WaitForSeconds(time);

            StopJumping();
        }

        private void StopJumping()
        {
            // Remove upwards force
            if (PlayerRb.velocity.y > 0)
                PlayerRb.AddForce(Vector3.down * PlayerRb.velocity.y, ForceMode.Impulse);
        }

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