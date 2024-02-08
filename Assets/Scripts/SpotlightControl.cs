using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SpotlightControl : MonoBehaviour
{
    public float rotationSpeed = 20f;
    private PlayerMovement playerMovement;
    public int itemsFound = 0;
    private GameObject currentTarget;
    public LayerMask layerMask;
    public Outline outline;

    ItemFoundEvent itemFoundEvent = new ItemFoundEvent();

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
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

    /// <summary>
    /// Adds listener to the Item Found Event
    /// </summary>
    /// <param name="listener">listener</param>
    public void AddItemFoundEventListener(UnityAction<int> listener)
    {
        itemFoundEvent.AddListener(listener);
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
                Debug.Log("Did Hit");

            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("Did not Hit");
            }
        }

        else if (currentTarget != null)
        {
            OnRaycastExit(currentTarget);
            currentTarget = null;
        }

        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        //{


        /*
        if (hit.collider.CompareTag("HiddenObject"))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            itemsFound++;
            Debug.Log("Items Found: " + itemsFound);

            //hit.collider.gameObject.GetComponent<Outline>().enabled = true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
            Debug.Log("Did not Hit");
            //hit.collider.gameObject.GetComponent<Outline>().enabled = false;
        }*/
        //}

    }

    void OnRaycastEnter(GameObject target)
    {
        outline = target.GetComponent<Outline>();
        outline.EnableOutline();
        if (target.CompareTag("HiddenObject"))
        {
            itemsFound++;
            Debug.Log("Items Found: " + itemsFound);
        }
        // Do something with the object that was hit by the raycast.

    }

    void OnRaycastExit(GameObject target)
    {
        outline = target.GetComponent<Outline>();
        outline.DisableOutline();
        // Do something with the object that was exited by the raycast.
    }

    void OnRaycast(GameObject target)
    {
        outline = target.GetComponent<Outline>();
        outline.EnableOutline();

    }
}
