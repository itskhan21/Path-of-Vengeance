using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    GameManager gameManager;
    Text timerText;
    private float levelStartTime; // Tracks the time the level started

    void Start()
    {
        timerText = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        gameManager = GameObject.FindObjectOfType<GameManager>();

        // Initialize level start time
        levelStartTime = Time.time;
    }

    void Update()
    {
        if (timerText != null)
        {
            if (gameManager.IsGameplay())
            {
                // Calculate elapsed time since level started
                float elapsedTime = Time.time - levelStartTime;

                int minutes = Mathf.FloorToInt(elapsedTime / 60f);
                int seconds = Mathf.FloorToInt(elapsedTime % 60f);

                timerText.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
            }
        }
        else
        {
            // Re-fetch the timer text in case it's missing
            timerText = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        }
        
    }

    // Call this method when starting a new level to reset the timer
    public void ResetTimer()
    {
        levelStartTime = Time.time;
    }
}
