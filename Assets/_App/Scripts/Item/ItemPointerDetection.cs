using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ItemPointerDetection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemEventSystem _itemEventSystem;
    private Coroutine _clickCheckCoroutine = null;
    
    [Inject]
    private void Construct(ItemEventSystem itemEventSystem)
    {
        _itemEventSystem = itemEventSystem;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _clickCheckCoroutine = StartCoroutine(CheckClick());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_clickCheckCoroutine != null)
        {
            StopCoroutine(_clickCheckCoroutine);
            _clickCheckCoroutine = null;
        }
    }

    private IEnumerator CheckClick()
    {
        bool wasMouseDown = Input.GetMouseButton(0);
    
        while (true)
        {
            bool isMouseDown = Input.GetMouseButton(0);
            if (!isMouseDown && wasMouseDown)
            {
                _itemEventSystem.PointerUpInvoke();
                break;
            }
        
            wasMouseDown = isMouseDown;
            yield return null;
        }
    }
}
