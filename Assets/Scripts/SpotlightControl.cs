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
    public float maxRotationAngle = 45f;

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

    //ItemFoundEvent itemFoundEvent = new ItemFoundEvent();

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        //mainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {
        if (playerMovement && playerMovement.isControllingSpotlight)
        {
            // Rotate the spotlight based on mouse input
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up, mouseX);

            float mouseY = Input.GetAxis("Mouse Y");
            transform.Rotate(Vector3.right, -mouseY);


            Raycast();
        }
    }


    private GameObject previousTarget; // Store the previous target object

    private void Raycast()
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
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

    void OnRaycastEnter(GameObject target)
    {
        if (target.GetComponent<Outline>() != null)
        {
            outline = target.GetComponent<Outline>();
            outline.EnableOutline();
        }
        if (target.CompareTag("HiddenObject"))
        {
            target.GetComponentInChildren<ChangeMaterial>().LightOn();
        }
        if (target.CompareTag("Monster") || target.CompareTag("Monster2") || target.CompareTag("Monster3"))
        {
            //Debug.Log("Looking at Monster");
            target.GetComponent<MonsterBehavior>().Hide();
        }

        if (target.CompareTag("LeftLimit"))
        {
            Debug.Log("LeftLimit Hit!");
            RotateCameraParent(-rotationSpeed);
        }
        // Do something with the object that was hit by the raycast.

    }

    void OnRaycastExit(GameObject target)
    {
        if (target.GetComponent<Outline>() != null)
        {
            outline = target.GetComponent<Outline>();
            outline.DisableOutline();
        }
        if (target.CompareTag("Monster") || target.CompareTag("Monster2") || target.CompareTag("Monster3"))
        {
            //Debug.Log("Looked away from Monster");
        }
        // Do something with the object that was exited by the raycast.
    }

    void OnRaycast(GameObject target)
    {
        if (target.GetComponent<Outline>() != null)
        {
            outline = target.GetComponent<Outline>();
            outline.EnableOutline();
        }



    }
    
    // Rotate the camera's parent transform (player capsule)
    void RotateCameraParent(float rotationAmount)
    {
        // Stop any existing camera rotation coroutine
        if (rotateCameraCoroutine != null)
        {
            StopCoroutine(rotateCameraCoroutine);
        }

        // Start a new coroutine to smoothly rotate the camera's parent
        rotateCameraCoroutine = StartCoroutine(RotateCameraCoroutine(rotationAmount));
    }

    // Coroutine to smoothly rotate the camera's parent transform
    IEnumerator RotateCameraCoroutine(float rotationAmount)
    {
        // Calculate the target rotation based on the rotation amount
        Quaternion targetRotation = Quaternion.Euler(0f, rotationAmount, 0f) * mainCameraParentTransform.rotation;

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
