using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trolley : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //add self as trolley exit event listener
        PlayerAdditions playerScript = GameObject.FindWithTag("MainCamera").GetComponent<PlayerAdditions>();
        playerScript.AddTrolleyExitEventListener(trolleyExit);
        Debug.Log("Trolley added self as exit event listener");
    }


    #region Events
    void trolleyExit()
    {
        Debug.Log("trolleyExit Invoked");
    }
    #endregion
}
