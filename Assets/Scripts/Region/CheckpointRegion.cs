using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.Region
{
    public class CheckpointRegion : RegionBase
    {
        [SerializeField] private Transform respawnPoint;

        private void OnTriggerEnter(Collider other)
        {
            // Only interact with OrigamiMesh
            if (!other.CompareTag("OrigamiMesh")) return;

            GameManager.Instance.CheckpointReached(this);
        }

        public void RespawnPlayer()
        {
            PlayerManager.Instance.RespawnPlayer(respawnPoint);
        }
    }
}