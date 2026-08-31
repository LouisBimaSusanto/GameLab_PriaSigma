using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public static TrashBin Instance { get; private set; }

    [Header("Referensi")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Sprite Stages (urutkan dari KOSONG -> PENUH)")]
    [Tooltip("Index 0 = kosong, index terakhir = penuh")]
    [SerializeField] private Sprite[] trashSprites;

    [Header("Pengaturan")]
    [Tooltip("Jumlah sampah maksimal yang dibutuhkan sampai toca penuh")]
    [SerializeField] private int maxTrashCount = 10;

    private int currentTrashCount = 0;

    private void Awake()
    {
        // Singleton, mengikuti pola AudioManager.Instance / PlayerInventory.Instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Auto-assign kalau lupa drag di Inspector
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateVisual();
    }

    public void AddTrash(int amount = 1)
    {
        currentTrashCount = Mathf.Clamp(currentTrashCount + amount, 0, maxTrashCount);
        UpdateVisual();

        if (currentTrashCount >= maxTrashCount)
        {
            OnBinFull();
        }
    }

    private void UpdateVisual()
    {
        if (trashSprites == null || trashSprites.Length == 0) return;

        // Hitung index sprite berdasarkan progress (0 - 1)
        float progress = (float)currentTrashCount / maxTrashCount;
        int spriteIndex = Mathf.FloorToInt(progress * (trashSprites.Length - 1));
        spriteIndex = Mathf.Clamp(spriteIndex, 0, trashSprites.Length - 1);

        spriteRenderer.sprite = trashSprites[spriteIndex];
    }

    private void OnBinFull()
    {
        
        Debug.Log("Toca sampah sudah penuh!");
    }

    public int CurrentTrashCount => currentTrashCount;
    public int MaxTrashCount => maxTrashCount;
}