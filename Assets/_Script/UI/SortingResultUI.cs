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

    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text perfectScoreText;

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

    public void Show(int sessionScore, int perfectScore, Action onComplete)
    {
        onCompleteCallback = onComplete;

        scoreText.text = sessionScore.ToString();
        perfectScoreText.text = "/ " + perfectScore.ToString();

        int stars = CalculateStars(sessionScore, perfectScore);

        panel.SetActive(true);
        AnimateStars(stars);
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
        // Reset semua bintang dulu
        SetStar(star1, false);
        SetStar(star2, false);
        SetStar(star3, false);

        // Animasi bintang satu per satu
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
        star.transform
            .DOPunchScale(Vector3.one * (starPunchScale - 1f), 0.4f, 5, 0.5f);
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