using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatSpotScript : MonoBehaviour
{

    [SerializeField]
    public bool hideUp;
    [SerializeField]
    public bool hideDown;
    [SerializeField]
    public bool hideLeft;
    [SerializeField]
    public bool hideRight;

    float moveDistance;

    // Start is called before the first frame update
    void Start()
    {

        moveDistance = 20f;

        if (hideLeft)
        {
            transform.position += new Vector3(moveDistance, 0f, 0f);
        }
        if (hideRight)
        {
            transform.position += new Vector3(-moveDistance, 0f, 0f);
        }
        if (hideUp)
        {
            transform.position += new Vector3(0f, moveDistance, 0f);
        }
        if (hideDown)
        {
            transform.position += new Vector3(0f, -moveDistance, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
