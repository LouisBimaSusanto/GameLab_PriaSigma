using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
public class IrisTransition : MonoBehaviour
{
    public static IrisTransition Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Image irisImage;
    [SerializeField] private GameObject transitionRoot;

    [Header("Settings")]
    [SerializeField] private float closeDuration = 0.6f;
    [SerializeField] private float openDuration = 0.5f;
    [SerializeField] private Ease closeEase = Ease.InQuad;
    [SerializeField] private Ease openEase = Ease.OutQuad;

    // Radius in normalised UV space (aspect-ratio-corrected).
    // 1.5 safely covers all screen corners even at 2:1 ultra-wide aspect ratio.
    private const float OpenRadius = 1.5f;
    private const float ClosedRadius = 0f;
    private const string RadiusProperty = "_Radius";

    private Material _irisMat;
    private bool isPlaying = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Create a per-instance material so tweening doesn't affect shared assets.
        _irisMat = new Material(irisImage.material);
        irisImage.material = _irisMat;
        _irisMat.SetFloat(RadiusProperty, OpenRadius);

        transitionRoot.SetActive(false);
    }

    private void OnDestroy()
    {
        if (_irisMat != null)
            Destroy(_irisMat);
    }

    /// <paramref name="onMidpoint"/> fires when the screen is fully black.
    /// <paramref name="onComplete"/> fires once the iris has fully re-opened.
    public void PlayTransition(Action onMidpoint, Action onComplete = null)
    {
        if (isPlaying) return;
        transitionRoot.SetActive(true);
        _irisMat.SetFloat(RadiusProperty, OpenRadius);

        Sequence seq = DOTween.Sequence();

        // Phase 1 – iris close (radius shrinks → screen turns black)
        seq.Append(BuildRadiusTween(OpenRadius, ClosedRadius, closeDuration, closeEase));

        // Phase 2 – mid-point callback (full black; safe to swap content)
        seq.AppendCallback(() => onMidpoint?.Invoke());

        // Phase 3 – iris open (radius grows → game becomes visible again)
        seq.Append(BuildRadiusTween(ClosedRadius, OpenRadius, openDuration, openEase));

        seq.OnComplete(() =>
        {
            transitionRoot.SetActive(false);
            isPlaying = false;
            onComplete?.Invoke();
        });
    }

    public void CloseOnly(Action onComplete = null)
    {
        if (isPlaying) return;
        transitionRoot.SetActive(true);
        _irisMat.SetFloat(RadiusProperty, OpenRadius);

        BuildRadiusTween(OpenRadius, ClosedRadius, closeDuration, closeEase)
            .OnComplete(() =>
            {
                isPlaying = false;
                onComplete?.Invoke();
            });
    }

    private Tweener BuildRadiusTween(float from, float to, float duration, Ease ease)
    {
        _irisMat.SetFloat(RadiusProperty, from);

        return DOTween.To(
            () => _irisMat.GetFloat(RadiusProperty),
            r => _irisMat.SetFloat(RadiusProperty, r),
            to,
            duration
        ).SetEase(ease);
    }
}
