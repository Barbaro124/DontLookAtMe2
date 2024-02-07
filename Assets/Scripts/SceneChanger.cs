using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    private static SceneChanger instance;

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


    public enum Scene
    {
        MainMenu,
        Chamber_1,
        Instructions,
        GameOver
    }
    public void Instructions()
    {
        SceneManager.LoadScene("Instructions");
    }
    public void Chamber_1()
    {
        SceneManager.LoadScene("Chamber_1");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver()");
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

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

}
