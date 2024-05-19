using EVOGAMI.Core;
using EVOGAMI.Origami;
using UnityEngine;

namespace EVOGAMI.Region
{
    public class GainFormRegion : Region
    {
        public OrigamiContainer.OrigamiForm form;

        private void OnTriggerEnter(Collider other)
        {
            // Only interact with OrigamiMesh
            if (!other.CompareTag("OrigamiMesh")) return;

            PlayerManager.Instance.GainForm(form);
        }
    }
}