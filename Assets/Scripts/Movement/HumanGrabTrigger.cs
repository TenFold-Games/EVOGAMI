using UnityEngine;

namespace EVOGAMI.Movement
{
    [RequireComponent(typeof(Collider))]
    public class HumanGrabTrigger : MonoBehaviour
    {
        public delegate void TriggerCallback(GameObject obj);
        
        public event TriggerCallback TriggerEnterCallback = delegate { };
        public event TriggerCallback TriggerStayCallback = delegate { };
        public event TriggerCallback TriggerExitCallback = delegate { };
        
        private void OnTriggerEnter(Collider other)
        {
            TriggerEnterCallback.Invoke(other.gameObject);
        }
        
        private void OnTriggerStay(Collider other)
        {
            TriggerStayCallback.Invoke(other.gameObject);
        }
        
        private void OnTriggerExit(Collider other)
        {
            TriggerExitCallback.Invoke(other.gameObject);
        }
    }
}