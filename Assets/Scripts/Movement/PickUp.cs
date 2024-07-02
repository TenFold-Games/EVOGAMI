using EVOGAMI.Origami;
using EVOGAMI.Region;
using UnityEngine;

namespace EVOGAMI.Movement
{
    /// <summary>
    ///     Handles picking up and dropping objects.
    /// </summary>
    public class PickUp : MovementBase
    {
        // The point where the object will be picked up. Think of it as the hand.
        [SerializeField] [Tooltip("The point where the object will be picked up. Think of it as the hand.")]
        public Transform pickUpPoint;
        // The region where the object can be picked up.
        [SerializeField] [Tooltip("The region within which objects can be picked up.")]
        private CallbackRegion pickUpRegion;
        
        private GameObject _objectToPickUp;
        
        // Flags
        private bool _canPickUp;
        private bool _isPickingUp;

        #region Callbacks

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();
            
            // InputManager.OnPickUpPerformed += OnPickUpPerformed;
            // InputManager.OnDropPerformed += OnDropPerformed;
            InputManager.OnInteractPerformed += OnInteractPerformed;

            PlayerManager.PlayerOrigami.OnFormChange += OnFormChange;
        }
        
        protected override void UnregisterCallbacks()
        {
            base.UnregisterCallbacks();
            
            // InputManager.OnPickUpPerformed -= OnPickUpPerformed;
            // InputManager.OnDropPerformed -= OnDropPerformed;
            InputManager.OnInteractPerformed -= OnInteractPerformed;

            PlayerManager.PlayerOrigami.OnFormChange -= OnFormChange;
        }

        #endregion

        #region Input Events
        
        private void OnInteractPerformed()
        {
            if (!_isPickingUp) PickUpObject();
            else DropObject();
        }

        private void OnPickUpPerformed()
        {
            PickUpObject();
        }
        
        private void OnDropPerformed()
        {
            DropObject();
        }

        private void OnFormChange(OrigamiContainer.OrigamiForm oldForm, OrigamiContainer.OrigamiForm newForm)
        {
            if (oldForm != newForm && _isPickingUp)
                DropObject();
        }

        #endregion

        #region Unity Functions

        private void Awake()
        {
            // Initialize flags
            _canPickUp = false;
            _isPickingUp = false;
            
            // Register callbacks
            pickUpRegion.RegionEnterCallback.AddListener(OnPickUpTriggerEnter);
            pickUpRegion.RegionExitCallback.AddListener(OnPickUpTriggerExit);
        }

        #endregion
        
        #region Callbacks
        
        private void OnPickUpTriggerEnter(Collider other)
        {
            if (_isPickingUp) return; // Already picking up

            // Set the object to pick up
            _canPickUp = true;
            _objectToPickUp = other.gameObject;
        }
        
        private void OnPickUpTriggerExit(Collider other)
        {
            if (_isPickingUp) return; // Already picking up

            // Reset the object to pick up
            _canPickUp = false;
            _objectToPickUp = null;
        }
        
        #endregion
        
        #region Pick Up and Drop
        
        private void PickUpObject()
        {
            // Nothing to pick up
            if (!_objectToPickUp) return;
            // Already picking up
            if (!_canPickUp || _isPickingUp) return;
            
            // Disable physics
            _objectToPickUp.GetComponent<Rigidbody>().isKinematic = true;
            // Mount to pick up point
            _objectToPickUp.transform.position = pickUpPoint.position;
            _objectToPickUp.transform.parent = pickUpPoint;
            
            _isPickingUp = true;
        }
        
        private void DropObject()
        {
            // Nothing to drop
            if (!_isPickingUp || !_objectToPickUp) return;
            
            // Unmount from pick up point
            _objectToPickUp.transform.parent = null;
            // Enable physics
            Rigidbody rb = _objectToPickUp.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            // rb.AddForce(rb.mass * PlayerTransform.forward, ForceMode.Impulse);
            
            _isPickingUp = false;
        }
        
        #endregion
    }
}