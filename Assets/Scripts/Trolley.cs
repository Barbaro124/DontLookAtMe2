using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Trolley : MonoBehaviour
{

    
    Vector3 trolleyExitPos;
    float moveSpeed = 5f;

    private Rigidbody rb;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        //add self as trolley exit event listener
        PlayerAdditions playerScript = GameObject.FindWithTag("MainCamera").GetComponent<PlayerAdditions>();
        playerScript.AddTrolleyExitEventListener(trolleyExit);
        Debug.Log("Trolley added trolleyExit method as event");

        rb = gameObject.GetComponent<Rigidbody>();
        trolleyExitPos = GameObject.FindWithTag("TrolleyExit").transform.position;

        rb.isKinematic = true;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            Vector3 direction = (trolleyExitPos - transform.position).normalized;
            Vector3 targetVelocity = direction * moveSpeed;

            // Calculate the velocity change needed
            Vector3 velocityChange = targetVelocity - rb.velocity;

            // Move the rigidbody using MovePosition
            rb.MovePosition(transform.position + velocityChange * Time.fixedDeltaTime);

            // Check if the trolley has reached or passed the target position
            if (Vector3.Distance(transform.position, trolleyExitPos) <= 0.1f)
            {
                rb.velocity = Vector3.zero; // Stop the trolley
                isMoving = false; // Reset the flag
            }
        }
    }
    #region Events
    void trolleyExit()
    {
        Debug.Log("trolleyExit Invoked");

        if (!isMoving)
        {
            isMoving = true; // Set the flag to indicate that movement is initiated
        }

    }
    #endregion
}
