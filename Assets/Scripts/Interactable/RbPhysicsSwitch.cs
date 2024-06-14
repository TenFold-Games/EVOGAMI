using UnityEngine;

namespace EVOGAMI.Interactable
{
    public class RbPhysicsSwitch : MonoBehaviour
    {
        [Header("Settings")]
        // The rigidbody of whom to enable/disable physics
        [SerializeField] [Tooltip("The rigidbody of whom to enable/disable physics.")]
        private Rigidbody rb;

        #region Unity Functions

        private void Start()
        {
            DisableRbPhysics();
        }

        #endregion

        /// <summary>
        ///     Enable the rigidbody physics
        /// </summary>
        public void EnableRbPhysics()
        {
            rb.isKinematic = false;
        }
        
        /// <summary>
        ///     Disable the rigidbody physics
        /// </summary>
        public void DisableRbPhysics()
        {
            rb.isKinematic = true;
        }
    }
}