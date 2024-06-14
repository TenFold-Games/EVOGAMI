using System;
using UnityEngine;
using UnityEngine.Events;

namespace EVOGAMI.Interactable
{
    /// <summary>
    ///     A press button.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class PressButton : MonoBehaviour
    {
        [Header("Callbacks")]
        // Event invoked when the button is pressed.
        [SerializeField] [Tooltip("Event invoked when the button is pressed.")]
        private ButtonPressedEvent onPress = new();
        
        /// <summary>
        ///     Event invoked when the button is pressed.
        /// </summary>
        [Serializable] public class ButtonPressedEvent : UnityEvent {}
        
        #region Unity Functions

        private void OnCollisionEnter(Collision other)
        {
            onPress.Invoke();
        }

        #endregion

    }
}