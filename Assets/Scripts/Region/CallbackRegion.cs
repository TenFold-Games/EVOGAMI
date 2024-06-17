using System;
using System.Linq;
using EVOGAMI.Custom.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace EVOGAMI.Region
{
    /// <summary>
    ///     A region that invokes callbacks when a collider enters or exits the area.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class CallbackRegion : MonoBehaviour
    {
        [Header("Callbacks")]
        // Event invoked when a collider enters the area.
        [SerializeField] [Tooltip("Event invoked when a collider enters the area.")]
        protected RegionEnterEvent onRegionEnter = new();
        // Event invoked when a collider exits the area.
        [SerializeField] [Tooltip("Event invoked when a collider exits the area.")]
        protected RegionExitEvent onRegionExit = new();
        
        /// <summary>
        ///     Event that is invoked when a collider enters the trigger.
        /// </summary>
        [Serializable] public class RegionEnterEvent : UnityEvent<Collider> {}
        /// <summary>
        ///     Event that is invoked when a collider exits the trigger.
        /// </summary>
        [Serializable] public class RegionExitEvent : UnityEvent<Collider> {}

        /// <summary>
        ///     The callback for when a collider enters the trigger.
        /// </summary>
        public RegionEnterEvent RegionEnterCallback => onRegionEnter;
        /// <summary>
        ///     The callback for when a collider exits the trigger.
        /// </summary>
        public RegionExitEvent RegionExitCallback => onRegionExit;

        [Header("Region Conditions")] 
        [SerializeField] [TagSelector] [Tooltip("The tags that the collider must have to trigger the callback.")]
        private string[] tags = { };
        [SerializeField] [Tooltip("The layer mask that the collider must be in to trigger the callback.")]
        private LayerMask layerMask = 0;

        private MeshRenderer _meshRenderer;

        /// <summary>
        ///     Check if the condition is met for the callback to be invoked.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        /// <returns>True if the condition is met, false otherwise.</returns>
        protected bool IsConditionMet(Collider other)
        {
            // Condition for layer mask
            if (layerMask != 0)
                if ((layerMask & (1 << other.gameObject.layer)) == 0) return false;

            // Condition for tags
            if (tags.Length > 0)
                if (!tags.Contains(other.tag)) return false;
            
            return true;
        }

        #region Unity Functions

        protected virtual void Awake()
        {
            // Hide the mesh renderer if it exists.
            _meshRenderer = GetComponent<MeshRenderer>();
            if (_meshRenderer != null) _meshRenderer.enabled = false;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!IsConditionMet(other)) return;

            onRegionEnter.Invoke(other);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (!IsConditionMet(other)) return;
            
            onRegionExit.Invoke(other);
        }

        #endregion
    }
}