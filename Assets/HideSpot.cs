using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpot : MonoBehaviour
{
    [SerializeField]
    public bool occupied;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = gameObject.transform.position;
        transform.localScale = gameObject.transform.localScale;
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
