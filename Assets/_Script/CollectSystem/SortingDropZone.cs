using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class SortingDropZone : MonoBehaviour, IDropHandler
{
    public event Action OnItemDropped;

    public void OnDrop(PointerEventData eventData)
    {
        // Cek apakah yang di-drop adalah DraggableItem
        if (eventData.pointerDrag == null) return;
        if (!eventData.pointerDrag.CompareTag("DraggableItem")) return;

        OnItemDropped?.Invoke();
    }
}