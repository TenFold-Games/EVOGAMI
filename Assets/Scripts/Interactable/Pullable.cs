using UnityEngine;

namespace EVOGAMI.Interactable
{
    public class Pullable : MonoBehaviour
    {
        [SerializeField] [Tooltip("The maximum distance the object can be pulled. Set to 0 to disable.")]
        private float maxDistance = 1.5f;
        
        private Transform _sourceTransform;
        private float _distanceTravelled;
        
        public void Pull(float speed)
        {
            if (maxDistance != 0 && _distanceTravelled >= maxDistance) return;
            
            // Move the object towards the source transform.
            transform.position = Vector3.MoveTowards(
                transform.position,
                _sourceTransform.position,
                speed
            );
            _distanceTravelled += speed;

        }
        
        public void SetPullSource(Transform sourceTransform)
        {
            _sourceTransform = sourceTransform;
        }
    }
}