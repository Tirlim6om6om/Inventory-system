using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Класс представляющий слот инвентаря в пользовательском интерфейсе.
/// Обеспечивает функциональность для отображения предметов и взаимодействия с ними.
/// </summary>
public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")] [SerializeField]
    private Image itemIcon;

    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Image slotBackground;
    [SerializeField] private Image highlightImage;

    [Header("Settings")] [SerializeField] private Color emptySlotColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);
    [SerializeField] private Color filledSlotColor = new Color(0.5f, 0.5f, 0.5f, 0.8f);
    [SerializeField] private Color highlightColor = new Color(1f, 0.9f, 0.5f, 1f);

    private InventoryView inventoryUI;
    private Canvas parentCanvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private Transform originalParent;
    private GameObject placeholderObject;
    private InventorySlot inventorySlot;
    private int slotIndex = -1;
    private int originalSiblingIndex;

    #region Unity Lifecycle

    private void Awake()
    {
        // Получаем необходимые компоненты
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        inventoryUI = GetComponentInParent<InventoryView>();
        parentCanvas = GetComponentInParent<Canvas>();

        // Инициализируем состояние слота
        if (highlightImage != null)
            highlightImage.gameObject.SetActive(false);

        SetEmpty();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Устанавливает данные слота и обновляет отображение
    /// </summary>
    /// <param name="slot">Данные слота инвентаря</param>
    /// <param name="index">Индекс слота в инвентаре</param>
    public void SetSlot(InventorySlot slot, int index)
    {
        inventorySlot = slot;
        slotIndex = index;

        UpdateVisual();
    }

    /// <summary>
    /// Обновляет визуальное отображение слота
    /// </summary>
    public void UpdateVisual()
    {
        if (inventorySlot != null && inventorySlot.item != null)
        {
            // Слот содержит предмет
            itemIcon.sprite = inventorySlot.item.icon;
            itemIcon.enabled = true;

            // Настраиваем отображение количества
            if (inventorySlot.amount > 1)
            {
                amountText.text = inventorySlot.amount.ToString();
                amountText.gameObject.SetActive(true);
            }
            else
            {
                amountText.gameObject.SetActive(false);
            }

            // Настраиваем цвет фона
            if (slotBackground != null)
                slotBackground.color = filledSlotColor;
        }
        else
        {
            // Слот пуст
            SetEmpty();
        }
    }

    /// <summary>
    /// Получает индекс слота
    /// </summary>
    public int GetSlotIndex()
    {
        return slotIndex;
    }

    /// <summary>
    /// Получает предмет в слоте
    /// </summary>
    public Item GetItem()
    {
        return inventorySlot?.item;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Устанавливает слот в пустое состояние
    /// </summary>
    private void SetEmpty()
    {
        if (itemIcon != null)
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false;
        }

        if (amountText != null)
            amountText.gameObject.SetActive(false);

        if (slotBackground != null)
            slotBackground.color = emptySlotColor;
    }

    #endregion

    #region Drag & Drop Handlers

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Проверяем, содержит ли слот предмет
        if (inventorySlot == null || inventorySlot.item == null)
            return;

        // Сохраняем оригинальную позицию, родителя и индекс
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        // Создаем плейсхолдер на месте перетаскиваемого слота
        placeholderObject = new GameObject("SlotPlaceholder");
        placeholderObject.transform.SetParent(originalParent);
        placeholderObject.transform.SetSiblingIndex(originalSiblingIndex);
    
        // Добавляем RectTransform к плейсхолдеру
        RectTransform placeholderRect = placeholderObject.AddComponent<RectTransform>();
        placeholderRect.sizeDelta = rectTransform.sizeDelta;
        placeholderRect.anchoredPosition = originalPosition;

        // Делаем элемент полупрозрачным и отключаем взаимодействие с ним
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        // Перемещаем иконку предмета на верхний уровень Canvas для перетаскивания
        transform.SetParent(parentCanvas.transform);
    
        if (inventoryUI != null)
            inventoryUI.OnItemDragBegin(slotIndex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Если слот пуст или нет предмета, прерываем выполнение
        if (inventorySlot == null || inventorySlot.item == null)
            return;

        // Перемещаем элемент вместе с курсором
        rectTransform.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Если слот пуст или нет предмета, прерываем выполнение
        if (inventorySlot == null || inventorySlot.item == null)
            return;

        // Восстанавливаем прозрачность и возможность взаимодействия
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Получаем объект, над которым завершилось перетаскивание
        GameObject targetObject = eventData.pointerCurrentRaycast.gameObject;

        bool successfulDrop = false;

        if (targetObject != null)
        {
            // Если перетащили на другой слот инвентаря
            InventorySlotUI targetSlot = targetObject.GetComponent<InventorySlotUI>();
            if (targetSlot != null && targetSlot != this)
            {
                if (inventoryUI != null)
                {
                    // Обмениваем предметы между слотами
                    successfulDrop = inventoryUI.SwapItems(slotIndex, targetSlot.GetSlotIndex());
                }
            }

            // Если перетащили на зону выброса предметов
            BackpackDropZone dropZone = targetObject.GetComponent<BackpackDropZone>();
            if (dropZone != null)
            {
                if (inventoryUI != null)
                {
                    // Пытаемся выбросить предмет из инвентаря
                    successfulDrop = inventoryUI.DropItemToWorld(slotIndex, dropZone.GetDropPosition());
                }
            }
        }

        // Если перетаскивание не завершилось успешно, возвращаем элемент на исходную позицию
        if (!successfulDrop)
        {
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
            transform.SetSiblingIndex(originalSiblingIndex);
        }
        
        if (placeholderObject != null)
        {
            Destroy(placeholderObject);
            placeholderObject = null;
        }
        
        if (inventoryUI != null)
            inventoryUI.OnItemDragEnd();
    }

    #endregion

    #region Tooltip & Highlight Handlers

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Если слот содержит предмет
        if (inventorySlot != null && inventorySlot.item != null)
        {
            // Включаем подсветку слота
            if (highlightImage != null)
            {
                highlightImage.gameObject.SetActive(true);
                highlightImage.color = highlightColor;
            }

            // Отображаем всплывающую подсказку с информацией о предмете
            if (inventoryUI != null)
                inventoryUI.ShowItemTooltip(inventorySlot.item, transform.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Отключаем подсветку
        if (highlightImage != null)
            highlightImage.gameObject.SetActive(false);

        // Скрываем всплывающую подсказку
        if (inventoryUI != null)
            inventoryUI.HideItemTooltip();
    }

    #endregion
}



