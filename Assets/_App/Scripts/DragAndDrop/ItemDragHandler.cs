using UnityEngine;
using Zenject;

public interface IDraggable
{
    bool IsDragging { get; }
    bool SetDrag(bool active);
    void UpdatePos(Vector3 target);
}

[RequireComponent(typeof(Rigidbody))]
public class ItemDragHandler : MonoBehaviour, IDraggable
{
    public bool IsDragging { get { return _isDragging; } }

    [SerializeField] private float height;
    [SerializeField] private float lerpSpeed;
    
    private Rigidbody _rb;
    private ItemEventSystem _itemEventSystem;
    private ItemLocker _locker;
    private bool _isDragging;
    private Quaternion _startRot;

    [Inject]
    public void Construct(Rigidbody rb, ItemEventSystem itemEventSystem, ItemLocker locker)
    {
        _rb = rb;
        _itemEventSystem = itemEventSystem;
        _locker = locker;
    }

    public bool SetDrag(bool active)
    {
        if(_locker.IsLock)
            return false;
        
        _isDragging = active;
        _rb.useGravity = !active;
        
        if (active)
        {
           _itemEventSystem.DragInvoke();
        }
        else
        {
            _itemEventSystem.DropInvoke();
        }

        return true;
    }

    public void UpdatePos(Vector3 target)
    {
        _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, 0.1f);
        _rb.angularVelocity = Vector3.Lerp(_rb.angularVelocity, Vector3.zero, 0.1f);
        
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.x, height, target.z), lerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, _startRot, 0.5f);
    }
}
