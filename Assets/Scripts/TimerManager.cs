using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerManager : MonoBehaviour
{
    // public Text timerText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI countdownTimerText;
    private float startTime;
    private bool timerActive = true;
    private float initialCountdownTime = 60;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
         if (timerActive)
        {
            // Calculate the time since the start of the timer
            float t = Time.time - startTime;

            // Convert to minutes and seconds
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f2");
            // Display the timer
            timerText.text = minutes + ":" + seconds;

            // Countdown
            float cd = initialCountdownTime - t;
            string cdMinutes = ((int)cd / 60).ToString();
            string cdSeconds = (cd % 60).ToString("f2");
            // Display the timer
            if ( cd <= 0 ) cd = 0;
            countdownTimerText.text = cd.ToString(); 
        }
    }

    public void StopTimer()
    {
        timerActive = false;
    }

    public void ResetTimer()
    {
        startTime = Time.time;
        timerActive = true;
    }
}
