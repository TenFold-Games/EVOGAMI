using UnityEngine;
using UnityEngine.Events;

public class CarMovement : MonoBehaviour
{
    public UnityEvent onStartMoving;

    [SerializeField]
    private Rigidbody carRigidbody;
    private Vector3 previousPosition;
    [SerializeField] private bool isMoving = false;

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        bool currentlyMoving = Vector3.Distance(previousPosition, transform.position) > 0.01f;
        if (currentlyMoving && !isMoving)
        {
            isMoving = true;
            onStartMoving?.Invoke(); // Trigger the event
        }
        else if (!currentlyMoving && isMoving)
        {
            isMoving = false;
        }

        previousPosition = transform.position;
    }
}