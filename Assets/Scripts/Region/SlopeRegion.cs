using UnityEngine;

namespace EVOGAMI.Region
{
    public class SlopeRegion : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!(other.CompareTag("Player") || other.CompareTag("OrigamiMesh"))) return;

            // Unfreeze the rotation of the player
            other.attachedRigidbody.constraints = RigidbodyConstraints.None;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!(other.CompareTag("Player") || other.CompareTag("OrigamiMesh"))) return;

            // Freeze the rotation of the player
            other.attachedRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}