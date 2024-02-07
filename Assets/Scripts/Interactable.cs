using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public SpotlightControl spotlightControl;
    public PlayerMovement playerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        spotlightControl = FindObjectOfType<SpotlightControl>();
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (playerMovement.isControllingSpotlight == false)
        {

        }
    }
}
