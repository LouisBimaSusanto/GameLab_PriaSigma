using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SortingUIController : MonoBehaviour
{
    public static SortingUIController Instance { get; private set; }

    [Header("Item Display")]
    [SerializeField] private Image itemSprite;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private GameObject draggableItem;

    [Header("Info Panel")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Image infoPanelSprite;
    [SerializeField] private TMP_Text infoPanelName;
    [SerializeField] private TMP_Text infoPanelDescription;
    [SerializeField] private Button closePanelButton;

    [Header("Drop Zones")]
    [SerializeField] private SortingDropZone organicZone;
    [SerializeField] private SortingDropZone anorganicZone;
    [SerializeField] private SortingDropZone magnifierZone;

    [Header("Empty State")]
    [SerializeField] private GameObject emptyStateUI; //Semua sampah sudah disortir

    private WasteItemData currentItem;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void OnEnable()
    {
        // Subscribe ke drop zone events
        organicZone.OnItemDropped += () => HandleSort(WasteCategory.Organic);
        anorganicZone.OnItemDropped += () => HandleSort(WasteCategory.Anorganic);
        magnifierZone.OnItemDropped += ShowInfoPanel;

        closePanelButton.onClick.AddListener(CloseInfoPanel);

        LoadNextItem();
    }

    private void OnDisable()
    {
        organicZone.OnItemDropped -= () => HandleSort(WasteCategory.Organic);
        anorganicZone.OnItemDropped -= () => HandleSort(WasteCategory.Anorganic);
        magnifierZone.OnItemDropped -= ShowInfoPanel;

        closePanelButton.onClick.RemoveListener(CloseInfoPanel);
    }

    private void LoadNextItem()
    {
        if (!PlayerInventory.Instance.HasItems())
        {
            ShowEmptyState();
            return;
        }

        currentItem = PlayerInventory.Instance.PeekNextItem();
        itemSprite.sprite = currentItem.itemSprite;
        itemNameText.text = currentItem.itemName;
        draggableItem.SetActive(true);
        infoPanel.SetActive(false);
    }

    private void HandleSort(WasteCategory chosenCategory)
    {
        if (currentItem == null) return;

        bool isCorrect = chosenCategory == currentItem.wasteCategory;

        if (isCorrect)
        {
            AudioManager.Instance?.PlaySFX("correct");
            StageManager.Instance.AddScore(currentItem.scoreValue);
            PlayerInventory.Instance.DequeueNextItem(); // konsumsi item dari queue
        }
        else
        {
            AudioManager.Instance?.PlaySFX("wrong");
            Timer.Instance?.AddTime(-5f); // penalty -5 detik
            // Item TIDAK di-dequeue — player harus sortir ulang item yang sama
        }

        LoadNextItem();
    }

    private void ShowInfoPanel()
    {
        if (currentItem == null) return;

        draggableItem.SetActive(false);
        infoPanel.SetActive(true);
        infoPanelSprite.sprite = currentItem.itemSprite;
        infoPanelName.text = currentItem.itemName;
        infoPanelDescription.text = currentItem.description;
    }

    private void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
        draggableItem.SetActive(true); // item muncul lagi, siap disortir
    }

    private void ShowEmptyState()
    {
        draggableItem.SetActive(false);
        emptyStateUI.SetActive(true);

        // Semua item selesai disortir, kembali ke drive
        GameFlowController.Instance.ExitSortingPhase();
    }
}