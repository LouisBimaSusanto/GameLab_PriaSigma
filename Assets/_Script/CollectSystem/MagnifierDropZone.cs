using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class MagnifierDropZone : MonoBehaviour, IDropHandler
{
    public event Action<WasteItemData> OnItemInspected;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        SortingItemUI item = eventData.pointerDrag.GetComponent<SortingItemUI>();
        if (item == null) return;

        OnItemInspected?.Invoke(item.Data);
    }
}