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
        public bool JumpInput { get; private set; }
        public bool SprintInput { get; private set; }
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
            Controls.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
            Controls.Player.Move.canceled += _ => MoveInput = Vector2.zero;
            // Player - Jump
            Controls.Player.Jump.performed += _ => JumpInput = true;
            Controls.Player.Jump.canceled += _ => JumpInput = false;
            // Player - Sprint
            Controls.Player.Sprint.performed += _ => SprintInput = true;
            Controls.Player.Sprint.canceled += _ => SprintInput = false;
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

        /// <summary>
        ///     Determines if the player is moving.
        /// </summary>
        /// <returns>True if the player is moving, false otherwise.</returns>
        public bool IsMoving()
        {
            return MoveInput.magnitude > Mathf.Epsilon;
        }

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