using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScreenFader : MonoBehaviour
{
    public float fadeDuration = 1.0f; // Duration of the fade effect in seconds
    private Image image;

    private void Start()
    {
        // Get the Image component
        image = GetComponent<Image>();

        // Start the fade out effect
        StartCoroutine(FadeIn());
    }

    // Coroutine for fading out (from visible to transparent)
    private IEnumerator FadeIn()
    {
        // Set initial alpha value to fully opaque
        image.color = new Color(0, 0, 0, 1);

        // Calculate the alpha decrement per second
        float alphaDecrement = 1.0f / fadeDuration;

        // Fade out over time
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeDuration)
        {
            // Calculate the new alpha value based on elapsed time
            float newAlpha = 1 - (elapsedTime / fadeDuration);

            // Update the image color with the new alpha value
            image.color = new Color(0, 0, 0, newAlpha);

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final alpha value is fully transparent
        image.color = new Color(0, 0, 0, 0);
    }

    public IEnumerator FadeOut()
    {
        // Set initial alpha value to fully transparent
        image.color = new Color(0, 0, 0, 0);

        // Calculate the alpha increment per second
        float alphaIncrement = 1.0f / fadeDuration;

        // Fade in over time
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeDuration)
        {
            // Calculate the new alpha value based on elapsed time
            float newAlpha = elapsedTime / fadeDuration;

            // Update the image color with the new alpha value
            image.color = new Color(0, 0, 0, newAlpha);

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final alpha value is fully opaque
        image.color = new Color(0, 0, 0, 1);
    }
}
