using UnityEngine;

public class EarthRotatorY : MonoBehaviour
{
    [SerializeField]
    public float rotationSpeed = 10.0f; // Rotation speed in degrees per second, editable in the Inspector

    void Update()
    {
        Quaternion deltaRotation = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
        transform.rotation = transform.rotation * deltaRotation;
    }
}
