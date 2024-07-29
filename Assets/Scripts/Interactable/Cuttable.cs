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
        [SerializeField]
        [Tooltip("Event invoked when cut is performed.")]
        private CutEvent onCutPerformed = new();

        /// <summary>
        ///     Event that is invoked when cut is performed.
        /// </summary>
        [Serializable] public class CutEvent : UnityEngine.Events.UnityEvent<Cuttable> { }

        /// <summary>
        ///     The callback for when cut is performed.
        /// </summary>
        public CutEvent CutPerformedCallback => onCutPerformed;

        // Outline component for highlighting
        private Outline _outline;

        void Start()
        {
            _outline = GetComponent<Outline>();
            if (_outline != null)
            {
                _outline.enabled = false; // Ensure the outline is initially disabled
            }
        }

        public void EnableHighlight()
        {
            if (_outline != null)
            {
                _outline.enabled = true; // Enable the outline
            }
        }

        public void DisableHighlight()
        {
            if (_outline != null)
            {
                _outline.enabled = false; // Disable the outline
            }
        }
    }
}
