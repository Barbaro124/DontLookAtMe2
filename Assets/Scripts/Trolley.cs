using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Trolley : MonoBehaviour
{

    
    Vector3 trolleyExitPos;
    float moveSpeed = 1000f;

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
    }

    void FixedUpdate()
    {
       Vector3 direction = (trolleyExitPos - transform.position).normalized;
       rb.AddForce(direction * moveSpeed, ForceMode.Acceleration);
        Debug.Log("applying force...");
       if (transform.position != trolleyExitPos) 
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

    }
    #region Events
    void trolleyExit()
    {
        Debug.Log("trolleyExit Invoked");

        if (!isMoving)
        {
           // Vector3 direction = (trolleyExitPos - transform.position).normalized;
          //  rb.velocity = direction * moveSpeed;
        }
            
            //while (gameObject.transform.position != trolleyExitPos)
            //{
            //    float distance = Vector3.Distance(transform.position, trolleyExitPos);

            //    Vector3 exitDirection = trolleyExitPos - transform.position;
            //    exitDirection.Normalize();

            //    transform.position += exitDirection * Mathf.Min(moveSpeed * Time.deltaTime, distance);
            //    Debug.Log("Trolley is moving");
            //}

    }
    #endregion
}
