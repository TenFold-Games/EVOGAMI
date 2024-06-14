using UnityEngine;

namespace EVOGAMI.Movement.CheckProvider.Ground
{
    public class BoxCastGcp : GroundCheckProviderBase
    {
        // The size of the box to check for ground.
        [SerializeField] [Tooltip("The size of the box to check for ground.")]
        private Vector3 boxSize;
        // The distance to check for ground.
        [SerializeField] [Tooltip("The distance to check for ground.")]
        private float groundCheckDistance;

        private void OnDrawGizmos()
        {
            Gizmos.color = IsCheckTrue ? Color.green : Color.red;
            Gizmos.DrawWireCube(checkTransform.position, boxSize);
            Gizmos.DrawLine(checkTransform.position,
                checkTransform.position + Vector3.down * groundCheckDistance);
        }

        public override bool Check()
        {
            IsCheckTrue = Physics.BoxCast(
                checkTransform.position,
                boxSize,
                Vector3.down,
                out CheckHit,
                checkTransform.rotation,
                groundCheckDistance,
                checkLayer
            );
            return IsCheckTrue;
        }
    }
}