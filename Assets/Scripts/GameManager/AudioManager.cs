using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
//using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;


[System.Serializable]
public class AudioManager : MonoBehaviour
{

    private static AudioManager instance;
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        // Check if an instance of the game manager already exists
        if (instance == null)
        {
            // If not, set this instance as the singleton instance
            instance = this;

            // Make sure this object persists between scenes
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.priority = s.priority;
        }
    }

    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.PlayOneShot(s.clip);
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void Stop (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public bool isPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s.source.isPlaying == true)
            return true;
        else return false;
    }

    public void SetPitch(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.pitch = pitch;
    }
    
    public void SetVolume(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.volume = volume;
    }
    public void FadeOut(string name, float seconds)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        StartCoroutine(FadeOutCoroutine(s, seconds));
    }

    private IEnumerator FadeOutCoroutine(Sound sound, float seconds)
    {
        float startVolume = sound.source.volume;
        float targetVolume = 0f;

        float elapsedTime = 0f;

        while (elapsedTime < seconds)
        {
            sound.source.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / seconds);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        //sound.source.volume = targetVolume; // Ensure volume is exactly 0

        // Reset volume to startVolume after fade-out is complete
        sound.source.volume = startVolume;
    }
}
