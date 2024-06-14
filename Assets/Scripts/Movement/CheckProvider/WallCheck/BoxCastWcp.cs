using UnityEngine;

namespace EVOGAMI.Movement.CheckProvider.WallCheck
{
    public class BoxCastWcp : WallCheckProviderBase
    {
        // The size of the box to check for walls.
        [SerializeField] [Tooltip("The size of the box to check for walls.")]
        private Vector3 boxSize;
        // The distance to check for walls.
        [SerializeField] [Tooltip("The distance to check for walls")]
        private float wallCheckDistance;

        private void OnDrawGizmos()
        {
            Gizmos.color = IsCheckTrue ? Color.yellow : Color.magenta;
            Gizmos.DrawWireCube(checkTransform.position, boxSize);
            Gizmos.DrawLine(checkTransform.position,
                checkTransform.position + Vector3.down * wallCheckDistance);
        }

        public override bool Check()
        {
            IsCheckTrue = Physics.BoxCast(
                checkTransform.position,
                boxSize,
                checkTransform.forward,
                out CheckHit,
                checkTransform.rotation,
                wallCheckDistance,
                checkLayer
            );
            return IsCheckTrue;
        }
    }
}