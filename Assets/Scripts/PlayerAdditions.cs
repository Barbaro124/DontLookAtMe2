using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class PlayerAdditions : MonoBehaviour
{

    public Outline outline;
    public GameObject spotlight;
    public GameObject button;
    private PlayerMovement playerMovement;
    public float selectionDistance = Mathf.Infinity;
    public LayerMask layerMask;
    private GameObject currentTarget;

    public int itemsFound = 0;

    public bool canInteract = false;

    NextRoomEvent nextRoomEvent = new NextRoomEvent();

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>(); 
        spotlight = GameObject.Find("Spotlight");
        button = GameObject.Find("Button");
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Raycast();
    }

    public void AddNextRoomEventListener(UnityAction listener)
    {
        nextRoomEvent.AddListener(listener);
    }

    public void Raycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, selectionDistance, layerMask))
        {
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

    protected virtual void OnRaycastEnter(GameObject target)
    {
        outline = target.GetComponent<Outline>();
        outline.EnableOutline();
        // Do something with the object that was hit by the raycast.

    }

    protected virtual void OnRaycastExit(GameObject target)
    {
        outline = target.GetComponent<Outline>();
        outline.DisableOutline();
        // Do something with the object that was exited by the raycast.
    }

    protected virtual void OnRaycast(GameObject target)
    {
        outline = target.GetComponent<Outline>();
        outline.EnableOutline();

        if (target.CompareTag("Spotlight"))
        {
            ControlLight();
        }

        if (target.CompareTag("Button"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Button Pressed");
                nextRoomEvent.Invoke();
            }
        }




    }

    void ControlLight ()
    {
        //Debug.Log("Control Light");
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (spotlight.GetComponent<Light>().enabled)
            {
                spotlight.GetComponent<Light>().enabled = false;

                playerMovement.ToggleSpotlightControl(false);
            }
            else
            {
                spotlight.GetComponent<Light>().enabled = true;

                playerMovement.ToggleSpotlightControl(true);
            }
        }

    }

    void PressButton()
    { 
        Debug.Log("Button Pressed");
    }
}
