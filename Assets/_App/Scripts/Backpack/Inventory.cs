using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public List<Item> items { get { return _items; } }
    public event Action<Item> OnAddItem;
    public event Action<Item> OnRemoveItem;

    private List<Item> _items = new List<Item>();

    public void AddItem(Item item)
    {
        _items.Add(item);
        OnAddItem?.Invoke(item);
    }

    public void RemoveItem(Item item)
    {
        _items.Remove(item);
        OnRemoveItem?.Invoke(item);
    }
}
