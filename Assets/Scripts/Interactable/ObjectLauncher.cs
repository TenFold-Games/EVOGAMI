using UnityEngine;

namespace EVOGAMI.Interactable
{
    public class ObjectLauncher : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float launchForce = 10f;
        [SerializeField] private Vector3 launchDirection;
        
        public void Launch()
        {
            // Freeze the object's rotation
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            // Add launch force to the object
            rb.AddForce(CalculateLaunchVector(), ForceMode.Impulse);
        }
        
        private Vector3 CalculateLaunchVector()
        {
            return launchDirection.normalized * launchForce;
        }
    }
}