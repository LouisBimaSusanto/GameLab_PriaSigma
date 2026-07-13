using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class CollectZone : MonoBehaviour
{
    [Header("Follow Target")]
    [SerializeField] private Transform followTarget;

    [Header("Radius Formula")]
    [SerializeField] private float baseRadius = 2f;
    [SerializeField] private float growthPerStage = 0.5f;

    private CircleCollider2D triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<CircleCollider2D>();
        triggerCollider.isTrigger = true;
    }

    private void Start()
    {
        SetStage(StageManager.Instance.CurrentStage);
        StageManager.Instance.OnStageChanged += SetStage;
    }

    private void OnDestroy()
    {
        if (StageManager.Instance != null)
            StageManager.Instance.OnStageChanged -= SetStage;
    }

    private void LateUpdate()
    {
        if (followTarget == null) return;
        transform.position = followTarget.position;
    }

    private void SetStage(int stage)
    {
        triggerCollider.radius = baseRadius + (growthPerStage * stage);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<ICollectible>(out var collectible))
        {
            collectible.OnCaptured(followTarget != null ? followTarget : transform);
        }
    }
}