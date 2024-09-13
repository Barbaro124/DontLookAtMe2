using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueMonster : MonoBehaviour
{

    public Transform monsterTransform; // Reference to the monster's transform
    public float moveDuration = 1f; // Duration of the movement in seconds
    // Start is called before the first frame update
    void Start()
    {
        monsterTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DissapearDelay(float delayTime)
    {
        Invoke("Dissapear", delayTime);
    }

    void Dissapear()
    {
        StartCoroutine(MoveMonsterSmoothly());
    }

    IEnumerator MoveMonsterSmoothly()
    {
        // Get the initial position of the monster
        Vector3 initialPosition = monsterTransform.position;

        // Calculate the target position with an offset of +20 units on the X-axis
        Vector3 targetPosition = initialPosition + new Vector3(20f, 0f, 0f);

        // Time elapsed since the start of the movement
        float elapsedTime = 0f;

        // Loop until the elapsed time exceeds the move duration
        while (elapsedTime < moveDuration)
        {
            // Calculate the interpolation factor (0 to 1) based on elapsed time
            float t = elapsedTime / moveDuration;

            // Smoothly move the monster towards the target position using Lerp
            monsterTransform.position = Vector3.Lerp(initialPosition, targetPosition, t);

            // Increment the elapsed time by the time passed since the last frame
            elapsedTime += Time.deltaTime;

            // Wait for the end of the frame before the next iteration
            yield return null;
        }

        // Ensure that the monster reaches the target position exactly
        monsterTransform.position = targetPosition;
        // Disable the game object
        gameObject.SetActive(false);
    }
}
