using UnityEngine;

public class InputDetection : MonoBehaviour
{
    public Camera cam;
    public GameObject spotlight;
    private PlayerMovement playerMovement;
    public GameObject button;
    public PlayerAdditions playerAdditions;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        spotlight = GameObject.Find("Spotlight");
        cam = Camera.main;
        playerAdditions = FindObjectOfType<PlayerAdditions>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (spotlight.GetComponent<Light>().enabled)
            {
                spotlight.GetComponent<Light>().enabled = false;

                playerMovement.ToggleSpotlightControl(false);
            }
            else
            {
                spotlight.GetComponent<Light>().enabled = true;
               
                playerMovement.ToggleSpotlightControl(true);
            }
        }
    }


}
