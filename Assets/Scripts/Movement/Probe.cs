using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace EVOGAMI.Movement
{
    public class Probe : MonoBehaviour
    {
        [Header("Settings")]
        // The main camera.
        [SerializeField] [Tooltip("The main camera.")]
        private Camera mainCamera;
        // The maximum distance the object can be probed.
        [SerializeField] [Tooltip("The maximum distance the object can be probed.")]
        private float maxProbeDistance = 20f;
        // The radius of the sphere used to check if the object is within reach.
        [SerializeField] [Tooltip("The radius of the sphere used to check if the object is within reach.")]
        private float probeCheckRadius = 1f;
        // The layer mask used to check for objects that can be probed.
        [SerializeField] [Tooltip("The layer mask used to check for objects that can be probed.")]
        private LayerMask probeLayer;

        [Header("Callbacks")]
        // Event invoked on probe found.
        [SerializeField] [Tooltip("Event invoked on probe found.")]
        private ProbeEvent onProbeFound = new();
        // Event invoked on probe lost.
        [SerializeField] [Tooltip("Event invoked on probe lost.")]
        private ProbeEvent onProbeLost = new();
        
        /// <summary>
        ///     The event that is triggered when a probe is found or lost.
        /// </summary>
        [Serializable] public class ProbeEvent : UnityEvent<GameObject> {}
        
        /// <summary>
        ///     The callback for when a probe is found.
        /// </summary>
        public ProbeEvent ProbeFoundCallback => onProbeFound;
        /// <summary>
        ///     The callback for when a probe is lost.
        /// </summary>
        public ProbeEvent ProbeLostCallback => onProbeLost;

        private RaycastHit _probeHit;

        private GameObject _probedObject;
        // private GameObject _previousProbedObject;

        #region Physics

        private bool IsObjectProbed(Ray ray)
        {
            var found = Physics.SphereCast(ray, probeCheckRadius, out _probeHit, maxProbeDistance, probeLayer);
            // var found = Physics.Raycast(ray, out _probeHit, maxProbeDistance, probeLayer);

            // If hit too close, ignore
            // Debug.Log($"Found a hit with distance {_probeHit.distance}");
            return found && !(_probeHit.distance < probeCheckRadius);
        }

        private bool IsObjectProbed()
        {
            var found = Physics.SphereCast(
                transform.position, 
                probeCheckRadius, 
                mainCamera.transform.forward,
                out _probeHit, 
                maxProbeDistance, 
                probeLayer
            );
            
            // If hit too close, ignore
            // Debug.Log($"Found a hit with distance {_probeHit.distance}");
            return found && !(_probeHit.distance < probeCheckRadius);
        }
        
        // private bool IsPathBlocked(GameObject hitObject)
        private bool IsPathBlocked()
        {
            var blocked = Physics.Linecast(
                transform.position, 
                _probeHit.point,
                ~probeLayer & ~LayerMask.GetMask("Ignore Raycast")
            );
            Debug.DrawLine(transform.position, _probeHit.point, blocked ? Color.magenta : Color.blue);
            return blocked;
        }

        #endregion

        #region Unity Functions
        
        private void Start()
        {
            if (!mainCamera) mainCamera = Camera.main;
        }

        protected void FixedUpdate()
        {
            Scan();
        }

        #endregion

        private void Scan()
        {
            var ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            // if (IsObjectProbed(ray))
            // if (IsObjectProbed(ray) && !IsPathBlocked())
            if (IsObjectProbed() && !IsPathBlocked())
            {
                var hitObject = _probeHit.collider.gameObject;
                // if (IsPathBlocked(hitObject))
                // {
                //     if (!_probedObject) return;
                //
                //     // Invoke the probe lost event
                //     onProbeLost.Invoke(_probedObject);
                //     // _previousProbedObject = null;
                //     _probedObject = null;
                //     
                //     return;
                // }

                // Skip if the probed object is the same
                if (hitObject == _probedObject) return;

                // Cache the previous probed object
                if (_probedObject)
                {
                    // _previousProbedObject = _probedObject;
                    onProbeLost.Invoke(_probedObject);
                }

                // Set the probed object
                _probedObject = hitObject;
                onProbeFound.Invoke(_probedObject);
            }
            else
            {
                if (!_probedObject) return;

                // Invoke the probe lost event
                onProbeLost.Invoke(_probedObject);
                // _previousProbedObject = null;
                _probedObject = null;
            }
        }

        private void OnDrawGizmos()
        {
            if (!mainCamera) return;
            
            Gizmos.color = _probedObject ? Color.green : Color.red;
            // var ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            // Gizmos.DrawRay(ray.origin, ray.direction * maxProbeDistance);
            Gizmos.DrawRay(transform.position, mainCamera.transform.forward * maxProbeDistance);
            
            Gizmos.color = _probedObject ? Color.green : Color.red;
            // Gizmos.DrawWireSphere(ray.origin + ray.direction * maxProbeDistance, probeCheckRadius);
            Gizmos.DrawWireSphere(transform.position + mainCamera.transform.forward * maxProbeDistance, probeCheckRadius);
            
            // Draw a wire sphere surrounding the player, radius = 2.5
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, probeCheckRadius);
        }
    }
}