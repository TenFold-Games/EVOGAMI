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
                if (!animator)
                {
                    Debug.LogError("Failed to find Animator component on the human GameObject.");
                }
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            ManageAnimations();
        }

        private void ManageAnimations()
        {
            if (!animator)
            {
                return; // Exit if animator is not assigned
            }

            var isMoving = _playerRb.velocity.magnitude > 0.1f;  // Check if the player is moving
            animator.SetBool("isWalk", isMoving);  // Directly set the 'isWalk' animation parameter
        }
    }
}