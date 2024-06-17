using EVOGAMI.Audio;
using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.Region
{
    public class DeadRegion : 
        CallbackRegion,
        ISfxPlayer
    {
        [Header("Audio")]
        // The audio source for the dead region
        [SerializeField] [Tooltip("The audio source for the dead region")]
        private AudioSource audioSource;
        
        protected override void OnTriggerEnter(Collider other)
        {
            if (!IsConditionMet(other)) return;

            PlaySfx(audioSource);
            onRegionEnter.Invoke(other);
            
            PlayerManager.Instance.RespawnAtLastCheckpoint();
        }

        public void PlaySfx(AudioSource sfx)
        {
            sfx.Play();
        }
    }
}