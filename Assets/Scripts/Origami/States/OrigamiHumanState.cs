using EVOGAMI.Core;
using EVOGAMI.Movement;

namespace EVOGAMI.Origami.States
{
    public class OrigamiHumanState : OrigamiState
    {
        public OrigamiHumanState(OrigamiContainer origamiContainer, OrigamiContainer.OrigamiForm form)
            : base(origamiContainer, form)
        {
        }
        
        public override void Exit()
        {
            OrigamiContainer.humanLocomotion.DropObject();
            
            base.Exit();
        }
    }
}