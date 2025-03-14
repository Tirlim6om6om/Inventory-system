using System;
using UnityEngine;

public interface IDraggable
{
    void SetDrag(bool active);
    void UpdatePos(Vector3 target);
}

[RequireComponent(typeof(Rigidbody))]
public class ItemDragHandler : MonoBehaviour, IDraggable
{
    public bool IsDragging { get { return _isDragging; } }
    
    [SerializeField] private float height;
    [SerializeField] private float lerpSpeed;
    
    private Rigidbody _rb;
    private bool _isDragging;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void SetDrag(bool active)
    {
        _isDragging = active;
        _rb.isKinematic = active;
    }

    public void UpdatePos(Vector3 target)
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.x, height, target.z), lerpSpeed);
    }
}
