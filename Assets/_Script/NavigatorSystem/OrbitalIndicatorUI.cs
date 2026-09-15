using UnityEngine;

public class OrbitalIndicatorUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform dotRect;
    [SerializeField] private Transform carTransform;

    [Header("Settings")]
    [SerializeField] private float orbitRadius = 50f;

    private Vector3 targetWorldPos;
    private bool isActive;
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (!isActive || carTransform == null) return;

        // Pakai WorldToScreenPoint supaya rotation camera Cinemachine ikut diperhitungkan
        Vector2 carScreenPos = RectTransformUtility.WorldToScreenPoint(mainCam, carTransform.position);
        Vector2 itemScreenPos = RectTransformUtility.WorldToScreenPoint(mainCam, targetWorldPos);

        Vector2 dir = (itemScreenPos - carScreenPos).normalized;

        dotRect.anchoredPosition = dir * orbitRadius;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        dotRect.localRotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    public void Show(Vector3 itemWorldPos)
    {
        targetWorldPos = itemWorldPos;
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        isActive = true;
    }

    public void Hide()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}