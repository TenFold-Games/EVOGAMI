using EVOGAMI.Control;
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

        // TBD: START --------------------------------------------------------------------------------------------------
        // Origami form changing
        public OrigamiContainer.OrigamiForm NewForm { get; private set; }
        public delegate void FormChangeCallback(OrigamiContainer.OrigamiForm form);
        public event FormChangeCallback OnFormChange = delegate { };

        private void GetFormFromInput(InputAction.CallbackContext ctx)
        {
            var input = ctx.ReadValue<Vector2>();

            if (input.y > 0) // Up
                NewForm = OrigamiContainer.OrigamiForm.NinjaStar;
            else if (input.x < 0) // Left
                NewForm = OrigamiContainer.OrigamiForm.Frog;
            else if (input.x > 0) // Right
                NewForm = OrigamiContainer.OrigamiForm.Crab;
            else
                NewForm = OrigamiContainer.OrigamiForm.None;

            OnFormChange(NewForm);
        }
        // TBD: END ----------------------------------------------------------------------------------------------------

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
            // Player - Pick Up
            Controls.Player.PickUp.started += PickUpStartedCallback;
            Controls.Player.PickUp.performed += PickUpPerformedCallback;
            Controls.Player.PickUp.canceled += PickUpCancelledCallback;
            // Player - Drop
            Controls.Player.Drop.started += DropStartedCallback;
            Controls.Player.Drop.performed += DropPerformedCallback;
            Controls.Player.Drop.canceled += DropCancelledCallback;
            
            // UI - Pause
            Controls.UI.Pause.started += PauseStartedCallback;
            Controls.UI.Pause.performed += PausePerformedCallback;
            Controls.UI.Pause.canceled += PauseCancelledCallback;
            
            // TBD: START ----------------------------------------------------------------------------------------------
            // Origami - Transform
            Controls.Origami.Transform.performed += GetFormFromInput;
            Controls.Origami.Transform.canceled += _ => NewForm = OrigamiContainer.OrigamiForm.None;
            // TBD: END ------------------------------------------------------------------------------------------------
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

        private void MoveStartedCallback(InputAction.CallbackContext ctx)
        {
            OnMoveStarted();
        }

        private void MovePerformedCallback(InputAction.CallbackContext ctx)
        {
            IsMoving = true;
            MoveInput = ctx.ReadValue<Vector2>();

            OnMovePerformed();
        }

        private void MoveCancelledCallback(InputAction.CallbackContext ctx)
        {
            IsMoving = false;
            MoveInput = Vector2.zero;

            OnMoveCancelled();
        }

        private void JumpStartedCallback(InputAction.CallbackContext ctx)
        {
            OnJumpStarted();
        }

        private void JumpPerformedCallback(InputAction.CallbackContext ctx)
        {
            OnJumpPerformed();
        }

        private void JumpCancelledCallback(InputAction.CallbackContext ctx)
        {
            OnJumpCancelled();
        }

        private void SprintHoldStartedCallback(InputAction.CallbackContext ctx)
        {
            OnSprintHoldStarted();
        }

        private void SprintHoldPerformedCallback(InputAction.CallbackContext ctx)
        {
            OnSprintHoldPerformed();
        }

        private void SprintHoldCancelledCallback(InputAction.CallbackContext ctx)
        {
            OnSprintHoldCancelled();
        }

        private void SprintPressStartedCallback(InputAction.CallbackContext ctx)
        {
            OnSprintPressStarted();
        }

        private void SprintPressPerformedCallback(InputAction.CallbackContext ctx)
        {
            OnSprintPressPerformed();
        }

        private void SprintPressCancelledCallback(InputAction.CallbackContext ctx)
        {
            OnSprintPressCancelled();
        }

        private void PickUpStartedCallback(InputAction.CallbackContext ctx)
        {
            OnPickUpStarted();
        }

        private void PickUpPerformedCallback(InputAction.CallbackContext ctx)
        {
            OnPickUpPerformed();
        }

        private void PickUpCancelledCallback(InputAction.CallbackContext ctx)
        {
            OnPickUpCancelled();
        }

        private void DropStartedCallback(InputAction.CallbackContext ctx)
        {
            OnDropStarted();
        }

        private void DropPerformedCallback(InputAction.CallbackContext ctx)
        {
            OnDropPerformed();
        }

        private void DropCancelledCallback(InputAction.CallbackContext ctx)
        {
            OnDropCancelled();
        }

        private void PauseStartedCallback(InputAction.CallbackContext ctx)
        {
            OnPauseStarted();
        }

        private void PausePerformedCallback(InputAction.CallbackContext ctx)
        {
            OnPausePerformed();
        }

        private void PauseCancelledCallback(InputAction.CallbackContext ctx)
        {
            OnPauseCancelled();
        }

        #endregion

        #region Input Event Exposure

        // Move
        public delegate void MoveCallback();

        public event MoveCallback OnMoveStarted = delegate { };
        public event MoveCallback OnMovePerformed = delegate { };
        public event MoveCallback OnMoveCancelled = delegate { };

        // Jump
        public delegate void JumpCallback();

        public event JumpCallback OnJumpStarted = delegate { };
        public event JumpCallback OnJumpPerformed = delegate { };
        public event JumpCallback OnJumpCancelled = delegate { };

        // Sprint (Hold)
        public delegate void SprintCallback();

        public event SprintCallback OnSprintHoldStarted = delegate { };
        public event SprintCallback OnSprintHoldPerformed = delegate { };

        public event SprintCallback OnSprintHoldCancelled = delegate { };

        // Sprint (Press)
        public event SprintCallback OnSprintPressStarted = delegate { };
        public event SprintCallback OnSprintPressPerformed = delegate { };
        public event SprintCallback OnSprintPressCancelled = delegate { };

        // Pick Up
        public delegate void PickUpCallback();

        public event PickUpCallback OnPickUpStarted = delegate { };
        public event PickUpCallback OnPickUpPerformed = delegate { };
        public event PickUpCallback OnPickUpCancelled = delegate { };

        // Drop
        public delegate void DropCallback();

        public event DropCallback OnDropStarted = delegate { };
        public event DropCallback OnDropPerformed = delegate { };
        public event DropCallback OnDropCancelled = delegate { };

        // Pause
        public delegate void PauseCallback();

        public event PauseCallback OnPauseStarted = delegate { };
        public event PauseCallback OnPausePerformed = delegate { };
        public event PauseCallback OnPauseCancelled = delegate { };

        #endregion
    }
}