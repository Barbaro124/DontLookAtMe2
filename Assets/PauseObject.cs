using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseObject : MonoBehaviour
{
    public static PauseObject instance;
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

}
