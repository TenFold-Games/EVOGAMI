using EVOGAMI.Control;
using EVOGAMI.Custom.Enums;
using EVOGAMI.Origami;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EVOGAMI.Core
{
    /// <summary>
    ///     Manages player input and exposes events for other classes to subscribe to.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        /// <summary>
        ///     Singleton instance of the InputManager.
        /// </summary>
        public static InputManager Instance { get; private set; }
        
        /// <summary>
        ///     The player controls.
        /// </summary>
        public PlayerControls Controls { get; private set; }

        /// <summary>
        ///     The value of the player's movement input.
        /// </summary>
        public Vector2 MoveInput { get; private set; }
        /// <summary>
        ///     Whether the player is moving.
        /// </summary>
        public bool IsMoving { get; private set; }

        private Directions _sequenceInput;

        #region Input Controls

        public void DisablePlayerControls()
        {
            Controls.Player.Disable();
        }
        
        public void DisableOrigamiControls()
        {
            Controls.Origami.Disable();
        }
        
        public void EnablePlayerControls()
        {
            Controls.Player.Enable();
        }
        
        public void EnableOrigamiControls()
        {
            Controls.Origami.Enable();
        }

        #endregion

        #region Unity Functions

        public void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);

            // Initialize controls
            Controls = new PlayerControls();

            // Player - Move
            Controls.Player.Move.started += MoveStartedCallback;
            Controls.Player.Move.performed += MovePerformedCallback;
            Controls.Player.Move.canceled += MoveCancelledCallback;
            // Player - Jump
            Controls.Player.Jump.started += JumpStartedCallback;
            Controls.Player.Jump.performed += JumpPerformedCallback;
            Controls.Player.Jump.canceled += JumpCancelledCallback;
            // Player - Sprint (Hold)
            Controls.Player.Sprint_Hold.started += SprintHoldStartedCallback;
            Controls.Player.Sprint_Hold.performed += SprintHoldPerformedCallback;
            Controls.Player.Sprint_Hold.canceled += SprintHoldCancelledCallback;
            // Player - Sprint (Press)
            Controls.Player.Sprint_Press.started += SprintPressStartedCallback;
            Controls.Player.Sprint_Press.performed += SprintPressPerformedCallback;
            Controls.Player.Sprint_Press.canceled += SprintPressCancelledCallback;
            // Player - Interact
            Controls.Player.Interact.started += InteractStartedCallback;
            Controls.Player.Interact.performed += InteractPerformedCallback;
            Controls.Player.Interact.canceled += InteractCancelledCallback;
            
            // UI - Pause
            Controls.UI.Pause.started += PauseStartedCallback;
            Controls.UI.Pause.performed += PausePerformedCallback;
            Controls.UI.Pause.canceled += PauseCancelledCallback;
            
            // Origami - Transform
            Controls.Origami.Transform.started += TransformStartedCallback;
            Controls.Origami.Transform.performed += TransformPerformedCallback;
            Controls.Origami.Transform.canceled += TransformCancelledCallback;
            // Origami - Sequence
            // started
            Controls.Origami.Up.started += _ => SequenceStartedCallback(Directions.U);
            Controls.Origami.Down.started += _ => SequenceStartedCallback(Directions.D);
            Controls.Origami.Left.started += _ => SequenceStartedCallback(Directions.L);
            Controls.Origami.Right.started += _ => SequenceStartedCallback(Directions.R);
            // performed
            Controls.Origami.Up.performed += _ => SequencePerformedCallback(Directions.U);
            Controls.Origami.Down.performed += _ => SequencePerformedCallback(Directions.D);
            Controls.Origami.Left.performed += _ => SequencePerformedCallback(Directions.L);
            Controls.Origami.Right.performed += _ => SequencePerformedCallback(Directions.R);
            // cancelled
            Controls.Origami.Up.canceled += _ => SequenceCancelledCallback(Directions.U);
            Controls.Origami.Down.canceled += _ => SequenceCancelledCallback(Directions.D);
            Controls.Origami.Left.canceled += _ => SequenceCancelledCallback(Directions.L);
            Controls.Origami.Right.canceled += _ => SequenceCancelledCallback(Directions.R);
        }

        public void OnEnable()
        {
            Controls.Enable();
        }

        public void OnDisable()
        {
            Controls.Disable();
        }

        #endregion

        #region Input Events

        // Player - Move
        #region Move

        private void MoveStartedCallback(InputAction.CallbackContext ctx) { OnMoveStarted(); }

        private void MovePerformedCallback(InputAction.CallbackContext ctx)
        {
            IsMoving = true;
            MoveInput = ctx.ReadValue<Vector2>().normalized;

            OnMovePerformed();
        }

        private void MoveCancelledCallback(InputAction.CallbackContext ctx)
        {
            IsMoving = false;
            MoveInput = Vector2.zero;

            OnMoveCancelled();
        }

        #endregion

        // Player - Jump
        #region Jump

        private void JumpStartedCallback(InputAction.CallbackContext ctx) { OnJumpStarted(); }
        private void JumpPerformedCallback(InputAction.CallbackContext ctx) { OnJumpPerformed(); }
        private void JumpCancelledCallback(InputAction.CallbackContext ctx) { OnJumpCancelled(); }

        #endregion

        // Player - Sprint (Hold)
        #region Sprint (Hold)

        private void SprintHoldStartedCallback(InputAction.CallbackContext ctx) { OnSprintHoldStarted(); }
        private void SprintHoldPerformedCallback(InputAction.CallbackContext ctx) { OnSprintHoldPerformed(); }
        private void SprintHoldCancelledCallback(InputAction.CallbackContext ctx) { OnSprintHoldCancelled(); }
        
        #endregion

        // Player - Sprint (Press)
        #region Sprint (Press)
        
        private void SprintPressStartedCallback(InputAction.CallbackContext ctx) { OnSprintPressStarted(); }
        private void SprintPressPerformedCallback(InputAction.CallbackContext ctx) { OnSprintPressPerformed(); }
        private void SprintPressCancelledCallback(InputAction.CallbackContext ctx) { OnSprintPressCancelled(); }
        
        #endregion

        // Player - Interact
        #region Interact
        
        private void InteractStartedCallback(InputAction.CallbackContext ctx) { OnInteractStarted(); }
        private void InteractPerformedCallback(InputAction.CallbackContext ctx) { OnInteractPerformed(); }
        private void InteractCancelledCallback(InputAction.CallbackContext ctx) { OnInteractCancelled(); }

        #endregion

        // UI - Pause
        #region Pause

        private void PauseStartedCallback(InputAction.CallbackContext ctx) { OnPauseStarted(); }
        private void PausePerformedCallback(InputAction.CallbackContext ctx) { OnPausePerformed(); }
        private void PauseCancelledCallback(InputAction.CallbackContext ctx) { OnPauseCancelled(); }
        
        #endregion

        // Origami - Transform
        #region Transform
        
        private void TransformStartedCallback(InputAction.CallbackContext ctx) { OnTransformStarted(); }
        private void TransformPerformedCallback(InputAction.CallbackContext ctx) { OnTransformPerformed(); }
        private void TransformCancelledCallback(InputAction.CallbackContext ctx) { OnTransformCancelled(); }
        
        #endregion

        // Origami - Sequence
        #region Sequence

        private void SequenceStartedCallback(Directions sequenceInput) { OnSequenceStarted(sequenceInput); }
        private void SequencePerformedCallback(Directions sequenceInput) { OnSequencePerformed(sequenceInput); }
        private void SequenceCancelledCallback(Directions sequenceInput) { OnSequenceCancelled(sequenceInput); }
        
        #endregion
        
        #endregion

        #region Input Event Exposure

        // Player - Move
        #region Move
        
        public delegate void MoveCallback();

        public event MoveCallback OnMoveStarted = delegate { };
        public event MoveCallback OnMovePerformed = delegate { };
        public event MoveCallback OnMoveCancelled = delegate { };
        
        #endregion

        // Player - Jump
        #region Jump
        public delegate void JumpCallback();

        public event JumpCallback OnJumpStarted = delegate { };
        public event JumpCallback OnJumpPerformed = delegate { };
        public event JumpCallback OnJumpCancelled = delegate { };
        
        #endregion

        // Player - Sprint (Hold)
        #region Sprint (Hold)
        public delegate void SprintCallback();

        public event SprintCallback OnSprintHoldStarted = delegate { };
        public event SprintCallback OnSprintHoldPerformed = delegate { };

        public event SprintCallback OnSprintHoldCancelled = delegate { };
        
        #endregion

        // Player - Sprint (Press)
        #region Sprint (Press)
        public event SprintCallback OnSprintPressStarted = delegate { };
        public event SprintCallback OnSprintPressPerformed = delegate { };
        public event SprintCallback OnSprintPressCancelled = delegate { };
        
        #endregion

        // Player - Interact
        #region Interact

        public delegate void InteractCallback();
        public event InteractCallback OnInteractStarted = delegate { };
        public event InteractCallback OnInteractPerformed = delegate { };
        public event InteractCallback OnInteractCancelled = delegate { };

        #endregion
        
        // UI - Pause
        #region Pause 

        public delegate void PauseCallback();

        public event PauseCallback OnPauseStarted = delegate { };
        public event PauseCallback OnPausePerformed = delegate { };
        public event PauseCallback OnPauseCancelled = delegate { };

        #endregion
        
        // Origami - Transform
        #region Transform
        public delegate void TransformCallback();
        
        public event TransformCallback OnTransformStarted = delegate { };
        public event TransformCallback OnTransformPerformed = delegate { };
        public event TransformCallback OnTransformCancelled = delegate { };
        
        #endregion

        // Origami - Sequence
        #region Sequence
        
        public delegate void SequenceCallback(Directions direction);
        
        public event SequenceCallback OnSequenceStarted = delegate { };
        public event SequenceCallback OnSequencePerformed = delegate { };
        public event SequenceCallback OnSequenceCancelled = delegate { };
        
        #endregion

        #endregion
    }
}