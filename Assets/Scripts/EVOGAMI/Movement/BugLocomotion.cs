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

        [SerializeField] private float wallCheckLength = 0.3f;
        [SerializeField] private float wallCheckRadius = 0.2f;
        [SerializeField] private LayerMask wallLayer;
        private RaycastHit _wallHit;
        private bool _isTouchingWall;
        
        [Header("Climbing")]
        [Range(0, 1)]
        [SerializeField] private float climbSpeed = .75f;

        private void Awake()
        {
            // Set the form
            form = OrigamiContainer.OrigamiForm.Bug;

            // Set the wall layer
            if (wallLayer == 0)
                wallLayer = LayerMask.GetMask("Wall");
        }
        
        protected override void FixedUpdate()
        {
            GroundCheck();
            WallCheck();

            if (_isTouchingWall)
            {
                PlayerRb.useGravity = false;
                Climb(Time.fixedDeltaTime);
            }
            else
            {
                PlayerRb.useGravity = true;
                Move(Time.fixedDeltaTime);
            }
        }

        #region Input Events

        protected override void OnJumpPerformed()
        {
            if (_isTouchingWall)
                WallJump(0.75f);
            else
                base.OnJumpPerformed();
        }

        #endregion
        
        #region Climbing

        /// <summary>
        ///     Climb on the wall
        /// </summary>
        /// <param name="delta">The time between frames</param>
        private void Climb(float delta)
        {
            PlayerRb.useGravity = false;
            
            // Climb on the wall
            var moveDirection = InputManager.MoveInput.y * Vector3.up +
                                InputManager.MoveInput.x * wallCheck.right;
            moveDirection *= climbSpeed;
            moveDirection = Vector3.ProjectOnPlane(moveDirection, _wallHit.normal);
            
            // Move player on the wall
            PlayerRb.velocity = moveDirection * speed;
            
            // Make the player look at the wall
            var lookDirection = Vector3.ProjectOnPlane(-_wallHit.normal, Vector3.up);
            PlayerTransform.forward = lookDirection;
        }
        
        /// <summary>
        ///     Jump from the wall
        /// </summary>
        /// <param name="upMultiplier"></param>
        private void WallJump(float upMultiplier)
        {
            var wallJumpForce = Vector3.up * (jumpForce * upMultiplier) +
                                _wallHit.normal * (jumpForce / 2);
            PlayerRb.AddForce(wallJumpForce, ForceMode.Impulse);
        }
        
        #endregion

        #region Collision
        
        /// <summary>
        ///     Check if the player is touching a wall
        /// </summary>
        private void WallCheck()
        {
            _isTouchingWall = Physics.SphereCast(wallCheck.position, wallCheckRadius, wallCheck.forward, out _wallHit, wallCheckLength, wallLayer);
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + wallCheck.forward * wallCheckLength);
        }

        #endregion
    }
}