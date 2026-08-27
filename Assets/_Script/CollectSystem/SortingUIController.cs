using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SortingUIController : MonoBehaviour
{
    public static SortingUIController Instance { get; private set; }

    [Header("Item Spawning")]
    [SerializeField] private GameObject sortingItemPrefab;
    [SerializeField] private RectTransform itemSpawnArea;
    [SerializeField] private int maxItemsOnScreen = 6;

    [Header("Drop Zones")]
    [SerializeField] private SortingDropZone organicZone;
    [SerializeField] private SortingDropZone anorganicZone;
    [SerializeField] private SortingDropZone limbahZone;
    [SerializeField] private MagnifierDropZone magnifierZone;

    [Header("Info Panel")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Image infoPanelSprite;
    [SerializeField] private TMP_Text infoPanelName;
    [SerializeField] private TMP_Text infoPanelDescription;
    [SerializeField] private Button closePanelButton;

    [Header("Inventory Counter")]
    [SerializeField] private GameObject inventoryCounter;
    [SerializeField] private TMP_Text inventoryCountText;
    [SerializeField] private Image inventoryIconImage;

    [Header("Empty State")]
    [SerializeField] private GameObject emptyStateUI;

    [Header("Scatter Settings")]
    [SerializeField] private float scatterPadding = 80f;

    private List<SortingItemUI> activeItems = new List<SortingItemUI>();
    private int remainingInQueue => PlayerInventory.Instance.ItemCount;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void OnEnable()
    {
        SubscribeDropZones();
        closePanelButton.onClick.AddListener(CloseInfoPanel);
        infoPanel.SetActive(false);
        emptyStateUI.SetActive(false);

        LoadBatch();
        UpdateInventoryCounter();
    }

    private void OnDisable()
    {
        UnsubscribeDropZones();
        closePanelButton.onClick.RemoveListener(CloseInfoPanel);
    }


    private void SubscribeDropZones()
    {
        organicZone.OnCorrectDrop += HandleCorrectDrop;
        organicZone.OnWrongDrop += HandleWrongDrop;

        anorganicZone.OnCorrectDrop += HandleCorrectDrop;
        anorganicZone.OnWrongDrop += HandleWrongDrop;

        limbahZone.OnCorrectDrop += HandleCorrectDrop;
        limbahZone.OnWrongDrop += HandleWrongDrop;

        magnifierZone.OnItemInspected += ShowInfoPanel;
    }

    private void UnsubscribeDropZones()
    {
        organicZone.OnCorrectDrop -= HandleCorrectDrop;
        organicZone.OnWrongDrop -= HandleWrongDrop;

        anorganicZone.OnCorrectDrop -= HandleCorrectDrop;
        anorganicZone.OnWrongDrop -= HandleWrongDrop;

        limbahZone.OnCorrectDrop -= HandleCorrectDrop;
        limbahZone.OnWrongDrop -= HandleWrongDrop;

        magnifierZone.OnItemInspected -= ShowInfoPanel;
    }

    private void LoadBatch()
    {
        if (!PlayerInventory.Instance.HasItems())
        {
            CheckAllSorted();
            return;
        }

        int toSpawn = Mathf.Min(maxItemsOnScreen, PlayerInventory.Instance.ItemCount);

        for (int i = 0; i < toSpawn; i++)
        {
            WasteItemData data = PlayerInventory.Instance.DequeueNextItem();
            SpawnItem(data);
        }

        UpdateInventoryCounter();
    }

    private void SpawnItem(WasteItemData data)
    {
        GameObject go = Instantiate(sortingItemPrefab, itemSpawnArea);
        SortingItemUI itemUI = go.GetComponent<SortingItemUI>();

        // Posisi random dalam spawn area dengan padding
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchoredPosition = GetRandomPosition();

        itemUI.Setup(data);
        activeItems.Add(itemUI);
    }

    private Vector2 GetRandomPosition()
    {
        float halfW = (itemSpawnArea.rect.width / 2f) - scatterPadding;
        float halfH = (itemSpawnArea.rect.height / 2f) - scatterPadding;

        return new Vector2(
            UnityEngine.Random.Range(-halfW, halfW),
            UnityEngine.Random.Range(-halfH, halfH)
        );
    }


    private void HandleCorrectDrop(SortingItemUI item)
    {
        AudioManager.Instance?.PlaySFX("correct");
        StageManager.Instance.AddScore(item.Data.scoreValue);
        activeItems.Remove(item);

        item.PlayCorrectAnimation(() =>
        {
            Destroy(item.gameObject);
            CheckAllSorted();
        });
    }

    private void HandleWrongDrop(SortingItemUI item)
    {
        AudioManager.Instance?.PlaySFX("wrong");
        Timer.Instance?.AddTime(-5f);
        item.PlayWrongAnimation();
    }

    private void CheckAllSorted()
    {
        // Masih ada item aktif di layar
        if (activeItems.Count > 0) return;

        if (PlayerInventory.Instance.HasItems())
        {
            LoadBatch();
            return;
        }

        // Semua selesai!
        ShowEmptyState();
    }

    private void ShowInfoPanel(WasteItemData data)
    {
        infoPanel.SetActive(true);
        infoPanelSprite.sprite = data.itemSprite;
        infoPanelName.text = data.itemName;
        infoPanelDescription.text = data.description;
    }

    private void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
    }

    private void UpdateInventoryCounter()
    {
        int remaining = PlayerInventory.Instance.ItemCount;

        inventoryCounter.SetActive(remaining > 0);

        if (remaining > 0)
            inventoryCountText.text = remaining.ToString();
    }

    private void ShowEmptyState()
    {
        emptyStateUI.SetActive(true);

        // Delay sebentar sebelum kembali ke drive
        DOVirtual.DelayedCall(1.5f, () =>
        {
            emptyStateUI.SetActive(false);
            GameFlowController.Instance.ExitSortingPhase();
        });
    }
}