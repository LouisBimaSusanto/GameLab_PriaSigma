using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class SortingDropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] private WasteCategory acceptedCategory;

    public event Action<SortingItemUI> OnCorrectDrop;
    public event Action<SortingItemUI> OnWrongDrop;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        SortingItemUI item = eventData.pointerDrag.GetComponent<SortingItemUI>();
        if (item == null) return;

        if (item.Data.wasteCategory == acceptedCategory)
            OnCorrectDrop?.Invoke(item);
        else
            OnWrongDrop?.Invoke(item);
    }
}