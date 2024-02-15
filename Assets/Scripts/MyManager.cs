using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
//using static UnityEditor.Timeline.Actions.MenuPriority;

public class MyManager : MonoBehaviour
{
    private static MyManager instance;
    public int itemsFound;
    //SceneChanger sceneChanger;
    CursorControl cursorControlScript;
    public enum Scene
    {
        MainMenu,
        Chamber_1,
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
        //sceneChanger = GetComponent<SceneChanger>();
        cursorControlScript = GetComponent<CursorControl>();

    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    #region Events
    void AddItem()
    {
        itemsFound++;
        Debug.Log("Items Found: " + itemsFound);
    }
    void NextRoom()
    {
        Debug.Log("NextRoom Invoked");
        if (itemsFound >= 3)
        {
            Debug.Log("Next Scene!");
            //sceneChanger.LevelProceed();
            LevelProceed();
            //OnSceneChange();
        }
        else
        {
            Debug.Log("Not enough Items Found!");
        }
    }
    #endregion

    #region Scene Management
    public void Instructions()
    {
        //OnSceneChange();
        SceneManager.LoadScene("Instructions");
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

    #region Events

    #endregion

    /// <summary>
    /// Proceeds through levels in the intended order
    /// </summary>
    public void LevelProceed()
    {
        

        if (SceneManager.GetActiveScene().name == "Chamber_1")
        {
            SceneManager.LoadScene("Chamber_2");
            //OnSceneChange();
        }
        else if (SceneManager.GetActiveScene().name == "Chamber_2")
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    /// <summary>
    /// executes certain actions upon a gameplay scene load, such as controlling the cursor, adding event listeners, and resetting the items found.
    /// NOTE: make sure scene is loaded BEFORE executing this method
    /// </summary>
    //void OnSceneChange()
    //{
    //    cursorControlScript.setCursor();
    //    itemsFound = 0;

    //    ChangeMaterial changeMaterial1 = GameObject.FindWithTag("LightSensorLight").GetComponent<ChangeMaterial>();
    //    changeMaterial1.AddItemFoundEventListener(AddItem);

    //    ChangeMaterial changeMaterial2 = GameObject.FindWithTag("LightSensorLight2").GetComponent<ChangeMaterial>();
    //    changeMaterial2.AddItemFoundEventListener(AddItem);

    //    ChangeMaterial changeMaterial3 = GameObject.FindWithTag("LightSensorLight3").GetComponent<ChangeMaterial>();
    //    changeMaterial3.AddItemFoundEventListener(AddItem);

    //    //add self as next room event listener
    //    PlayerAdditions playerScript = GameObject.FindWithTag("MainCamera").GetComponent<PlayerAdditions>();
    //    playerScript.AddNextRoomEventListener(NextRoom);
    //}

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1 || level == 2)
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
        }
    }
    #endregion

}
