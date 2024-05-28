using UnityEngine;

namespace EVOGAMI.Movement
{
    public class HumanLocomotion : OrigamiLocomotion
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
            ManageAnimations();
        }

        private void ManageAnimations()
        {
            animator.SetBool("isWalk", _inputManager.IsMoving);  // Directly set the 'isWalk' animation parameter
        }
    }
}