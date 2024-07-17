using EVOGAMI.Audio;
using EVOGAMI.Core;
using EVOGAMI.Utils;
using FMODUnity;
using UnityEngine;

namespace EVOGAMI.Region
{
    public class CollectableRegion :
        CallbackRegion
    {
        [Header("Display")]
        // The MeshRenderer for the collectable region
        [SerializeField] [Tooltip("The MeshRenderer for the collectable region")]
        private MeshRenderer meshRenderer;
        // The Collider for the collectable region
        [SerializeField] [Tooltip("The Collider for the collectable region")]
        private Collider m_collider;

        [Header("Audio")]
        // The audio source for the collectable region
        [SerializeField] StudioEventEmitter collectableSfx;

        [SerializeField] private float rotationSpeed = 10f;

        protected override void OnTriggerEnter(Collider other)
        {
            if (!IsConditionMet(other)) return;

            onRegionEnter.Invoke(other);
            Disappear();

            PlayerManager.Instance.CraneCollected();
            PlayAudioAndDestroy();
        }

        private void Update()
        {
            // Rotate the object slowly around the Y-axis
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }

        public void PlayAudio()
        {
            collectableSfx.Play();
        }

        private void Disappear()
        {
            meshRenderer.enabled = false;
            m_collider.enabled = false;
        }

        private void PlayAudioAndDestroy()
        {
            PlayAudio();
       
        }
    }
}