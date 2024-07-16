using EVOGAMI.Core;
using EVOGAMI.Interactable;
using EVOGAMI.Origami;
using UnityEngine;

namespace EVOGAMI.Movement
{
    [RequireComponent(typeof(Probe))]
    public class Throw : MovementBase
    {
        private enum NinjaStarStates
        {
            Aim,
            Throw, 
            None,
        }

        // The probe for ThrowAt
        [SerializeField] [Tooltip("The probe for ThrowAt")]
        private Probe probe;
        // The speed at which the throwable moves
        [SerializeField] [Tooltip("The speed at which the throwable moves.")]
        private float speed = 10f;
        // The speed at which the throwable rotates, in degrees per second.
        [SerializeField] [Tooltip("The speed at which the throwable rotates, in degrees per second.")]
        private float rotationSpeed = 360f;

        private GameObject _targetObject;
        private ThrowAt _throwAt;
        
        [SerializeField]
        private float flyCutOff = 5f;
        private float _flyTimer = 0f;

        // State Machine
        private NinjaStarStates _state;

        #region Callbacks

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();

            InputManager.OnInteractPerformed += OnInteractionPerformed;

            PlayerManager.Instance.PlayerOrigami.OnFormChange += OnFormChanged;
        }

        protected override void UnregisterCallbacks()
        {
            base.UnregisterCallbacks();

            InputManager.OnInteractPerformed -= OnInteractionPerformed;

            PlayerManager.Instance.PlayerOrigami.OnFormChange -= OnFormChanged;
        }

        private void OnFormChanged(OrigamiContainer.OrigamiForm oldForm, OrigamiContainer.OrigamiForm newForm)
        {
            if (_throwAt)
                OnProbeLost(_throwAt.gameObject);

            if (_state != NinjaStarStates.Aim)
            {
                ExitThrowState();
                EnterAimState();
            }

            // Enable gravity
            PlayerManager.Instance.PlayerRb.useGravity = true;
        }

        private void OnProbeFound(GameObject obj)
        {
            _targetObject = obj;
            _throwAt = obj.GetComponent<ThrowAt>();
            _throwAt.ToggleOutline(true);
        }

        private void OnProbeLost(GameObject obj)
        {
            if (!obj || !_throwAt) return;

            _throwAt.ToggleOutline(false);
            _throwAt = null;
            _targetObject = null;
        }

        #endregion

        #region Input Events

        private void OnInteractionPerformed()
        {
            // Check if the player can throw the ninja star and is in the aim state.
            if (!_throwAt || _state != NinjaStarStates.Aim) return;

            ExitAimState();
            EnterThrowState();
        }

        #endregion

        #region State Machine

        public void EnterAimState()
        {
            probe.enabled = true;

            _state = NinjaStarStates.Aim;
        }

        public void ExitAimState()
        {
            probe.enabled = false;

            _state = NinjaStarStates.None;
        }

        public void EnterThrowState()
        {
            // Disable user input
            InputManager.DisablePlayerControls();
            InputManager.DisableOrigamiControls();

            // Disable gravity
            PlayerManager.Instance.PlayerRb.useGravity = false;

            _state = NinjaStarStates.Throw;
        }

        public void ExitThrowState()
        {
            // Enable gravity
            PlayerManager.Instance.PlayerRb.useGravity = true;

            // Enable user input
            InputManager.EnablePlayerControls();
            InputManager.EnableOrigamiControls();

            _flyTimer = 0f;

            _state = NinjaStarStates.None;
        }

        #endregion

        #region Unity Functions

        private void Awake()
        {
            if (!probe) probe = GetComponent<Probe>();

            probe.ProbeFoundCallback.AddListener(OnProbeFound);
            probe.ProbeLostCallback.AddListener(OnProbeLost);
        }

        protected override void Start()
        {
            base.Start();

            EnterAimState();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (_state != NinjaStarStates.Throw) return;

            // Move the ninja star towards the target transform.
            PlayerTransform.position = Vector3.MoveTowards(
                PlayerTransform.position,
                _targetObject.transform.position,
                speed * Time.fixedDeltaTime
            );

            // Rotate the y-axis of the ninja star, rotationSpeed degrees per second.
            transform.Rotate(Vector3.up, rotationSpeed * Time.fixedDeltaTime);

            _flyTimer += Time.fixedDeltaTime;
            if (_flyTimer > flyCutOff)
            {
                ExitThrowState();
                EnterAimState();
            }
        }

        #endregion
    }
}