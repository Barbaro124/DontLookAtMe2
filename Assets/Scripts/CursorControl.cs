using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorControl : MonoBehaviour
{
    //array to specify the scene names where cursor visibility should be controlled

    string[] scenesToControlCursor = { "Chamber_1", "Chamber_2" };

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
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                // Show and unlock the cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        else
        {
            // Show and unlock the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
