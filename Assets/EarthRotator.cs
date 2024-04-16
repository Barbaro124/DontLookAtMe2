using UnityEngine;

public class EarthRotatorZ : MonoBehaviour
{
    [SerializeField]
    public float rotationSpeed = 10.0f; // Rotation speed in degrees per second

    void Update()
    {
        Quaternion deltaRotation = Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);
        transform.rotation = transform.rotation * deltaRotation;
    }
}
