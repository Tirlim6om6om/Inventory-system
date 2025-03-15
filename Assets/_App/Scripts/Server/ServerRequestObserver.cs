using UnityEngine;
using Zenject;

public class ServerRequestObserver : MonoBehaviour
{
    private ServerSender _sender;
    private Inventory _inventory;
    
    [Inject]
    public void Construct(ServerSender sender, Inventory inventory)
    {
        _sender = sender;
        _inventory = inventory;
        _inventory.OnAddItem += AddItemInvoke;
        _inventory.OnRemoveItem += RemoveItemInvoke;
    }

    private void OnDestroy()
    {
        _inventory.OnAddItem -= AddItemInvoke;
        _inventory.OnRemoveItem -= RemoveItemInvoke;
    }

    public void AddItemInvoke(Item item)
    {
        _sender.SendItemToServer(item, true);
    }

    public void RemoveItemInvoke(Item item)
    {
        _sender.SendItemToServer(item, false);
    }
}
