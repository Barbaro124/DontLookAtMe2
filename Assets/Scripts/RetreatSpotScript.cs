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

        moveDistance = 20;

        if (hideLeft)
        {
            transform.Translate(new Vector3(Mathf.RoundToInt(moveDistance), 0f, 0f), Space.Self);
        }
        if (hideRight)
        {
            transform.Translate(new Vector3(Mathf.RoundToInt(-moveDistance), 0f, 0f), Space.Self);
        }
        if (hideUp)
        {
            transform.Translate(new Vector3(0f, Mathf.RoundToInt(moveDistance), 0f), Space.Self);
        }
        if (hideDown)
        {
            transform.Translate(new Vector3(0f, Mathf.RoundToInt(-moveDistance), 0f), Space.Self);
        }
    }

    // Update is called once per frame

}
