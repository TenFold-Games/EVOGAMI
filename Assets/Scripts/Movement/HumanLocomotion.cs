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
        }
        
        protected override void FixedUpdate()
        {
            if (!isVaulting)
            {
                base.FixedUpdate();
                ManageAnimations(Time.deltaTime);
            }
            
            if (canpickup)
            {
                if (Input.GetKeyDown("e"))
                {
                    objectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = true;
                    objectIwantToPickUp.transform.position = myHands.transform.position;
                    objectIwantToPickUp.transform.parent = myHands.transform;
                    hasItem = true;
                }
            }
            if (Input.GetKeyDown("q") && hasItem)
            {
                objectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = false;
                objectIwantToPickUp.transform.parent = null;
                hasItem = false;
            }
            
        }
        
        #region Input Events
        
        protected override void OnJumpPerformed()
        {
            // Check if an obstacle is in front and vault if detected
            if (DetectObject())
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
        
        #region Collision

        /// <summary>
        ///     Check if the player is in front of a vaultable obstacle.
        /// </summary>
        private bool DetectObject()
        {
            LayerMask obstacleLayer = LayerMask.GetMask("Ground", "Default", "Wall");
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

                if (_obstacleHit.collider.CompareTag("movable"))
                {
                    canpickup = true;
                    objectIwantToPickUp = _obstacleHit.collider.gameObject;
                    // objectIwantToPickUp = hit.collider.gameObject;
                }
                else
                {
                    canpickup = false;
                    objectIwantToPickUp = null;
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

/* original
private bool DetectObstacles()
   {
       LayerMask obstacleLayer = LayerMask.GetMask("Ground", "Default", "Wall");
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
*/

/*
 ARCHIVED METHODS 2 Disable/Enable player control
 
        private void DisablePlayerControl()
        {
            isVaulting = true;

            // Disable player input.
            // InputManager.Instance.Controls.Disable();

            // Disable player movement input.
            InputManager.Instance.Controls.Player.Disable();
        }

        private void EnablePlayerControl()
        {
            isVaulting = false;

            // Re-enable player input.
            // InputManager.Instance.Controls.Enable();

            // Re-enable player movement input.
            InputManager.Instance.Controls.Player.Enable();
        }
*/

/*
    [Header("Grab and Drop")]
   [SerializeField] private Transform playerCameraTransform;
   [SerializeField] private Transform objectGrabPointTransform;
   [SerializeField] private LayerMask pickUpLayerMask;

   private ObjectGrabbable objectGrabbable;
   
   <FixedUpdate>
   
   if (Input.GetKeyDown(KeyCode.E)) {
       Debug.Log("E command received. Started pickup");
       if (objectGrabbable == null) {
           // Not carrying an object, try to grab
           Debug.Log("Attempting pick up");
           float pickUpDistance = 4f;
           if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask)) {
               Debug.Log("Raycast successful");
               if (raycastHit.transform.TryGetComponent(out objectGrabbable)) {
                   objectGrabbable.Grab(objectGrabPointTransform);
                   Debug.Log("Pick Up successful");
               }
           }
           else
           {
               Debug.Log("Pick Up failed.");
           }
           //Debug.Log("Pick Up failed.");
       } else {
           Debug.Log("Raycast did not hit any object");
           // Currently carrying something, drop
           objectGrabbable.Drop();
           objectGrabbable = null;
       }
   }
   
#region Grab and Drop items

   private void GrabObject()
   {
       
   }

   private void DropObject()
   {
       
   }
#endregion

*/

        /*
        [Header("PickUp and Drop Items")]
        [SerializeField] private LayerMask movableLayerMask;
        [SerializeField] private Transform playerCameraTransform;

        // text: "use E key to pick up objects"
        // [SerializeField] GameObject pickUpUI;

        [SerializeField]
        [Min(1)]
        private float hitRange = 3;

        private RaycastHit hit;

        private void Update()
        {
            Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * hitRange, Color.red);
            if (hit.collider != null)
            {
                hit.collider.GetComponent<Highlight>()?.ToggleHighlight(false);
                // pickUpUI.SetActive(false);
            }

            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, hitRange,
                    movableLayerMask))
            {
                hit.collider.GetComponent<Highlight>()?.ToggleHighlight(true);
                // pickUpUI.SetActive(true);
            }
        }
        */
