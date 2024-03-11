using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpot : MonoBehaviour
{
    [SerializeField]
    public bool occupied; // Check this in inspector to mark the intended starting hidespots for monsters.

    [SerializeField]
    public GameObject retreatSpot; // assign child in inspector

    RetreatSpotScript retreatSpotScript;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = gameObject.transform.position;
        transform.localScale = gameObject.transform.localScale;
        transform.rotation = gameObject.transform.rotation;

        //get this hidespot's child retreatSpot's script
        retreatSpotScript = retreatSpot.GetComponent<RetreatSpotScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void claimSpot()
    {
        if (!occupied)
        {
            occupied = true;
        }
    }

    public void makeFree()
    {
        if (occupied)
        {
            occupied = false;
        }
    }
}
