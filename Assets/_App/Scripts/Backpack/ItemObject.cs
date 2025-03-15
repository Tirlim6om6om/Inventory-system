using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public IDraggableWithStates Draggable { get { return draggable; } }
    
    [SerializeField] private Item item;
    [SerializeField] private ItemDragHandler draggable;
    
    public Item PickUp()
    {
        Destroy(gameObject);
        return item;
    }
}
