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
        
        public GameObject Other { get; private set; }
        
        private void OnTriggerEnter(Collider other)
        {
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