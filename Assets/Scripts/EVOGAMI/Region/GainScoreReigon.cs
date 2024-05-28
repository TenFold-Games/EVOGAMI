using EVOGAMI.Core;
using EVOGAMI.Origami;
using UnityEngine;

namespace EVOGAMI.Region
{
    public class GainScoreRegion : Region
    {
        public int score;

        private void OnTriggerEnter(Collider other)
        {
            // Only interact with OrigamiMesh
            if (!other.CompareTag("OrigamiMesh")) return;

            PlayerManager.Instance.IncreaseScores();
            Destroy(gameObject);
        }
    }
}