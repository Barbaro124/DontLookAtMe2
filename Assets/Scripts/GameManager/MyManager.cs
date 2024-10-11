using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;


[System.Serializable]
public struct SubtitleText
{
    public float time;
    public string text;
}

public class MyManager : MonoBehaviour
{
    private static MyManager instance;
    public int itemsFound;
    CursorControl cursorControlScript;
    PauseMenu pauseMenu;
    ScreenFader screenFader;
    [SerializeField]
    public GameObject subtitleGO;
    
    public TextMeshProUGUI subtitles;

    public SubtitleText[] subtitleText;

    public enum Scene
    {
        MainMenu,
        Chamber_0,
        Chamber_1,
        Chamber_2,
        Chamber_3,
        Chamber_4,
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
        screenFader = GetComponentInChildren<ScreenFader>();
    }

    // Start is called before the first frame update
    void Start()
    {
        itemsFound = 0;

        subtitles = GetComponentInChildren<TextMeshProUGUI>();

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (FindObjectOfType<AudioManager>().isPlaying("TitleMusic") != true)
            {
                FindObjectOfType<AudioManager>().Play("TitleMusic");
            }
        }
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
        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    Chamber_0();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    Chamber_1();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    Chamber_2();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    Chamber_3();
        //}
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

    public void StartSubtitles()
    {
        Debug.Log("Starting Subtitles...");
        StartCoroutine(SubtitleCoroutine());
    }

    IEnumerator SubtitleCoroutine()
    {
        subtitleGO.SetActive(true);
        foreach (var voiceLine in subtitleText)
        {
            if(SceneManager.GetActiveScene().name != "Chamber_0")
            {
                subtitleGO.SetActive(false);
                yield return null;
            }
            else
            {
                subtitles.text = voiceLine.text;

                yield return new WaitForSecondsRealtime(voiceLine.time);
            }

        }
        subtitleGO.SetActive(false);

    }
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
    public void Chamber_0()
    {
        SceneManager.LoadScene("Chamber_0");
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
    public void Chamber_3()
    {
        SceneManager.LoadScene("Chamber_3");
    }

    public void Chamber_4()
    {
        SceneManager.LoadScene("Chamber_4");
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
        if (SceneManager.GetActiveScene().name == "Chamber_0")
        {
            SceneManager.LoadScene("Chamber_4");
        }
        if (SceneManager.GetActiveScene().name == "Chamber_4")
        {
            SceneManager.LoadScene("Chamber_1");
        }
        if (SceneManager.GetActiveScene().name == "Chamber_1")
        {
            SceneManager.LoadScene("Chamber_2");
        }
        else if (SceneManager.GetActiveScene().name == "Chamber_2")
        {
            SceneManager.LoadScene("Chamber_3");
        }
        else if (SceneManager.GetActiveScene().name == "Chamber_3")
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    #endregion

    public void FadeOut()
    {
        screenFader.FadeOutCommand();
    }

    /// <summary>
    /// executes actions such as Event listeners, itemsFound reset, and restricting cursor depending on what level was loaded. Chambers must match level number in build settings.
    /// </summary>
    /// <param name="level"></param>
    private void OnLevelWasLoaded(int chamber)
    {

        cursorControlScript.setCursor();

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (FindObjectOfType<AudioManager>().isPlaying("TitleMusic") != true)
            {
                FindObjectOfType<AudioManager>().Play("TitleMusic");
            }
        }
        
        //gameplay chambers, event listeners
        if (chamber == 1|| chamber == 2 || chamber == 3 || chamber == 4 || chamber == 5)
        {

            if (FindObjectOfType<AudioManager>().isPlaying("TitleMusic") == true)
            {
                FindObjectOfType<AudioManager>().Stop("TitleMusic");
            }
            //  screenFader.Enable();
            screenFader.FadeInCommand();
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
            FindObjectOfType<AudioManager>().Stop("TitleMusic");

            if (chamber != 1)
            {
                FindObjectOfType<AudioManager>().Stop("welcomespeech");
            }
            
        }
        else
        {
            if (FindObjectOfType<AudioManager>().isPlaying("Ambient") == true)
            {
                FindObjectOfType<AudioManager>().Stop("Ambient");
            }
            if (FindObjectOfType<AudioManager>().isPlaying("Train") == true)
            {
                FindObjectOfType<AudioManager>().Stop("Train");
            }
            if (FindObjectOfType<AudioManager>().isPlaying("welcomespeech") == true)
            {
                FindObjectOfType<AudioManager>().Stop("welcomespeech");
            }
            
        }
    }


}
