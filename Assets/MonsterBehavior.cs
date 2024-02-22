using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide()
    {
        Debug.Log("Hide Method Called");
        //gameObject.SetActive(false);
    }

    public void Appear()
    {
        Debug.Log("Appear Method Called");
        //gameObject.SetActive(true);
    }
}
