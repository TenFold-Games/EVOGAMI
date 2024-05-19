using EVOGAMI.Origami.States;

namespace EVOGAMI.Origami
{
    public class OrigamiStateMachine
    {
        private readonly OrigamiContainer _origamiContainer;

        public OrigamiStateMachine(OrigamiContainer origamiContainer)
        {
            _origamiContainer = origamiContainer;
        }

        public OrigamiState CurrentState { get; private set; }

        /// <summary>
        ///     Initialize the state machine with a state
        /// </summary>
        /// <param name="state">The state to initialize the state machine with</param>
        public void Initialize(OrigamiState state)
        {
            // Unset all forms
            _origamiContainer.UnsetAllForms();
            
            CurrentState = state;
            CurrentState.Enter();
        }

        /// <summary>
        ///     Change the state of the state machine
        /// </summary>
        /// <param name="state">The state to change to</param>
        public void ChangeState(OrigamiState state)
        {
            // If the state is the same, do nothing
            if (CurrentState == state) return;

            CurrentState.Exit();
            CurrentState = state;
            CurrentState.Enter();
        }

        /// <summary>
        ///     Update the current state
        /// </summary>
        /// <param name="delta">The time since the last frame</param>
        public void Update(float delta)
        {
            CurrentState.Update(delta);
        }

        /// <summary>
        ///     FixedUpdate the current state
        /// </summary>
        /// <param name="delta">The time since the last fixed frame</param>
        public void FixedUpdate(float delta)
        {
            CurrentState.FixedUpdate(delta);
        }
    }
}