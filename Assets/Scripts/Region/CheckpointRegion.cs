using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.Region
{
    public class CheckpointRegion : CallbackRegion
    {
        [Header("Settings")]
        // The point where the player will respawn at
        [SerializeField] [Tooltip("The point where the player will respawn at.")]
        private Transform respawnPoint;

        protected override void OnTriggerEnter(Collider other)
        {
            if (!IsConditionMet(other)) return;

            GameManager.Instance.CheckpointReached(this);
            
            onRegionEnter.Invoke(other);
        }

        public void RespawnPlayer()
        {
            PlayerManager.Instance.RespawnPlayer(respawnPoint);
        }
    }
}