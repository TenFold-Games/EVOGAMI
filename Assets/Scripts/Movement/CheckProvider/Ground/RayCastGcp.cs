using UnityEngine;

namespace EVOGAMI.Movement.CheckProvider.Ground
{
    public class RayCastGcp : GroundCheckProviderBase
    {
        // The length of the ray to check for ground.
        [SerializeField] [Tooltip("The length of the ray to check for ground.")]
        private float rayLength;

        private void OnDrawGizmos()
        {
            Gizmos.color = IsCheckTrue ? Color.green : Color.red;
            Gizmos.DrawLine(checkTransform.position, checkTransform.position + Vector3.down * rayLength);
        }

        public override bool Check()
        {
            IsCheckTrue = Physics.Raycast(
                checkTransform.position,
                Vector3.down,
                out CheckHit,
                rayLength,
                checkLayer
            );
            return IsCheckTrue;
        }
    }
}