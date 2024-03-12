using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

public class Trolley : MonoBehaviour
{
    Vector3 trolleyExitPos;
    Vector3 trolleyEnterPos;
    Vector3 trolleyStopPos;
    float moveSpeed = 8f;

    private Rigidbody rb;
    private bool isMoving = false;

    public CharacterController playerController;

    MyManager manager;

    bool exiting = false;
    bool entering = true;


    void Start()
    {
        //add self as trolley exit event listener
        PlayerAdditions playerScript = GameObject.FindWithTag("MainCamera").GetComponent<PlayerAdditions>();
        playerScript.AddTrolleyExitEventListener(trolleyExit);
        //Debug.Log("Trolley added trolleyExit method as event");

        rb = gameObject.GetComponent<Rigidbody>();
        trolleyExitPos = GameObject.FindWithTag("TrolleyExit").transform.position;
        trolleyEnterPos = GameObject.FindWithTag("TrolleyEntrance").transform.position;
        trolleyStopPos = GameObject.FindWithTag("TrolleyStop").transform.position;

        rb.isKinematic = true;

        manager = GameObject.FindWithTag("GameManager").GetComponent<MyManager>();

        trolleyEnter();
    }

    void FixedUpdate()
    {

        // Exiting
        if (isMoving && exiting)
        {
            FindObjectOfType<TimerScript>().StopTimer();
            Vector3 direction = (trolleyExitPos - transform.position).normalized;
            Vector3 targetVelocity = direction * moveSpeed;

            // Calculate the velocity change needed
            Vector3 velocityChange = targetVelocity - rb.velocity;

            Vector3 movementVector = transform.position + velocityChange * Time.deltaTime;
            // Move the rigidbody using MovePosition
            rb.MovePosition(movementVector);



            // Check if the trolley has reached or passed the target position
            if (Vector3.Distance(transform.position, trolleyExitPos) <= 0.1f)
            {
                rb.velocity = Vector3.zero; // Stop the trolley
                isMoving = false; // Reset the flag
                FindObjectOfType<AudioManager>().Stop("train");
            }

            // Synchronize player's movement with trolley's movement
            Vector3 trolleyMovement = CalculateTrolleyMovement(); // Calculate trolley's movement
            playerController.Move(trolleyMovement * Time.fixedDeltaTime); // Apply movement to player
        }

        // Entering
        if (isMoving && entering)
        {
            Vector3 direction = (trolleyStopPos - transform.position).normalized;
            Vector3 targetVelocity = direction * moveSpeed;

            // Calculate the velocity change needed
            Vector3 velocityChange = targetVelocity - rb.velocity;

            Vector3 movementVector = transform.position + velocityChange * Time.deltaTime;
            // Move the rigidbody using MovePosition
            rb.MovePosition(movementVector);


            //start playing brake noise within this distance
            if (Vector3.Distance(transform.position, trolleyStopPos) <= 10f)
            {
                FindObjectOfType<AudioManager>().Play("brake");
                FindObjectOfType<AudioManager>().FadeOut("train",3f);
                //FindObjectOfType<AudioManager>().Play("brake");
                
            }

            // Check if the trolley has reached or passed the target position
            if (Vector3.Distance(transform.position, trolleyStopPos) <= 0.1f)
            {
                rb.velocity = Vector3.zero; // Stop the trolley
                isMoving = false; // Reset the flag
                entering = false;
                FindObjectOfType<AudioManager>().Stop("train");

                FindObjectOfType<TimerScript>().StartTimer();
            }

            // Synchronize player's movement with trolley's movement
            Vector3 trolleyMovement = CalculateTrolleyMovement(); // Calculate trolley's movement
            playerController.Move(trolleyMovement * Time.fixedDeltaTime); // Apply movement to player
        }


    }

    private Vector3 CalculateTrolleyMovement()
    {
        Vector3 direction = (trolleyExitPos - transform.position).normalized;
        Vector3 trolleyMovement = direction * (moveSpeed / 2);

        // Debug.Log to inspect the calculated trolleyMovement vector
        //Debug.Log("Player Movement: " + trolleyMovement);
        return trolleyMovement;
    }

    #region Events
    void trolleyExit()
    {

        Debug.Log("Items found on trolleyExit: " + manager.getFoundCount());
        if (manager.getFoundCount() >= 3)
        {
            Debug.Log("trolleyExit Invoked");

            if (!isMoving)
            {
                FindObjectOfType<AudioManager>().Play("train");
                FindObjectOfType<AudioManager>().SetVolume("train", 0.76f);
                isMoving = true;
                exiting = true;// Set the flag to indicate that movement is initiated
            }
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("wrong");
        }

    }

    void trolleyEnter()
    {
        if (!isMoving) 
        {
            FindObjectOfType<AudioManager>().Play("train");
            isMoving = true;
        }
    }

    public void JumpScare()
    {
        //Debug.Log("Jumpscare Movement");
        //float hitForce = 10f;
        // Move the targetTransform along its local forward direction by hitForce units
        //transform.Translate(Vector3.forward * hitForce, Space.Self);
        FindObjectOfType<AudioManager>().Play("monsterScream");
        TimerScript timerScript = GameObject.FindObjectOfType<TimerScript>();
        timerScript.timeLeft = 3;
        timerScript.StartTimer();
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TrolleyExit"))
        {
            // Perform actions or call methods specific to the collision with the other object
            Debug.Log("Trolley collided with " + other.gameObject.name);
            FindObjectOfType<AudioManager>().Stop("train");
            FindObjectOfType<AudioManager>().Stop("Ambient");
            manager.MainMenu();
        }
    }
}
