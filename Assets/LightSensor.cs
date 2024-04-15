using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightSensor : MonoBehaviour
{

    float blackLimit = 0f;
    float greenLimit = 1f;

    // Rates of change for transitioning from black to green and from green to black
    float rateOfChangeToGreen = 0.2f; // Adjust this for faster transition to green
    float rateOfChangeToBlack = 0.1f; // Adjust this for slower transition to black

    public bool aimedAt = false;

    bool lighton;

    bool itemAdded = false;

    [SerializeField]
    Material lightMaterial;

    private Renderer rend;

    ItemFoundEvent itemFoundEvent = new ItemFoundEvent();
    ItemLostEvent itemLostEvent = new ItemLostEvent();


    // Start is called before the first frame update
    void Start()
    {
        // Get the renderer component attached to the GameObject
        rend = GetComponent<Renderer>();

        // Ensure the material has emission enabled
        rend.material.EnableKeyword("_EMISSION");

        lighton = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ModifyGreenLevel();
    }


    // Function to modify the green level of the emission color
    void ModifyGreenLevel()
    {
        // Get the current emission color of the material
        Color currentEmission = rend.material.GetColor("_EmissionColor");

        // Calculate the target green level based on whether the light sensor is aimed at
        float targetGreen = aimedAt ? greenLimit : blackLimit;

        // Determine the rate of change based on the target state
        float rateOfChange = aimedAt ? rateOfChangeToGreen : rateOfChangeToBlack;

        // Interpolate the current green level towards the target green level
        currentEmission.g = Mathf.Lerp(currentEmission.g, targetGreen, rateOfChange * Time.deltaTime);

        // Set the modified emission color back to the material
        rend.material.SetColor("_EmissionColor", currentEmission);

        Debug.Log(currentEmission.g);

        if (currentEmission.g >= 0.3f)
        {
            LightOn(true);
        }
        else
        {
            LightOn(false);
        }

    }

    /// <summary>
    /// used to add an item as found, or lose an item
    /// </summary>
    /// <param name="lighton"></param>
    void LightOn(bool lighton)
    {
        //Debug.Log("LightOn Method Called");
        if (lighton == true)
        {
            if (!itemAdded)
            {
                itemFoundEvent.Invoke();
                itemAdded = true;
            }
        }

        else if (lighton == false)
        {
            if (itemAdded)
            {
                itemLostEvent.Invoke();
                itemAdded = false;
            }
        }

    }

    public void AddItemFoundEventListener(UnityAction listener)
    {
        itemFoundEvent.AddListener(listener);
    }

    public void AddItemLostEventListener(UnityAction listener)
    {
        itemLostEvent.AddListener(listener);
    }
}
