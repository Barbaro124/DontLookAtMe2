using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyShake : MonoBehaviour
{
    public float shakeAngle = 120f; // The angle the trolley rotates
    public float shakeSpeed = 6f; // The speed of the shaking motion
    public float shakeDuration = 2f; // The duration of the shaking
    public float fallForce = 100f; // The force applied to the trolley when falling


    private Quaternion initialRotation; // The initial rotation of the trolley
    private float shakeTimer = 0f; // Timer for the shaking duration

    GameObject player;
    PlayerMovement playerMovement;
    public ScreenFader screenFader;

    private void Start()
    {
        // Store the initial rotation of the trolley
        initialRotation = transform.rotation;

        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        screenFader = GameObject.FindGameObjectWithTag("BlackImage").GetComponent<ScreenFader>();
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            if (playerMovement.isControllingSpotlight)
            {
                playerMovement.isControllingSpotlight = false;
            }
            // Calculate the progress of the shake (from 0 to 1)
            float shakeProgress = 1 - Mathf.Clamp01(shakeTimer / shakeDuration);

            // Calculate the easing factor using Sinusoidal function
            float easingFactor = Mathf.Sin(shakeProgress * Mathf.PI);

            // Calculate the shaking angle based on sine wave motion
            float shakeAngleOffset = Mathf.Sin(Time.time * shakeSpeed) * shakeAngle * easingFactor;

            // Create a rotation quaternion with the shaking angle
            Quaternion shakeRotation = Quaternion.Euler(0f, shakeAngleOffset, 0f);

            // Apply the rotation to the trolley's rotation
            transform.rotation = initialRotation * shakeRotation;

            // Decrease the shake timer
            shakeTimer -= Time.deltaTime;
        }
    }

    // Call this method to start the shaking behavior
    public void StartShaking()
    {
        // Reset the shake timer
        shakeTimer = shakeDuration;
        FindObjectOfType<AudioManager>().Play("trolleycrash");

        // Start the coroutine to wait for the shaking duration and then make the trolley fall
        StartCoroutine(FallAfterShaking());
    }

    //NOTE: CALLED FROM TROLLEY.CS
    public void StartShakingDelayed(float delay)
    {
        Invoke("StartShaking", delay);
    }

    // Coroutine to make the trolley fall after shaking
    private IEnumerator FallAfterShaking()
    {
        // Wait for the shaking duration
        yield return new WaitForSeconds(shakeDuration);
        
        // Calculate the duration of the fall
        float fallDuration = 1.6f; // Adjust as needed

        // Get the initial position of the trolley
        Vector3 initialPosition = transform.position;

        // Calculate the target position for the fall (e.g., move downwards by a certain amount)
        Vector3 targetPosition = initialPosition - Vector3.up * 70f; // Adjust the downward distance as needed

        // Perform the fall over time
        float elapsedTime = 0.0f;
        while (elapsedTime < fallDuration)
        {
            // Calculate the interpolation factor based on elapsed time
            float t = elapsedTime / fallDuration;

            // Interpolate the trolley's position between the initial and target positions
            transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }
        if (elapsedTime > fallDuration -1f)
        {
            StartCoroutine(screenFader.FadeOut());
        }

        // Ensure the trolley reaches the target position
        transform.position = targetPosition;
    }


}
