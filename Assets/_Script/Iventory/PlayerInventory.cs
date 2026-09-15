using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    private Queue<WasteItemData> collectedItems =
        new Queue<WasteItemData>();

    public int ItemCount =>
        collectedItems.Count;

    public event Action OnInventoryChanged;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void AddItem(
        WasteItemData item)
    {
        if (item == null)
        {
            Debug.LogWarning(
                "[PlayerInventory] " +
                "Mencoba memasukkan item NULL."
            );

            return;
        }

        collectedItems.Enqueue(item);

        Debug.Log(
            $"[PlayerInventory] " +
            $"Item added: {item.name}. " +
            $"Total: {collectedItems.Count}"
        );

        OnInventoryChanged?.Invoke();
    }

    public WasteItemData DequeueNextItem()
    {
        if (collectedItems.Count == 0)
            return null;

        WasteItemData item =
            collectedItems.Dequeue();

        OnInventoryChanged?.Invoke();

        return item;
    }

    public bool HasItems()
    {
        return collectedItems.Count > 0;
    }


    public List<WasteItemData> PeekBatch(
        int count)
    {
        List<WasteItemData> result =
            new List<WasteItemData>();

        foreach (WasteItemData item
                 in collectedItems)
        {
            if (result.Count >= count)
                break;

            result.Add(item);
        }

        return result;
    }


    public void ClearInventory()
    {
        collectedItems.Clear();

        OnInventoryChanged?.Invoke();

        Debug.Log(
            "[PlayerInventory] Inventory cleared."
        );
    }
}