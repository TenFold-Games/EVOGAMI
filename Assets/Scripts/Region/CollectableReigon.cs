using EVOGAMI.Audio;
using EVOGAMI.Core;
using EVOGAMI.Utils;
using UnityEngine;

namespace EVOGAMI.Region
{
    public class CollectableRegion :
        CallbackRegion,
        IAudioPlayer
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
        [SerializeField] [Tooltip("The audio source for the collectable region")]
        private AudioSource audioSource;

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

        public void PlayAudio(AudioSource sfx)
        {
            sfx.Play();
        }

        private void Disappear()
        {
            meshRenderer.enabled = false;
            m_collider.enabled = false;
        }

        private void PlayAudioAndDestroy()
        {
            PlayAudio(audioSource);
            StartCoroutine(CoroutineUtils.DelayAction(audioSource.clip.length, () =>
            {
                audioSource.Stop();
                Destroy(gameObject);
            }));
        }
    }
}