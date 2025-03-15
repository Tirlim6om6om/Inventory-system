using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Класс для отображения всплывающей подсказки с информацией о предмете
/// </summary>
public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemWeightText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private Image itemImage;
    
    private RectTransform rectTransform;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Показывает всплывающую подсказку для указанного предмета
    /// </summary>
    public void ShowTooltip(Item item, Vector3 position)
    {
        if (item == null)
            return;
            
        // Заполняем информацию о предмете
        itemNameText.text = item.itemName;
        itemTypeText.text = "Тип: " + item.itemType.ToString();
        itemWeightText.text = "Вес: " + item.weight.ToString("0.0");
        itemDescriptionText.text = item.description ?? "Нет описания";
        
        if (itemImage != null && item.icon != null)
            itemImage.sprite = item.icon;
            
        // Устанавливаем позицию подсказки
        Vector3 newPos = position;
        newPos.y -= rectTransform.rect.height * 0.5f;
        rectTransform.position = newPos;
        
        // Убеждаемся, что подсказка не выходит за границы экрана
        AdjustTooltipPosition();
        
        // Показываем подсказку
        gameObject.SetActive(true);
    }
    
    /// <summary>
    /// Скрывает всплывающую подсказку
    /// </summary>
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Корректирует позицию подсказки, чтобы она оставалась в пределах экрана
    /// </summary>
    private void AdjustTooltipPosition()
    {
        Vector3 position = rectTransform.position;
        Vector2 size = rectTransform.sizeDelta;
        Vector2 halfSize = size * 0.5f;
        
        // Определяем границы экрана
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        
        // Корректируем позицию
        if (position.x - halfSize.x < 0)
            position.x = halfSize.x;
        else if (position.x + halfSize.x > screenSize.x)
            position.x = screenSize.x - halfSize.x;
            
        if (position.y - halfSize.y < 0)
            position.y = halfSize.y;
        else if (position.y + halfSize.y > screenSize.y)
            position.y = screenSize.y - halfSize.y;
            
        rectTransform.position = position;
    }
}
