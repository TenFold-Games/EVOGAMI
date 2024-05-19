namespace EVOGAMI.Origami.States
{
    public abstract class OrigamiState
    {
        public readonly OrigamiContainer OrigamiContainer;

        protected OrigamiState(OrigamiContainer origamiContainer, OrigamiContainer.OrigamiForm form)
        {
            OrigamiContainer = origamiContainer;
            Form = form;
        }

        public OrigamiContainer.OrigamiForm Form { get; }

        public virtual void Enter()
        {
            OrigamiContainer.SetForm(Form);
        }

        public virtual void Exit()
        {
            OrigamiContainer.UnsetForm(Form);
        }

        public virtual void Update(float delta)
        {
        }

        public virtual void FixedUpdate(float delta)
        {
        }
    }
}