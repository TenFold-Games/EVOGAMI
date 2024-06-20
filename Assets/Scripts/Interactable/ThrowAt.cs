using EVOGAMI.Core;
using EVOGAMI.Movement;
using UnityEngine;

namespace EVOGAMI.Interactable
{
    public class ThrowAt : MonoBehaviour
    {
        // Managers
        private PlayerManager _playerManager;
        
        // The force applied to the player when they are hit by the ninja star.
        [SerializeField] [Tooltip("The force applied to the player when they are hit by the ninja star.")]
        private float bounceForce = 2f;
        
        private void Start()
        {
            _playerManager = PlayerManager.Instance;
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (!other.collider.CompareTag("OrigamiMesh") && !other.collider.CompareTag("Player")) return;

            var ninjaStar = other.collider.GetComponent<NinjaStar>();
            if (ninjaStar == null) return; // Should never happen.

            ninjaStar.ExitThrowState();
            
            // Add a small backwards force to the player.
            _playerManager.PlayerRb.AddForce(-_playerManager.Player.transform.forward * bounceForce, ForceMode.Impulse);
            
            ninjaStar.EnterAimState();
        }
    }
}