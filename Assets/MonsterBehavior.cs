using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehavior : MonoBehaviour
{
    Vector3 hidePos;
    private Rigidbody rb;

    float moveSpeed = 30f;

    bool hiding = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        hidePos = GameObject.FindWithTag("MonsterHide1").transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (hiding)
        {
            Vector3 direction = (transform.position - hidePos).normalized;
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
}
