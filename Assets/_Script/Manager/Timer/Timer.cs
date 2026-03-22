using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeRemaining = 60f;
    private bool isRunning = true;

    [Header("UI")]
    public TMP_Text timerText;

    void Start()
    {
        UpdateTimerDisplay(timeRemaining);
    }

    void Update()
    {
        if (!isRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining < 0)
                timeRemaining = 0;

            UpdateTimerDisplay(timeRemaining);
        }
        else
        {
            isRunning = false;
            TimeUp();
        }
    }

    void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        
        if (time <= 10)
        {
            timerText.color = Color.red;
        }
    }

    void TimeUp()
    {
        Debug.Log("Waktu habis!");

        
        Time.timeScale = 0f;
    }

    
    public void StopTimer()
    {
        isRunning = false;
        Debug.Log("Finish! Sisa waktu: " + timeRemaining);
    }
}