using UnityEngine;

namespace EVOGAMI.Movement.CheckProvider.Ground
{
    public class SphereCastGcp : GroundCheckProviderBase
    {
        [SerializeField] [Tooltip("Radius of the sphere cast")]
        private float groundCheckRadius;
        [SerializeField] [Tooltip("Distance to check for ground")]
        private float groundCheckDistance;

        private void OnDrawGizmos()
        {
            Gizmos.color = IsCheckTrue ? Color.green : Color.red;
            Gizmos.DrawWireSphere(checkTransform.position, groundCheckRadius);
            Gizmos.DrawLine(checkTransform.position,
                checkTransform.position + Vector3.down * groundCheckDistance);
        }

        public override bool Check()
        {
            IsCheckTrue = Physics.SphereCast(
                checkTransform.position,
                groundCheckRadius,
                Vector3.down,
                out CheckHit,
                groundCheckDistance,
                checkLayer
            );
            return IsCheckTrue;
        }
    }
}