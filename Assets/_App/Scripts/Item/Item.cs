using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int itemId;
    public float weight;
    public string description;
    public ItemType itemType;
    public Sprite icon;
    
    public enum ItemType
    {
        Cube,
        Sphere,
        Capsule
    }
}
