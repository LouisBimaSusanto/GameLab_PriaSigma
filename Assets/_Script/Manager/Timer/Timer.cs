using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeRemaining = 60f;
    private bool isRunning = true;

    [Header("UI")]
    public TMP_Text timerText;
    public static Timer Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // Subscribe ke event scene loaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe untuk menghindari memory leak
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Dipanggil otomatis setiap kali scene baru selesai dimuat
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cari TimerUIBinder di scene baru dan ambil referensi timerText-nya
        TimerUIBinder binder = Object.FindFirstObjectByType<TimerUIBinder>();
        if (binder != null)
        {
            SetTimerText(binder.timerText);
        }
        else
        {
            // Tidak ada timer UI di scene ini (misal: scene menu)
            timerText = null;
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
        Time.timeScale = 0f;
    }

    public void StopTimer()
    {
        isRunning = false;
        Debug.Log("Finish! Sisa waktu: " + timeRemaining);
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