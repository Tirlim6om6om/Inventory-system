using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI countText;
    
    private Item currentItem;
    
    public void SetupSlot(Item item, int count)
    {
        currentItem = item;
        
        // Настраиваем иконку
        if (iconImage != null && item.icon != null)
        {
            iconImage.sprite = item.icon;
            iconImage.enabled = true;
        }
        
        // Настраиваем название
        if (nameText != null)
        {
            nameText.text = item.itemName;
        }
        
        // Настраиваем счетчик
        if (countText != null)
        {
            countText.text = count > 1 ? count.ToString() : "";
            countText.gameObject.SetActive(count > 1);
        }
    }
}