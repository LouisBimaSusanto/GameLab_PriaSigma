using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using System;

public class SortingItemUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("References")]
    [SerializeField] private Image itemImage;

    public WasteItemData Data { get; private set; }

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Vector2 originalPosition;
    private Transform originalParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void Setup(WasteItemData data)
    {
        Data = data;
        itemImage.sprite = data.itemSprite;

        // Animasi muncul
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;

        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();

        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Kembalikan ke parent & posisi asal kalau tidak di-drop ke zona valid
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = originalPosition;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void PlayCorrectAnimation(Action onComplete)
    {
        // Animasi benar: scale up lalu hilang
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(1.2f, 0.15f));
        seq.Append(transform.DOScale(0f, 0.2f).SetEase(Ease.InBack));
        seq.OnComplete(() => onComplete?.Invoke());
    }

    public void PlayWrongAnimation()
    {
        // Shake lalu balik ke posisi asal
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = originalPosition;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        rectTransform.DOShakeAnchorPos(0.4f, 20f, 30)
            .SetEase(Ease.OutElastic);
    }
}