using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    public static event Action OnItemDragStarted;
    public static event Action OnItemDragEnded;

    public Inventory inventory;
    public GameObject slotPrefab;
    public Transform slotParent;

    [SerializeField] private ItemTooltip itemTooltip;

    private List<GameObject> slotObjects = new List<GameObject>();

    private void OnEnable()
    {
        inventory.OnItemAdded += RefreshUI;
        inventory.OnItemRemoved += RefreshUI;
        RefreshUI();
    }

    public void RefreshUI(Item item = null)
    {
        foreach (GameObject slot in slotObjects)
        {
            Destroy(slot);
        }

        slotObjects.Clear();

        for (int i = 0; i < inventory.slots.Count; i++)
        {
            GameObject slotObject = Instantiate(slotPrefab, slotParent);
            InventorySlotUI slotUI = slotObject.GetComponent<InventorySlotUI>();

            slotUI.SetSlot(inventory.slots[i], i);

            slotObjects.Add(slotObject);
        }
    }

    public bool SwapItems(int sourceIndex, int targetIndex)
    {
        bool success = inventory.SwapItems(sourceIndex, targetIndex);
        if (success)
        {
            RefreshUI();
        }

        return success;
    }

    /// <summary>
    /// Выбрасывает предмет из инвентаря в мир
    /// </summary>
    public bool DropItemToWorld(int slotIndex, Vector3 dropPosition)
    {
        Item item = inventory.slots[slotIndex].item;
        if (item == null)
            return false;

        // Создаем физический предмет в мире
        if (item.prefab != null)
        {
            Instantiate(item.prefab, dropPosition, Quaternion.identity);
        }

        // Удаляем предмет из инвентаря
        bool success = inventory.RemoveItem(slotIndex);
        if (success)
        {
            RefreshUI();
        }

        return success;
    }

    /// <summary>
    /// Показывает всплывающую подсказку с информацией о предмете
    /// </summary>
    public void ShowItemTooltip(Item item, Vector3 position)
    {
        if (itemTooltip != null)
            itemTooltip.ShowTooltip(item, position);
    }

    /// <summary>
    /// Скрывает всплывающую подсказку
    /// </summary>
    public void HideItemTooltip()
    {
        if (itemTooltip != null)
            itemTooltip.HideTooltip();
    }

    // Метод для вызова события начала перетаскивания
    public void OnItemDragBegin(int slotIndex)
    {
        // Вызываем статическое событие
        OnItemDragStarted?.Invoke();

        // Можно добавить звуковой эффект или другую обратную связь
        Debug.Log($"Started dragging item from slot {slotIndex}");
    }

    // Метод для вызова события завершения перетаскивания
    public void OnItemDragEnd()
    {
        // Вызываем статическое событие
        OnItemDragEnded?.Invoke();

        // Можно добавить звуковой эффект или другую обратную связь
        Debug.Log("Finished dragging item");
    }
}


