using UnityEngine;
using UnityEngine.EventSystems;

public class Backpack : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Inventory inventory;
    public GameObject inventoryUI;
    
    private bool isMouseDown = false;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isMouseDown = true;
            inventoryUI.SetActive(true);
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isMouseDown = false;
            inventoryUI.SetActive(false);
        }
    }
}
