using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeRemaining = 60f;
    public float InitialTime { get; private set; }
    private bool isRunning = true;

    [Header("UI")]
    public TMP_Text timerText;

    public static Timer Instance;
    public event Action OnTimeUp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitialTime = timeRemaining;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TimerUIBinder binder = UnityEngine.Object.FindFirstObjectByType<TimerUIBinder>();

        if (binder != null)
        {
            SetTimerText(binder.timerText);

            ResetTimer(InitialTime);
        }
        else
        {
            timerText = null;

            PauseTimer();
        }
    }

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
            if (timeRemaining < 0) timeRemaining = 0;
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
        if (timerText == null) return;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.color = time <= 10 ? Color.red : Color.white;
    }

    void TimeUp()
    {
        Debug.Log("Waktu habis!");
        OnTimeUp?.Invoke();
    }

    public void StopTimer()
    {
        isRunning = false;
        Debug.Log("Finish! Sisa waktu: " + timeRemaining);
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        if (timeRemaining > 0)
            isRunning = true;
    }

    public void ResetTimer(float newTime = 60f)
    {
        timeRemaining = newTime;
        InitialTime = newTime;
        isRunning = true;
        UpdateTimerDisplay(timeRemaining);
    }

    public void AddTime(float amount)
    {
        timeRemaining += amount;
        UpdateTimerDisplay(timeRemaining);
    }

    public void SetTimerText(TMP_Text newText)
    {
        timerText = newText;
        UpdateTimerDisplay(timeRemaining);
    }
}