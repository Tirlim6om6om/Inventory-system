using UnityEngine;

public class Slot : MonoBehaviour
{
    public int Index { get { return _index; } }
    
    [SerializeField] private BackpackZone backpackZone;

    private int _index;
    private ItemObject _itemObject;

    private void OnEnable()
    {
        backpackZone.OnAddItem += OnAddItem;
    }

    private void OnDisable()
    {
        backpackZone.OnAddItem -= OnAddItem; 
    }

    private void OnAddItem(ItemObject itemObject)
    {
        if(_itemObject != null)
            return;
        
        itemObject.ItemLocker.SetLock(true);
        itemObject.transform.position = transform.position;
        _itemObject = itemObject;
        
    }
}
