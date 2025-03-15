using UnityEngine;

/// <summary>
/// Представляет слот в инвентаре, содержащий предмет и его количество
/// </summary>
[System.Serializable]
public class InventorySlot
{
    /// <summary>
    /// Предмет в слоте
    /// </summary>
    public Item item;
    
    /// <summary>
    /// Количество предметов в слоте
    /// </summary>
    public int amount;
    
    /// <summary>
    /// Создает новый слот инвентаря
    /// </summary>
    public InventorySlot()
    {
        Clear();
    }
    
    /// <summary>
    /// Создает новый слот с указанным предметом и количеством
    /// </summary>
    public InventorySlot(Item _item, int _amount = 1)
    {
        item = _item;
        amount = Mathf.Max(1, _amount); // Убеждаемся, что количество не меньше 1
    }
    
    /// <summary>
    /// Добавляет указанное количество предметов в слот
    /// </summary>
    public bool AddAmount(int amountToAdd)
    {
        if (item == null)
            return false;
            
        amount += amountToAdd;
        return true;
    }
    
    /// <summary>
    /// Уменьшает количество предметов в слоте
    /// </summary>
    public bool RemoveAmount(int amountToRemove)
    {
        if (item == null || amountToRemove > amount)
            return false;
            
        amount -= amountToRemove;
        
        // Если количество стало 0, очищаем слот
        if (amount <= 0)
            Clear();
            
        return true;
    }
    
    /// <summary>
    /// Очищает слот (удаляет предмет)
    /// </summary>
    public void Clear()
    {
        item = null;
        amount = 0;
    }
    
    /// <summary>
    /// Проверяет, пуст ли слот
    /// </summary>
    public bool IsEmpty()
    {
        return item == null || amount <= 0;
    }
    
    /// <summary>
    /// Возвращает копию этого слота
    /// </summary>
    public InventorySlot Clone()
    {
        return new InventorySlot(item, amount);
    }
}