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

    protected bool isCollected;

    protected float currentSpeed;

    // =========================================================
    // PUBLIC
    // =========================================================

    public WasteItemData Data => data;

    public bool IsCaptured => isCaptured;

    public bool IsCollected => isCollected;

    private SpriteRenderer spriteRenderer;

    private Animator animator;

    // =========================================================
    // AWAKE
    // =========================================================

    protected virtual void Awake()
    {
        spriteRenderer =
            GetComponent<SpriteRenderer>();

        animator =
            GetComponent<Animator>();

        ApplyData();
    }

    // =========================================================
    // APPLY DATA
    // =========================================================

    private void ApplyData()
    {
        if (data == null)
        {
            Debug.LogWarning(
                $"[CollectibleItem] " +
                $"{gameObject.name} tidak memiliki " +
                "WasteItemData."
            );

            return;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite =
                data.itemSprite;
        }
    }

    // =========================================================
    // UPDATE
    // =========================================================

    protected virtual void Update()
    {
        if (!isCaptured)
            return;

        if (isCollected)
            return;

        if (target == null)
            return;

        currentSpeed +=
            pullAcceleration *
            Time.deltaTime;

        transform.position =
            Vector3.MoveTowards(
                transform.position,
                target.position,
                currentSpeed *
                Time.deltaTime
            );

        if (Vector3.Distance(
            transform.position,
            target.position
        ) <= collectDistance)
        {
            Collect();
        }
    }

    // =========================================================
    // CAPTURE
    // =========================================================

    public virtual void OnCaptured(
        Transform capturer)
    {
        if (isCaptured)
            return;

        if (isCollected)
            return;

        if (capturer == null)
        {
            Debug.LogWarning(
                $"[CollectibleItem] " +
                $"{gameObject.name} menerima " +
                "capturer NULL."
            );

            return;
        }

        isCaptured = true;

        target = capturer;

        currentSpeed = pullSpeed;

        Debug.Log(
            $"[CollectibleItem] " +
            $"{gameObject.name} captured."
        );
    }

    // =========================================================
    // COLLISION
    // =========================================================

    private void OnCollisionEnter2D(
        Collision2D collision)
    {
        if (isCollected)
            return;

        if (!collision.gameObject.CompareTag(
            "Player"))
        {
            return;
        }

        Collect();
    }

    // =========================================================
    // COLLECT
    // =========================================================

    public virtual void Collect()
    {
        // ==========================================
        // PREVENT DOUBLE COLLECT
        // ==========================================

        if (isCollected)
            return;

        isCollected = true;

        // ==========================================
        // CAPTURE STATE
        // ==========================================

        isCaptured = true;

        Debug.Log(
            $"[CollectibleItem] " +
            $"{gameObject.name} COLLECTED."
        );

        // ==========================================
        // AUDIO
        // ==========================================

        AudioManager.Instance?.PlaySFX(
            "collect"
        );

        // ==========================================
        // INVENTORY
        // ==========================================

        if (data != null)
        {
            if (PlayerInventory.Instance != null)
            {
                PlayerInventory.Instance.AddItem(
                    data
                );

                Debug.Log(
                    $"[CollectibleItem] " +
                    $"Item masuk inventory: " +
                    $"{data.name}"
                );
            }
            else
            {
                Debug.LogError(
                    "[CollectibleItem] " +
                    "PlayerInventory.Instance NULL!"
                );
            }
        }

        // ==========================================
        // TRASH BIN VISUAL
        // ==========================================

        TrashBin.Instance?.AddTrash(1);

        // ==========================================
        // REGISTER TO STAGE
        // ==========================================

        if (StageManager.Instance != null)
        {
            StageManager.Instance
                .RegisterTrashCollected();
        }
        else
        {
            Debug.LogError(
                "[CollectibleItem] " +
                "StageManager.Instance NULL!"
            );
        }

        // ==========================================
        // DESTROY
        // ==========================================

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

    // =========================================================
    // PLAY DESTROY ANIMATION
    // =========================================================

    protected virtual void PlayDestroyAnimation()
    {
        animator.SetTrigger(
            "Destroy"
        );
    }
}