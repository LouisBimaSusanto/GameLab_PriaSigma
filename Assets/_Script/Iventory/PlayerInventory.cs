using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    private Queue<WasteItemData> collectedItems = new Queue<WasteItemData>();

    public int ItemCount => collectedItems.Count;
    public event Action OnInventoryChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void AddItem(WasteItemData item)
    {
        collectedItems.Enqueue(item);
        OnInventoryChanged?.Invoke();
    }

    public WasteItemData DequeueNextItem()
    {
        if (collectedItems.Count == 0) return null;
        var item = collectedItems.Dequeue();
        OnInventoryChanged?.Invoke();
        return item;
    }

    public bool HasItems() => collectedItems.Count > 0;

    public List<WasteItemData> PeekBatch(int count)
    {
        var result = new List<WasteItemData>();
        foreach (var item in collectedItems)
        {
            if (result.Count >= count) break;
            result.Add(item);
        }
        return result;
    }

    public void ClearInventory() => collectedItems.Clear();
}