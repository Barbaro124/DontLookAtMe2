using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using JetBrains.Annotations;

public class SpotlightControl : MonoBehaviour
{
    // Reference to the spotlight object
    public Transform spotlightTransform;

    // Reference to the main camera
    public Transform mainCameraTransform;

    // Sensitivity of camera rotation
    public float rotationSpeed = 2f;

    // Limit for camera rotation
    public float maxRotationAngle = 45f;

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

        mainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
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


    private void Raycast()
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
            if (currentTarget == null)
            {
                currentTarget = hit.transform.gameObject;
                OnRaycastEnter(currentTarget);
            }
            else if (currentTarget != hit.transform.gameObject)
            {
                OnRaycastExit(currentTarget);
                currentTarget = hit.transform.gameObject;
                //OnRaycastEnter(currentTarget);
            }
            OnRaycast(hit.transform.gameObject);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);


            if (hit.collider.gameObject.CompareTag("Interactables"))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                //Debug.Log("Did Hit");

            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                //Debug.Log("Did not Hit");
            }
        }

        else if (currentTarget != null)
        {
            OnRaycastExit(currentTarget);
            currentTarget = null;
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


        if (target.tag == "LeftLimit")
        {
            Debug.Log("LeftLimit Hit!");
            PanLeft();
        }
    }

    void PanLeft()
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
    }

  
}
