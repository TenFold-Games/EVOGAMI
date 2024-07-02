using System;
using EVOGAMI.Interactable;
using EVOGAMI.Movement.CheckProvider;
using UnityEngine;
using UnityEngine.Events;

namespace EVOGAMI.Movement
{
    public class Pull :
        MovementBase,
        ICheckProvider
    {
        private enum PullStates
        {
            Aim,
            Pull,
            None
        }

        [Header("Settings")] 
        // The speed at which the object is pulled.
        [SerializeField] [Tooltip("The speed at which the object is pulled.")]
        private float pullSpeed = 1f;
        
        [Header("Pull Detection")]
        // The point the player will pull the object from.
        [SerializeField] [Tooltip("The point the player will pull the object from.")]
        private Transform pullPoint;
        // The furthest distance the player can pull the object.
        [SerializeField] [Tooltip("The furthest distance the player can pull the object.")]
        private float maxPullDistance = 5f;
        // The radius of the sphere used to check if the object is within reach.
        [SerializeField] [Tooltip("The radius of the sphere used to check if the object is within reach.")]
        private float pullCheckRadius = 0.1f;
        // The layer mask used to check for objects that can be pulled.
        [SerializeField] [Tooltip("The layer mask used to check for objects that can be pulled.")]
        private LayerMask pullLayer;
        // The raycast hit used to store the pull hit.
        private RaycastHit _pullHit;

        // Object Being Pulled
        private Pullable _pullable;

        // State Machine
        private PullStates _state;

        // Outline
        private Outline _targetOutline;

        // Flags
        private bool _canPull;
        private bool _isCheckTrue;
        
        #region Callbacks

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();

            InputManager.OnInteractPerformed += OnInteractPerformed;
        }

        protected override void UnregisterCallbacks()
        {
            base.UnregisterCallbacks();

            InputManager.OnInteractPerformed -= OnInteractPerformed;
        }

        #endregion

        #region Input Events
        
        private void OnInteractPerformed()
        {
            // Chef if the player can pull the object and is in the aim state.
            if (!_canPull || _state != PullStates.Aim) return;
            
            // Pull the object.
            ExitAimState();
            EnterPullState();
        }

        #endregion

        #region Physics

        public bool Check()
        {
            _isCheckTrue = Physics.SphereCast(
                pullPoint.position,
                pullCheckRadius,
                pullPoint.forward,
                out _pullHit,
                maxPullDistance,
                pullLayer
            );
            return _isCheckTrue;
        }

        #endregion

        #region State Machine

        private void EnterAimState()
        {
            _state = PullStates.Aim;
        }

        private void ExitAimState()
        {
            _state = PullStates.None;
        }

        private void EnterPullState()
        {
            // Get the pullable component.
            _pullable = _pullHit.collider.GetComponent<Pullable>();
            if (_pullable == null) return; // Should never happen.
            _pullable.SetPullSource(pullPoint);

            _state = PullStates.Pull;
        }

        private void ExitPullState()
        {
            // Reset the pullable component.
            _pullable = null;

            _state = PullStates.None;
        }

        #endregion

        #region Unity Functions
        
        private void Awake()
        {
            // Initialize flags
            _canPull = false;
            _isCheckTrue = false;
            
            // Initialize state
            EnterAimState();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            var ret = Check();
            switch (_state)
            {
                case PullStates.Aim:
                {
                    switch (ret)
                    {
                        case true when !_canPull:
                        {
                            // Cannot pull -> can pull
                            // TODO: Highlight the object.
                            break;
                        }
                        case false when _canPull:
                        {
                            // Can pull -> cannot pull
                            // TODO: Unhighlight the object.
                            break;
                        }
                    }
                    
                    _canPull = ret;
                    break;
                }
                case PullStates.Pull:
                {
                    _pullable.Pull(pullSpeed * Time.fixedDeltaTime);
                    
                    // Check if the pullable stopped
                    if (_pullable.IsStopped)
                    {
                        ExitPullState();
                        EnterAimState();
                    }
                    
                    break;
                }
                case PullStates.None:
                default:
                    throw new ArgumentOutOfRangeException();
                    
            }
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Check() ? Color.green : Color.red;
            Gizmos.DrawWireSphere(pullPoint.position, pullCheckRadius);
            Gizmos.DrawLine(pullPoint.position, pullPoint.position + pullPoint.forward * maxPullDistance);
        }

        #endregion
    }
}