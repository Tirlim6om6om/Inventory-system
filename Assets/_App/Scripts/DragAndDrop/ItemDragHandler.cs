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
    public event Action OnDrag;
    public event Action OnDrop;
    
    [SerializeField] private float height;
    [SerializeField] private float lerpSpeed;
    
    private Rigidbody _rb;
    private bool _isDragging;
    private Quaternion _startRot;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void SetDrag(bool active)
    {
        _isDragging = active;
        _rb.useGravity = !active;
        
        if (active)
        {
            OnDrag?.Invoke();
        }
        else
        {
            OnDrop?.Invoke();
        }
    }

    public void UpdatePos(Vector3 target)
    {
        _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, 0.1f);
        _rb.angularVelocity = Vector3.Lerp(_rb.angularVelocity, Vector3.zero, 0.1f);
        
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.x, height, target.z), lerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, _startRot, 0.5f);
    }
}
