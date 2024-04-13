using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayScript : MonoBehaviour
{

    public GameObject objectToDuplicate;
    public int numberOfDuplicates = 5;
    public float distanceBetweenDuplicates = 1.0f;
    public Vector3 duplicationAxis = Vector3.right;
    // Start is called before the first frame update
    void Start()
    {
        if (objectToDuplicate == null) return;

        for (int i = numberOfDuplicates; i >= 1; i--)
        {
            Vector3 positionOffset = i * distanceBetweenDuplicates * duplicationAxis.normalized;
            Instantiate(objectToDuplicate, objectToDuplicate.transform.position + positionOffset, objectToDuplicate.transform.rotation, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
