using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class CollectibleItem : MonoBehaviour, ICollectible
{
    [Header("Data")]
    [SerializeField] private WasteItemData data;

    [Header("Pull Settings")]
    [SerializeField] protected float pullSpeed = 6f;
    [SerializeField] protected float pullAcceleration = 8f;
    [SerializeField] protected float collectDistance = 0.15f;

    protected Transform target;
    protected bool isCaptured;
    protected float currentSpeed;

    // Public getter buat sorting mini-game akses kategori & score
    public WasteItemData Data => data;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // null kalau belum ada

        ApplyData();
    }

    private void ApplyData()
    {
        if (data == null) return;
        if (spriteRenderer != null)
            spriteRenderer.sprite = data.itemSprite;
    }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        Collect();
    }

    public virtual void Collect()
    {
        AudioManager.Instance?.PlaySFX("collect");

        // Tambahkan ke inventory player
        if (data != null)
            PlayerInventory.Instance?.AddItem(data);

        // untuk visual toca sampah
        TrashBin.Instance?.AddTrash(1);

        if (animator != null)
        {
            PlayDestroyAnimation();
            return;
        }

        Destroy(gameObject);
    }

    public void OnDestroyAnimationComplete()
    {
        Destroy(gameObject);
    }

    protected virtual void PlayDestroyAnimation()
    {
        // TODO: isi trigger name sesuai Animator kamu nanti
        // Contoh: animator.SetTrigger("Destroy");
        animator.SetTrigger("Destroy");
    }
}