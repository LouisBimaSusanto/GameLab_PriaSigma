using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; }

    [Header("References")]
    [SerializeField] private RectTransform circle;       // Image lingkaran (child dari canvas)
    [SerializeField] private CanvasGroup canvasGroup;    // CanvasGroup di canvas

    [Header("Settings")]
    [SerializeField] private float closeDuration = 0.45f;
    [SerializeField] private float openDuration = 0.45f;
    [SerializeField] private float holdDelay = 0.05f;
    [SerializeField] private Ease closeEase = Ease.InOutSine;
    [SerializeField] private Ease openEase = Ease.InOutSine;

    private bool isTransitioning;
    private Tween activeTween;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        circle.localScale = Vector3.one;
        canvasGroup.blocksRaycasts = true;
        Open();
    }

    
    public void LoadScene(string sceneName)
    {
        if (isTransitioning) return;
        StartCoroutine(TransitionRoutine(sceneName));
    }

    private IEnumerator TransitionRoutine(string sceneName)
    {
        isTransitioning = true;
        canvasGroup.blocksRaycasts = true;

        // 1. Lingkaran membesar sampai layar tertutup
        activeTween?.Kill();
        activeTween = circle.DOScale(1f, closeDuration)
            .SetEase(closeEase)
            .SetUpdate(true); // tetap jalan walau Time.timeScale = 0

        yield return activeTween.WaitForCompletion();

        yield return new WaitForSecondsRealtime(holdDelay);

        // 2. Ganti scene
        SceneManager.LoadScene(sceneName);

        yield return null; // tunggu 1 frame supaya scene baru selesai Awake/Start

        // 3. Lingkaran mengecil, layar terbuka
        yield return OpenAndWait();

        isTransitioning = false;
    }

    private void Open()
    {
        activeTween?.Kill();
        activeTween = circle.DOScale(0f, openDuration)
            .SetEase(openEase)
            .SetUpdate(true)
            .OnComplete(() => canvasGroup.blocksRaycasts = false);
    }

    private IEnumerator OpenAndWait()
    {
        Open();
        yield return activeTween.WaitForCompletion();
    }

    private void OnDestroy()
    {
        activeTween?.Kill();
    }
}