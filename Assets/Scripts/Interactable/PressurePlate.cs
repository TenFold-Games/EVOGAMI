using System;
using UnityEngine;
using UnityEngine.Events;

namespace EVOGAMI.Interactable
{
    [RequireComponent(typeof(Collider))]
    public class PressurePlate : MonoBehaviour
    {
        private Collider _collider;
        [SerializeField] private int minimumMass;

        [Serializable] public class PlatePressedEvent : UnityEvent {}
        [Serializable] public class PlateHoldEvent : UnityEvent {}
        [Serializable] public class PlateReleaseEvent : UnityEvent {}
        
        [SerializeField] private PlatePressedEvent onPlatePressed = new();
        [SerializeField] private PlateHoldEvent onPlateHold = new();
        [SerializeField] private PlateReleaseEvent onPlateRelease = new();
        
        private float _pressTime;
        private bool _isPressed;

        public void OnEnable()
        {
            _collider = GetComponent<Collider>();
        }
        
        public void OnCollisionEnter(Collision other)
        {
            if (other.rigidbody.mass < minimumMass) return;
                
            _isPressed = true;
            _pressTime = Time.time;
            onPlatePressed.Invoke();
        }
        
        public void OnCollisionStay(Collision other)
        {
            if (!_isPressed) return;
            
            onPlateHold.Invoke();
        }

        public void OnCollisionExit(Collision other)
        {
            if (Time.time - _pressTime < 0.1f) return; // Ignore quick presses
            if (!_isPressed) return;
            if (other.rigidbody.mass < minimumMass) return;
            
            _isPressed = false;
            onPlateRelease.Invoke();
        }
    }
}