using System;
using UnityEngine;

public class ItemEventSystem : MonoBehaviour
{
    public event Action OnDrag;
    public event Action OnDrop;
    
    public void DragInvoke()
    {
        OnDrag?.Invoke();
    }
    
    public void DropInvoke()
    {
        OnDrop?.Invoke();
    }
}
