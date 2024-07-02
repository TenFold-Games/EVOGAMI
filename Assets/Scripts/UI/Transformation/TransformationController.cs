using System;
using System.Collections.Generic;
using EVOGAMI.Core;
using EVOGAMI.Custom.Enums;
using EVOGAMI.Custom.Serializable;
using EVOGAMI.Origami;
using EVOGAMI.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace EVOGAMI.UI.Transformation
{
    public class TransformationController : CallbackMonoBehaviour
    {
        [Header("Recipe Processing")]
        // The recipes (transformation sequences) for each form.
        [SerializeField] [Tooltip("The recipes (transformation sequences) for each form.")]
        private OriSeqMapping[] recipes;
        // Event invoked when a sequence input is read.
        [SerializeField] [Tooltip("Event invoked when a sequence input is read.")]
        private SequenceReadEvent onSequenceRead;
        // Event invoked when the sequence is broken.
        [SerializeField] [Tooltip("Event invoked when the sequence is broken.")]
        private SequenceBreakEvent onSequenceBreak;
        // Event invoked when the transformation is completed.
        [SerializeField] [Tooltip("Event invoked when the transformation is completed.")]
        private SequenceCompleteEvent onSequenceComplete;
        // The length of the transformation sequence.
        [SerializeField] [Tooltip("The length of the transformation sequence.")]
        public int sequenceLength = 3;
        
        [Header("Origami")]
        [SerializeField] [Tooltip("The origami container.")]
        private OrigamiContainer origamiContainer;

        /// <summary>
        ///     The event that is triggered when a sequence input is read.
        /// </summary>
        [Serializable] public class SequenceReadEvent : UnityEvent<string> {}
        /// <summary>
        ///     The event that is triggered when the sequence is broken.
        /// </summary>
        [Serializable] public class SequenceBreakEvent : UnityEvent<string> {}
        /// <summary>
        ///     The event that is triggered when the transformation is completed.
        /// </summary>
        [Serializable] public class SequenceCompleteEvent : UnityEvent<OrigamiContainer.OrigamiForm> {}
        
        /// <summary>
        ///     The callback for when a sequence input is read.
        /// </summary>
        public SequenceReadEvent SequenceReadCallback => onSequenceRead;
        /// <summary>
        ///     The callback for when the sequence is broken.
        /// </summary>
        public SequenceBreakEvent SequenceBreakCallback => onSequenceBreak;
        /// <summary>
        ///     The callback for when the transformation is completed.
        /// </summary>
        public SequenceCompleteEvent SequenceCompleteCallback => onSequenceComplete;

        private SequenceMatcher<OrigamiContainer.OrigamiForm> _sequenceMatcher;
        private readonly Dictionary<string, OrigamiContainer.OrigamiForm> _seqFormMapping = new();
        private Dictionary<OrigamiContainer.OrigamiForm, bool> _unlockStatus = new();

        public void Reset()
        {
            _sequenceMatcher.ClearBuffer();
        }

        #region Callbacks

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();
            
            InputManager.OnSequencePerformed += OnSequencePerformed;
        }
        
        protected override void UnregisterCallbacks()
        {
            base.UnregisterCallbacks();
            
            InputManager.OnSequencePerformed  -= OnSequencePerformed;
        }

        #endregion

        #region Input Events

        private void OnSequencePerformed(Directions direction)
        {
            _sequenceMatcher.OnSequencePerformed(
                direction.ToString()[0], 
                s => _unlockStatus[_seqFormMapping[s]]
            );
        }

        #endregion

        #region Unity Functions

        private void Awake()
        {    
            // Initialize the sequence matcher.
            _sequenceMatcher = new SequenceMatcher<OrigamiContainer.OrigamiForm>(sequenceLength);
            foreach (var recipe in recipes)
                _sequenceMatcher.AddRecipe(recipe.ToString());
            
            // Build sequence (string) to form mapping.
            foreach (var recipe in recipes)
                _seqFormMapping.Add(recipe.ToString(), recipe.form);
            
            _sequenceMatcher.OnSequenceRead += sequence =>
            {
                onSequenceRead.Invoke(sequence);
            };
            _sequenceMatcher.OnSequenceBreak += sequence =>
            {
                onSequenceBreak.Invoke(sequence);
                _sequenceMatcher.ClearBuffer();
            };
            _sequenceMatcher.OnSequenceComplete += sequence =>
            {
                onSequenceComplete.Invoke(_seqFormMapping[sequence]);
                _sequenceMatcher.ClearBuffer();
                origamiContainer.ChangeForm(_seqFormMapping[sequence]);
            };
        }

        protected override void Start()
        {
            base.Start();
            
            origamiContainer ??= PlayerManager.Instance.Player.GetComponent<OrigamiContainer>();
            Debug.Assert(origamiContainer != null, "Origami container is not set.");

            // Load form unlock states
            foreach (var (form, isUnlocked) in GameManager.Instance.origamiSettings.formUnlockStates)
                _unlockStatus.Add(form, isUnlocked);
        }
        
        #endregion
    }
}