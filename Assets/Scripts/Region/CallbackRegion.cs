using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace EVOGAMI.Region
{
    public class CallbackRegion : MonoBehaviour
    {
        [Serializable] public class TriggerEnterCallback : UnityEvent {}
        [Serializable] public class TriggerStayCallback : UnityEvent {}
        [Serializable] public class TriggerExitCallback : UnityEvent {}
        
        [SerializeField] private TriggerEnterCallback m_onTriggerEnter = new();
        [SerializeField] private TriggerStayCallback m_onTriggerStay = new();
        [SerializeField] private TriggerExitCallback m_onTriggerExit = new();
        
        public TriggerEnterCallback TriggerEnter => m_onTriggerEnter;
        public TriggerStayCallback TriggerStay => m_onTriggerStay;
        public TriggerExitCallback TriggerExit => m_onTriggerExit;

        [SerializeField] private LayerMask layerMask = 0;
        [SerializeField] private string otherTag = "Untagged";
        
        public GameObject Other { get; private set; }

        private bool ConditionMet(Collider other)
        {
            if (layerMask != 0)
            {
                Debug.Log(other.gameObject.layer);
                if ((layerMask & (1 << other.gameObject.layer)) == 0) return false;
            }
            
            Debug.Log(other.gameObject.tag);
            if (otherTag != "Untagged" && !string.IsNullOrEmpty(otherTag))
            {
                if (!other.gameObject.CompareTag(otherTag)) return false;
            }
            
            return true;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!ConditionMet(other)) return;
            
            Debug.Log("Trigger Enter");
            Other = other.gameObject;
            m_onTriggerEnter.Invoke();
        }
        
        private void OnTriggerStay(Collider other)
        {
            Other = other.gameObject;
            m_onTriggerStay.Invoke();
        }
        
        private void OnTriggerExit(Collider other)
        {
            Other = other.gameObject;
            m_onTriggerExit.Invoke();
        }
    }
}