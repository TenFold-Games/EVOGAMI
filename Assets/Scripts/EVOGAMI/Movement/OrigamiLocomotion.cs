using EVOGAMI.Core;
using EVOGAMI.Origami;
using UnityEngine;

namespace EVOGAMI.Movement
{
    public class OrigamiLocomotion : MonoBehaviour
    {
        // Managers
        protected InputManager _inputManager;
        protected PlayerManager _playerManager;

        // Player
        protected Transform _playerTransform;
        protected Rigidbody _playerRb;
        protected Transform _cameraTransform;
        
        // Movement
        [SerializeField] protected float speed = 5f;
        [SerializeField] protected float rotationSpeed = 10f;
        [SerializeField] protected float jumpForce = 5f;
        [SerializeField] protected float sprintMultiplier = 2f;

        // Ground check
        [SerializeField] protected Transform groundCheck;
        [SerializeField] protected float groundCheckDistance = 0.1f;
        [SerializeField] protected LayerMask groundLayer;
        [SerializeField] protected LayerMask waterLayer;
        
        [SerializeField] protected OrigamiContainer.OrigamiForm form;

        // Flags
        private bool _isGrounded;
        private bool _isSprinting;
        
        // Hit
        private RaycastHit groundHit;
        private RaycastHit waterHit;

        private void Start()
        {
            // Managers
            _playerManager = PlayerManager.Instance;
            _inputManager = InputManager.Instance;

            // Player
            _playerTransform = _playerManager.Player.transform;
            _playerRb = _playerManager.PlayerRb;
            _cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
            
            // Ground check layer
            groundLayer = LayerMask.GetMask("Ground");
            waterLayer = LayerMask.GetMask("Water");
            
            // Register events
            // Move
            _inputManager.OnMoveCancelled += OnMoveCancelled;
            // Jump
            _inputManager.OnJumpPerformed += OnJumpPerformed;
            // Sprint (Hold)
            _inputManager.OnSprintHoldStarted += OnSprintStarted;
            _inputManager.OnSprintHoldCancelled += OnSprintHoldCancelled;
            // Sprint (Press)
            _inputManager.OnSprintPressStarted += OnSprintStarted;
            
        }

        private void FixedUpdate()
        {
            IsGrounded();

            // if (IsWatered() && form != OrigamiContainer.OrigamiForm.Bug)
            // {
            //     PlayerManager.Instance.DecreaseLife();
            //     GameManager.Instance.currentCheckpoint.RespawnPlayer();
            //     return;
            // }

            Move(Time.fixedDeltaTime);
        }

        #region Input Events

        /// <summary>
        ///     Called when the move input is released
        /// </summary>
        private void OnMoveCancelled()
        {
            _isSprinting = false;
        }

        /// <summary>
        ///     Called when the jump input is pressed
        /// </summary>
        private void OnJumpPerformed()
        {
            if (!_isGrounded) return;
            
            _playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        /// <summary>
        ///     Called when the sprint input is started
        /// </summary>
        private void OnSprintStarted()
        {
            // Cannot sprint in the air
            if (!_isGrounded) return;
            
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
            // Player is not moving
            if (!_inputManager.IsMoving) return;
            // Player is not on the ground
            // if (!_isGrounded) return;

            // Calculate move direction
            var moveDirection = _cameraTransform.forward * _inputManager.MoveInput.y +
                                _cameraTransform.right * _inputManager.MoveInput.x;
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
        private void MovePlayer(Vector3 moveDirection, float delta)
        {
            // Velocity
            moveDirection *= speed;
            if (_isSprinting) moveDirection *= sprintMultiplier;
            var yVelocity = _playerRb.velocity.y;
            
            // Move player
            _playerRb.velocity = Vector3.ProjectOnPlane(moveDirection, Vector3.up);
            _playerRb.velocity += Vector3.up * yVelocity;
        }

        /// <summary>
        ///     Rotate the player towards the given direction
        /// </summary>
        /// <param name="moveDirection">The direction to rotate the player towards</param>
        /// <param name="delta">Time since last frame</param>
        private void RotatePlayer(Vector3 moveDirection, float delta)
        {
            var targetRotation = Quaternion.LookRotation(moveDirection);
            targetRotation = Quaternion.Slerp(_playerTransform.rotation, targetRotation, rotationSpeed * delta);
            _playerTransform.rotation = targetRotation;
        }

        #endregion

        #region Collision

        /// <summary>
        ///     Check if the player is grounded
        /// </summary>
        /// <returns>True if the player is grounded, false otherwise</returns>
        private bool IsGrounded()
        {
            _isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, out groundHit, groundCheckDistance, groundLayer);
            return _isGrounded;
        }
        
        private bool IsWatered()
        {
            return Physics.Raycast(groundCheck.position, Vector3.down, out waterHit, groundCheckDistance, waterLayer);
        }

        private void OnDrawGizmos()
        {
            if (groundCheck == null) return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawRay(groundCheck.position, Vector3.down * groundCheckDistance);
        }

        #endregion
    }
}