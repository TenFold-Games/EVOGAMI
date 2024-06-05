using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.Region
{
    public class DeadRegion : RegionBase
    {
        private void OnTriggerEnter(Collider other)
        {
            // Only interact with OrigamiMesh
            if (!other.CompareTag("OrigamiMesh")) return;

            Debug.Log("Dead region entered");
            PlayerManager.Instance.DecreaseLife();
            PlayerManager.Instance.RespawnPlayer(GameManager.Instance.currentCheckpoint.transform);
        }
    }
}