using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    [SerializeField] private Transform target;      // drag Player ke sini
    [SerializeField] private bool clampToBounds;
    [SerializeField] private Collider2D mapBounds;  // opsional: drag "Batas Map"

    private Camera cam;

    void Awake() => cam = GetComponent<Camera>();

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 pos = target.position;
        pos.z = transform.position.z;

        if (clampToBounds && mapBounds != null)
        {
            Bounds b = mapBounds.bounds;
            float halfH = cam.orthographicSize;
            float halfW = halfH * cam.aspect;
            pos.x = Mathf.Clamp(pos.x, b.min.x + halfW, b.max.x - halfW);
            pos.y = Mathf.Clamp(pos.y, b.min.y + halfH, b.max.y - halfH);
        }

        transform.position = pos;
    }
}