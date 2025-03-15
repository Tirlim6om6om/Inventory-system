using System.Collections.Generic;
using UnityEngine;

public class BackpackZone : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    private ItemObject _itemDraggable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ItemObject item))
        {
            if (item.Draggable.IsDragging)
            {
                _itemDraggable = item;
                _itemDraggable.Draggable.OnDrop += OnDropItem;
            }
            else
            {
                AddItem(item);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ItemObject item))
        {
            if (item.Draggable.IsDragging)
            {
                _itemDraggable.Draggable.OnDrop -= OnDropItem;
                _itemDraggable = null;
            }
        }
    }

    private void OnDropItem()
    {
        inventory.AddItem(_itemDraggable.PickUp());
        _itemDraggable = null;
    }

    private void AddItem(ItemObject itemObject)
    {
        inventory.AddItem(itemObject.PickUp());
    }
}
