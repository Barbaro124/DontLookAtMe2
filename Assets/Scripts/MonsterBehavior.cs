using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.HighDefinition;

public class MonsterBehavior : MonoBehaviour
{
    Vector3 hidePos;
    private Rigidbody rb;

    float moveSpeed = 30f;

    bool hiding = false;
    bool scaring = false;

    private List<GameObject> spotsSortedByDistance = new List<GameObject>();
    public Transform trolleyTransform;

    [SerializeField]
    public HideSpot currentSpot; // label in inspector this monster's starting spot.

    bool lightChange = false;

    JumpScareEvent jumpScareEvent = new JumpScareEvent();


    // Start is called before the first frame update
    void Start()
    {

        //need to set hidePos to be the RetreatSpotScript transform.position for whatever spot the monster is in.
        hidePos = currentSpot.retreatSpot.transform.position;

        rb = gameObject.GetComponent<Rigidbody>();
        /*
        if (gameObject.tag == "Monster")
        {
            hidePos = GameObject.FindWithTag("MonsterHide1").transform.position;
        }
        if (gameObject.tag == "Monster2")
        {
            hidePos = GameObject.FindWithTag("MonsterHide2").transform.position;
        }
        if (gameObject.tag == "Monster3")
        {
            hidePos = GameObject.FindWithTag("MonsterHide3").transform.position;
        }
        */


        trolleyTransform = GameObject.FindGameObjectWithTag("Trolley").transform;

        spotsSortedByDistance = GetSpotsSortedByDistance();

    }



    private void FixedUpdate()
    {
        if (hiding || scaring)
        {
            
            Vector3 direction = (hidePos - transform.position).normalized;
            Vector3 targetVelocity = direction * moveSpeed;

            // Calculate the velocity change needed
            Vector3 velocityChange = targetVelocity - rb.velocity;

            Vector3 movementVector = transform.position + velocityChange * Time.deltaTime * 2;
            // Move the rigidbody using MovePosition
            rb.MovePosition(movementVector);
            Debug.Log("code to move the monster called");

            // Check if the monster has reached or passed the target position
            if (Vector3.Distance(transform.position, hidePos) <= 1f)
            {
                rb.velocity = Vector3.zero; // Stop
                transform.position = hidePos;
                if(!scaring)
                {
                    FindNextSpot();// teleport to next spot
                }

                hiding = false;
                //scaring = false; enabling this makes the monster not move for some reason
                if (scaring)
                {
                    ChangeLight();
                }

            }
        }

        
    }

    public void Hide()
    {
        Debug.Log("Hide Method Called");

        //FindNextSpot(); // teleports monster to next spot, commented out because I want the monster to hide first
        //gameObject.SetActive(false);
        hiding = true; // causes hiding in FixedUpdate()

    }

    public void Appear()
    {
        Debug.Log("Appear Method Called");
        //gameObject.SetActive(true);
        hiding = false;
    }

    void FindNextSpot()
    {
        foreach (GameObject spot in spotsSortedByDistance)
        {
            Debug.Log("Checking a spot...");
            HideSpot hideSpot = spot.GetComponent<HideSpot>();

            // Check if the position is unoccupied
            if (!IsPositionOccupied(hideSpot))
            {
                Debug.Log("Unoccupied spot found");
                // teleport the "monster" GameObject to the unoccupied position
                Debug.Log("teleporting the \"monster\" GameObject to the unoccupied position");
                transform.position = spot.transform.position;
                transform.localScale = spot.transform.localScale;
                transform.rotation = spot.transform.rotation;

                hideSpot.claimSpot(); //set spot's occupied to true
                currentSpot = hideSpot; // set new current spot for monster
                Debug.Log("currentSpot = " + currentSpot.ToString());
                hidePos = currentSpot.retreatSpot.transform.position; // set new retreatSpot for monster
                Debug.Log("Corresponding Retreat Spot = " + currentSpot.retreatSpot.ToString());
                if (hideSpot.scareSpot == true)
                {
                    JumpScare();
                }
                break; // Exit the loop after finding an unoccupied position
            }

           
        }
    }

    public void JumpScare()
    {
        // on jumpscare, I need: The monster to come up from below, The light to get wider and less intense, the trolley to shake and make noise, the trolley to fall to the ground and crash
        scaring = true; //affects fixedupdate movement
        hiding = true;

    }

    void ChangeLight()
    {

        Light spotlight = GameObject.FindGameObjectWithTag("SpotlightRotator").GetComponent<Light>();
        spotlight.intensity = 2100;
        spotlight.spotAngle = 60;

        //done instead of an event:
        Trolley trolley = GameObject.FindGameObjectWithTag("Trolley").GetComponent<Trolley>();
        trolley.JumpScare();
    }

    private bool IsPositionOccupied(HideSpot spot)
    {
        // Check if the position is occupied by another object
        if (spot.occupied == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private List<GameObject> GetSpotsSortedByDistance()
    {
        // Find all GameObjects in the scene with the specified tag
        GameObject[] objects = GameObject.FindGameObjectsWithTag("HideSpot");

        // Calculate the distance between each object and the player and store them in a list
        foreach (GameObject obj in objects)
        {
            float distanceToTrolley = Vector3.Distance(obj.transform.position, trolleyTransform.position);
            spotsSortedByDistance.Add(obj);
        }

        // Sort the objects by their distance to the trolley (furthest to closest)
        spotsSortedByDistance.Sort((a, b) =>
            Vector3.Distance(b.transform.position, trolleyTransform.position)
                .CompareTo(Vector3.Distance(a.transform.position, trolleyTransform.position)));
        return spotsSortedByDistance;
    }
}
