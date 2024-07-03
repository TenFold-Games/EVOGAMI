// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace EVOGAMI
// {
//     public class TongueController : MonoBehaviour
//     {
//
//         public void Shoot()
//         {
//
//         }
//         // Start is called before the first frame update
//         void Start()
//         {
//         
//         }
//
//         // Update is called once per frame
//         void Update()
//         {
//         
//         }
//     }
// }

using System;
using UnityEngine;

namespace EVOGAMI.Animations
{
    public class TongueController : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("The object containing the tongue.")]
        [SerializeField] private GameObject tongueObject;
        
        private Animator tongueAnimator;

        private static readonly int IsPull = Animator.StringToHash("isPull");

        private void Awake()
        {
            if (tongueObject == null)
            {
                Debug.LogError("Tongue object is not assigned.");
                return;
            }

            tongueAnimator = tongueObject.GetComponent<Animator>();

            if (tongueAnimator == null)
            {
                Debug.LogError("Animator component is not found on the tongue object.");
            }
        }

        public void SetPullState(bool isPull)
        {
            if (tongueAnimator != null)
            {
                tongueAnimator.SetBool(IsPull, isPull);
            }
        }
    }
}
