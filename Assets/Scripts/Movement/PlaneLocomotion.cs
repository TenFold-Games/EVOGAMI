using UnityEngine;

namespace EVOGAMI.Movement
{
    public class PlaneLocomotion : LocomotionBase
    {
        [Header("Plane")] 
        
        [Tooltip("The plane's throttle increment value.")] 
        [SerializeField] private float throttleIncrement = 10f;
        
        [Tooltip("Maximum engine thrust at full throttle.")] 
        [SerializeField] private float maxThrust = 200f;
        
        [Tooltip("The plane's responsiveness to pitch, roll, and yaw.")] 
        [SerializeField] private float responsiveness = 1f;

        [Tooltip("The plane's lift force.")] [SerializeField]
        private float lift = 135f;
        
        [Tooltip("The plane's throttle.")] 
        [Range(0, 100)]
        [SerializeField] private float throttle = 0f;

        // Pitching is the rotation of the plane around the lateral axis.
        private float pitch;
        // Rolling is the rotation of the plane around the longitudinal axis.
        private float roll;
        // Yawing is the rotation of the plane around the vertical axis.
        private float yaw;

        private bool isLaunched;

        private float ResponseModifier => PlayerRb.mass / 10 * responsiveness;

        #region Input Events

        protected override void OnJumpPerformed()
        {
            if (!IsGrounded) return;

            PlayerRb.AddForce(Vector3.up * (jumpForce * PlayerRb.mass), ForceMode.Impulse);
            isLaunched = true;
            
            // PlayerRb.AddForce(PlayerTransform.forward * maxThrust, ForceMode.Impulse);
        }

        #endregion

        #region Unity Functions

        protected override void Start()
        {
            base.Start();

            isLaunched = true;

            InputManager.Controls.Plane.ThrottleUp.performed += _ =>
            {
                throttle += throttleIncrement;
                throttle = Mathf.Clamp(throttle, 0, 100f);
            };
            InputManager.Controls.Plane.ThrottleDown.performed += _ =>
            {
                throttle -= throttleIncrement;
                throttle = Mathf.Clamp(throttle, 0, 100f);
            };
        }

        private void ReadInput()
        {
            pitch = InputManager.MoveInput.y;
            roll = InputManager.MoveInput.x;
            yaw = InputManager.PlaneYawInput;
        }

        protected override void FixedUpdate()
        {
            GroundCheck();
            ReadInput();

            Move(Time.fixedDeltaTime);
            
            isLaunched = !IsGrounded;
        }

        #endregion

        #region Movement

        protected override void Move(float delta)
        {
            // Plane cannot move when grounded
            // if (IsGrounded) return;

            if (isLaunched)
            {
                PlayerRb.AddForce(Vector3.up * (PlayerRb.velocity.magnitude * lift));
            
                PlayerRb.AddForce(PlayerTransform.forward * (maxThrust * throttle));
                
                PlayerRb.AddTorque(PlayerTransform.up * (yaw * ResponseModifier));
                PlayerRb.AddTorque(PlayerTransform.right * (pitch * ResponseModifier));
                PlayerRb.AddTorque(-PlayerTransform.forward * (roll * ResponseModifier));
            }
        }

        protected override void MovePlayer(Vector3 moveDirection, float delta)
        {
            // base.MovePlayer(moveDirection, delta);
        }

        protected override void RotatePlayer(Vector3 moveDirection, float delta)
        {
            // base.RotatePlayer(moveDirection, delta);
        }

        #endregion

        #region Collision

        protected override void GroundCheck()
        {
            IsGrounded = Physics.SphereCast(
                groundCheck.position,
                groundCheckRadius,
                -PlayerTransform.up,
                out GroundHit,
                groundCheckDistance,
                GroundLayer
            );
        }
        
        protected override void OnDrawGizmos()
        {
            if (groundCheck == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            Gizmos.DrawLine(groundCheck.position, groundCheck.position - transform.forward * groundCheckDistance);
        }

        #endregion
    }
}