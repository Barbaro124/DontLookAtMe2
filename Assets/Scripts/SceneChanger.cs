//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using static UnityEditor.Timeline.Actions.MenuPriority;

//public class SceneChanger : MonoBehaviour
//{

//    private static SceneChanger instance;
//    CursorControl cursorControlScript;
//    MyManager managerScript;

//    void Awake()
//    {
//        // Check if an instance of the game manager already exists
//        if (instance == null)
//        {
//            // If not, set this instance as the singleton instance
//            instance = this;

//            // Make sure this object persists between scenes
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            // If an instance already exists, destroy this duplicate
//            //Destroy(gameObject);
//        }
//        cursorControlScript = GetComponent<CursorControl>();
//        managerScript = GetComponent<MyManager>();  
//    }


//    public enum Scene
//    {
//        MainMenu,
//        Chamber_1,
//        Instructions,
//        GameOver
//    }
//    public void Instructions()
//    {
//        OnSceneChange();
//        SceneManager.LoadScene("Instructions");
//    }
//    public void Chamber_1()
//    {
//        OnSceneChange();
//        SceneManager.LoadScene("Chamber_1");
//    }
//    public void MainMenu()
//    {
//        OnSceneChange();
//        SceneManager.LoadScene("MainMenu");
//    }
//    public void GameOver()
//    {
//        OnSceneChange();
//        SceneManager.LoadScene("GameOver()");
//    }
//    public void Quit()
//    {
//        Application.Quit();
//    }

//    public void Update()
//    {
//        if(Input.GetKey(KeyCode.Escape))
//        {
//            Application.Quit();
//        }
//    }

//    public void LevelProceed()
//    {
//        OnSceneChange();

//        if (SceneManager.GetActiveScene().name == "Chamber_1")
//        {
//            SceneManager.LoadScene("Chamber_2");
//        }
//        else if (SceneManager.GetActiveScene().name == "Chamber_2")
//        {
//            SceneManager.LoadScene("MainMenu");
//        }
//    }

//    void OnSceneChange()
//    {
//        cursorControlScript.setCursor();
//        managerScript.itemsFound = 0;

//        ChangeMaterial changeMaterial1 = GameObject.FindWithTag("LightSensorLight").GetComponent<ChangeMaterial>();
//        changeMaterial1.AddItemFoundEventListener(AddItem);

//        ChangeMaterial changeMaterial2 = GameObject.FindWithTag("LightSensorLight2").GetComponent<ChangeMaterial>();
//        changeMaterial2.AddItemFoundEventListener(AddItem);

//        ChangeMaterial changeMaterial3 = GameObject.FindWithTag("LightSensorLight3").GetComponent<ChangeMaterial>();
//        changeMaterial3.AddItemFoundEventListener(AddItem);

//        //add self as next room event listener
//        PlayerAdditions playerScript = GameObject.FindWithTag("MainCamera").GetComponent<PlayerAdditions>();
//        playerScript.AddNextRoomEventListener(NextRoom);
//    }

//}
