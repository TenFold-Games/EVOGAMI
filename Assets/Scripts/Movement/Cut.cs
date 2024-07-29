using EVOGAMI.Core;
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
            // if (_objectToCut != null)
            // {
            //     _objectToCut.DisableHighlight(); // Disable highlight on the previous object
            // }
            // Set the object to cut.
            _objectToCut = other.GetComponent<Cuttable>();

            if (_objectToCut != null)
            {
                _objectToCut.EnableHighlight(0.5f); // Enable highlight on the new object
            }
        }
        
        private void OnCutTriggerExit(Collider other)
        {
            if (_objectToCut != null)
            {
                _objectToCut.DisableHighlight();
            }
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
            InputManager.OnInteractPerformed += OnCutPerformed;
            // InputManager.OnInteractionCanceled += OnCutCanceled;
            // TBD: End ------------------------------------------------------------------------------------------------
        }
        
        protected override void UnregisterCallbacks()
        {
            base.UnregisterCallbacks();
            
            // TBD: Start ----------------------------------------------------------------------------------------------
            // TODO: set proper bindings
            InputManager.OnInteractPerformed -= OnCutPerformed;
            // InputManager.OnInteractionCanceled -= OnCutCanceled;
            // TBD: End ------------------------------------------------------------------------------------------------
        }

        #endregion
        
        #region Input Events

        private void OnCutPerformed()
        {
            if (_objectToCut == null) return; // No object to cut.
            
            InputManager.VibrateController(0.05f, 0.05f, 0.025f);
            _objectToCut.CutPerformedCallback.Invoke(_objectToCut);

            sfxcrabcut.Play();
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