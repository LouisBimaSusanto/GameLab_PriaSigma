using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using System;

public class SortingItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("References")]
    [SerializeField] private Image itemImage;

    public WasteItemData Data { get; private set; }

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Vector2 originalPosition;
    private Transform originalParent;

    private bool isSorted = false;

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

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSorted) return;

        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;

        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();

        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isSorted) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isSorted) return;

        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = originalPosition;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void PlayCorrectAnimation(Transform dropZoneTransform, Action onComplete)
    {
        isSorted = true;
        canvasGroup.blocksRaycasts = false;

        // Pindahkan parent ke tong sampah
        transform.SetParent(dropZoneTransform);
        transform.SetAsLastSibling();

        // Buat posisi acak di dalam kotak agar menumpuk natural
        float randomX = UnityEngine.Random.Range(-40f, 40f);
        float randomY = UnityEngine.Random.Range(-20f, 40f);
        float randomRot = UnityEngine.Random.Range(-30f, 30f);

        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOAnchorPos(new Vector2(randomX, randomY), 0.25f).SetEase(Ease.OutQuad));
        seq.Join(transform.DORotate(new Vector3(0, 0, randomRot), 0.25f));
        seq.Join(transform.DOScale(0.8f, 0.25f));
        seq.OnComplete(() => onComplete?.Invoke());
    }

    public void PlayWrongAnimation()
    {
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = originalPosition;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        rectTransform.DOShakeAnchorPos(0.4f, 20f, 30)
            .SetEase(Ease.OutElastic);
    }
}