using Unity.Mathematics;
using UnityEditor.Presets;
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
        
        private float throttle;
        // Pitching is the rotation of the plane around the lateral axis.
        private float pitch;
        // Rolling is the rotation of the plane around the longitudinal axis.
        private float roll;
        // Yawing is the rotation of the plane around the vertical axis.
        private float yaw;
        
        private float ResponseModifier => PlayerRb.mass / 10 * responsiveness;
        
        protected override void Start()
        {
            base.Start();
            
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
            // GroundCheck();
            ReadInput();
            
            Move(Time.fixedDeltaTime);
        }

        #region Movement

        protected override void Move(float delta)
        {
            // Plane cannot move when grounded
            if (IsGrounded) return;
            
            PlayerRb.AddForce(transform.forward * (maxThrust * throttle));
            
            PlayerRb.AddTorque(PlayerTransform.up * (yaw * ResponseModifier));
            PlayerRb.AddTorque(PlayerTransform.right * (pitch * ResponseModifier));
            PlayerRb.AddTorque(-PlayerTransform.forward * (roll * ResponseModifier));
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
    }
}