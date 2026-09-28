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

    [SerializeField]
    private Ease closeEase =
        Ease.InQuad;

    [SerializeField]
    private Ease openEase =
        Ease.OutQuad;

    private const float OpenRadius = 1.5f;
    private const float ClosedRadius = 0f;

    private const string RadiusProperty =
        "_Radius";

    private Material _irisMat;

    private bool isPlaying = false;

    // =========================================================
    // AWAKE
    // =========================================================

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (irisImage == null)
        {
            Debug.LogError(
                "[IrisTransition] " +
                "irisImage belum di-assign!"
            );

            return;
        }

        if (transitionRoot == null)
        {
            Debug.LogError(
                "[IrisTransition] " +
                "transitionRoot belum di-assign!"
            );

            return;
        }

        // Create material instance
        _irisMat =
            new Material(
                irisImage.material
            );

        irisImage.material =
            _irisMat;

        _irisMat.SetFloat(
            RadiusProperty,
            OpenRadius
        );

        transitionRoot.SetActive(false);
    }

    // =========================================================
    // DESTROY
    // =========================================================

    private void OnDestroy()
    {
        if (_irisMat != null)
        {
            Destroy(_irisMat);
        }
    }

    // =========================================================
    // PLAY TRANSITION
    // =========================================================

    public void PlayTransition(
        Action onMidpoint,
        Action onComplete = null)
    {
        if (isPlaying)
        {
            Debug.LogWarning(
                "[IrisTransition] " +
                "Transition sedang berjalan."
            );

            return;
        }

        if (_irisMat == null)
        {
            Debug.LogError(
                "[IrisTransition] " +
                "Material iris NULL!"
            );

            return;
        }

        isPlaying = true;

        transitionRoot.SetActive(true);

        _irisMat.SetFloat(
            RadiusProperty,
            OpenRadius
        );

        Sequence sequence =
            DOTween.Sequence();

        // =====================================================
        // CLOSE
        // =====================================================

        sequence.Append(
            BuildRadiusTween(
                OpenRadius,
                ClosedRadius,
                closeDuration,
                closeEase
            )
        );

        sequence.AppendCallback(() =>
        {
            Debug.Log(
                "[IrisTransition] " +
                "MIDPOINT."
            );

            onMidpoint?.Invoke();
        });

        sequence.Append(
            BuildRadiusTween(
                ClosedRadius,
                OpenRadius,
                openDuration,
                openEase
            )
        );

        sequence.OnComplete(() =>
        {
            Debug.Log(
                "[IrisTransition] " +
                "COMPLETE."
            );

            transitionRoot.SetActive(false);

            isPlaying = false;

            onComplete?.Invoke();
        });

        sequence.OnKill(() =>
        {
            isPlaying = false;
        });
    }

    public void CloseOnly(
        Action onComplete = null)
    {
        if (isPlaying)
        {
            Debug.LogWarning(
                "[IrisTransition] " +
                "Transition sedang berjalan."
            );

            return;
        }

        if (_irisMat == null)
        {
            Debug.LogError(
                "[IrisTransition] " +
                "Material iris NULL!"
            );

            return;
        }

        isPlaying = true;

        transitionRoot.SetActive(true);

        _irisMat.SetFloat(
            RadiusProperty,
            OpenRadius
        );

        BuildRadiusTween(
            OpenRadius,
            ClosedRadius,
            closeDuration,
            closeEase
        )
        .OnComplete(() =>
        {
            Debug.Log(
                "[IrisTransition] " +
                "CloseOnly COMPLETE."
            );

            isPlaying = false;

            onComplete?.Invoke();
        })
        .OnKill(() =>
        {
            isPlaying = false;
        });
    }

    private Tweener BuildRadiusTween(
        float from,
        float to,
        float duration,
        Ease ease)
    {
        _irisMat.SetFloat(
            RadiusProperty,
            from
        );

        return DOTween.To(
            () =>
                _irisMat.GetFloat(
                    RadiusProperty
                ),

            radius =>
                _irisMat.SetFloat(
                    RadiusProperty,
                    radius
                ),

            to,
            duration
        )
        .SetEase(ease);
    }
}