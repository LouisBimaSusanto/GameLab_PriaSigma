using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CollectibleItem : MonoBehaviour, ICollectible
{
    [Header("Item Settings")]
    [SerializeField] protected float pullSpeed = 6f;
    [SerializeField] protected float pullAcceleration = 8f;
    [SerializeField] protected float collectDistance = 0.15f;

    protected Transform target;
    protected bool isCaptured;
    protected float currentSpeed;

    protected virtual void Update()
    {
        if (!isCaptured || target == null) return;

        currentSpeed += pullAcceleration * Time.deltaTime;
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            currentSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) <= collectDistance)
        {
            Collect();
        }
    }

    public virtual void OnCaptured(Transform capturer)
    {
        if (isCaptured) return;
        isCaptured = true;
        target = capturer;
        currentSpeed = pullSpeed;
    }

    public virtual void Collect()
    {
        AudioManager.Instance?.PlaySFX("collect");
        Destroy(gameObject);
    }
}