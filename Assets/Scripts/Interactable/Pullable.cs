using System;
using UnityEngine;

namespace EVOGAMI.Interactable
{
    public class Pullable : MonoBehaviour
    {
        [Header("Settings")]
        // The maximum distance the object can be pulled. Set to 0 to disable.
        [SerializeField] [Tooltip("The maximum distance the object can be pulled. Set to 0 to disable.")]
        private float maxDistance = 1.5f;
        // Whether to prevent pulling the object up or down.
        [SerializeField] [Tooltip("Whether to prevent pulling the object up or down.")]
        private bool preventVerticalMovement = true;
        // The forward direction when being pulled. Set to <0, 0, 0> to disable.
        [SerializeField] [Tooltip("The forward direction when being pulled. Set to <0, 0, 0> to disable.")]
        private Vector3 forwardDirection;
        
        private Transform _sourceTransform;
        private Vector3 _targetPosition;
        private float _distanceTravelled;
        
        public void Pull(float speed)
        {
            if (maxDistance != 0 && _distanceTravelled >= maxDistance) return;
            float angularVelocity = GetAngularVelocity(speed, transform.position);
            
            // Move the object towards the source transform.
            transform.position = Vector3.MoveTowards(
                transform.position,
                _targetPosition,
                angularVelocity
            );

            _distanceTravelled += angularVelocity;
        }
        
        public void SetPullSource(Transform sourceTransform)
        {
            _sourceTransform = sourceTransform;
            _targetPosition = GetTargetPosition();
        }

        #region Pull Direction Physics

        private Vector3 GetTargetPosition()
        {
            var targetPosition = forwardDirection == Vector3.zero ? _sourceTransform.position : forwardDirection + transform.position;
            targetPosition.y = transform.position.y; // Prevent the object from moving up or down.
            return targetPosition;
        }
        
        private float GetAngularVelocity(float speed, Vector3 currentPosition)
        {
            // Calculate the speed based on the angle between the source and current position.
            // When the angle is 0, the speed is the same as the input speed.
            // When the angle is 90, the speed is 0.
            var angle = Vector3.Angle(_sourceTransform.position - currentPosition, _targetPosition - currentPosition);
            return speed * Mathf.Cos(angle * Mathf.Deg2Rad);
        }

        #endregion

        #region Unity Functions

        private void OnDrawGizmos()
        {
            if (forwardDirection == Vector3.zero) return;
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + forwardDirection);
        }

        #endregion
    }
}