using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehavior : MonoBehaviour
{
    Vector3 hidePos;
    private Rigidbody rb;

    float moveSpeed = 30f;

    bool hiding = false;

    private List<GameObject> spotsSortedByDistance = new List<GameObject>();
    public Transform trolleyTransform;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
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


        trolleyTransform = GameObject.FindGameObjectWithTag("Trolley").transform;

        spotsSortedByDistance = GetSpotsSortedByDistance();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (hiding)
        {
            Vector3 direction = (hidePos - transform.position).normalized;
            Vector3 targetVelocity = direction * moveSpeed;

            // Calculate the velocity change needed
            Vector3 velocityChange = targetVelocity - rb.velocity;

            Vector3 movementVector = transform.position + velocityChange * Time.deltaTime;
            // Move the rigidbody using MovePosition
            rb.MovePosition(movementVector);

            // Check if the monster has reached or passed the target position
            if (Vector3.Distance(transform.position, hidePos) <= 0.1f)
            {
                rb.velocity = Vector3.zero; // Stop
            }
        }
        
        if (!hiding)
        {

        }
    }

    public void Hide()
    {
        Debug.Log("Hide Method Called");
        //gameObject.SetActive(false);
        hiding = true;

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
            HideSpot hideSpot = spot.GetComponent<HideSpot>();
            // Check if the position is unoccupied
            if (!IsPositionOccupied(hideSpot))
            {
                // Move the "monster" GameObject to the unoccupied position
                transform.position = spot.transform.position;
                break; // Exit the loop after finding an unoccupied position
            }
        }
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
