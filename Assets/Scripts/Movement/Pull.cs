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
        private enum PullState
        {
            Aim,
            Mount,
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

        [Header("Callbacks")] 
        [SerializeField] private PullEvent onAimEnter;
        [SerializeField] private PullEvent onAimStay;
        [SerializeField] private PullEvent onAimExit;
        [SerializeField] private PullEvent onMountEnter;
        [SerializeField] private PullEvent onMountStay;
        [SerializeField] private PullEvent onMountExit;
        [SerializeField] private PullEvent onPullEnter;
        [SerializeField] private PullEvent onPullStay;
        [SerializeField] private PullEvent onPullExit;

        /// <summary>
        ///     Event triggered when the player aims at / mounts / pulls an object.
        /// </summary>
        [Serializable] public class PullEvent : UnityEvent<RaycastHit> {}

        public PullEvent OnAimEnter => onAimEnter;
        public PullEvent OnAimStay => onAimStay;
        public PullEvent OnAimExit => onAimExit;
        public PullEvent OnMountEnter => onMountEnter;
        public PullEvent OnMountStay => onMountStay;
        public PullEvent OnMountExit => onMountExit;
        public PullEvent OnPullEnter => onPullEnter;
        public PullEvent OnPullStay => onPullStay;
        public PullEvent OnPullExit => onPullExit;
        
        // Object Being Pulled
        private Pullable _pullable;
        
        // State Machine
        private PullState _pullState = PullState.None;
        
        // TBD: Start --------------------------------------------------------------------------------------------------
        private LineRenderer _lineRenderer;
        // TBD: End ----------------------------------------------------------------------------------------------------

        // Flags
        private bool _isAiming;     // 1. Aim
        private bool _isCheckTrue;  // 2. Check
        private bool _isMounted;    // 3. Mounted
        private bool _isPulling;    // 4. Pull

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

        #region Aim and Pull

        private void Aim()
        {
            Check();
            
            // TBD: Start ----------------------------------------------------------------------------------------------
            _lineRenderer.startColor = _isCheckTrue ? Color.green : Color.magenta;
            _lineRenderer.endColor = _isCheckTrue ? Color.blue : Color.red;

            _lineRenderer.SetPosition( 0, pullPoint.position);
            _lineRenderer.SetPosition( 1,
                _isCheckTrue ? _pullHit.point : pullPoint.position + pullPoint.forward * maxPullDistance);
            // TBD: End ------------------------------------------------------------------------------------------------
        }

        private void PullObject(float delta)
        {
            _pullable?.Pull(pullSpeed * delta);
        }

        #endregion

        #region Callbacks

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();

            // Pull (Aim)
            InputManager.OnPullAimStarted += OnPullAimStarted;
            InputManager.OnPullAimCancelled += OnPullAimCanceled;
            // Pull (Execute)
            InputManager.OnPullExecuteStarted += OnPullExecuteStarted;
            InputManager.OnPullExecuteCancelled += OnPullExecuteCancelled;
        }

        protected override void UnregisterCallbacks()
        {
            base.UnregisterCallbacks();

            // Pull (Aim)
            InputManager.OnPullAimStarted -= OnPullAimStarted;
            InputManager.OnPullAimCancelled -= OnPullAimCanceled;
            // Pull (Execute)
            InputManager.OnPullExecuteStarted -= OnPullExecuteStarted;
            InputManager.OnPullExecuteCancelled -= OnPullExecuteCancelled;
        }

        #endregion

        #region Input Events

        private void OnPullAimStarted()
        {
            if (_isAiming || _isPulling) return;

            if (_isMounted) ExitMountState();

            EnterAimState();
        }

        private void OnPullAimCanceled()
        {
            if (!_isAiming) return;

            ExitAimState();

            if (_isCheckTrue) EnterMountState();
        }

        private void OnPullExecuteStarted()
        {
            if (_isAiming || _isPulling) return;
            if (!_isMounted || !_isCheckTrue) return;

            EnterPullState();
        }

        private void OnPullExecuteCancelled()
        {
            if (!_isPulling) return;

            ExitPullState();

            if (_isMounted) ExitMountState();
        }

        #endregion

        #region State Machine

        private void EnterAimState()
        {
            _pullState = PullState.Aim;
            
            // TBD: Start ----------------------------------------------------------------------------------------------
            _lineRenderer.enabled = true;
            // TBD: End ------------------------------------------------------------------------------------------------

            _isAiming = true;
            onAimEnter.Invoke(_pullHit);
        }

        private void ExitAimState()
        {
            onAimExit.Invoke(_pullHit);
            _isAiming = false;
            
            // TBD: Start ----------------------------------------------------------------------------------------------
            _lineRenderer.SetPosition(0, Vector3.zero);
            _lineRenderer.SetPosition(1, Vector3.zero);
            _lineRenderer.enabled = false;
            // TBD: End ------------------------------------------------------------------------------------------------

            _pullState = PullState.None;
        }

        private void EnterMountState()
        {
            _pullState = PullState.Mount;
            
            // TBD: Start ----------------------------------------------------------------------------------------------
            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0, pullPoint.position);
            _lineRenderer.SetPosition(1, _pullHit.point);
            _lineRenderer.startColor = Color.cyan;
            _lineRenderer.endColor = Color.yellow;
            // TBD: End ------------------------------------------------------------------------------------------------

            _isMounted = true;
            onMountEnter.Invoke(_pullHit);
        }

        private void ExitMountState()
        {
            onMountExit.Invoke(_pullHit);
            _isMounted = false;
            
            // TBD: Start ----------------------------------------------------------------------------------------------
            _lineRenderer.SetPosition(0, Vector3.zero);
            _lineRenderer.SetPosition(1, Vector3.zero);
            _lineRenderer.enabled = false;
            // TBD: End ------------------------------------------------------------------------------------------------

            _pullState = PullState.None;
        }

        private void EnterPullState()
        {
            _pullState = PullState.Pull;

            _pullable = _pullHit.collider.GetComponent<Pullable>();
            _pullable!.SetPullSource(pullPoint);

            _isPulling = true;
            onPullEnter.Invoke(_pullHit);
        }

        private void ExitPullState()
        {
            onPullExit.Invoke(_pullHit);
            _isPulling = false;

            _pullState = PullState.None;
        }

        #endregion

        #region Unity Functions
        
        // TBD: Start --------------------------------------------------------------------------------------------------
        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            if (!_lineRenderer) _lineRenderer = gameObject.AddComponent<LineRenderer>();
            
            _lineRenderer.startWidth = pullCheckRadius;
            _lineRenderer.endWidth = 0;
            
            _lineRenderer.positionCount = 2;
            
            _lineRenderer.enabled = false;
        }
        // TBD: End ----------------------------------------------------------------------------------------------------

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            switch (_pullState)
            {
                case PullState.Aim:
                    Aim();
                    break;
                case PullState.Mount:
                    break;
                case PullState.Pull:
                    PullObject(Time.fixedDeltaTime);
                    break;
                case PullState.None:
                    break;
                default: // Should never happen
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