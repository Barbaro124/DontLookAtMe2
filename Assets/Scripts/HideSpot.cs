using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class HideSpot : MonoBehaviour
{
    [SerializeField]
    public bool occupied; // Check this in inspector to mark the intended starting hidespots for monsters.

    [SerializeField]
    public GameObject retreatSpot; // assign child in inspector

    [SerializeField]
    public bool scareSpot; //set one spot in inspector to be the scare spot

    RetreatSpotScript retreatSpotScript;

    public float distanceToTrolley;
    
    // Start is called before the first frame update
    void Start()
    {
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
            Debug.Log("A spot was claimed as occupied");
        }
    }

    public void makeFree()
    {
        if (occupied)
        {
            occupied = false;
        }
    }

    /// <summary>
    /// A Custom method to find a child GameObject with a specific tag recursively
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static GameObject FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }

            // Recursively search child's children
            //GameObject foundChild = FindChildWithTag(child, tag);
            //if (foundChild != null)
            //{
            //    return foundChild;
            //}
        }

        return null; // No child with the specified tag found
    }
}
