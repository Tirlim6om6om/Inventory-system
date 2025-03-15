using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Определяет зону, куда можно выбрасывать предметы из инвентаря
/// </summary>
public class BackpackDropZone : MonoBehaviour, IDropHandler
{
    [Header("Drop Settings")] [Tooltip("Точка, где будут появляться выброшенные предметы")] [SerializeField]
    private Transform dropPoint;

    [Tooltip("Радиус разброса предметов при выбросе")] [SerializeField]
    private float dropRadius = 0.5f;

    [Tooltip("Высота, на которой появляются предметы")] [SerializeField]
    private float dropHeight = 0.3f;

    [Header("Visual Settings")] [Tooltip("Подсвечивать зону при перетаскивании предмета")] [SerializeField]
    private bool highlightOnDrag = true;

    [Tooltip("Визуальный объект подсветки зоны")] [SerializeField]
    private GameObject highlightObject;

    [Tooltip("Цвет подсветки при наведении")] [SerializeField]
    private Color highlightColor = new Color(0.2f, 0.8f, 0.2f, 0.3f);

    private Renderer zoneRenderer;
    private Color originalColor;
    private bool isHighlighted = false;

    #region Unity Lifecycle

    private void Awake()
    {
        // Проверяем наличие точки для выброса предметов
        if (dropPoint == null)
        {
            // Если точка не задана, используем позицию этого объекта
            dropPoint = transform;
        }

        // Инициализируем визуальные компоненты
        if (highlightObject != null)
        {
            zoneRenderer = highlightObject.GetComponent<Renderer>();
            if (zoneRenderer != null)
            {
                originalColor = zoneRenderer.material.color;
                highlightObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        // Подписываемся на глобальные события перетаскивания
        InventoryView.OnItemDragStarted += HandleDragStarted;
        InventoryView.OnItemDragEnded += HandleDragEnded;
    }

    private void OnDisable()
    {
        // Отписываемся от глобальных событий
        InventoryView.OnItemDragStarted -= HandleDragStarted;
        InventoryView.OnItemDragEnded -= HandleDragEnded;
    }

    private void OnDrawGizmos()
    {
        // Отображаем в редакторе зону выброса предметов
        Vector3 center = dropPoint != null ? dropPoint.position : transform.position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, dropRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(center, center + Vector3.up * dropHeight);
        Gizmos.DrawWireCube(center + Vector3.up * dropHeight, new Vector3(0.1f, 0.1f, 0.1f));
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Возвращает позицию для выброса предмета
    /// </summary>
    public Vector3 GetDropPosition()
    {
        Vector3 position = dropPoint.position;

        // Добавляем случайное смещение в пределах радиуса
        Vector2 randomOffset = Random.insideUnitCircle * dropRadius;
        position += new Vector3(randomOffset.x, dropHeight, randomOffset.y);

        return position;
    }

    /// <summary>
    /// Активирует подсветку зоны выброса
    /// </summary>
    public void ActivateHighlight()
    {
        if (!highlightOnDrag || zoneRenderer == null)
            return;

        isHighlighted = true;
        highlightObject.SetActive(true);
        zoneRenderer.material.color = highlightColor;
    }

    /// <summary>
    /// Деактивирует подсветку зоны выброса
    /// </summary>
    public void DeactivateHighlight()
    {
        if (!highlightOnDrag || zoneRenderer == null)
            return;

        isHighlighted = false;
        highlightObject.SetActive(false);
        zoneRenderer.material.color = originalColor;
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Обрабатывает событие сброса предмета на эту зону
    /// </summary>
    public void OnDrop(PointerEventData eventData)
    {
        // Получаем компонент предмета из исходного элемента UI
        InventorySlotUI sourceSlot = eventData.pointerDrag?.GetComponent<InventorySlotUI>();
        if (sourceSlot == null)
            return;

        // Получаем ссылку на инвентарь
        InventoryView inventoryUI = sourceSlot.GetComponentInParent<InventoryView>();
        if (inventoryUI == null)
            return;

        // Выбрасываем предмет из инвентаря
        int slotIndex = sourceSlot.GetSlotIndex();
        inventoryUI.DropItemToWorld(slotIndex, GetDropPosition());

        // Проигрываем звуковой эффект выброса
        PlayDropSound();
    }

    private void HandleDragStarted()
    {
        if (highlightOnDrag)
            ActivateHighlight();
    }

    private void HandleDragEnded()
    {
        if (highlightOnDrag)
            DeactivateHighlight();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Проигрывает звук выброса предмета
    /// </summary>
    private void PlayDropSound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f); // Небольшая вариация звука
            audioSource.Play();
        }
    }

    #endregion
}
