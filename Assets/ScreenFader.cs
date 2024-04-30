using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScreenFader : MonoBehaviour
{
    public float fadeDuration = 0.5f; // Duration of the fade effect in seconds
    private Image image;

    bool fadeInRunning = false;
    bool fadeOutRunning = false;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    void Start()
    {

        // Start the fade in effect
        //StartCoroutine(FadeIn());
    }

    public void FadeInCommand()
    {
        if (!fadeInRunning)
        {
            StartCoroutine(FadeIn());
        }
    }

    public void FadeOutCommand()
    {
        StartCoroutine(FadeOut());
    }
    // Coroutine for fading out (from visible to transparent)
    private IEnumerator FadeIn()
    {
        fadeInRunning = true;
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

        fadeInRunning = false;
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

    private void OnEnable()
    {
        if (!fadeInRunning)
        {
            StartCoroutine(FadeIn());
        }
    }
}
