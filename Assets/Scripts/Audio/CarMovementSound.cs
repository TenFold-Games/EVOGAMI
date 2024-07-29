using Unity.VisualScripting;
using UnityEngine;

public class CarMovementSound : MonoBehaviour
{
    [SerializeField]
    private Rigidbody carRigidbody; // Rigidbody component of the car

    [SerializeField]
    [Tooltip("The audio source for the collectable region")]
    private AudioSource audioSource; // AudioSource to play the sound when the car moves

    private Vector3 previousPosition;
    [SerializeField] private bool isMoving = false;

    private void OnTriggerEnter(Collider other)
    {
        audioSource.Play();
    }
}