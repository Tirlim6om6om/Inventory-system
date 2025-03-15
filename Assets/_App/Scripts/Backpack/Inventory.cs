using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public List<Item> items;
    public event Action<Item> OnAddItem;
    public event Action<Item> OnRemoveItem;

    public void AddItem(Item item)
    {
        items.Add(item);
        OnAddItem?.Invoke(item);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        OnRemoveItem?.Invoke(item);
    }
}
