using UnityEngine;
using Zenject;

public class Slot : MonoBehaviour
{
    public int Index { get { return _index; } }
    
    [SerializeField] private BackpackZone backpackZone;

    private int _index;
    private ItemObject _itemObject;
    private Inventory _inventory;
    private BackpackController _backpack;

    [Inject]
    public void Construct(Inventory inventory, BackpackController backpack)
    {
        _inventory = inventory;
        _backpack = backpack;
    }

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
        
        _itemObject = itemObject;
        _itemObject.ItemLocker.SetLock(true);
        _itemObject.transform.position = transform.position;
        _itemObject.transform.rotation = transform.rotation;
        _itemObject.ItemEventSystem.OnPointerUp += OnPointerUp;
        
        _inventory.AddItem(itemObject.Item);
        Debug.Log("Add " + _itemObject.gameObject.name);
    }

    private void OnPointerUp()
    {
        if(!_backpack.WasMouseDown)
            return;
        
        _inventory.RemoveItem(_itemObject.Item); 
        _itemObject.ItemLocker.SetLock(false);
        _itemObject.transform.position = transform.position + transform.up * 0.25f;
        Debug.Log("Remove " + _itemObject.gameObject.name);
        _itemObject = null;
    }
}
