using UnityEngine;

namespace EVOGAMI.Interactable
{
    /// <summary>
    ///     Movable platform that moves between two positions
    /// </summary>
    public class MovablePlatform : MonoBehaviour
    {
        [Header("Settings")]
        // Initial position and displacement
        [SerializeField] [Tooltip("Initial position of the platform")]
        private Vector3 initialPosition;
        // Displacement from initial position
        [SerializeField] [Tooltip("Displacement from initial position")]
        private Vector3 displacement;
        // Speed of the platform
        [SerializeField] [Tooltip("Speed of the platform")]
        private float speed = 1f;
        
        private Vector3 _targetPosition;
        private Vector3 _moveTarget;
        
        // Flags
        private bool _isInTargetPosition;
        private bool _shouldMove;

        #region Unity Functions

        public void Start()
        {
            // Initialize flags
            _isInTargetPosition = false;
            _shouldMove = false;

            // Calculate target position
            initialPosition = transform.position;
            _targetPosition = initialPosition + displacement;
            _moveTarget = _targetPosition;
        }
        
        private void FixedUpdate()
        {
            if (!_shouldMove) return;
            
            // Move towards target position
            transform.position = Vector3.MoveTowards(transform.position, _moveTarget, speed * Time.fixedDeltaTime);

            if ((transform.position - _moveTarget).magnitude < 0.01f) // Reached target position
            {
                _shouldMove = false;
                _isInTargetPosition = !_isInTargetPosition;
            }
        }

        #endregion

        #region Move Platform

        /// <summary>
        ///     Move the platform to the target position
        /// </summary>
        public void MoveToTarget()
        {
            _moveTarget = _targetPosition;
            _shouldMove = true;
        }

        /// <summary>
        ///     Move the platform to the initial position
        /// </summary>
        public void MoveToInitial()
        {
            _moveTarget = initialPosition;
            _shouldMove = true;
        }

        #endregion
    }
}