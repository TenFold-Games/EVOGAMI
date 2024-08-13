using System.Collections;
using EVOGAMI.Interactable;
using EVOGAMI.Region;
using FMODUnity;
using UnityEngine;

namespace EVOGAMI.Movement
{
    public class Cut : MovementBase
    {
        // The region where the object can be picked up.
        [SerializeField] [Tooltip("The region within which objects can cut.")]
        private CallbackRegion cutRegion;
        
        private Cuttable _objectToCut;

        [SerializeField] StudioEventEmitter sfxcrabcut;

        #region Callback Region Events

        private void OnCutTriggerEnter(Collider other)
        {
            // Disable highlight on the previous object
            _objectToCut?.DisableHighlight();

            // Set the object to cut.
            _objectToCut = other.GetComponent<Cuttable>();

            // Enable highlight on the new object
            _objectToCut?.EnableHighlight(0.5f);
        }
        
        private void OnCutTriggerExit(Collider other)
        {
            _objectToCut?.DisableHighlight();

            // Reset the object to cut.
            _objectToCut = null;
        }

        #endregion

        #region Callbacks

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();
            
            InputManager.OnInteractPerformed += OnCutPerformed;
        }
        
        protected override void UnregisterCallbacks()
        {
            base.UnregisterCallbacks();
            
            InputManager.OnInteractPerformed -= OnCutPerformed;
        }

        #endregion
        
        #region Input Events

        private void OnCutPerformed()
        {
            sfxcrabcut.Play();

            if (_objectToCut == null) return; // No object to cut.
            
            InputManager.VibrateController(0.05f, 0.05f, 0.025f);
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

        public void ResetObjectToCut()
        {
            _objectToCut?.DisableHighlight();
            _objectToCut = null;
        }
    }
}