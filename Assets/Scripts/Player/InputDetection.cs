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

        // sound testing inputs
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            FindObjectOfType<AudioManager>().Play("distantHiss");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            FindObjectOfType<AudioManager>().Play("ambientBreath");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            FindObjectOfType<AudioManager>().Play("breathing");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            FindObjectOfType<AudioManager>().Play("Jumpscare");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            FindObjectOfType<AudioManager>().Play("laugh");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            FindObjectOfType<AudioManager>().Play("pitterpatter");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            FindObjectOfType<AudioManager>().Play("pitterpatter2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            FindObjectOfType<AudioManager>().Play("puhpuhpuh");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            FindObjectOfType<AudioManager>().Play("static");
        }
    }


}
