using System;
using EVOGAMI.Movement;
using EVOGAMI.Region;
using UnityEngine;

namespace EVOGAMI.Interactable
{
    /// <summary>
    ///     Handles objects that can be cut.
    /// </summary>
    public class Cuttable : MonoBehaviour
    {
        [Header("Callbacks")]
        // Event invoked when cut is performed.
        [SerializeField] [Tooltip("Event invoked when cut is performed.")]
        private CutEvent onCutPerformed = new();
        
        /// <summary>
        ///     Event that is invoked when cut is performed.
        /// </summary>
        [Serializable] public class CutEvent : UnityEngine.Events.UnityEvent<Cuttable> {}
        
        /// <summary>
        ///     The callback for when cut is performed.
        /// </summary>
        public CutEvent CutPerformedCallback => onCutPerformed;
    }
}