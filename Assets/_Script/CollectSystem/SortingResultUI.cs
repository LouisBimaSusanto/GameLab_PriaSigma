using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class SortingResultUI : MonoBehaviour
{
    public static SortingResultUI Instance { get; private set; }

    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Info")]
    [Tooltip("Teks untuk menampilkan Sisa Waktu")]
    [SerializeField] private TMP_Text timeUsedText;

    [Header("Stars")]
    [SerializeField] private Image star1;
    [SerializeField] private Image star2;
    [SerializeField] private Image star3;
    [SerializeField] private Sprite starFilled;
    [SerializeField] private Sprite starEmpty;

    [Header("Button")]
    [SerializeField] private Button continueButton;

    [Header("Animation Settings")]
    [SerializeField] private float starDelay = 0.3f;
    [SerializeField] private float starPunchScale = 1.4f;

    private Action onCompleteCallback;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        panel.SetActive(false);
    }

    private void Start()
    {
        continueButton.onClick.AddListener(OnContinueClicked);
    }

    private void OnDestroy()
    {
        continueButton.onClick.RemoveListener(OnContinueClicked);
    }

    public void BeginSession(float snapshotTimeRemaining)
    {
    }

    public void Show(int sessionScore, int perfectScore, Action onComplete)
    {
        onCompleteCallback = onComplete;

        Timer.Instance?.PauseTimer();

        float currentRemaining = Timer.Instance != null ? Timer.Instance.timeRemaining : 0f;

        Debug.Log($"[ResultUI] Sisa Waktu yang ditampilkan: {currentRemaining} detik");

        if (timeUsedText != null)
        {
            timeUsedText.text = FormatTime(currentRemaining);
        }

        int stars = CalculateStarsBasedOnTime(currentRemaining);

        panel.SetActive(true);
        AnimateStars(stars);
    }

    private int CalculateStarsBasedOnTime(float remainingTime)
    {
        if (remainingTime >= 15f) return 3; // Bintang 3: Sisa waktu >= 15 detik
        if (remainingTime >= 5f) return 2;  // Bintang 2: Sisa waktu antara 5 - 14.99 detik
        if (remainingTime > 0f) return 1;   // Bintang 1: Sisa waktu di bawah 5 detik

        return 0;
    }

    private string FormatTime(float time)
    {
        time = Mathf.Max(0f, time);

        int totalSeconds = Mathf.FloorToInt(time);

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void AnimateStars(int starCount)
    {
        SetStar(star1, false);
        SetStar(star2, false);
        SetStar(star3, false);

        if (starCount >= 1)
            DOVirtual.DelayedCall(starDelay * 1, () => PunchStar(star1));
        if (starCount >= 2)
            DOVirtual.DelayedCall(starDelay * 2, () => PunchStar(star2));
        if (starCount >= 3)
            DOVirtual.DelayedCall(starDelay * 3, () => PunchStar(star3));
    }

    private void PunchStar(Image star)
    {
        SetStar(star, true);
        star.transform.DOPunchScale(Vector3.one * (starPunchScale - 1f), 0.4f, 5, 0.5f);
    }

    private void SetStar(Image star, bool filled)
    {
        star.sprite = filled ? starFilled : starEmpty;
    }

    private void OnContinueClicked()
    {
        panel.SetActive(false);
        onCompleteCallback?.Invoke();
    }
}