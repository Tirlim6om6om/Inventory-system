using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

/// <summary>
/// Unity event - самые не оптимизированные события
/// Но по-скольку они есть в задании - они есть и здесь
/// Но я их использую как-бы опционально - чисто, если вдруг нужен будет воспроизвести звук и не писать код
/// </summary>
public class InventoryEventRecevier : MonoBehaviour
{
    public UnityEvent OnAddItem = new UnityEvent();
    public UnityEvent OnRemoveItem = new UnityEvent();

    private Inventory _inventory;
    
    [Inject]
    public void Construct(Inventory inventory)
    {
        _inventory = inventory;
        _inventory.OnAddItem += AddItemInvoke;
        _inventory.OnRemoveItem += RemoveItemInvoke;
    }

    private void OnDestroy()
    {
        _inventory.OnAddItem -= AddItemInvoke;
        _inventory.OnRemoveItem -= RemoveItemInvoke;
    }

    public void AddItemInvoke(Item item)
    {
        OnAddItem.Invoke();
    }

    public void RemoveItemInvoke(Item item)
    {
        OnRemoveItem.Invoke();
    }
}
