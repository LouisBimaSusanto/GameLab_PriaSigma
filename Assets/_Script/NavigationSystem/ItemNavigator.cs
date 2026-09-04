using UnityEngine;
using UnityEngine.EventSystems;

public class ItemNavigator : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private OrbitalIndicatorUI orbitalIndicator;

    [Header("Settings")]
    [SerializeField] private float updateInterval = 0.2f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < updateInterval) return;
        timer = 0f;

        CollectibleItem nearest = FindNearestItem();

        if (nearest != null)
        {
            orbitalIndicator.Show(nearest.transform.position);
        }
        else
        {
            orbitalIndicator.Hide();
        }
    }

    private CollectibleItem FindNearestItem()
    {
        CollectibleItem[] items = Object.FindObjectsByType<CollectibleItem>(FindObjectsSortMode.None);

        CollectibleItem nearest = null;
        float nearestDist = Mathf.Infinity;

        foreach (var item in items)
        {
            if (item.IsCaptured) continue;

            float dist = Vector2.Distance(transform.position, item.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = item;
            }
        }
        return nearest;
    }
}
