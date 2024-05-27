using EVOGAMI.Control;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EVOGAMI.Core
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }
        public PlayerControls Controls { get; private set; }

        // Input fields
        public Vector2 MoveInput { get; private set; }
        public bool IsMoving { get; private set; }
        
        public Origami.OrigamiContainer.OrigamiForm NewForm { get; private set; }
        
        public delegate void FormChangeCallback(Origami.OrigamiContainer.OrigamiForm form);
        public event FormChangeCallback OnFormChange = delegate { };

        public void Awake()
        {
            // Singleton
            if (Instance == null) Instance = this;
            else Destroy(this);

            // Keep between scenes
            DontDestroyOnLoad(this);

            // Initialize controls
            Controls = new PlayerControls();

            // Bind controls
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
            // Origami - Transform
            Controls.Origami.Transform.performed += GetFormFromInput;
            Controls.Origami.Transform.canceled += _ => NewForm = Origami.OrigamiContainer.OrigamiForm.None;
        }

        public void OnEnable()
        {
            Controls.Enable();
        }

        public void OnDisable()
        {
            Controls.Disable();
        }

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

        #endregion
        
        private void GetFormFromInput(InputAction.CallbackContext ctx)
        {
            var input = ctx.ReadValue<Vector2>();
            
            if (input.y > 0) // Up
                NewForm = Origami.OrigamiContainer.OrigamiForm.Humanoid;
            else if (input.y < 0) // Down
                NewForm = Origami.OrigamiContainer.OrigamiForm.Crane;
            else if (input.x < 0) // Left
                NewForm = Origami.OrigamiContainer.OrigamiForm.Airplane;
            else if (input.x > 0) // Right
                NewForm = Origami.OrigamiContainer.OrigamiForm.Boat;
            else
                NewForm = Origami.OrigamiContainer.OrigamiForm.None;
            
            OnFormChange(NewForm);
        }
    }
}