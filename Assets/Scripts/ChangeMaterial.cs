using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.LowLevel;

public class ChangeMaterial : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public Material black;
    [SerializeField]
    public Material green;

    bool lighton;

    ItemFoundEvent itemFoundEvent = new ItemFoundEvent();
    void Start()
    {
        black = GetComponent<Material>();
        lighton = false;
    }

    public void AddItemFoundEventListener(UnityAction listener)
    {
        itemFoundEvent.AddListener(listener);
    }
    public void LightOn()
    {
        //Debug.Log("LightOn Method Called");
        if (lighton == false)
        {
            gameObject.GetComponent<Renderer>().material = green;
            lighton = true;
            itemFoundEvent.Invoke();
        }
        

    }
}
