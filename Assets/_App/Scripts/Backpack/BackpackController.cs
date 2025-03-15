using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackpackController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool WasMouseDown { get { return _wasMouseDown; } }
    
    [SerializeField] private GameObject inventoryUI;
    
    private bool _isMouseDown = false;
    private bool _wasMouseDown = false;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _isMouseDown = true;
            _wasMouseDown = true;
            inventoryUI.SetActive(true);
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _isMouseDown = false;
            inventoryUI.SetActive(false);
            StartCoroutine(WaitMouseDown());
        }
    }

    private IEnumerator WaitMouseDown()
    {
        yield return new WaitForSeconds(0.1f);
        if(!_isMouseDown)
            _wasMouseDown = false;
    }
}
