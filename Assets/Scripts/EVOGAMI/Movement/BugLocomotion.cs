using Cinemachine;
using EVOGAMI.Origami;
using UnityEngine;
using UnityEngine.Serialization;

namespace EVOGAMI.Movement
{
    public class BugLocomotion : OrigamiLocomotion
    {
        [Header("Wall Check")]
        [SerializeField] private Transform wallCheck;

        [SerializeField] private float wallCheckLength = 0.7f;
        [SerializeField] private float wallCheckRadius = 0.2f;
        [SerializeField] private float maxWallLookAngle = 30f;
        private float _wallLookAngle;
        private LayerMask _wallLayer;
        private RaycastHit _wallHit;
        private bool _isTouchingWall;
        
        [Header("Climbing")]
        [SerializeField] private float climbForce = 25f;
        [SerializeField] private float maxClimbSpeed = 7.5f;
        private bool _isClimbing;

        private void Awake()
        {
            // Set the form
            form = OrigamiContainer.OrigamiForm.Bug;

            // Set the wall layer -- `Wall` and `Ground` layers
            _wallLayer = LayerMask.GetMask("Wall");
        }
        
        private new void FixedUpdate()
        {
            // Check if the player is touching a wall
            WallCheck();
            
            base.FixedUpdate();
            
            if (_inputManager.IsMoving && _isTouchingWall && _wallLookAngle < maxWallLookAngle)
            {
                _isClimbing = true;
                Climb();
            }
            else
            {
                _isClimbing = false;
            }
        }

        protected override void MovePlayer(Vector3 moveDirection, float delta)
        {
            // Velocity
            moveDirection *= speed;
            if (_isClimbing)
            {
                moveDirection *= 0.1f;
            }
            else if (_isSprinting)
            {
                moveDirection *= sprintMultiplier;
            }
            var yVelocity = _playerRb.velocity.y;
            
            // Move player
            _playerRb.velocity = Vector3.ProjectOnPlane(moveDirection, Vector3.up);
            _playerRb.velocity += Vector3.up * yVelocity;
        }

        #region Climbing

        private void Climb()
        {
            // _playerRb.velocity = new Vector3(_playerRb.velocity.x, climbSpeed, _playerRb.velocity.z);
            if (_playerRb.velocity.y < maxClimbSpeed)
                _playerRb.AddForce(Vector3.up * climbForce, ForceMode.Acceleration);
        }
        
        #endregion

        #region Collision
        
        private void WallCheck()
        {
            _isTouchingWall = Physics.SphereCast(wallCheck.position, wallCheckRadius, wallCheck.forward, out _wallHit, wallCheckLength, _wallLayer);
            _wallLookAngle = Vector3.Angle(wallCheck.forward, -_wallHit.normal);
        }
        
        #endregion
    }
}