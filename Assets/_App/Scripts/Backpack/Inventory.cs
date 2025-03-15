using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public int maxSize = 20;
    public List<InventorySlot> slots = new List<InventorySlot>();
    
    public UnityAction<Item> OnItemAdded;
    public UnityAction<Item> OnItemRemoved;
    public UnityAction<Item, int, int> OnItemMoved;
    
    public bool AddItem(Item item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = item;
                slots[i].amount = 1;
                OnItemAdded?.Invoke(item);
                SendServerRequest(item.itemId, "add");
                return true;
            }
        }
        return false;
    }
    
    public bool RemoveItem(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slots.Count && slots[slotIndex].item != null)
        {
            Item removedItem = slots[slotIndex].item;
            slots[slotIndex].Clear();
            OnItemRemoved?.Invoke(removedItem);
            SendServerRequest(removedItem.itemId, "remove");
            return true;
        }
        return false;
    }
    
    public bool SwapItems(int sourceIndex, int targetIndex)
{
    // Проверяем валидность индексов
    if (sourceIndex < 0 || sourceIndex >= slots.Count || 
        targetIndex < 0 || targetIndex >= slots.Count)
    {
        Debug.LogWarning($"Invalid slot indices: {sourceIndex}, {targetIndex}");
        return false;
    }
    
    // Получаем ссылки на слоты
    InventorySlot sourceSlot = slots[sourceIndex];
    InventorySlot targetSlot = slots[targetIndex];
    
    // Если исходный слот пустой, нет смысла меняться
    if (sourceSlot.item == null)
    {
        Debug.LogWarning("Source slot is empty, nothing to swap");
        return false;
    }
    
    // Проверяем, можно ли объединить предметы (одинаковые ID и предмет может складываться)
    if (targetSlot.item != null && 
        sourceSlot.item.itemId == targetSlot.item.itemId && 
        sourceSlot.item.isStackable)
    {
        // Добавляем количество из исходного слота в целевой
        targetSlot.amount += sourceSlot.amount;
        
        // Очищаем исходный слот
        Item movedItem = sourceSlot.item;
        sourceSlot.Clear();
        
        // Уведомляем о перемещении (необходимо для UI и логирования)
        OnItemMoved?.Invoke(movedItem, sourceIndex, targetIndex);

        return true;
    }
    
    // Если объединение не возможно или целевой слот пуст, просто меняем местами
    Item sourceItem = sourceSlot.item;
    int sourceAmount = sourceSlot.amount;
    
    Item targetItem = targetSlot.item;
    int targetAmount = targetSlot.amount;
    
    // Меняем местами предметы
    sourceSlot.item = targetItem;
    sourceSlot.amount = targetAmount;
    
    targetSlot.item = sourceItem;
    targetSlot.amount = sourceAmount;
    
    // Уведомляем о перемещении предметов
    if (sourceItem != null)
        OnItemMoved?.Invoke(sourceItem, sourceIndex, targetIndex);
    
    if (targetItem != null)
        OnItemMoved?.Invoke(targetItem, targetIndex, sourceIndex);
    
    return true;
}
    
    private void SendServerRequest(int itemId, string action)
    {
        // Реализация отправки запроса на сервер
        Debug.Log($"Sending server request: Item ID {itemId}, Action: {action}");
    }
}