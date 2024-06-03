using System;
using EVOGAMI.Core;
using EVOGAMI.Origami;
using UnityEngine;

namespace EVOGAMI.Movement
{
    public class LocomotionBase : MonoBehaviour
    {
        // Managers
        protected InputManager InputManager;
        protected PlayerManager PlayerManager;

        // Player
        protected Transform PlayerTransform;
        protected Rigidbody PlayerRb;
        protected Transform CameraTransform;
        
        // Movement
        [Header("Movement")]
        [SerializeField] protected float speed = 5f;
        [SerializeField] protected float rotationSpeed = 10f;
        [SerializeField] protected float jumpForce = 5f;
        [SerializeField] protected float sprintMultiplier = 2f;

        // Ground check
        [Header("Ground Check")]
        [SerializeField] protected Transform groundCheck;
        [SerializeField] protected float groundCheckDistance = 0.5f;
        [SerializeField] protected float groundCheckRadius = 0.2f;
        protected LayerMask GroundLayer;
        protected RaycastHit GroundHit;

        // Form
        [SerializeField] protected OrigamiContainer.OrigamiForm form;

        // Flags
        protected bool IsGrounded;
        protected bool IsSprinting;
        protected bool IsCallbackSet;

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

            // Ground check
            GroundLayer = LayerMask.GetMask("Ground", "Water");
            
            // Register events
            if (!IsCallbackSet) {
                // Move
                InputManager.OnMoveCancelled += OnMoveCancelled;
                // Jump
                InputManager.OnJumpPerformed += OnJumpPerformed;
                // Sprint (Hold)
                InputManager.OnSprintHoldStarted += OnSprintStarted;
                InputManager.OnSprintHoldCancelled += OnSprintHoldCancelled;
                // Sprint (Press)
                InputManager.OnSprintPressStarted += OnSprintStarted;
            }
        }

        protected void OnEnable()
        {
            if (!InputManager) return;
            
            // Register events
            // Move
            InputManager.OnMoveCancelled += OnMoveCancelled;
            // Jump
            InputManager.OnJumpPerformed += OnJumpPerformed;
            // Sprint (Hold)
            InputManager.OnSprintHoldStarted += OnSprintStarted;
            InputManager.OnSprintHoldCancelled += OnSprintHoldCancelled;
            // Sprint (Press)
            InputManager.OnSprintPressStarted += OnSprintStarted;
            
            IsCallbackSet = true;
        }
        
        private void OnDisable()
        {
            // Unregister events
            // Move
            InputManager.OnMoveCancelled -= OnMoveCancelled;
            // Jump
            InputManager.OnJumpPerformed -= OnJumpPerformed;
            // Sprint (Hold)
            InputManager.OnSprintHoldStarted -= OnSprintStarted;
            InputManager.OnSprintHoldCancelled -= OnSprintHoldCancelled;
            // Sprint (Press)
            InputManager.OnSprintPressStarted -= OnSprintStarted;
            
            IsCallbackSet = false;
        }

        protected virtual void FixedUpdate()
        {
            // Check if the player is on the ground
            GroundCheck();

            Move(Time.fixedDeltaTime);
        }

        #endregion
        
        #region Input Events

        /// <summary>
        ///     Called when the move input is released
        /// </summary>
        private void OnMoveCancelled()
        {
            IsSprinting = false;
        }

        /// <summary>
        ///     Called when the jump input is pressed
        /// </summary>
        protected virtual void OnJumpPerformed()
        {
            if (!IsGrounded) return;

            PlayerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        /// <summary>
        ///     Called when the sprint input is started
        /// </summary>
        private void OnSprintStarted()
        {
            // Cannot sprint in the air
            if (!IsGrounded) return;

            IsSprinting = true;
        }

        /// <summary>
        ///     Called when the sprint input (hold) is released
        /// </summary>
        private void OnSprintHoldCancelled()
        {
            IsSprinting = false;
        }

        #endregion

        #region Movement

        /// <summary>
        ///     Handle player movement
        /// </summary>
        /// <param name="delta">Time since last frame</param>
        protected virtual void Move(float delta)
        {
            if (!InputManager.IsMoving) return;
            
            // Player is not on the ground
            // if (!_isGrounded) return;

            // Calculate move direction
            var moveDirection = CameraTransform.forward * InputManager.MoveInput.y +
                                CameraTransform.right * InputManager.MoveInput.x;
            moveDirection.y = 0;

            // Move and rotate player
            MovePlayer(moveDirection, delta);
            RotatePlayer(moveDirection, delta);
        }

        /// <summary>
        ///     Move the player in the given direction
        /// </summary>
        /// <param name="moveDirection">The direction to move the player</param>
        /// <param name="delta">Time since last frame</param>
        protected virtual void MovePlayer(Vector3 moveDirection, float delta)
        {
            // Velocity
            moveDirection *= speed;
            if (IsSprinting) moveDirection *= sprintMultiplier;
            var yVelocity = PlayerRb.velocity.y;

            // Move player
            PlayerRb.velocity = Vector3.ProjectOnPlane(moveDirection, Vector3.up);
            PlayerRb.velocity += Vector3.up * yVelocity;
        }

        /// <summary>
        ///     Rotate the player towards the given direction
        /// </summary>
        /// <param name="moveDirection">The direction to rotate the player towards</param>
        /// <param name="delta">Time since last frame</param>
        protected virtual void RotatePlayer(Vector3 moveDirection, float delta)
        {
            var targetRotation = Quaternion.LookRotation(moveDirection);
            targetRotation = Quaternion.Slerp(PlayerTransform.rotation, targetRotation, rotationSpeed * delta);
            PlayerTransform.rotation = targetRotation;
        }

        #endregion

        #region Collision

        /// <summary>
        ///     Check if the player is grounded
        /// </summary>
        /// <returns>True if the player is grounded, false otherwise</returns>
        protected virtual void GroundCheck()
        {
            IsGrounded = Physics.SphereCast(
                groundCheck.position,
                groundCheckRadius,
                Vector3.down,
                out GroundHit,
                groundCheckDistance,
                GroundLayer
            );
        }

        protected virtual void OnDrawGizmos()
        {
            if (groundCheck == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }

        #endregion
    }
}