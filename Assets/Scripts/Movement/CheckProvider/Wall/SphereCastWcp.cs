using UnityEngine;

namespace EVOGAMI.Movement.CheckProvider.Wall
{
    public class SphereCastWcp : WallCheckProviderBase
    {
        // Radius of the sphere cast
        [SerializeField] [Tooltip("Radius of the sphere cast")]
        private float wallCheckRadius;
        // Distance to check for walls
        [SerializeField] [Tooltip("Distance to check for walls")]
        private float wallCheckDistance;

        private void OnDrawGizmos()
        {
            Gizmos.color = IsCheckTrue ? Color.yellow : Color.magenta;
            Gizmos.DrawWireSphere(checkTransform.position, wallCheckRadius);
            Gizmos.DrawLine(checkTransform.position,
                checkTransform.position + checkTransform.forward * wallCheckDistance);
        }

        public override bool Check()
        {
            IsCheckTrue = Physics.SphereCast(
                checkTransform.position,
                wallCheckRadius,
                checkTransform.forward,
                out CheckHit,
                wallCheckDistance,
                checkLayer
            );
            return IsCheckTrue;
        }
    }
}