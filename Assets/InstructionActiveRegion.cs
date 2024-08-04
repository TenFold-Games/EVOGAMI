using EVOGAMI.Core;
using EVOGAMI.Origami;
using UnityEngine;
using EVOGAMI.Utils;
using EVOGAMI.Audio;

namespace EVOGAMI.Region
{
    public class InstructionActiveRegion :
        CallbackRegion
    {

        [Header("Display")]
        [SerializeField]
        [Tooltip("The Collider for the collectable region")]
        private Collider m_collider;

 

        private void Update()
        {

        }

        private void Disappear()
        {
            m_collider.enabled = false;
        }




        [Header("Target")]
        // The UI to active when the player enters this region
        [SerializeField]
        [Tooltip("The UI to activate when the player enters this region")]
        private GameObject uiObject;

        protected override void OnTriggerEnter(Collider other)
        {
            if (!IsConditionMet(other)) return;

            onRegionEnter.Invoke(other);
            Disappear();

            if (uiObject != null)
            {
                uiObject.SetActive(true);
            }
            Destroy(gameObject);

        }
    }

}