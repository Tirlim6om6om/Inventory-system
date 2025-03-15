using System;
using UnityEngine;

public class ItemEventSystem : MonoBehaviour
{
    public event Action OnDrag;
    public event Action OnDrop;
    public event Action OnPointerUp;
    
    public void DragInvoke()
    {
        OnDrag?.Invoke();
    }
    
    public void DropInvoke()
    {
        OnDrop?.Invoke();
    }

    public void PointerUpInvoke()
    {
        OnPointerUp?.Invoke();
    }
}
