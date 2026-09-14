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
    [SerializeField] private TMP_Text stageTitleText;        // "Stage 1 Selesai!"
    [SerializeField] private TMP_Text totalTimeText;         // "01:00" — total waktu stage
    [SerializeField] private TMP_Text timeUsedText;          // "00:40" — waktu yang terpakai user

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
    private float sessionStartTime;
    private float totalTimeLimit;
    private float sessionStartTimeRemaining;

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

    public void BeginSession()
    {
        sessionStartTimeRemaining = Timer.Instance.timeRemaining;
        totalTimeLimit = Timer.Instance.InitialTime;
    }

    public void Show(int sessionScore, int perfectScore, Action onComplete)
    {
        onCompleteCallback = onComplete;

        int currentStage = StageManager.Instance.CurrentStage;
        stageTitleText.text = "Stage " + currentStage + " Selesai!";

        // Total waktu stage — dari InitialTime Timer
        totalTimeText.text = FormatTime(totalTimeLimit);

        // Waktu terpakai = waktu awal - sisa waktu saat masuk sorting
        float timeUsed = totalTimeLimit - sessionStartTimeRemaining;
        timeUsed = Mathf.Max(0f, timeUsed);
        timeUsedText.text = FormatTime(timeUsed);

        int stars = CalculateStars(sessionScore, perfectScore);

        panel.SetActive(true);
        AnimateStars(stars);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:02}", minutes, seconds);
    }

    private int CalculateStars(int score, int perfect)
    {
        if (perfect <= 0) return 1;

        float ratio = (float)score / perfect;

        if (ratio >= 1f) return 3;
        if (ratio >= 0.7f) return 2;
        return 1;
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