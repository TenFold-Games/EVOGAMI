using UnityEngine;

namespace EVOGAMI.Movement
{
    public class HumanLocomotion : LocomotionBase
    {
        [SerializeField] private Animator animator;  // Ensure this is assigned in the Unity Editor

        protected override void Start()
        {
            base.Start();
            if (!animator)
            {
                animator = GetComponent<Animator>();
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            ManageAnimations(Time.deltaTime);
        }

        private void ManageAnimations(float delta)
        {
            // animator.SetBool("isWalk", InputManager.IsMoving);  // Directly set the 'isWalk' animation parameter
            float horizontal = InputManager.MoveInput.magnitude;
            float vertical = PlayerManager.PlayerRb.velocity.y; 
            
            animator.SetFloat("vertical", vertical, 0.1f, delta);
            animator.SetFloat("horizontal", horizontal, 0.1f, delta);
            
        }
    }
}