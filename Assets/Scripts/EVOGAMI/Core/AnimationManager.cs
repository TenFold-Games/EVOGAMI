using System.Collections.Generic;
using EVOGAMI.Origami;
using UnityEngine;

namespace EVOGAMI.Core
{
    public class AnimationManager : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private void Awake()
        {
            if (!animator)
            {
                animator = GetComponent<Animator>();
            }
        }

        public void SetWalking(bool isWalking)
        {
            animator.SetBool("isWalk", isWalking); // Assuming 'isWalk' is a boolean for simplicity
        }
    }
}


