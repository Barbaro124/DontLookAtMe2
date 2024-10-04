using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerAdditions : MonoBehaviour
{

    public Outline outline;
    public GameObject spotlight;
    public GameObject button;
    private PlayerMovement playerMovement;
    public float selectionDistance = Mathf.Infinity;
    public LayerMask layerMask;
    private GameObject currentTarget;

    Trolley trolley;

    public int itemsFound = 0;

    public bool canInteract = false;

    GameObject aimSpotLock;
    GameObject player;
    CharacterController characterController;
    bool lockedIn = false;

    GameObject cineMachineTarget;


    NextRoomEvent nextRoomEvent = new NextRoomEvent();
    TrolleyExitEvent trolleyExitEvent = new TrolleyExitEvent();

    //"random noises"
    float timeSinceLastSound = 0f;
    //bool isPlayingSound = false;
    string[] randomSounds = new string[] { "distantHiss", "ambientBreath", "breathing", "laugh", "pitterpatter", "pitterpatter2", "puhpuhpuh", "static" };


    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>(); 
        spotlight = GameObject.Find("Spotlight");
        button = GameObject.Find("Button");
        playerMovement = FindObjectOfType<PlayerMovement>();

        aimSpotLock = GameObject.FindGameObjectWithTag("AimSpotLock");
        player = GameObject.FindGameObjectWithTag("Player");
        characterController = player.GetComponent<CharacterController>();
        trolley = FindObjectOfType<Trolley>();

        if (SceneManager.GetActiveScene().name != "Chamber_0")
        {
            StartCoroutine(playRandomSounds());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Raycast();
    }

    public void AddNextRoomEventListener(UnityAction listener)
    {
        Debug.Log("Added Next Room Event Listener");
        nextRoomEvent.AddListener(listener);
    }

    public void AddTrolleyExitEventListener(UnityAction listener)
    {
        //Debug.Log("Added Trolley as TrolleyExit Event Listener");
        trolleyExitEvent.AddListener(listener);
    }

    public void Raycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, selectionDistance, layerMask))
        {
            if (currentTarget == null)
            {
                currentTarget = hit.transform.gameObject;
                OnRaycastEnter(currentTarget);
            }
            else if (currentTarget != hit.transform.gameObject)
            {
                currentTarget = hit.transform.gameObject;
                OnRaycastExit(currentTarget);
            }

            OnRaycast(hit.transform.gameObject);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            
            //if (hit.collider.gameObject.CompareTag("Interactables"))
            //{
            //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //    //Debug.Log("Did Hit");

            //}
            //else
            //{
            //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //    //Debug.Log("Did not Hit");
            //}
        }

        else if (currentTarget != null)
        {
            OnRaycastExit(currentTarget);
            currentTarget = null;
        }

    }

    protected virtual void OnRaycastEnter(GameObject target)
    {
        // Do something with the object that was hit by the raycast.
        outline = target.GetComponent<Outline>();
        outline.EnableOutline();
    }

    protected virtual void OnRaycastExit(GameObject target)
    {
        // Do something with the object that was exited by the raycast.
        outline = target.GetComponent<Outline>();
        outline.DisableOutline();
    }

    protected virtual void OnRaycast(GameObject target)
    {
        outline = target.GetComponent<Outline>();

        outline.EnableOutline();

        if (target.CompareTag("Spotlight"))
        {
            // make outline red if unavailable.
            if (trolley.isMoving == true)
            {
                outline.OutlineColor = Color.red;
            }
            else
            {
                outline.OutlineColor = Color.white;
            }

            //Interaction/control Light Mode
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                //disable access to light if trolley is moving.
                if (trolley.isMoving == false)
                {
                    ControlLight(true);
                }
            }

        }

        if (target.CompareTag("Button"))
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                //nextRoomEvent.Invoke();
                PressButton();
            }
        }




    }

    void ControlLight (bool isRunning)
    {
        FindObjectOfType<AudioManager>().Play("flashlight");
        //Debug.Log("Control Light Method");
        if (spotlight.GetComponent<Light>().enabled && isRunning == true)
        {
            spotlight.GetComponent<Light>().enabled = false;
            playerMovement.ToggleSpotlightControl(false);
            isRunning = false;

            //unlock player
            lockedIn = false;
            PlaceLock();
        }
        else if (!spotlight.GetComponent<Light>().enabled && isRunning == true)
        {
            spotlight.GetComponent<Light>().enabled = true;
            playerMovement.ToggleSpotlightControl(true);
            isRunning = false;

            //lock in player
            lockedIn = true;
            PlaceLock();
        }
        
        

    }

    void PressButton()
    { 
        Debug.Log("Button Pressed");
        trolleyExitEvent.Invoke();
    }

    void PlaceLock()
    {
        if (lockedIn)
        {

            Vector3 distance = aimSpotLock.transform.position - player.transform.position;

            while(player.transform.position != aimSpotLock.transform.position)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, aimSpotLock.transform.position, 0.1f);
            }

            //snap to locked position
            //player.transform.position = aimSpotLock.transform.position;

            //player movement is already disabled via the playermovement script, so this is uneccesary.
            //characterController.enabled = false;
        }
        else
        {
            //re-enable character movement
            //characterController.enabled = true;
        }
    }

    IEnumerator playRandomSounds()
    {
        //Every minimum of 10 seconds to a maximum of 20 seconds, a sound should be played.

        while (true)
        {
            // Wait for 1 second intervals to check the time since the last sound
            yield return new WaitForSeconds(1f);
            timeSinceLastSound++;

            // Check if it's time to play a sound based on the intervals
            if (timeSinceLastSound >= 20)
            {
                PlaySound();
            }
            else if (timeSinceLastSound >= 19)
            {
                if (Random.Range(0f, 1f) <= 0.9f)
                {
                    PlaySound();
                }
            }
            else if (timeSinceLastSound >= 18)
            {
                if (Random.Range(0f, 1f) <= 0.8f)
                {
                    PlaySound();
                }
            }
            else if (timeSinceLastSound >= 17)
            {
                if (Random.Range(0f, 1f) <= 0.7f)
                {
                    PlaySound();
                }
            }
            else if (timeSinceLastSound >= 16)
            {
                if (Random.Range(0f, 1f) <= 0.6f)
                {
                    PlaySound();
                }
            }
            else if (timeSinceLastSound >= 15)
            {
                if (Random.Range(0f, 1f) <= 0.5f)
                {
                    PlaySound();
                }
            }
            else if (timeSinceLastSound >= 14)
            {
                if (Random.Range(0f, 1f) <= 0.4f)
                {
                    PlaySound();
                }
            }
            else if (timeSinceLastSound >= 13)
            {
                if (Random.Range(0f, 1f) <= 0.3f)
                {
                    PlaySound();
                }
            }
            else if (timeSinceLastSound >= 12)
            {
                if (Random.Range(0f, 1f) <= 0.2f)
                {
                    PlaySound();
                }
            }
            else if (timeSinceLastSound >= 11)
            {
                if (Random.Range(0f, 1f) <= 0.1f)
                {
                    PlaySound();
                }
            }
        }

        //The sound played should be a random choice between "distantHiss, ambientBreath, breathing, laugh, pitterpatter, pitterpatter2, puhpuhpuh, and static"
    }

    void PlaySound()
    {
        Debug.Log("Random Sound Played");
        int randomIndex = Random.Range(0, randomSounds.Length);
        FindObjectOfType<AudioManager>().Play(randomSounds[randomIndex]);
        timeSinceLastSound = 0f;
    }
}
