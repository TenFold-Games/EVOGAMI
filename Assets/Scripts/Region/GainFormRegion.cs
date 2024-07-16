using EVOGAMI.Core;
using EVOGAMI.Origami;
using UnityEngine;
using EVOGAMI.Utils;
using EVOGAMI.Audio;

namespace EVOGAMI.Region
{
    public class GainFormRegion : 
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
        
        [SerializeField] private float rotationSpeed = 10f;
        
        [Header("Audio")]
        // The audio source for the collectable region
        [SerializeField] [Tooltip("The audio source for the collectable region")]
        private AudioSource audioSource;
        public void PlayAudio(AudioSource sfx)
        {
            sfx.Play();
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
        
        private void Update()
        {
            // Rotate the object slowly around the Y-axis
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        
        private void Disappear()
        {
            meshRenderer.enabled = false;
            m_collider.enabled = false;
        }



    
        [Header("Settings")]
        // The form to gain when the player enters this region
        [SerializeField] [Tooltip("The form to gain when the player enters this region")]
        public OrigamiContainer.OrigamiForm form;

        protected override void OnTriggerEnter(Collider other)
        {
            if (!IsConditionMet(other)) return;
            
            onRegionEnter.Invoke(other);
            Disappear();

            PlayerManager.Instance.GainForm(form);
            PlayAudioAndDestroy();

        }
    }
    
}