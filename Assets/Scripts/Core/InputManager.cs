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
        // Player
        public Vector2 MoveInput { get; private set; }
        public bool IsMoving { get; private set; }
        // Plane
        public float PlaneYawInput { get; private set; }
        
        public Origami.OrigamiContainer.OrigamiForm NewForm { get; private set; }
        
        public delegate void FormChangeCallback(Origami.OrigamiContainer.OrigamiForm form);
        public event FormChangeCallback OnFormChange = delegate { };

        public void Awake()
        {
            // Singleton
            if (Instance == null) Instance = this;
            else Destroy(this);

            // Keep between scenes
            // DontDestroyOnLoad(this);

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
            // Player - Pick Up
            Controls.Player.PickUp.started += PickUpStartedCallback;
            Controls.Player.PickUp.performed += PickUpPerformedCallback;
            Controls.Player.PickUp.canceled += PickUpCancelledCallback;
            // Player - Drop
            Controls.Player.Drop.started += DropStartedCallback;
            Controls.Player.Drop.performed += DropPerformedCallback;
            Controls.Player.Drop.canceled += DropCancelledCallback;
            // Origami - Transform
            Controls.Origami.Transform.performed += GetFormFromInput;
            Controls.Origami.Transform.canceled += _ => NewForm = Origami.OrigamiContainer.OrigamiForm.None;
            // UI - Pause
            Controls.UI.Pause.started += PauseStartedCallback;
            Controls.UI.Pause.performed += PausePerformedCallback;
            Controls.UI.Pause.canceled += PauseCancelledCallback;
            // Plane - Yaw
            Controls.Plane.Yaw.started += PlaneYawStartedCallback;
            Controls.Plane.Yaw.performed += PlaneYawPerformedCallback;
            Controls.Plane.Yaw.canceled += PlaneYawCancelledCallback;
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
        
        private void PlaneYawStartedCallback(InputAction.CallbackContext ctx)
        {
            OnPlaneYawStarted();
        }
        
        private void PlaneYawPerformedCallback(InputAction.CallbackContext ctx)
        {
            PlaneYawInput = ctx.ReadValue<float>();
            
            OnPlaneYawPerformed();
        }
        
        private void PlaneYawCancelledCallback(InputAction.CallbackContext ctx)
        {
            PlaneYawInput = 0;
            
            OnPlaneYawCancelled();
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
        
        // Plane - Yaw
        public delegate void PlaneYawCallBack();
        public event PlaneYawCallBack OnPlaneYawStarted = delegate { };
        public event PlaneYawCallBack OnPlaneYawPerformed = delegate { };
        public event PlaneYawCallBack OnPlaneYawCancelled = delegate { };

        #endregion
        
        private void GetFormFromInput(InputAction.CallbackContext ctx)
        {
            var input = ctx.ReadValue<Vector2>();
            
            if (input.y > 0) // Up
                NewForm = Origami.OrigamiContainer.OrigamiForm.Human;
            else if (input.y < 0) // Down
                NewForm = Origami.OrigamiContainer.OrigamiForm.Frog;
            else if (input.x < 0) // Left
                NewForm = Origami.OrigamiContainer.OrigamiForm.Plane;
            else if (input.x > 0) // Right
                NewForm = Origami.OrigamiContainer.OrigamiForm.Bug;
            else
                NewForm = Origami.OrigamiContainer.OrigamiForm.None;
            
            OnFormChange(NewForm);
        }
    }
}