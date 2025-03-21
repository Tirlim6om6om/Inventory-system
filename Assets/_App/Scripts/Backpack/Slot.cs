using UnityEngine;
using Zenject;

public class Slot : MonoBehaviour
{
    [SerializeField] private BackpackZone backpackZone;

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
        if (_itemObject != null || itemObject.ItemLocker.IsLock)
            return;

        _itemObject = itemObject;
        _itemObject.ItemLocker.SetLock(true);
        _itemObject.transform.position = transform.position;
        _itemObject.transform.rotation = transform.rotation;
        _itemObject.ItemEventSystem.OnPointerUp += OnPointerUp;
        
        _inventory.AddItem(itemObject.Item);
    }

    private void OnPointerUp()
    {
        if(!_backpack.WasMouseDown | _inventory == null)
            return;
        _itemObject.ItemLocker.SetLock(false);
        _itemObject.transform.position = transform.position + transform.up * 0.25f;
        _itemObject.ItemEventSystem.OnPointerUp -= OnPointerUp;
        
        _inventory.RemoveItem(_itemObject.Item); 
        _itemObject = null;
    }
}
