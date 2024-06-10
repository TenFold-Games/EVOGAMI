using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace EVOGAMI.Interactable
{
    public class MovablePlatform : MonoBehaviour
    {
        [SerializeField] private Vector3 initialPosition;
        [SerializeField] private Vector3 displacement;
        [SerializeField] private float speed = 1f;
        
        private Vector3 _targetPosition;
        private Vector3 _moveTarget;
        
        private bool _isInTargetPosition;
        private bool _shouldMove;

        public void Start()
        {
            initialPosition = transform.position;
            _targetPosition = initialPosition + displacement;
            _moveTarget = _targetPosition;
            
            _isInTargetPosition = false;
            _shouldMove = false;
        }
        
        public void MoveToTarget()
        {
            _moveTarget = _targetPosition;
            _shouldMove = true;
        }
        
        public void MoveToInitial()
        {
            _moveTarget = initialPosition;
            _shouldMove = true;
        }

        private void FixedUpdate()
        {
            if (!_shouldMove) return;
            
            transform.position = Vector3.MoveTowards(transform.position, _moveTarget, speed * Time.fixedDeltaTime);

            if ((transform.position - _moveTarget).magnitude < 0.01f)
            {
                _shouldMove = false;
                _isInTargetPosition = !_isInTargetPosition;
            }
        }
    }
}