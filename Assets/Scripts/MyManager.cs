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
        sceneChanger = GetComponent<SceneChanger>();

        //add self as item found event listener
        //SpotlightControl spotlightScript = GameObject.FindWithTag("SpotlightRotator").GetComponent<SpotlightControl>();
        //spotlightScript.AddItemFoundEventListener(AddItem);

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


    void AddItem()
    {
        itemsFound++;
        Debug.Log("Items Found: " + itemsFound);

        //GameObject sensorLight = GameObject.FindGameObjectWithTag("LightSensorLight");

        //sensorLight.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        
    
    }

    void NextRoom()
    {
        Debug.Log("NextRoom Invoked");
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
