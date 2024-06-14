using System;
using UnityEngine;
using UnityEngine.Events;

namespace EVOGAMI.Interactable
{
    /// <summary>
    ///     A pressure plate.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class PressurePlate : MonoBehaviour
    {
        [Header("Settings")]
        // Minimum mass of the object to trigger the pressure plate.
        [SerializeField] [Tooltip("Minimum mass of the object to trigger the pressure plate.")]
        private float minimumMass;
        
        [Header("Callbacks")]
        // Event invoked when objects are placed on the plate.
        [SerializeField] [Tooltip("Event invoked when objects are placed on the plate.")]
        private PlatePressedEvent onPlatePressed = new();
        // Event invoked when objects are removed from the plate.
        [SerializeField] [Tooltip("Event invoked when objects are removed from the plate.")]
        private PlateReleaseEvent onPlateRelease = new();

        /// <summary>
        ///     Event invoked when objects are placed on the plate.
        /// </summary>
        [Serializable] public class PlatePressedEvent : UnityEvent {}
        /// <summary>
        ///     Event invoked when objects are removed from the plate.
        /// </summary>
        [Serializable] public class PlateReleaseEvent : UnityEvent {}
        
        private float _lastPressedTime;
        
        // Flags
        private bool _isPressed;

        #region Collision

        public void OnCollisionEnter(Collision other)
        {
            if (other.rigidbody.mass < minimumMass) return;
                
            _isPressed = true;
            _lastPressedTime = Time.time;
            
            onPlatePressed.Invoke();
        }

        public void OnCollisionExit(Collision other)
        {
            if (Time.time - _lastPressedTime < 0.1f) return; // Ignore quick presses
            if (!_isPressed) return; // Should not happen
            if (other.rigidbody.mass < minimumMass) return;
            
            _isPressed = false;
            
            onPlateRelease.Invoke();
        }
        
        #endregion
    }
}