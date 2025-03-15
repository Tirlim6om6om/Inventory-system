using System;
using System.Collections.Generic;
using UnityEngine;

public class BackpackZone : MonoBehaviour
{
    public event Action<ItemObject> OnAddItem;
    
    private ItemObject _itemInZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ItemObject item))
        {
            if (item.Draggable.IsDragging)
            {
                _itemInZone = item;
                _itemInZone.ItemEventSystem.OnDrop += OnDropItem;
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
                _itemInZone.ItemEventSystem.OnDrop -= OnDropItem;
                _itemInZone = null;
            }
        }
    }

    private void OnDropItem()
    {
        AddItem(_itemInZone);
    }

    private void AddItem(ItemObject itemObject)
    {
        if (_itemInZone != null)
        {
            _itemInZone.ItemEventSystem.OnDrop -= OnDropItem;
            _itemInZone = null;
        }

        OnAddItem?.Invoke(itemObject);
    }
}
