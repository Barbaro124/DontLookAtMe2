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
    PauseMenu pauseMenu;

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

        cursorControlScript = GetComponent<CursorControl>();

        pauseMenu = GetComponent<PauseMenu>();
    }

    // Start is called before the first frame update
    void Start()
    {
        itemsFound = 0;


    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenu.isPaused)
            {
                pauseMenu.ResumeGame();
                cursorControlScript.setCursor();
            }
            else
            {
                pauseMenu.PauseGame();
                cursorControlScript.setCursor();
            }
        }
    }

    #region Events
    void AddItem()
    {
        itemsFound++;
        Debug.Log("Items Found: " + itemsFound);
        FindObjectOfType<AudioManager>().Play("itemFound");
    }
    void LostItem()
    {
        itemsFound--;
        Debug.Log("Items Found: " + itemsFound);
        //add itemLost sound here?
    }

    public void NextRoom()
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

    }
    public void Chamber_2()
    {
        SceneManager.LoadScene("Chamber_2");
    }
    public void Chamber_1()
    {
        SceneManager.LoadScene("Chamber_1");
        //OnSceneChange();
    }
    public void MainMenu()
    {
        //OnSceneChange();
        SceneManager.LoadScene("MainMenu");

    }
    public void GameOver()
    {
        //OnSceneChange();
        SceneManager.LoadScene("GameOver()");

    }
    public void Quit()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        // Get the name of the currently active scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Reload the current scene by its name
        SceneManager.LoadScene(currentSceneName);

    }

    /// <summary>
    /// Proceeds through levels in the intended order. Update once more levels are added.
    /// </summary>
    public void LevelProceed()
    {

        if (SceneManager.GetActiveScene().name == "Chamber_1")
        {
            SceneManager.LoadScene("Chamber_2"); 
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

        cursorControlScript.setCursor();
        //gameplay chambers, event listeners
        if (chamber == 1 || chamber == 2 || chamber == 3)
        {
            
            itemsFound = 0;

            LightSensor LightSensor1 = GameObject.FindWithTag("LightSensorLight").GetComponent<LightSensor>();
            LightSensor1.AddItemFoundEventListener(AddItem);
            LightSensor1.AddItemLostEventListener(LostItem);

            LightSensor LightSensor2 = GameObject.FindWithTag("LightSensorLight2").GetComponent<LightSensor>();
            LightSensor2.AddItemFoundEventListener(AddItem);
            LightSensor2.AddItemLostEventListener(LostItem);

            LightSensor LightSensor3 = GameObject.FindWithTag("LightSensorLight3").GetComponent<LightSensor>();
            LightSensor3.AddItemFoundEventListener(AddItem);
            LightSensor3.AddItemLostEventListener(LostItem);

            //add self as next room event listener
            PlayerAdditions playerScript = GameObject.FindWithTag("MainCamera").GetComponent<PlayerAdditions>();
            playerScript.AddNextRoomEventListener(NextRoom);

            FindObjectOfType<AudioManager>().Play("Ambient");
            
        }
    }


}
