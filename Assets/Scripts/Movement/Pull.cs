using System;
using EVOGAMI.Animations;
using EVOGAMI.Core;
using EVOGAMI.Interactable;
using EVOGAMI.Movement.CheckProvider;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

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
        
        Gamepad _gamepad;

        // TBD: Start --------------------------------------------------------------------------------------------------
        [SerializeField] [Tooltip("The line renderer used to show the pull line.")]
        private LineRenderer lineRenderer;
        // TBD: End ----------------------------------------------------------------------------------------------------

        // Flags
        private bool _canPull;
        private bool _isCheckTrue;

        // FMOD Studio Event Emitter
        [SerializeField] StudioEventEmitter frogtongueoutsfx;
        
        private Color _frogTongueColor = new Color(1.0f, 0.5f, 0.5f); // Example RGB values for a pinkish color

        
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

            // Play Frog Tongue Out SFX
            frogtongueoutsfx.Play();
            
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
        
        #region Animation
        [SerializeField] private TongueController tongueController;
        
        #endregion

        #region State Machine

        private void EnterAimState()
        {
            // TBD: Start ----------------------------------------------------------------------------------------------
            
            // TBD: End ------------------------------------------------------------------------------------------------
            _state = PullStates.Aim;
        }

        private void ExitAimState()
        {
            // TBD: Start ----------------------------------------------------------------------------------------------
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);
            lineRenderer.enabled = false;
            // TBD: End ------------------------------------------------------------------------------------------------
            _state = PullStates.None;
        }

        private void EnterPullState()
        {
            // Get the pullable component.
            _pullable = _pullHit.collider.GetComponent<Pullable>();
            if (_pullable == null) return; // Should never happen.
            _pullable.SetPullSource(pullPoint);

            InputManager.DisablePlayerControls();
            InputManager.DisableOrigamiControls();

            _state = PullStates.Pull;
            
            // Set isPull to true
            // if (tongueController != null && !_pullable.IsStopped) tongueController.SetPullState(true);
            
            _gamepad = InputManager.VibrateController(0.005f, 0.005f, -1);
        }

        private void ExitPullState()
        {
            // Reset the pullable component.
            _pullable = null;

            InputManager.EnablePlayerControls();
            InputManager.EnableOrigamiControls();

            _state = PullStates.None;
            
            // Set isPull to false
            // if (tongueController) tongueController.SetPullState(false);
            
            if (_gamepad != null)
                InputManager.Instance.StopVibration(_gamepad);
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
            
            // TBD: Start ----------------------------------------------------------------------------------------------
            lineRenderer.startWidth = pullCheckRadius;
            lineRenderer.endWidth = 0;
            
            lineRenderer.positionCount = 2;
            
            lineRenderer.enabled = false; // Hide the lineRenderer initially
            // TBD: End ------------------------------------------------------------------------------------------------
        }

        // TBD: Start --------------------------------------------------------------------------------------------------
        private void Aim(bool canPull)
        {
            
            if (!canPull)
            {
                lineRenderer.enabled = false; // Change: Hide the lineRenderer if cannot pull
                return;
            }
            
            // if can pull
            lineRenderer.startColor = _frogTongueColor;
            lineRenderer.endColor = _frogTongueColor;
            
            lineRenderer.SetPosition(0, pullPoint.position);
            lineRenderer.SetPosition(1, _isCheckTrue ? _pullHit.point : pullPoint.position + pullPoint.forward * maxPullDistance);
            
            lineRenderer.enabled = true; //  Show the lineRenderer if can pull
        }
        // TBD: End ----------------------------------------------------------------------------------------------------

        
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
                            OnPullAtFound();
                            break;
                        }
                        case false when _canPull:
                        {
                            // Can pull -> cannot pull
                            OnPullAtLost();
                            break;
                        }
                    }
            
                    _canPull = ret;
                    if (_pullable && _pullable.IsStopped) _canPull = false;
                    Aim(_canPull);
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
        
        private void OnPullAtFound()
        {
            _targetOutline = _pullHit.transform.GetComponent<Outline>();
            if (!_targetOutline) return;
            
            _pullable = _pullHit.collider.GetComponent<Pullable>();
            if (!_pullable) return; // Should never happen.
            _pullable.OnMount();
            
            if (!_pullable.IsStopped)
            {
                InputManager.VibrateController(0.05f, 0.05f, 0.025f);
                _targetOutline.enabled = true;
            }
        }
        
        private void OnPullAtLost()
        {
            if (!_targetOutline) return;
            
            InputManager.VibrateController(0.05f, 0.05f, 0.025f);
            _targetOutline.enabled = false;
        }

        #endregion
    }
}