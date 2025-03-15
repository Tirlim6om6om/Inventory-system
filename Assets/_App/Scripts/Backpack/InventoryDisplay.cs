using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private Transform gridContainer; // Контейнер с Grid Layout Group
    [SerializeField] private GameObject itemSlotPrefab; // Префаб ячейки инвентаря

    private Dictionary<int, GameObject> itemSlots = new Dictionary<int, GameObject>();
    private Inventory _inventory;
    
    [Inject]
    public void Construct(Inventory inventory)
    {
        _inventory = inventory;
        _inventory.OnAddItem += OnItemAdded;
        _inventory.OnRemoveItem += OnItemRemoved;
        RefreshDisplay(_inventory.items);
    }

    private void OnDestroy()
    {
        if (_inventory != null)
        {
            _inventory.OnAddItem -= OnItemAdded;
            _inventory.OnRemoveItem -= OnItemRemoved;
        }
    }

    private void OnItemAdded(Item item)
    {
        RefreshDisplay(_inventory.items);
    }

    private void OnItemRemoved(Item item)
    {
        RefreshDisplay(_inventory.items);
    }

    public void RefreshDisplay(List<Item> items)
    {
        ClearDisplay();

        Dictionary<int, int> itemCounts = new Dictionary<int, int>();
        Dictionary<int, Item> uniqueItems = new Dictionary<int, Item>();

        foreach (Item item in items)
        {
            if (itemCounts.ContainsKey(item.itemId))
            {
                itemCounts[item.itemId]++;
            }
            else
            {
                itemCounts[item.itemId] = 1;
                uniqueItems[item.itemId] = item;
            }
        }

        foreach (var kvp in uniqueItems)
        {
            CreateItemSlot(kvp.Value, itemCounts[kvp.Key]);
        }
    }

    private void CreateItemSlot(Item item, int count)
    {
        // Создаем слот из префаба
        GameObject slotObject = Instantiate(itemSlotPrefab, gridContainer);

        // Настраиваем отображение предмета в слоте
        ItemSlotUI slotUI = slotObject.GetComponent<ItemSlotUI>();
        if (slotUI != null)
        {
            slotUI.SetupSlot(item, count);
        }

        // Сохраняем слот в словаре
        itemSlots[item.itemId] = slotObject;
    }

    private void ClearDisplay()
    {
        // Удаляем все текущие слоты
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        itemSlots.Clear();
    }
}