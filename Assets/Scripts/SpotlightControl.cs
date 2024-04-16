using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using JetBrains.Annotations;
using System.Collections;

public class SpotlightControl : MonoBehaviour
{
    // Reference to the spotlight object
    public Transform spotlightTransform;

    // Reference to the main camera's parent (player Capsule)
    public Transform mainCameraParentTransform;

    // Sensitivity of camera rotation
    public float rotationSpeed = 2f;

    // Limit for camera rotation
    public int maxRotationAngle = 45;

    // Limit for Light Rotation
    float maxLightAngle = -50f;

    // Sensitivity of light rotation
    float spotlightRotationSpeed = 2f;

    // Duration for the camera rotation coroutine
    public float rotationDuration = 0.5f;

    // Coroutine reference for camera rotation
    private Coroutine rotateCameraCoroutine;

    private PlayerMovement playerMovement;
    //public int itemsFound = 0;
    private GameObject currentTarget;
    public LayerMask layerMask;
    public Outline outline;

    public Material lightoff;
    public Material lighton;
    Renderer meshRenderer;

    private bool isCameraRotating = false;
    //ItemFoundEvent itemFoundEvent = new ItemFoundEvent();

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        mainCameraParentTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (playerMovement && playerMovement.isControllingSpotlight)
        {
            // Rotate the spotlight based on mouse input
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            float currentRotationY = transform.localRotation.y;
            //Debug.Log(currentRotationY);
            // Rotate the spotlight around the y-axis (horizontal rotation)

            if (isCameraRotating == false)
            {
                transform.Rotate(Vector3.up, mouseX);
                transform.Rotate(Vector3.right, -mouseY);
            }


            Raycast();
        }
    }

    // Function to clamp the rotation quaternion within specified limits
    Quaternion ClampRotation(Quaternion rotation, float minAngle, float maxAngle)
    {
        // Convert the quaternion rotation to Euler angles
        Vector3 euler = rotation.eulerAngles;

        // Clamp the rotation angles individually
        euler.x = Mathf.Clamp(euler.x, minAngle, maxAngle);
        euler.y = Mathf.Clamp(euler.y, minAngle, maxAngle);

        // Convert the clamped Euler angles back to a quaternion rotation
        return Quaternion.Euler(euler);
    }

    // Function to clamp an angle within specified limits
    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    void LimitSpotlightRotation()
    {
        // Define the maximum rotation angles for left, right, up, and down
        float maxLeftRotation = -45f;   // Maximum rotation to the left
        float maxRightRotation = 45f;   // Maximum rotation to the right
        float maxUpRotation = 45f;      // Maximum rotation upwards
        float maxDownRotation = -45f;   // Maximum rotation downwards

        // Get the current rotation angles
        Vector3 currentRotation = transform.localRotation.eulerAngles;

        // Clamp the rotation angles within the specified ranges
        float clampedXRotation = Mathf.Clamp(currentRotation.x, maxDownRotation, maxUpRotation);
        float clampedYRotation = Mathf.Clamp(currentRotation.y, maxLeftRotation, maxRightRotation);

        // Apply the clamped rotation angles
        transform.localRotation = Quaternion.Euler(clampedXRotation, clampedYRotation, 0f);
    }


    private GameObject previousTarget; // Store the previous target object

    private void Raycast()
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
            if (previousTarget != hit.transform.gameObject)
            {
                // If the current target object is different from the previous one, it means the ray is entering a new object
                OnRaycastEnter(hit.transform.gameObject);
                previousTarget = hit.transform.gameObject;
            }
            /*
            else if (currentTarget != hit.transform.gameObject)
            {
                OnRaycastExit(currentTarget);
                currentTarget = hit.transform.gameObject;
            }*/


            OnRaycast(hit.transform.gameObject);
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

        }

        else
        {
            // If the ray doesn't hit anything, reset the previous target
            if (previousTarget != null)
            {
                OnRaycastExit(previousTarget);
                previousTarget = null;
            }
        }

    }
    
    private bool rotatedLeft = false; // Flag to track if the camera has rotated left
    private bool rotatedRight = false;
    void OnRaycastEnter(GameObject target)
    {
        if (target.GetComponent<Outline>() != null)
        {
            outline = target.GetComponent<Outline>();
            outline.EnableOutline();
        }
        if (target.CompareTag("HiddenObject"))
        {
            target.GetComponentInChildren<LightSensor>().aimedAt = true;
        }
        if (target.CompareTag("Monster") || target.CompareTag("Monster2") || target.CompareTag("Monster3"))
        {
            //Debug.Log("Looking at Monster");
            target.GetComponent<MonsterBehavior>().Hide();
        }

        if (target.CompareTag("LeftLimit"))
        {
            // Rotate the camera's parent to the left only if it's not already rotated left
            if (!rotatedLeft)
            {
                RotateCameraParent(-rotationSpeed);
                rotatedLeft = true; // Set the flag to indicate that the camera has rotated left
            }
        }
        
        if (target.CompareTag("RightLimit"))
        {
            if (!rotatedRight)
            {
                RotateCameraParent(rotationSpeed);
                rotatedRight = true;
            }
        }

    }

    void OnRaycastExit(GameObject target)
    {
        if (target.CompareTag("HiddenObject"))
        {
            target.GetComponentInChildren<LightSensor>().aimedAt = false;
        }

        if (target.GetComponent<Outline>() != null)
        {
            outline = target.GetComponent<Outline>();
            outline.DisableOutline();
        }

        if (target.CompareTag("LeftLimit"))
        {
            // Rotate the camera's parent to the right when leaving the "LeftLimit" collider
            RotateCameraParent(rotationSpeed);
            rotatedLeft = false; // Reset the flag when leaving the "LeftLimit" collider
            
        }

        if (target.CompareTag("RightLimit"))
        {
            RotateCameraParent(-rotationSpeed);
            rotatedRight = false;
            
        }

    }

    void OnRaycast(GameObject target)
    {
        if (target.GetComponent<Outline>() != null)
        {
            outline = target.GetComponent<Outline>();
            outline.EnableOutline();
        }
        if (target.CompareTag("HiddenObject"))
        {
            // If the raycast hits the "Light Sensor", do nothing
            return;
        }

    }



    // Rotate the camera's parent transform (player capsule)
    void RotateCameraParent(float rotationAmount)
    {
        // Check if the camera is currently rotating
        if (!isCameraRotating)
        {
            // Calculate the target rotation based on the rotation amount
            Quaternion targetRotation = Quaternion.Euler(0, rotationAmount, 0) * mainCameraParentTransform.rotation;

            // Start a new coroutine to smoothly rotate the camera's parent
            rotateCameraCoroutine = StartCoroutine(RotateCameraCoroutine(targetRotation));
        }
    }

    // Coroutine to smoothly rotate the camera's parent transform
    IEnumerator RotateCameraCoroutine(Quaternion targetRotation)
    {
        // Set the flag to indicate that the camera is now rotating
        isCameraRotating = true;

        // Get the initial rotation and time
        Quaternion initialRotation = mainCameraParentTransform.rotation;
        float elapsedTime = 0f;

        // Interpolate between the initial rotation and target rotation gradually over time
        while (elapsedTime < rotationDuration)
        {
            // Calculate the interpolation factor based on elapsed time
            float t = elapsedTime / rotationDuration;

            // Interpolate between initial and target rotation
            mainCameraParentTransform.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final rotation is exactly the target rotation
        mainCameraParentTransform.rotation = targetRotation;

        // Reset the flag to indicate that the camera rotation is complete
        isCameraRotating = false;
    }



    /*void PanLeft()
    {
        // Calculate rotation amount based on hit position
        float targetRotationY = 30f;//Mathf.Atan2(hit.point.x - mainCameraTransform.position.x, hit.point.z - mainCameraTransform.position.z) * Mathf.Rad2Deg;

        // Apply rotation speed
        targetRotationY *= rotationSpeed * Time.deltaTime;

        // Clamp rotation within limits
        targetRotationY = Mathf.Clamp(targetRotationY, -maxRotationAngle, maxRotationAngle);

        // Smoothly rotate the camera towards the target rotation
        Quaternion targetRotation = Quaternion.Euler(0f, targetRotationY, 0f);
        mainCameraTransform.rotation = Quaternion.Slerp(mainCameraTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }*/


}
