using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


[Serializable]
public class TimerScript : MonoBehaviour
{
    AudioManager audioManager;

    public float timerDuration;
    public float timeLeft;
    public bool timerOn = false;
    public TMP_Text timerTxt;

    public float startingPitch = 1.0f;
    public float maxPitch = 3.0f;
    

    MyManager myManager;
    //public TMP_Text minutes;
    //public TMP_Text seconds;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = timerDuration;
        //timerOn = true;
        //timerTxt.text  = "00:00";
        //timeLeft = 60;
        audioManager = FindObjectOfType<AudioManager>();
    }
    public void StartTimer()
    {
        timerOn = true;
        audioManager.Play("ticktock");
    }

    public void ScareTimer()
    {
        timerOn = true;
    }
    public void StopTimer()
    {
        timerOn = false;
        audioManager.Stop("ticktock");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(timerOn)
        {
            if(timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                //updateTimerText(timeLeft);

                float pitchRange = maxPitch - startingPitch;
                float pitchIncrement = pitchRange * ((timerDuration - timeLeft) / timerDuration);
                audioManager.SetPitch("ticktock", startingPitch + pitchIncrement);
            }
            else
            {
                audioManager.Stop("ticktock");
                timeLeft = 0;
                timerOn = false; 
                SceneManager.LoadScene("GameOver");

            }
        }
        
    }

    //void updateTimerText(float currentTime)
    //{
    //    currentTime += 1;

    //    float min = Mathf.FloorToInt(currentTime / 60);
    //    float sec = Mathf.FloorToInt(currentTime % 60);

    //    if (timerTxt == null)
    //    {
    //        Debug.LogError("timerTxt is null!");
    //        return;
    //    }

    //    timerTxt.text = string.Format("{0:00}:{1:00}", min, sec);

    //}
}
