using UnityEngine;
using Zenject;

public class ItemLocker : MonoBehaviour
{
    public bool IsLock { get { return _isLock; } }

    [Inject] private Rigidbody _rb;
    private bool _isLock;

    public void SetLock(bool active)
    {
        _isLock = active;
        _rb.isKinematic = true;
    }
}
