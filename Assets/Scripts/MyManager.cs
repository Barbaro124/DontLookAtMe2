using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class MyManager : MonoBehaviour
{
    private static MyManager instance;
    public int itemsFound;
    CursorControl cursorControlScript;
    public enum Scene
    {
        MainMenu,
        Chamber_1,
        Chamber_2,
        Instructions,
        GameOver
    }

    void Awake()
    {
        // Check if an instance of the game manager already exists
        if (instance == null)
        {
            // If not, set this instance as the singleton instance
            instance = this;

            // Make sure this object persists between scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this duplicate
            //Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        itemsFound = 0;

        cursorControlScript = GetComponent<CursorControl>();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }

    #region Events
    void AddItem()
    {
        itemsFound++;
        Debug.Log("Items Found: " + itemsFound);
        FindObjectOfType<AudioManager>().Play("itemFound");
    }

    void NextRoom()
    {
        Debug.Log("NextRoom Invoked");

        LevelProceed();

    }
    #endregion


    public int getFoundCount()
    {
        return itemsFound;
    }
    #region Scene Management
    public void Instructions()
    {
        //OnSceneChange();
        SceneManager.LoadScene("Instructions");
        cursorControlScript.setCursor();
    }
    public void Chamber_1()
    {
        SceneManager.LoadScene("Chamber_1");
        cursorControlScript.setCursor();
        //OnSceneChange();
    }
    public void MainMenu()
    {
        //OnSceneChange();
        SceneManager.LoadScene("MainMenu");
        cursorControlScript.setCursor();
    }
    public void GameOver()
    {
        //OnSceneChange();
        SceneManager.LoadScene("GameOver()");
        cursorControlScript.setCursor();
    }
    public void Quit()
    {
        Application.Quit();
    }


    /// <summary>
    /// Proceeds through levels in the intended order. Update once more levels are added.
    /// </summary>
    public void LevelProceed()
    {

        if (SceneManager.GetActiveScene().name == "Chamber_1")
        {
            SceneManager.LoadScene("MainMenu"); //Change this once there's more than one level
        }
        else if (SceneManager.GetActiveScene().name == "Chamber_2")
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    #endregion

    /// <summary>
    /// executes actions such as Event listeners, itemsFound reset, and restricting cursor depending on what level was loaded. Chambers must match level number in build settings.
    /// </summary>
    /// <param name="level"></param>
    private void OnLevelWasLoaded(int chamber)
    {
        //gameplay chambers
        if (chamber == 1 || chamber == 2)
        {
            cursorControlScript.setCursor();
            itemsFound = 0;

            ChangeMaterial changeMaterial1 = GameObject.FindWithTag("LightSensorLight").GetComponent<ChangeMaterial>();
            changeMaterial1.AddItemFoundEventListener(AddItem);

            ChangeMaterial changeMaterial2 = GameObject.FindWithTag("LightSensorLight2").GetComponent<ChangeMaterial>();
            changeMaterial2.AddItemFoundEventListener(AddItem);

            ChangeMaterial changeMaterial3 = GameObject.FindWithTag("LightSensorLight3").GetComponent<ChangeMaterial>();
            changeMaterial3.AddItemFoundEventListener(AddItem);

            //add self as next room event listener
            PlayerAdditions playerScript = GameObject.FindWithTag("MainCamera").GetComponent<PlayerAdditions>();
            playerScript.AddNextRoomEventListener(NextRoom);

            FindObjectOfType<AudioManager>().Play("Ambient");
            
        }
    }


}
