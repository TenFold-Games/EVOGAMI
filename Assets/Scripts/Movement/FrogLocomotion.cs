using EVOGAMI.Animations;
using UnityEngine;

namespace EVOGAMI.Movement
{
    public class FrogLocomotion : LocomotionBase, IAnimHandler
    {
        [SerializeField] private Animator animator;
        
        [SerializeField] protected Transform groundCheckBack;
        protected RaycastHit GroundHitBack;
        
        protected override void Start()
        {
            base.Start();
            
            if (!animator)
                animator = GetComponent<Animator>();
        }
        
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            SetAnimParams(Time.deltaTime);
        }

        #region Movement

        protected override void MovePlayer(Vector3 moveDirection, float delta)
        {
            // Frog can only jump
            if (IsGrounded) return;
            
            base.MovePlayer(moveDirection, delta);
        }

        #endregion

        #region Collision

        protected override void GroundCheck()
        {
            base.GroundCheck();
            IsGrounded |= Physics.Raycast(groundCheckBack.position, Vector3.down, out GroundHitBack, groundCheckDistance, GroundLayer);
        }

        #endregion

        public void SetAnimParams(float delta)
        {
            float horizontal = InputManager.MoveInput.magnitude;
            float vertical = PlayerManager.PlayerRb.velocity.y;
            
            animator.SetFloat("vertical", vertical, 0.1f, delta);
            animator.SetFloat("horizontal", horizontal, 0.1f, delta);
        }
    }
}