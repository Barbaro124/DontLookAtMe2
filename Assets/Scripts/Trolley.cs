using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trolley : MonoBehaviour
{

    Rigidbody rb;
    Vector3 trolleyExitPos;
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


    #region Events
    void trolleyExit()
    {
        Debug.Log("trolleyExit Invoked");
        if (rb.position != trolleyExitPos)
        {
            Debug.Log("Trolley is moving");
            rb.MovePosition(rb.position + trolleyExitPos * Time.deltaTime);
        }
            
        
    }
    #endregion
}
