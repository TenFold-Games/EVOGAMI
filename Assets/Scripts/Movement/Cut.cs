using System;
using EVOGAMI.Interactable;
using EVOGAMI.Region;
using UnityEngine;

namespace EVOGAMI.Movement
{
    public class Cut : MovementBase
    {
        // The region where the object can be picked up.
        [SerializeField] [Tooltip("The region within which objects can cut.")]
        private CallbackRegion cutRegion;
        
        private Cutable _objectToCut;

        #region Callback Region Events

        private void OnCutTriggerEnter(Collider other)
        {
            // Set the object to cut.
            _objectToCut = other.GetComponent<Cutable>();
        }
        
        private void OnCutTriggerExit(Collider other)
        {
            // Reset the object to cut.
            _objectToCut = null;
        }

        #endregion

        #region Callbacks

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();
            
            // TBD: Start ----------------------------------------------------------------------------------------------
            // TODO: set proper bindings
            InputManager.OnPullExecutePerformed += OnCutPerformed;
            // InputManager.OnInteractionCanceled += OnCutCanceled;
            // TBD: End ------------------------------------------------------------------------------------------------
        }
        
        protected override void UnregisterCallbacks()
        {
            base.UnregisterCallbacks();
            
            // TBD: Start ----------------------------------------------------------------------------------------------
            // TODO: set proper bindings
            InputManager.OnPullExecutePerformed -= OnCutPerformed;
            // InputManager.OnInteractionCanceled -= OnCutCanceled;
            // TBD: End ------------------------------------------------------------------------------------------------
        }

        #endregion
        
        #region Input Events

        private void OnCutPerformed()
        {
            if (_objectToCut == null) return; // No object to cut.
            
            _objectToCut.CutPerformedCallback.Invoke(_objectToCut);
        }
        
        // private void OnCutCanceled()
        // {
        //     _objectToCut.CutCanceledCallback.Invoke(_objectToCut);
        // }
        
        #endregion

        #region Unity Functions

        private void Awake()
        {
            cutRegion.RegionEnterCallback.AddListener(OnCutTriggerEnter);
            cutRegion.RegionExitCallback.AddListener(OnCutTriggerExit);
        }

        #endregion
    }
}