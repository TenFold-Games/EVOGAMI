using System.Collections;
using EVOGAMI.Audio;
using EVOGAMI.Core;
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

        protected override void OnTriggerEnter(Collider other)
        {
            if (!IsConditionMet(other)) return;

            onRegionEnter.Invoke(other);
            Disappear();

            PlayerManager.Instance.CraneCollected();
            StartCoroutine(PlayAudioAndDestroy());
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

        private IEnumerator PlayAudioAndDestroy()
        {
            PlayAudio(audioSource);
            yield return new WaitForSeconds(audioSource.clip.length);
            audioSource.Stop();
            Destroy(gameObject);
        }
    }
}