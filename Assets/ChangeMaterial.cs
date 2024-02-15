using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class ChangeMaterial : MonoBehaviour
{
    // Start is called before the first frame update
    public Material black;
    public Material green;
    void Start()
    {
        black = GetComponent<Material>();
    }


    public void LightOn()
    {
        gameObject.GetComponent<Renderer>().material = green;
    }
}
