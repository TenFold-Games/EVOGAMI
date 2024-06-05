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
        [SerializeField] private float detectionRange = 0.1f;
        // [SerializeField] private LayerMask obstacleLayer;
        private RaycastHit _obstacleHit;
        // private bool _isTouchingObstacle;
           
        [Header("Vaulting")]
        [SerializeField] private float vaultSpeed = .75f;
        private bool isVaulting = false;
        
        private void Awake()
        {
            // Set the form
            form = OrigamiContainer.OrigamiForm.Human;
            
            // Bind input actions
            // InputManager.Instance.Controls.Player.Jump.performed += ctx => OnJumpPerformed();
        }
            
        protected override void Start()
        {
            base.Start();
            if (!animator)
            {
                animator = GetComponent<Animator>();
            }
        }
        
        protected override void FixedUpdate()
        {
            // base.FixedUpdate();
            // ManageAnimations(Time.deltaTime);
            
            if (!isVaulting)
            {
                base.FixedUpdate();
                // GroundCheck();
                // DetectObstacles();
                ManageAnimations(Time.deltaTime);
            }

            // DetectObstacles();
            // Debug.Log(isObstacleDetected);

            // If human (hand) touching obstacle => ???
            // if (_isTouchingObstacle)
        }
        
        #region Input Events
        
        protected override void OnJumpPerformed()
        {
            // Check if an obstacle is in front and vault if detected
            if (DetectObstacles())
            {
                return;
            }

            // If no obstacle is detected, perform the regular jump
            base.OnJumpPerformed();
        }
        
        #endregion
        
        #region Vaulting
        
        // TODO: should param delta be included?
        // <param name="delta">The time between frames</param>

        /// <summary>
        ///     Vault over obstacle.
        /// </summary>
        /// <param name="obstacle">The time between frames</param>
        private void Vault(Collider obstacle)
        {
            // Disable player control during vault.
            //DisablePlayerControl();

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

            // Re-enable player control after vault.
            // EnablePlayerControl();
            
            // Reset vault animation here
            // animator.ResetTrigger("Vault");
        }
        
        /*
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
        #endregion
        
        #region Collision

        private bool isObstacleDetected = false;
        
        /// <summary>
        ///     Check if the player is in front of a vaultable obstacle.
        /// </summary>
        private bool DetectObstacles()
        {
            LayerMask obstacleLayer = LayerMask.GetMask("Ground", "Default", "Wall");
            if (Physics.SphereCast(obstacleCheck.position, obstacleCheckRadius, obstacleCheck.forward, out _obstacleHit, obstacleCheckLength))
            // if (
            //     Physics.SphereCast(obstacleCheck.position, obstacleCheckRadius, obstacleCheck.forward, out _obstacleHit, detectionRange, obstacleLayer) ||
            //     Physics.SphereCast(obstacleCheck.position, obstacleCheckRadius, -obstacleCheck.forward, out _obstacleHit, detectionRange, obstacleLayer) ||
            //     Physics.SphereCast(obstacleCheck.position, obstacleCheckRadius, -obstacleCheck.right, out _obstacleHit, detectionRange, obstacleLayer) ||
            //     Physics.SphereCast(obstacleCheck.position, obstacleCheckRadius, obstacleCheck.right, out _obstacleHit, detectionRange, obstacleLayer)
            //     )
            {
                // TODO: create "obstacle" tag 
                if (_obstacleHit.collider.CompareTag("obstacle"))
                {
                    // float obstacleHeight = _obstacleHit.collider.bounds.size.y;
                    // float playerHeight = GetComponent<CapsuleCollider>().height;
                    
                    // if (obstacleHeight <= (1.0f / 3.0f) * playerHeight)
                    // {
                        // Debug.Log("Obstacle detected and suitable for vaulting.");
                        Vault(_obstacleHit.collider);
                        isObstacleDetected = true;
                        return true;
                    // }
                }
            }
            isObstacleDetected = false;
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

        /*
        [SerializeField] private Animator animate;  // Ensure this is assigned in the Unity Editor

        protected override void Start()
        {
            base.Start();
            if (!animator)
            {
                animator = GetComponent<Animator>();
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            ManageAnimations(Time.deltaTime);
        }

        private void ManageAnimations(float delta)
        {
            // animator.SetBool("isWalk", InputManager.IsMoving);  // Directly set the 'isWalk' animation parameter
            float horizontal = InputManager.MoveInput.magnitude;
            float vertical = PlayerManager.PlayerRb.velocity.y;

            animator.SetFloat("vertical", vertical, 0.1f, delta);
            animator.SetFloat("horizontal", horizontal, 0.1f, delta);

        }
        */
        
        /*
        #region Input Events
           protected override void OnJumpPerformed()
           {
               // Check if an obstacle is in front and vault if detected
               if (Physics.Raycast(obstacleCheck.position, obstacleCheck.forward, out _obstacleHit, detectionRange))
               {
                   if (_obstacleHit.collider.CompareTag("obstacle"))
                   {
                       float obstacleHeight = _obstacleHit.collider.bounds.size.y;
                       float playerHeight = GetComponent<CapsuleCollider>().height;

                       if (obstacleHeight <= (1.0f / 3.0f) * playerHeight)
                       {
                           Vault(_obstacleHit.collider);
                           return;
                       }
                   }
               }

               // If no obstacle is detected, perform the regular jump
               if (IsGrounded)
               {
                   PlayerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
               }
           }
           #endregion
        */