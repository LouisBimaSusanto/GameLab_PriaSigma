using UnityEngine;

public class OrbitalIndicatorUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform dotRect;
    [SerializeField] private Transform carTransform;

    [Header("Settings")]
    [SerializeField] private float orbitRadius = 50f;

    private Camera mainCam;
    private Vector3 targetWorldPos;
    private bool isActive;

    private void Awake()
    {
        mainCam = Camera.main;
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (!isActive || carTransform == null) return;

        Vector2 dir = (targetWorldPos - carTransform.position).normalized;

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