using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.Region
{
    public class DeadRegion : CallbackRegion
    {
        protected override void OnTriggerEnter(Collider other)
        {
            if (!IsConditionMet(other)) return;

            onRegionEnter.Invoke(other);
            
            PlayerManager.Instance.RespawnAtLastCheckpoint();
        }
    }
}