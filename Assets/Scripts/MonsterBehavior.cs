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

    float moveSpeed = 20f;

    bool hiding = false;
    bool scaring = false;

    private List<GameObject> spotsSortedByDistance = new List<GameObject>();
    public Transform trolleyTransform;

    [SerializeField]
    public HideSpot currentSpot; // label in inspector this monster's starting spot.

    bool lightChange = false;

    bool isMovingRandomly = false;
    
    //how often the monster should move randomly
    float randomMoveInterval = 10f;

    // the time of the last random movement
    float lastRandomMoveTime = 0f;

    private Animator animator; // Reference to the Animator

    // Start is called before the first frame update
    void Start()
    {

        //need to set hidePos to be the RetreatSpotScript transform.position for whatever spot the monster is in.
        hidePos = currentSpot.retreatSpot.transform.position;

        rb = gameObject.GetComponent<Rigidbody>();

        trolleyTransform = GameObject.FindGameObjectWithTag("TrolleyStop").transform;

        spotsSortedByDistance = GetSpotsSortedByDistance();

        lastRandomMoveTime = Time.time;

        animator = GetComponentInChildren<Animator>();

    }



    private void FixedUpdate()
    {

        if (hiding)
        {
            animator.SetBool("isSeen", true);  // Play hide animation
        }
        else
        {
            animator.SetBool("isSeen", false); // Stop hide animation
        }

        //if (hiding || scaring)
        //{
            
            //Vector3 direction = (hidePos - transform.position).normalized;
            //Vector3 targetVelocity = direction * moveSpeed;

            //// Calculate the velocity change needed
            //Vector3 velocityChange = targetVelocity - rb.velocity;

            //Vector3 movementVector = transform.position + velocityChange * Time.deltaTime * 3;
            //// Move the rigidbody using MovePosition
            //rb.MovePosition(movementVector);
            //Debug.Log("code to move the monster called");

            

            // Check if the monster has reached or passed the target position
            //if (Vector3.Distance(transform.position, hidePos) <= 1f)
            //{
            //    rb.velocity = Vector3.zero; // Stop
            //    transform.position = hidePos;
            //    if(!scaring)
            //    {
            //        FindNextSpot();// teleport to next spot
            //    }

            //    hiding = false;
            //    //scaring = false; enabling this makes the monster not move for some reason
            //    if (scaring)
            //    {
            //        ChangeLight();
            //    }

            //}
        //}

        if (!isMovingRandomly && Time.time - lastRandomMoveTime >= randomMoveInterval)
        {
            StartCoroutine(MoveRandomly());
            lastRandomMoveTime = Time.time;
        }
    }

    IEnumerator MoveRandomly()
    {
        Debug.Log("MoveRandomly called");
        isMovingRandomly = true;

        // Determine the target spot randomly
        int randomIndex = Random.Range(0, spotsSortedByDistance.Count);
        HideSpot targetSpot = spotsSortedByDistance[randomIndex].GetComponent<HideSpot>();

        // Check if the target spot is unoccupied and further away from the player
        if (!IsPositionOccupied(targetSpot) && targetSpot.distanceToTrolley > currentSpot.distanceToTrolley)
        {
            // Move to the target spot
            Debug.Log("Monster Moved Randomly");
            MoveToSpot(targetSpot);
        }
        else
        {
            Debug.Log("Monster Not Moved Randomly");
        }

        // Wait for a short duration before allowing another random move
        yield return new WaitForSeconds(Random.Range(5f, 10f));

        isMovingRandomly = false;
    }

    public void OnHidingAnimationEnd()
    {
        if (!scaring)
        {
            FindNextSpot();// teleport to next spot
        }

        hiding = false;

        if (scaring)
        {
            ChangeLight();
        }
    }

    public void Hide()
    {
        // Set the 'isSeen' parameter to true to play the hide animation
        //animator.SetBool("isSeen", true);

        //Debug.Log("Hide Method Called");
        hiding = true; // causes hiding in FixedUpdate()

    }

    void MoveToSpot(HideSpot targetSpot)
    {
        currentSpot.makeFree();
        transform.position = targetSpot.transform.position;
        transform.localScale = targetSpot.transform.localScale;
        transform.rotation = targetSpot.transform.rotation;
        targetSpot.claimSpot(); //set spot's occupied to true
        currentSpot = targetSpot; // set new current spot for monster
        hidePos = currentSpot.retreatSpot.transform.position; // set new retreatSpot for monster
        
    }


    void FindNextSpot()
    {
        foreach (GameObject spot in spotsSortedByDistance)
        {
            
            HideSpot targetSpot = spot.GetComponent<HideSpot>();

            // Check if the position is unoccupied
            if (!IsPositionOccupied(targetSpot) && targetSpot.distanceToTrolley < currentSpot.distanceToTrolley)
            {

                MoveToSpot(targetSpot);
                Debug.Log("Monster Moved closer");
                if (currentSpot.scareSpot == true)
                {
                    JumpScare();
                }
                break; // Exit the loop after finding an unoccupied position
            }
           
        }
    }



    public void JumpScare()
    {
        
        scaring = true; //affects fixedupdate movement
        hiding = true;

        //done instead of an event:
        Trolley trolley = GameObject.FindGameObjectWithTag("Trolley").GetComponent<Trolley>();
        trolley.JumpScare();
        FindObjectOfType<AudioManager>().Stop("Ambient");
        FindObjectOfType<AudioManager>().Stop("ticktock");
        FindObjectOfType<AudioManager>().Play("monsterScream");

    }

    void ChangeLight()
    {

        Light spotlight = GameObject.FindGameObjectWithTag("SpotlightRotator").GetComponent<Light>();
        spotlight.intensity = 2100;
        spotlight.spotAngle = 60;

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
            HideSpot hideSpot = obj.GetComponent<HideSpot>();
            if (hideSpot != null)
            {
                hideSpot.distanceToTrolley = distanceToTrolley;
            }

            spotsSortedByDistance.Add(obj);
        }

        // Sort the objects by their distance to the trolley (furthest to closest)
        spotsSortedByDistance.Sort((a, b) =>
            Vector3.Distance(b.transform.position, trolleyTransform.position)
                .CompareTo(Vector3.Distance(a.transform.position, trolleyTransform.position)));

        // Print the sorted list to the console
        foreach (GameObject obj in spotsSortedByDistance)
        {

            //Debug.Log("Object Name: " + obj.name + ", Distance to Trolley: " + Vector3.Distance(obj.transform.position, trolleyTransform.position));
        }

        return spotsSortedByDistance;
    }

   
}
