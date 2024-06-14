using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.Region
{
    public class DeadRegion : CallbackRegion
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!IsConditionMet(other)) return;

            PlayerManager.Instance.RespawnPlayer(GameManager.Instance.CurrentCheckpoint.transform);
        }
    }
}