using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    float speed = 2f; // Speed at which the door slides
    Vector3 openPositionLeft = new Vector3(-35, 0, 0); // Target position to open the door to the left. Adjust this value based on your scene setup.
    Vector3 openPositionRight = new Vector3(35, 0, 0);
    Vector3 globalOpenRight;
    Vector3 globalOpenLeft;

    private bool isOpening = false;

    Rigidbody rightRB;
    Rigidbody leftRB;

    GameObject doorRight;
    GameObject doorLeft;
    MyManager myManager;
    // Start is called before the first frame update
    void Start()
    {
        doorRight = GameObject.FindGameObjectWithTag("DoorRight");
        rightRB = doorRight.GetComponent<Rigidbody>();

        doorLeft = GameObject.FindGameObjectWithTag("DoorLeft");
        leftRB = doorLeft.GetComponent<Rigidbody>();

        globalOpenRight = doorRight.transform.TransformPoint(openPositionRight);
        globalOpenLeft = doorLeft.transform.TransformPoint(openPositionLeft);
        myManager = GameObject.FindWithTag("GameManager").GetComponent<MyManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isOpening)
        {
            // Move the door towards the global open position
            doorRight.transform.position = Vector3.MoveTowards(doorRight.transform.position, globalOpenRight, speed * Time.deltaTime);
            doorLeft.transform.position = Vector3.MoveTowards(doorLeft.transform.position, globalOpenLeft, speed * Time.deltaTime);
        }

        if (myManager.getFoundCount() >= 3)
        {
            OpenDoors();
        }
    }

    public void OpenDoors()
    {
        isOpening = true;
    }
}
