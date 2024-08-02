using EVOGAMI.Audio;
using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.Region
{
    public class DeadRegion :
        CallbackRegion,
        IAudioPlayer
    {
        [Header("Audio")]
        // The audio source for the dead region
        [SerializeField] [Tooltip("The audio source for the dead region")]
        private AudioSource audioSource;

        protected override void OnTriggerEnter(Collider other)
        {
            if (!IsConditionMet(other)) return;

            PlayAudio(audioSource);
            onRegionEnter.Invoke(other);

            GameManager.Instance.PlayerDied();
        }

        public void PlayAudio(AudioSource sfx)
        {
            sfx.Play();
        }
    }
}