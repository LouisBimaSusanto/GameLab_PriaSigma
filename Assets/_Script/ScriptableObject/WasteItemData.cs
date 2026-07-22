using UnityEngine;

public enum WasteCategory
{
    Anorganic,
    Organic
}

[CreateAssetMenu(fileName = "NewWasteItem", menuName ="Waste Sorter/Waste Item Data")]
public class WasteItemData : ScriptableObject
{
    [Header("Identity")]
    public string itemName;
    public WasteCategory wasteCategory;
    public Sprite itemSprite;

    [Header("Score")]
    public int scoreValue = 1;

    [Header("Education")]
    [TextArea(3, 6)]
    public string description;
}
