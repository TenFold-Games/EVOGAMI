using System.Collections;
using EVOGAMI.Origami;
using UnityEngine;
using UnityEngine.Serialization;
using Cinemachine;
using EVOGAMI.Core;


namespace EVOGAMI.Movement
{
    public class HumanLocomotion : LocomotionBase
    {
        [SerializeField] private Animator animator;  // Ensure this is assigned in the Unity Editor
        
        [Header("Obstacle Check")]
        [SerializeField] private Transform obstacleCheck;
        
        [SerializeField] private float obstacleCheckLength = 0.3f;
        [SerializeField] private float obstacleCheckRadius = 0.2f;
        private RaycastHit _obstacleHit;
           
        [Header("Vaulting")]
        [SerializeField] private float vaultSpeed = .75f;
        private bool isVaulting = false;
        
        [Header("PickUp and Drop")]
        //reference to your hands/the position where you want your object to go
        [SerializeField] public GameObject myHands; 
        
        //a bool to see if you can or cant pick up the item
        [SerializeField] private bool canpickup;
        
        // the game object on which you collided with
        [SerializeField] private GameObject objectIwantToPickUp;
        
        // a bool to see if you have an item in your hand
        [SerializeField] private bool hasItem;
        
        // Bool to prevent multiple inputs for picking up
        private bool isPickingUp = false;
        
        private void Awake()
        {
            // Set the form
            form = OrigamiContainer.OrigamiForm.Human;
        }
            
        protected override void Start()
        {
            base.Start();
            if (!animator)
            {
                animator = GetComponent<Animator>();
            }
            
            canpickup = false;
            hasItem = false;
            // Debug.Log("Start: canpickup = " + canpickup + ", hasItem = " + hasItem);
            
            // Set the pick-up and drop callbacks
            InputManager.OnPickUpPerformed += PickUpObject;
            InputManager.OnDropPerformed += DropObject;
        }
        
        protected override void FixedUpdate()
        {
            if (!isVaulting)
            {
                base.FixedUpdate();
                ManageAnimations(Time.deltaTime);
            }
            
            /*
            if (canpickup)
            {
                Debug.Log("FixedUpdate: canpickup is true ");
                if (Input.GetKeyDown("e")) // Change key if necessary
                {
                    Debug.Log("FixedUpdate: detected key e pressed ");
                    objectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = true;
                    objectIwantToPickUp.transform.position = myHands.transform.position;
                    objectIwantToPickUp.transform.parent = myHands.transform;
                    hasItem = true;
                }
            }
            if (Input.GetKeyDown("q") && hasItem) // Change key if necessary
            {
                Debug.Log("FixedUpdate: detected key q pressed");
                objectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = false;
                objectIwantToPickUp.transform.parent = null;
                hasItem = false;
            }
            */
        }
        
        #region Input Events
        
        protected override void OnJumpPerformed()
        {
            // Check if an obstacle is in front and vault if detected
            if (DetectObstacle())
            {
                return;
            }

            // If no obstacle is detected, perform the regular jump
            base.OnJumpPerformed();
        }
        
        #endregion
        
        #region Vaulting

        /// <summary>
        ///     Vault over obstacle.
        /// </summary>
        /// <param name="obstacle">The time between frames</param>
        private void Vault(Collider obstacle)
        {
            // Disable player control during vault: DisablePlayerControl();

            // Calculate the target position for vaulting.
            Vector3 vaultTarget = obstacle.transform.position + new Vector3(0, obstacle.bounds.size.y + 0.5f, 0); 

            StartCoroutine(VaultRoutine(vaultTarget));
        }
        
        private IEnumerator VaultRoutine(Vector3 targetPosition)
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = PlayerTransform.position;
            
            // Trigger vault animation here
            // animator.SetTrigger("Vault");

            while (elapsedTime < vaultSpeed)
            {
                PlayerTransform.position = Vector3.Lerp(startingPosition, targetPosition, (elapsedTime / vaultSpeed));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            PlayerTransform.position = targetPosition;

            // Re-enable player control after vault: EnablePlayerControl();
            
            // Reset vault animation here
            // animator.ResetTrigger("Vault");
        }
        
        #endregion
        
        #region Grab and Drop items

        private void PickUpObject()
        {
            // Nothing to pick up
            if (!objectIwantToPickUp) return; 
            // Prevent multiple inputs
            if (!canpickup || isPickingUp) return;
            
            StartCoroutine(PickUpObjectCoroutine());
        }
        
        private IEnumerator PickUpObjectCoroutine()
        {
            isPickingUp = true;
            objectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = true;
            objectIwantToPickUp.transform.position = myHands.transform.position;
            objectIwantToPickUp.transform.parent = myHands.transform;
            hasItem = true;
            // Debug.Log("Item picked up successfully.");
            yield return new WaitForSeconds(0.2f); // Add a slight delay to debounce input
            isPickingUp = false;
        }

        private void DropObject()
        {
            // Nothing to drop
            if (!hasItem || !objectIwantToPickUp) return;
            
            objectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = false;
            objectIwantToPickUp.transform.parent = null;
            hasItem = false;
            // Debug.Log("Item dropped successfully.");
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // Debug.Log("Entered trigger with object tagged: " + other.gameObject.tag);
            if (other.gameObject.CompareTag("PickUp"))
            {
                // Debug.Log("Entered trigger with a movable object.");
                canpickup = true;
                objectIwantToPickUp = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Debug.Log("Exited trigger with object tagged: " + other.gameObject.tag);
            if (other.gameObject.CompareTag("PickUp"))
            {
                // Debug.Log("Exited trigger with a movable object.");
                canpickup = false;
                objectIwantToPickUp = null;
            }
        }
        #endregion
        
        #region Collision

        /// <summary>
        ///     Check if the player is in front of a vaultable obstacle.
        /// </summary>
        private bool DetectObstacle()
        {
            LayerMask obstacleLayer = LayerMask.GetMask("Ground", "Default", "Wall");
            // Debug.Log("DetectObject: sphere cast to detect obstacle");
            if (Physics.SphereCast(obstacleCheck.position, obstacleCheckRadius, obstacleCheck.forward, out _obstacleHit, obstacleCheckLength))
            {
                if (_obstacleHit.collider.CompareTag("obstacle"))
                {
                    Vault(_obstacleHit.collider);
                    return true;
                    // float obstacleHeight = _obstacleHit.collider.bounds.size.y;
                    // float playerHeight = GetComponent<CapsuleCollider>().height;
                    
                    // if (obstacleHeight <= (1.0f / 3.0f) * playerHeight)
                    // {
                        // Debug.Log("Obstacle detected and suitable for vaulting.");
                        //Vault(_obstacleHit.collider);
                        // return true;
                    // }
                }
            }
            return false;
        }
        
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(obstacleCheck.position, obstacleCheckRadius);
            Gizmos.DrawLine(obstacleCheck.position, obstacleCheck.position + obstacleCheck.forward * obstacleCheckLength);
        }
        
        #endregion

        #region Animation

        [SerializeField] private Animator animate;
        
        private void ManageAnimations(float delta)
        {
            // animator.SetBool("isWalk", InputManager.IsMoving);  // Directly set the 'isWalk' animation parameter
            float horizontal = InputManager.MoveInput.magnitude;
            float vertical = PlayerManager.PlayerRb.velocity.y; 
            
            animator.SetFloat("vertical", vertical, 0.1f, delta);
            animator.SetFloat("horizontal", horizontal, 0.1f, delta);
        }
        

        #endregion
    }
}
