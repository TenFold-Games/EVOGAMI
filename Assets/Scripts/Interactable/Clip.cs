using EVOGAMI.Region;
using UnityEngine;

namespace EVOGAMI.Interactable
{
    public class Clip : MonoBehaviour
    {
        [SerializeField] private CallbackRegion ballDetector;
        [SerializeField] private CallbackRegion playerDetector;
        
        private ObjectLauncher _objectLauncher;
        
        public bool _ballInPlace = false;
        
        private void Awake()
        {
            ballDetector.TriggerEnter.AddListener(OnBallEnter);
            ballDetector.TriggerStay.AddListener(OnBallStay);
            ballDetector.TriggerExit.AddListener(OnBallExit);
            
            playerDetector.TriggerEnter.AddListener(OnPlayerEnter);
            
            _objectLauncher = GetComponent<ObjectLauncher>();
        }
        
        private void OnBallEnter()
        {
            if (ballDetector.Other.layer != LayerMask.NameToLayer("Launchable")) return;
            
            _ballInPlace = true;
            _objectLauncher.SetBall(ballDetector.Other);
        }
        
        private void OnBallStay()
        {
            if (ballDetector.Other.layer == LayerMask.NameToLayer("Launchable")) _ballInPlace = true;
        }
        
        private void OnBallExit()
        {
            if (ballDetector.Other.layer == LayerMask.NameToLayer("Launchable")) _ballInPlace = false;
        }
        
        private void OnPlayerEnter()
        {
            if (!_ballInPlace) return;
            
            _objectLauncher.Launch();
        }
    }
}