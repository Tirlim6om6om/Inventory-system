using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int itemId;
    public float weight;
    public string description;
    public bool isStackable;
    public ItemType itemType;
    public Sprite icon;
    public GameObject prefab;
    
    public enum ItemType
    {
        Weapon,
        Consumable,
        Resource
    }
}