using EVOGAMI.Core;
using EVOGAMI.Origami;
using UnityEngine;

namespace EVOGAMI.Region
{
    public class GainFormRegion : CallbackRegion
    {
        [Header("Settings")]
        // The form to gain when the player enters this region
        [SerializeField] [Tooltip("The form to gain when the player enters this region")]
        public OrigamiContainer.OrigamiForm form;

        protected override void OnTriggerEnter(Collider other)
        {
            if (!IsConditionMet(other)) return;
            
            onRegionEnter.Invoke(other);

            PlayerManager.Instance.GainForm(form);
        }
    }
}