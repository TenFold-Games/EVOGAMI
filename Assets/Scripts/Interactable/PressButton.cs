using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace EVOGAMI.Interactable
{
    public class PressButton : MonoBehaviour
    {
        /// <summary>
        ///     Function definition for a button press event.
        /// </summary>
        [Serializable]
        public class ButtonPressedEvent : UnityEvent {}

        /// <summary>
        ///     Event delegates triggered on press.
        /// </summary>
        [SerializeField]
        private ButtonPressedEvent onPress = new();
        
        #region Unity Functions

        private void OnCollisionEnter(Collision other)
        {
            onPress.Invoke();
        }

        #endregion

    }
}