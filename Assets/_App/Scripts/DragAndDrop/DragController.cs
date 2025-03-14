using UnityEngine;

public class DragController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask groundLayer;
    
    private IDraggable _currentDraggable;
    private bool _isDragging;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isDragging)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                StartDragging(hit.collider.gameObject);
            }
        }
        else if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            StopDragging();
        }

        if (_isDragging)
        {
            UpdateDragPosition();
        }
    }

    private void StartDragging(GameObject hitObject)
    {
        if(!hitObject.TryGetComponent(out _currentDraggable))
            return;
        _isDragging = true;
        _currentDraggable.SetDrag(true);
    }
    
    private void StopDragging()
    {
        _isDragging = false;
        _currentDraggable.SetDrag(false);
        _currentDraggable = null;
    }
    
    private void UpdateDragPosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            _currentDraggable.UpdatePos(hit.point);
        }
    }
}
