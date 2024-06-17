using UnityEngine;

namespace EVOGAMI.Movement.CheckProvider.Wall
{
    public class RayCastWcp : WallCheckProviderBase
    {
        // The length of the ray to check for walls.
        [SerializeField] [Tooltip("The length of the ray to check for walls.")]
        private float rayLength;

        private void OnDrawGizmos()
        {
            Gizmos.color = IsCheckTrue ? Color.yellow : Color.magenta;
            Gizmos.DrawLine(checkTransform.position, checkTransform.position + Vector3.down * rayLength);
        }


        public override bool Check()
        {
            IsCheckTrue = Physics.Raycast(
                checkTransform.position,
                checkTransform.forward,
                out CheckHit,
                rayLength,
                checkLayer
            );
            return IsCheckTrue;
        }
    }
}