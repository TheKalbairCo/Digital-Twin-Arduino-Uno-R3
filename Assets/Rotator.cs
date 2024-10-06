using UnityEngine;

public class Rotator : MonoBehaviour
{
    Rigidbody rb; // Reference to the Rigidbody component
    [SerializeField] private float rotationSpeed = 100f; // Speed of rotation

    bool dragging = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }
    void OnMouseDrag()
    {
        dragging = true; // Set dragging to true when the mouse is being dragged
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) // When the left mouse button is released
        {
            dragging = false; // Stop dragging
        }
    }

    void FixedUpdate()
    {
        if (dragging) // If currently dragging
        {
            float x = Input.GetAxis("Mouse X") * rotationSpeed * Time.fixedDeltaTime;
            float y = Input.GetAxis("Mouse Y") * rotationSpeed * Time.fixedDeltaTime;
           
            rb.AddTorque(Vector3.up * x); // Apply torque to rotate the model around the Y-axis
            rb.AddTorque(Vector3.right * y);
        }
    }
}
