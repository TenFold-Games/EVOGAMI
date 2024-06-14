using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.Region
{
    public class CollectableRegion : CallbackRegion
    {
        protected override void OnTriggerEnter(Collider other)
        {
            if (!IsConditionMet(other)) return;
            
            onRegionEnter.Invoke(other);
            
            PlayerManager.Instance.CraneCollected();
            Destroy(gameObject);
        }
    }
}