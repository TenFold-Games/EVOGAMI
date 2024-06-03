using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.Origami.States
{
    public class OrigamiPlaneState : OrigamiState
    {
        private float originalAngularDrag;
        private float originalDrag;
        private float originalMass;
        
        private InputManager inputManager;

        public OrigamiPlaneState(OrigamiContainer origamiContainer, OrigamiContainer.OrigamiForm form)
            : base(origamiContainer, form)
        {
        }

        public override void Enter()
        {
            inputManager = InputManager.Instance;
            
            base.Enter();
            
            inputManager.Controls.Plane.Enable();
            inputManager.Controls.Camera.Disable();
            
            // Unfreeze player rigidbody rotation
            PlayerManager.Instance.PlayerRb.constraints = RigidbodyConstraints.None;

            // Angular drag
            originalAngularDrag = PlayerManager.Instance.PlayerRb.angularDrag;
            PlayerManager.Instance.PlayerRb.angularDrag = 1;
            // Drag
            originalDrag = PlayerManager.Instance.PlayerRb.drag;
            PlayerManager.Instance.PlayerRb.drag = 1;
            // Mass
            originalMass = PlayerManager.Instance.PlayerRb.mass;
            PlayerManager.Instance.PlayerRb.mass = 400;
            
            // Set the camera
            OrigamiContainer.SetPlaneCamera();
        }

        public override void Exit()
        {
            PlayerManager.Instance.PlayerRb.angularDrag = originalAngularDrag;
            PlayerManager.Instance.PlayerRb.drag = originalDrag;
            PlayerManager.Instance.PlayerRb.mass = originalMass;
            
            PlayerManager.Instance.PlayerRb.constraints = RigidbodyConstraints.FreezeRotation;
            
            OrigamiContainer.UnsetPlaneCamera();
            
            InputManager.Instance.Controls.Plane.Disable();
            InputManager.Instance.Controls.Camera.Enable();
            
            base.Exit();
        }
    }
}