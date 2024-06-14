using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.Region
{
    public class GainScoreRegion : CallbackRegion
    {
        private void OnTriggerEnter(Collider other)
        {
            // Only interact with OrigamiMesh
            if (!other.CompareTag("OrigamiMesh")) return;

            PlayerManager.Instance.CraneCollected();
            
            // Destroy the collectible
            Destroy(gameObject);
        }
    }
}