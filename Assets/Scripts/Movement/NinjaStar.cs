using System;
using EVOGAMI.Movement.CheckProvider;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EVOGAMI.Movement
{
    public class NinjaStar :
        MovementBase, 
        ICheckProvider
    {
        private enum NinjaStarStates
        {
            Aim,
            Throw, 
            None,
        }
        
        [Header("Shoot Detection")]
        // The main camera.
        [SerializeField] [Tooltip("The main camera.")]
        private Camera mainCamera;
        // The furthest distance the ninja star can travel.
        [SerializeField] [Tooltip("The furthest distance the ninja star can travel.")]
        private float maxThrowDistance = 20f;
        // The radius of the sphere used to check if the throw is possible.
        [SerializeField] [Tooltip("The radius of the sphere used to check if the throw is possible.")]
        private float throwCheckRadius = 0.5f;
        // The layer mask used to check for objects that can be thrown at.
        [SerializeField] [Tooltip("The layer mask used to check for objects that can be thrown at.")]
        private LayerMask throwLayer;
        // The raycast hit used to store the hit information.
        private RaycastHit _hit;
        
        [Header("Movement")]
        // The speed at which the ninja star moves.
        [SerializeField] [Tooltip("The speed at which the ninja star moves.")]
        private float speed = 10f;
        // The speed at which the ninja star rotates.
        [SerializeField] [Tooltip("The speed at which the ninja star rotates, in degrees per second.")]
        private float rotationSpeed = 10f;
        // The transform of the target object.
        [SerializeField] [Tooltip("The transform of the target object.")]
        private Transform targetTransform;
        // The minimum distance the player must be from the target object.
        [SerializeField] [Tooltip("The minimum distance the player must be from the target object.")]
        private float minDistance = 2f;
        
        // State Machine
        private NinjaStarStates _state;
        
        // Outline
        private Outline _targetOutline;
        
        // Flags
        private bool _canThrow;

        #region Callbacks

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();
            
            InputManager.OnJumpPerformed += OnJumpPerformed;
        }
        
        protected override void UnregisterCallbacks()
        {
            base.UnregisterCallbacks();
            
            InputManager.OnJumpCancelled -= OnJumpPerformed;
        }

        #endregion

        #region Input Events

        private void OnJumpPerformed()
        {
            // Check if the player can throw the ninja star and is in the aim state.
            if (!_canThrow || _state != NinjaStarStates.Aim) return;
            
            // Don't throw if the player is too close to the target.
            if (Vector3.Distance(PlayerTransform.position, _hit.point) < minDistance) return;
            
            // Don't throw if the path is blocked.
            if (Physics.Linecast(
                    transform.position, 
                    _hit.point, 
                    ~throwLayer & ~LayerMask.GetMask("Ignore Raycast"))
            ) return;
            
            ExitAimState();
            EnterThrowState();
        }

        #endregion

        #region Physics

        public bool Check()
        {
            return Physics.SphereCast(
                // mainCamera.transform.position, 
                transform.position, 
                throwCheckRadius, 
                mainCamera.transform.forward,
                out _hit, 
                maxThrowDistance, 
                throwLayer
            );
        }

        #endregion

        #region State Machine

        public void EnterAimState()
        {
            _state = NinjaStarStates.Aim;
        }
        
        public void ExitAimState()
        {
            _state = NinjaStarStates.None;
        }
        
        public void EnterThrowState()
        {
            // Set the target transform
            targetTransform = _hit.transform;
            
            // Disable user input
            InputManager.DisablePlayerControls();
            // InputManager.DisableCameraControls();
            
            // Disable gravity
            PlayerRb.useGravity = false;
            
            _state = NinjaStarStates.Throw;
        }
        
        public void ExitThrowState()
        {
            // Reset the target transform
            targetTransform = null;
            
            // Enable user input
            InputManager.EnablePlayerControls();
            // InputManager.EnableCameraControls();
            
            // Enable gravity
            PlayerRb.useGravity = true;
            
            _state = NinjaStarStates.None;
        }

        #endregion

        #region Unity Functions

        protected override void Start()
        {
            base.Start();
            
            // Set the main camera
            if (!mainCamera)
                mainCamera = Camera.main;

            // Set the state
            EnterAimState();
        }

        private void OnDrawGizmos()
        {
            if (!mainCamera) return;
            
            Gizmos.color = _canThrow ? Color.green : Color.red;
            Gizmos.DrawWireSphere(mainCamera.transform.position + mainCamera.transform.forward * maxThrowDistance, 
                throwCheckRadius);
            Gizmos.DrawLine(mainCamera.transform.position,
                mainCamera.transform.position + mainCamera.transform.forward * maxThrowDistance);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            switch (_state)
            {
                case NinjaStarStates.Aim:
                {
                    var ret = Check();
                
                    switch (ret)
                    {
                        case true when !_canThrow:
                            // Cannot throw -> can throw
                            OnThrowAtFound();
                            break;
                        case false when _canThrow:
                            // Can throw -> cannot throw
                            OnThrowAtLost();
                            break;
                    }
                
                    _canThrow = ret;
                    break;
                }
                case NinjaStarStates.Throw:
                    // Move the ninja star towards the target transform.
                    PlayerTransform.position = Vector3.MoveTowards(
                        PlayerTransform.position, 
                        targetTransform.position, 
                        speed * Time.fixedDeltaTime
                    );
                
                    // Rotate the y-axis of the ninja star, rotationSpeed degrees per second.
                    transform.Rotate(Vector3.up, rotationSpeed * Time.fixedDeltaTime);
                    break;
                case NinjaStarStates.None:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnThrowAtFound()
        {
            _targetOutline = _hit.transform.GetComponent<Outline>();
            if (!_targetOutline) return;
            
            _targetOutline.enabled = true;
        }
        
        private void OnThrowAtLost()
        {
            if (!_targetOutline) return;
            
            _targetOutline.enabled = false;
        }

        #endregion
    }
}