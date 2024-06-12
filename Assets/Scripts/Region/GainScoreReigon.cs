using System;
using System.Collections;
using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.Region
{
    public class GainScoreRegion : RegionBase
    {
        public int score;

        [SerializeField] private GameObject ScoreUi;
        [SerializeField] private GameObject ChangeFormUi;
        [SerializeField] private GameObject CraneCollectedUi;
        
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Collider collider;

        private void Start()
        {
            if (!meshRenderer) meshRenderer = GetComponentInChildren<MeshRenderer>();
            if (!collider) collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            // Only interact with OrigamiMesh
            if (!other.CompareTag("OrigamiMesh")) return;

            PlayerManager.Instance.IncreaseScores();
            
            meshRenderer.enabled = false;
            collider.enabled = false;

            StartCoroutine(ShowCraneCollectedUi());
        }

        private IEnumerator ShowCraneCollectedUi()
        {
            if (ScoreUi) ScoreUi.SetActive(false);
            if (ChangeFormUi) ChangeFormUi.SetActive(false);
            CraneCollectedUi.SetActive(true);

            yield return new WaitForSeconds(2);

            CraneCollectedUi.SetActive(false);
            if (ChangeFormUi) ChangeFormUi.SetActive(true);
            if (ScoreUi) ScoreUi.SetActive(true);
            
            Destroy(gameObject);
        }
    }
}