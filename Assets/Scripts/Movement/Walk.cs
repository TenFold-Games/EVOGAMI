using EVOGAMI.Animations;
using EVOGAMI.Movement.CheckProvider.Ground;
using UnityEngine;

namespace EVOGAMI.Movement
{
    [RequireComponent(typeof(GroundCheckProviderBase))]
    public class Walk : 
        MovementBase,
        IAnimationHandler
    {
        [Header("Walk Restrictions")]
        // Whether the player can walk on the ground
        [SerializeField] [Tooltip("Whether the player can walk on the ground")]
        private bool canWalkOnGround = true;
        // Whether the player can walk in the air
        [SerializeField] [Tooltip("Whether the player can walk in the air")]
        private bool canWalkInAir = true;

        [Header("Walk")]
        // The speed at which the player walks
        [SerializeField] [Tooltip("The speed at which the player walks")]
        private float walkSpeed = 5f;
        // The speed at which the player rotates
        [SerializeField] [Tooltip("The speed at which the player rotates")]
        private float rotationSpeed = 10f;

        [Header("Sprint")]
        // Whether the player can sprint
        [SerializeField] [Tooltip("Whether the player can sprint")]
        private bool canSprint = true;
        // The speed at which the player sprints
        [SerializeField] [Tooltip("The speed at which the player sprints")]
        private float sprintSpeed = 10f;

        [Header("Ground Check")]
        // The ground check provider
        [SerializeField] [Tooltip("The ground check provider")]
        private GroundCheckProviderBase groundCheckProvider;

        [Header("Animation")]
        // The animator component
        [SerializeField] [Tooltip("The animator component")]
        private Animator animator;

        // Flags
        private bool _isSprinting;

        // Cashed Property Indices
        private static readonly int Horizontal = Animator.StringToHash("horizontal");
        private static readonly int Vertical = Animator.StringToHash("vertical");

        #region Animation

        public void SetAnimationParams(float delta)
        {
            var horizontal = InputManager.MoveInput.magnitude;
            var vertical = PlayerManager.PlayerRb.velocity.y; 
            
            animator.SetFloat(Horizontal, horizontal, 0.1f, delta);
            animator.SetFloat(Vertical, vertical, 0.1f, delta);
        }

        #endregion

        #region Callbacks

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();
            
            // Walk
            InputManager.OnMoveCancelled += OnMoveCancelled;
            // Sprint
            if (canSprint)
            {
                // Hold
                InputManager.OnSprintHoldStarted += OnSprintStarted;
                InputManager.OnSprintHoldCancelled += OnSprintHoldCancelled;
                // Press
                InputManager.OnSprintPressStarted += OnSprintStarted;
            }
        }
        
        protected override void UnregisterCallbacks()
        {
            base.UnregisterCallbacks();
            
            // Walk
            InputManager.OnMoveCancelled -= OnMoveCancelled;
            // Sprint
            if (canSprint)
            {
                // Hold
                InputManager.OnSprintHoldStarted -= OnSprintStarted;
                InputManager.OnSprintHoldCancelled -= OnSprintHoldCancelled;
                // Press
                InputManager.OnSprintPressStarted -= OnSprintStarted;
            }
        }

        #endregion
        
        #region Input Events

        /// <summary>
        ///     Called when the move input is released
        /// </summary>
        private void OnMoveCancelled()
        {
            _isSprinting = false;
        }
        
        /// <summary>
        ///     Called when the sprint input is started
        /// </summary>
        private void OnSprintStarted()
        {
            // Cannot sprint in the air
            if (!groundCheckProvider.IsCheckTrue) return;

            _isSprinting = true;
        }
        
        /// <summary>
        ///     Called when the sprint input (hold) is released
        /// </summary>
        private void OnSprintHoldCancelled()
        {
            _isSprinting = false;
        }

        #endregion
        
        #region Movement
        
        /// <summary>
        ///     Handle player movement
        /// </summary>
        /// <param name="delta">Time since last frame</param>
        private void Move(float delta)
        {
            // Not moving
            if (!InputManager.IsMoving) return;
            
            var moveDirection = CameraTransform.forward * InputManager.MoveInput.y +
                                CameraTransform.right * InputManager.MoveInput.x;
            moveDirection.y = 0;
            
            // Move and rotate player
            if (canWalkOnGround && groundCheckProvider.IsCheckTrue || canWalkInAir && !groundCheckProvider.IsCheckTrue)
                MovePlayer(moveDirection, delta);
            else // Set player velocity to zero to avoid sliding
                PlayerRb.velocity = new Vector3(0, PlayerRb.velocity.y, 0);
            RotatePlayer(moveDirection, delta);
        }
        
        /// <summary>
        ///     Move the player in the given direction
        /// </summary>
        /// <param name="moveDirection">The direction to move the player</param>
        /// <param name="delta">Time since last frame</param>
        protected virtual void MovePlayer(Vector3 moveDirection, float delta)
        {
            // // Velocity
            // moveDirection *= _isSprinting ? sprintSpeed : walkSpeed;
            // var yVelocity = PlayerRb.velocity.y;
            //
            // // Move player
            // PlayerRb.velocity = Vector3.ProjectOnPlane(moveDirection, Vector3.up);
            // // PlayerRb.AddForce( moveDirection - PlayerRb.velocity, ForceMode.VelocityChange);
            // PlayerRb.velocity += Vector3.up * yVelocity;
            
            var targetSpeed = moveDirection * (_isSprinting ? sprintSpeed : walkSpeed);
            var acceleration = walkSpeed;
            
            var velocity = Vector3.Lerp(PlayerRb.velocity, targetSpeed, acceleration * delta);
            velocity.y = PlayerRb.velocity.y;
            PlayerRb.velocity = velocity;
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

        #region Unity Functions
        
        protected void Awake()
        {
            _isSprinting = false;
            
            // Find components
            if (!animator) animator = GetComponent<Animator>();
            if (!groundCheckProvider) groundCheckProvider = GetComponent<GroundCheckProviderBase>();
        }
        
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            // Move player
            Move(Time.fixedDeltaTime);
            
            // Set animation parameters
            SetAnimationParams(Time.fixedDeltaTime);
        }
        
        #endregion
    }
}