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
            // groundLayer = LayerMask.GetMask("Ground");
        }

        private void FixedUpdate()
        {
            IsGrounded();
            if (IsWatered() && form != OrigamiContainer.OrigamiForm.Boat)
            {
                PlayerManager.Instance.DecreaseLife();
                GameManager.Instance.currentCheckpoint.RespawnPlayer();
                return;
            }
            Move(Time.fixedDeltaTime);
            Jump(Time.fixedDeltaTime);
        }

        #region Movement

        /// <summary>
        ///     Handle player movement
        /// </summary>
        /// <param name="delta">Time since last frame</param>
        private void Move(float delta)
        {
            // Player is not moving
            if (!_inputManager.IsMoving())
            {
                _isSprinting = false;
                return;
            }
            // Player is not on the ground
            // if (!_isGrounded) return;

            // Calculate move direction
            var moveDirection = _cameraTransform.forward * _inputManager.MoveInput.y +
                                _cameraTransform.right * _inputManager.MoveInput.x;
            moveDirection.y = 0;
            // moveDirection.Normalize();
            
            // Update sprint flag
            _isSprinting |= _inputManager.SprintInput;

            // Move and rotate player
            MovePlayer(moveDirection, delta);
            RotatePlayer(moveDirection, delta);
        }

        /// <summary>
        ///     Handle player jumping
        /// </summary>
        /// <param name="delta">Time since last frame</param>
        private void Jump(float delta)
        {
            if (!_inputManager.JumpInput || !_isGrounded) return;

            _playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
            // TODO: Calculate normal vector
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
            _isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, groundLayer);
            return _isGrounded;
        }
        
        private bool IsWatered()
        {
            return Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, waterLayer);
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