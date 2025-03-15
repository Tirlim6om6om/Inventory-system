using UnityEngine;
using Zenject;

/// <summary>
/// Class that allows you to control the subject from the outside
/// </summary>
public class ItemObject : MonoBehaviour
{
    public Item Item { get { return _item; } }
    public ItemEventSystem ItemEventSystem { get { return _itemEventSystem; } }
    public ItemLocker ItemLocker { get { return _itemLocker; } }
    public IDraggable Draggable { get { return _draggable; } }
    
    private Item _item;
    private ItemEventSystem _itemEventSystem;
    private ItemLocker _itemLocker;
    private IDraggable _draggable;

    [Inject]
    public void Construct(
        Item item, 
        ItemEventSystem itemEventSystem, 
        ItemLocker itemLocker,
        IDraggable draggable)
    {
        _item = item;
        _itemEventSystem = itemEventSystem;
        _itemLocker = itemLocker;
        _draggable = draggable;
    }
}
