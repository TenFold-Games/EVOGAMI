using UnityEngine;

namespace EVOGAMI.Interactable
{
    /// <summary>
    ///     Launches an object in a specified direction with a specified force
    /// </summary>
    public class ObjectLauncher : MonoBehaviour
    {
        [Header("Settings")]
        // The rigidbody of the object to be launched
        [SerializeField] [Tooltip("The rigidbody of the object to be launched")]
        private Rigidbody rb;
        // The force to launch the object with
        [SerializeField] [Tooltip("The force to launch the object with")]
        private float launchForce = 10f;
        // The direction to launch the object in
        [SerializeField] [Tooltip("The direction to launch the object in")]
        private Vector3 launchDirection;

        /// <summary>
        ///     Set the object to be launched
        /// </summary>
        /// <param name="other">The object to be launched</param>
        public void SetObject(GameObject other)
        {
            rb = other ? other.GetComponent<Rigidbody>() : null;
        }

        /// <summary>
        ///     Launch the object
        /// </summary>
        public void Launch()
        {
            if (!rb) return; // Should not happen

            // Add launch force to the object
            rb.AddForce(CalculateLaunchVector(), ForceMode.Impulse);
        }

        /// <summary>
        ///     Calculate the launch vector based on the launch direction and force
        /// </summary>
        /// <returns>The launch vector</returns>
        private Vector3 CalculateLaunchVector()
        {
            return launchDirection.normalized * launchForce;
        }
    }
}