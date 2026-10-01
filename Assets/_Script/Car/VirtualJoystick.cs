using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("UI References")]
    public RectTransform backgroundArea; 
    public RectTransform handle;       

    [Header("Settings")]
    [Range(0f, 1f)] public float handleLimit = 1f;

    public Vector2 InputDirection { get; private set; }

    private RectTransform touchZone; 
    private CanvasGroup canvasGroup; 

    private void Awake()
    {
        touchZone = GetComponent<RectTransform>();

        canvasGroup = backgroundArea.GetComponent<CanvasGroup>();

        HideJoystick();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            touchZone,
            eventData.position,
            eventData.pressEventCamera,
            out localPos
        );
        backgroundArea.anchoredPosition = localPos;

        ShowJoystick();

        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            backgroundArea,
            eventData.position,
            eventData.pressEventCamera,
            out localPos
        );

        float x = localPos.x / (backgroundArea.sizeDelta.x / 2f);
        float y = localPos.y / (backgroundArea.sizeDelta.y / 2f);

        InputDirection = new Vector2(x, y);

        if (InputDirection.magnitude > 1f)
        {
            InputDirection = InputDirection.normalized;
        }

        handle.anchoredPosition = new Vector2(
            InputDirection.x * (backgroundArea.sizeDelta.x / 2f) * handleLimit,
            InputDirection.y * (backgroundArea.sizeDelta.y / 2f) * handleLimit
        );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputDirection = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;

        HideJoystick();
    }

    private void ShowJoystick()
    {
        if (canvasGroup != null) canvasGroup.alpha = 1f;
        else backgroundArea.gameObject.SetActive(true);
    }

    private void HideJoystick()
    {
        if (canvasGroup != null) canvasGroup.alpha = 0f;
        else backgroundArea.gameObject.SetActive(false);
    }
}