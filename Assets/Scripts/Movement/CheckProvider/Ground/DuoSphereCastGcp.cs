using System;
using UnityEngine;

namespace EVOGAMI.Movement.CheckProvider.Ground
{
    public class DuoSphereCastGcp : GroundCheckProviderBase
    {
        // The second sphere cast transform
        [SerializeField] [Tooltip("The second sphere cast transform")]
        private Transform secondCheckTransform;

        [Header("Sphere Cast Settings")]
        // Radius of the sphere cast
        [SerializeField] [Tooltip("Radius of the sphere cast")]
        private float groundCheckRadius;
        // Distance to check for ground
        [SerializeField] [Tooltip("Distance to check for ground")]
        private float groundCheckDistance;

        public override bool Check()
        {
            var cast1 = Physics.SphereCast(
                checkTransform.position,
                groundCheckRadius,
                Vector3.down,
                out CheckHit,
                groundCheckDistance,
                checkLayer
            );

            var cast2 = Physics.SphereCast(
                secondCheckTransform.position,
                groundCheckRadius,
                Vector3.down,
                out CheckHit,
                groundCheckDistance,
                checkLayer
            );
            
            IsCheckTrue = cast1 || cast2;
            
            return IsCheckTrue;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = IsCheckTrue ? Color.green : Color.red;
            Gizmos.DrawWireSphere(checkTransform.position, groundCheckRadius);
            Gizmos.DrawLine(checkTransform.position,
                checkTransform.position + Vector3.down * groundCheckDistance);
            
            Gizmos.color = IsCheckTrue ? Color.green : Color.red;
            Gizmos.DrawWireSphere(secondCheckTransform.position, groundCheckRadius);
            Gizmos.DrawLine(secondCheckTransform.position,
                secondCheckTransform.position + Vector3.down * groundCheckDistance);
        }
    }
}