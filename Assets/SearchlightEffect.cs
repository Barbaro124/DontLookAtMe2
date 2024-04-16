using UnityEngine;

public class SearchlightEffect : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 30.0f; // Speed of rotation
    [SerializeField]
    private float angleRange = 45.0f; // Range of rotation to either side of the initial angle

    private Quaternion startRotation;
    private float currentAngle = 0;

    void Start()
    {
        // Record the initial rotation of the object
        startRotation = transform.rotation;
    }

    void Update()
    {
        // Calculate rotation using a sine wave to oscillate back and forth around the initial rotation
        currentAngle = Mathf.Sin(Time.time * rotationSpeed) * angleRange;
        transform.rotation = startRotation * Quaternion.Euler(0, currentAngle, 0);
    }
}
