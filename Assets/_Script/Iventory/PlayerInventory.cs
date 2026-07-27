using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    private Queue<WasteItemData> collectedItems = new Queue<WasteItemData>();

    public int ItemCount => collectedItems.Count;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void AddItem(WasteItemData item)
    {
        collectedItems.Enqueue(item);
        Debug.Log($"Collected: {item.itemName} | Total: {collectedItems.Count}");
    }

    public WasteItemData PeekNextItem()
    {
        return collectedItems.Count > 0 ? collectedItems.Peek() : null;
    }

    public WasteItemData DequeueNextItem()
    {
        return collectedItems.Count > 0 ? collectedItems.Dequeue() : null;
    }

    public bool HasItems() => collectedItems.Count > 0;

    public void ClearInventory()
    {
        collectedItems.Clear();
    }
}