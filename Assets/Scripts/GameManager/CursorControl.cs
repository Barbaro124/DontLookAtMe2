using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

public class CursorControl : MonoBehaviour
{
    //array to specify the scene names where cursor visibility should be controlled

    string[] scenesToControlCursor = { "Chamber_1", "Chamber_2" };
    private static CursorControl instance;

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

    }

    void Start()
    {
 
    }

    bool IsSceneInArray(string sceneName)
    {
        // Check if the current scene is in the scenesToControlCursor array
        foreach (string scene in scenesToControlCursor)
        {
            if (sceneName == scene)
            {
                return true;
            }
        }
        return false;
    }

    public void setCursor()
    {
        Debug.Log("setCursor method called");
        // Check if the current scene is in the scenesToControlCursor array
        if (IsSceneInArray(SceneManager.GetActiveScene().name))
        {
            // Hide and lock the cursor by default
            if (PauseMenu.isPaused == false)
            {
                Debug.Log("Cursor Locked, not paused");
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                // Show and unlock the cursor
                Debug.Log("Cursor Unlocked, paused");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        else
        {
            // Show and unlock the cursor by default
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
