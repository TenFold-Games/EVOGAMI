using EVOGAMI.Core;
using EVOGAMI.Origami;
using EVOGAMI.Utils;
using UnityEngine;

namespace EVOGAMI.Region
{
    public class GainFormRegion : CallbackRegion
    {
        [Header("Display")]
        // The MeshRenderer for the collectable region
        [SerializeField]
        [Tooltip("The MeshRenderer for the collectable region")]
        private MeshRenderer meshRenderer;

        // The Collider for the collectable region
        [SerializeField] [Tooltip("The Collider for the collectable region")]
        private Collider m_collider;

        // The speed that the object rotates at
        [SerializeField] [Tooltip("The speed that the object rotates at")]
        private float rotationSpeed = 10f;

        [Header("Settings")]
        // The form to gain when the player enters this region
        [SerializeField]
        [Tooltip("The form to gain when the player enters this region")]
        public OrigamiContainer.OrigamiForm form;

        [Header("Particle Effect")]
        // Reference to the Particle Effect Prefab
        [SerializeField]
        [Tooltip("Reference to the Particle Effect Prefab")]
        private GameObject particleEffectPrefab;

        #region Unity Functions

        private void Update()
        {
            // Rotate the object slowly around the Y-axis
            transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
        }

        #endregion


        #region Callback Events

        protected override void OnTriggerEnter(Collider other)
        {
            if (!IsConditionMet(other)) return;

            Debug.Log("Trigger entered by: " + other.name);

            onRegionEnter.Invoke(other);
            // Particle effect
            var particleEffect =
                Instantiate(particleEffectPrefab, transform.position, Quaternion.identity); // Play particle effect
            particleEffect.GetComponent<ParticleSystem>().Play(); // Ensure it plays

            Disappear();

            PlayerManager.Instance.GainForm(form);
            // PlayAudioAndDestroy();
            DelayedDestroy();

            Debug.Log("Particle effect and form gain completed");
        }

        #endregion

        // private void PlayAudioAndDestroy()
        // {
        //     PlayAudio(audioSource);
        //     StartCoroutine(CoroutineUtils.DelayAction(audioSource.clip.length, () =>
        //     {
        //         audioSource.Stop();
        //         Destroy(gameObject);
        //     }));
        // }

        private void DelayedDestroy()
        {
            StartCoroutine(CoroutineUtils.DelayAction(1f, () => { Destroy(gameObject); }));
        }

        private void Disappear()
        {
            meshRenderer.enabled = false;
            m_collider.enabled = false;
        }
    }
}