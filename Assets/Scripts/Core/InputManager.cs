using System;
using System.Collections;
using Cinemachine;
using EVOGAMI.Control;
using EVOGAMI.Custom.Enums;
using EVOGAMI.Origami;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

namespace EVOGAMI.Core
{
    /// <summary>
    ///     Manages player input and exposes events for other classes to subscribe to.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        public enum InputDeviceScheme
        {
            KeyboardMouse,
            XboxController,
            PSController,
            Unsupported
        }

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

        private CinemachineCore.AxisInputDelegate _defaultGetInputAxis;
        
        // Input device
        private InputDevice _inputDevice;
        public InputDeviceScheme InputScheme { get; private set; }
        public Gamepad ControllerDevice { get; private set; }

        private void UpdateInputScheme(InputDevice inputDevice)
        {
            switch (inputDevice)
            {
                // Get the type of input device
                case Keyboard or Mouse:
                {
                    if (InputScheme == InputDeviceScheme.KeyboardMouse) break;

                    // Switch to keyboard and mouse
                    OnInputSchemeChanged(InputScheme, InputDeviceScheme.KeyboardMouse);
                    InputScheme = InputDeviceScheme.KeyboardMouse;
                    ControllerDevice = null;
                    break;
                }
                case DualShockGamepad dualShock:
                {
                    if (InputScheme == InputDeviceScheme.PSController) break;

                    // Switch to PS controller
                    OnInputSchemeChanged(InputScheme, InputDeviceScheme.PSController);
                    InputScheme = InputDeviceScheme.PSController;
                    ControllerDevice = dualShock;
                    break;
                }
                case Gamepad gamepad:
                {
                    if (InputScheme == InputDeviceScheme.XboxController) break;

                    // Default to Xbox controller
                    OnInputSchemeChanged(InputScheme, InputDeviceScheme.XboxController);
                    InputScheme = InputDeviceScheme.XboxController;
                    ControllerDevice = gamepad;
                    break;
                }
                default:
                {
                    if (InputScheme == InputDeviceScheme.Unsupported) break;

                    // Unsupported input device
                    Debug.LogWarning($"Unsupported input device: {inputDevice.displayName} ({inputDevice.name})");

                    // Default to keyboard and mouse
                    OnInputSchemeChanged(InputScheme, InputDeviceScheme.KeyboardMouse);
                    InputScheme = InputDeviceScheme.KeyboardMouse;
                    break;
                }
            }
        }

        #region Input Controls

        public void DisablePlayerControls()
        {
            Controls.Player.Disable();
        }

        public void DisableCameraControls()
        {
            Controls.Camera.Disable();
            CinemachineCore.GetInputAxis = delegate { return 0; };
        }
        
        public void DisableOrigamiControls()
        {
            Controls.Origami.Disable();
        }
        
        public void DisableAllControls()
        {
            Controls.Disable();
            DisableCameraControls();
        }
        
        public void EnablePlayerControls()
        {
            Controls.Player.Enable();
        }
        
        public void EnableCameraControls()
        {
            Controls.Camera.Enable();
            CinemachineCore.GetInputAxis = _defaultGetInputAxis;
        }
        
        public void EnableOrigamiControls()
        {
            Controls.Origami.Enable();
        }
        
        public void EnableAllControls()
        {
            Controls.Enable();
        }

        #endregion

        #region Unity Functions

        public void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

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
            
            // UI - Cancel
            Controls.UI.Cancel.started += CancelStartedCallback;
            Controls.UI.Cancel.performed += CancelPerformedCallback;
            Controls.UI.Cancel.canceled += CancelCancelledCallback;
            
            // Origami - Transform
            Controls.Origami.Transform.started += TransformStartedCallback;
            Controls.Origami.Transform.performed += TransformPerformedCallback;
            Controls.Origami.Transform.canceled += TransformCancelledCallback;
            // Origami - Sequence
            // started
            Controls.Origami.Up.started += ctx => SequenceStartedCallback(ctx, Directions.U);
            Controls.Origami.Down.started += ctx => SequenceStartedCallback(ctx, Directions.D);
            Controls.Origami.Left.started += ctx => SequenceStartedCallback(ctx, Directions.L);
            Controls.Origami.Right.started += ctx => SequenceStartedCallback(ctx, Directions.R);
            // performed
            Controls.Origami.Up.performed += ctx => SequencePerformedCallback(ctx, Directions.U);
            Controls.Origami.Down.performed += ctx => SequencePerformedCallback(ctx, Directions.D);
            Controls.Origami.Left.performed += ctx => SequencePerformedCallback(ctx, Directions.L);
            Controls.Origami.Right.performed += ctx => SequencePerformedCallback(ctx, Directions.R);
            // cancelled
            Controls.Origami.Up.canceled += ctx => SequenceCancelledCallback(ctx, Directions.U);
            Controls.Origami.Down.canceled += ctx => SequenceCancelledCallback(ctx, Directions.D);
            Controls.Origami.Left.canceled += ctx => SequenceCancelledCallback(ctx, Directions.L);
            Controls.Origami.Right.canceled += ctx => SequenceCancelledCallback(ctx, Directions.R);
            
            // Camera
            Controls.Camera.Orbit.started += ctx => UpdateInputScheme(ctx.control.device);
            
            // UI
            // Controls.UI.Pause.started += ctx => UpdateInputScheme(ctx.control.device);
            Controls.UI.Navigate.started += ctx => UpdateInputScheme(ctx.control.device);
            Controls.UI.Submit.started += ctx => UpdateInputScheme(ctx.control.device);
            // Controls.UI.Cancel.started += ctx => UpdateInputScheme(ctx.control.device);
            Controls.UI.Point.started += ctx => UpdateInputScheme(ctx.control.device);
            Controls.UI.Click.started += ctx => UpdateInputScheme(ctx.control.device);
            Controls.UI.ScrollWheel.started += ctx => UpdateInputScheme(ctx.control.device);
            Controls.UI.MiddleClick.started += ctx => UpdateInputScheme(ctx.control.device);
            Controls.UI.RightClick.started += ctx => UpdateInputScheme(ctx.control.device);
            Controls.UI.TrackedDevicePosition.started += ctx => UpdateInputScheme(ctx.control.device);
            Controls.UI.TrackedDeviceOrientation.started += ctx => UpdateInputScheme(ctx.control.device);

            _defaultGetInputAxis = CinemachineCore.GetInputAxis;
        }

        public void OnEnable()
        {
            Controls.Enable();
        }

        public void OnDisable()
        {
            Controls?.Disable();
        }

        #endregion

        #region Input Events

        // Player - Move
        #region Move

        private void MoveStartedCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnMoveStarted();
        }

        private void MovePerformedCallback(InputAction.CallbackContext ctx)
        {
            IsMoving = true;
            MoveInput = ctx.ReadValue<Vector2>().normalized;

            UpdateInputScheme(ctx.control.device);
            OnMovePerformed();
        }

        private void MoveCancelledCallback(InputAction.CallbackContext ctx)
        {
            IsMoving = false;
            MoveInput = Vector2.zero;

            UpdateInputScheme(ctx.control.device);
            OnMoveCancelled();
        }

        #endregion

        // Player - Jump
        #region Jump

        private void JumpStartedCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnJumpStarted();
        }

        private void JumpPerformedCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnJumpPerformed();
        }

        private void JumpCancelledCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnJumpCancelled();
        }

        #endregion

        // Player - Sprint (Hold)
        #region Sprint (Hold)

        private void SprintHoldStartedCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnSprintHoldStarted();
        }

        private void SprintHoldPerformedCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnSprintHoldPerformed();
        }

        private void SprintHoldCancelledCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnSprintHoldCancelled();
        }
        
        #endregion

        // Player - Sprint (Press)
        #region Sprint (Press)

        private void SprintPressStartedCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnSprintPressStarted();
        }

        private void SprintPressPerformedCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnSprintPressPerformed();
        }
        private void SprintPressCancelledCallback(InputAction.CallbackContext ctx) { 
            UpdateInputScheme(ctx.control.device);
            OnSprintPressCancelled(); 
        }
        
        #endregion

        // Player - Interact
        #region Interact

        private void InteractStartedCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnInteractStarted();
        }

        private void InteractPerformedCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnInteractPerformed();
        }

        private void InteractCancelledCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnInteractCancelled();
        }

        #endregion

        // UI - Pause
        #region Pause

        private void PauseStartedCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnPauseStarted();
        }

        private void PausePerformedCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnPausePerformed();
        }

        private void PauseCancelledCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnPauseCancelled();
        }
        
        // UI - Cancel
        private void CancelStartedCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnCancelStarted();
        }

        private void CancelPerformedCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnCancelPerformed();
        }
        
        private void CancelCancelledCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnCancelCancelled();
        }
        
        #endregion

        // Origami - Transform
        #region Transform

        private void TransformStartedCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnTransformStarted();
        }

        private void TransformPerformedCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnTransformPerformed();
        }

        private void TransformCancelledCallback(InputAction.CallbackContext ctx)
        {
            UpdateInputScheme(ctx.control.device);
            OnTransformCancelled();
        }
        
        #endregion

        // Origami - Sequence
        #region Sequence

        private void SequenceStartedCallback(InputAction.CallbackContext ctx, Directions sequenceInput)
        {
            UpdateInputScheme(ctx.control.device);
            OnSequenceStarted(sequenceInput);
        }

        private void SequencePerformedCallback(InputAction.CallbackContext ctx, Directions sequenceInput)
        {
            UpdateInputScheme(ctx.control.device);
            OnSequencePerformed(sequenceInput);
        }

        private void SequenceCancelledCallback(InputAction.CallbackContext ctx, Directions sequenceInput)
        {
            UpdateInputScheme(ctx.control.device);
            OnSequenceCancelled(sequenceInput);
        }
        
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
        
        // UI - Cancel
        #region Cancel
        
        public event PauseCallback OnCancelStarted = delegate { };
        public event PauseCallback OnCancelPerformed = delegate { };
        public event PauseCallback OnCancelCancelled = delegate { };
        
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
        
        public delegate void InputSchemeChangedCallback(InputDeviceScheme oldInputDeviceScheme, InputDeviceScheme newInputDeviceScheme);
        public event InputSchemeChangedCallback OnInputSchemeChanged = delegate { };

        #endregion

        #region Haptic Feedback

        public Gamepad VibrateController(float lowFrequency = 0.5f, float highFrequency = 0.5f, float duration = 0.15f)
        {
            if (Instance.ControllerDevice == null) return null;

            var currentGamepad = Instance.ControllerDevice;
            currentGamepad.SetMotorSpeeds(lowFrequency, highFrequency);

            if (duration > 0)
                // Instance.Invoke(nameof(Instance.StopVibration), Mathf.Max(duration, 0.05f));
                Instance.StartCoroutine(Instance.StopVibrationAfterDuration(currentGamepad, duration));
            return currentGamepad;
        }
        
        public IEnumerator StopVibrationAfterDuration(Gamepad gamepad, float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            StopVibration(gamepad);
        }
        
        public void StopVibration(Gamepad gamepad)
        {
            gamepad.SetMotorSpeeds(0, 0);
        }

        #endregion
    }
}