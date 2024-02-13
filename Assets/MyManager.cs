using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class MyManager : MonoBehaviour
{

    int itemsFound;
    SceneChanger sceneChanger;
    // Start is called before the first frame update
    void Start()
    {
        itemsFound = 0;
        //sceneChanger = new SceneChanger();

        //add self as item found event listener
        SpotlightControl spotlightScript = GameObject.FindWithTag("SpotlightRotator").GetComponent<SpotlightControl>();
        spotlightScript.AddItemFoundEventListener(AddItem);

        //add self as next room event listener
        PlayerAdditions playerScript = GameObject.FindWithTag("MainCamera").GetComponent<PlayerAdditions>();
        playerScript.AddNextRoomEventListener(NextRoom);
    }


    void AddItem()
    {
        itemsFound++;
        Debug.Log("Items Found: " + itemsFound);
    }

    void NextRoom()
    { 
        if (itemsFound >= 3)
        {
            Debug.Log("Next Scene!");
            sceneChanger.LevelProceed();
        }
        else
        {
            Debug.Log("Not enough Items Found!");
        }
    }

}
