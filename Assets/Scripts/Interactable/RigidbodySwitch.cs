using System;
using UnityEngine;

namespace EVOGAMI.Interactable
{
    public class RigidbodySwitch : MonoBehaviour
    {
        [SerializeField] private Rigidbody m_Rigidbody;

        private void Awake()
        {
            if (m_Rigidbody == null) m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            DisableRb();
        }

        public void EnableRb()
        {
            // Enable the rigidbody
            m_Rigidbody.isKinematic = false;
        }
        
        public void DisableRb()
        {
            // Disable the rigidbody
            m_Rigidbody.isKinematic = true;
        }
    }
}