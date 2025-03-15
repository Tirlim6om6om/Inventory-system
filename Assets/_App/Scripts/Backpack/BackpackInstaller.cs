using UnityEngine;
using Zenject;

public class BackpackInstaller : MonoInstaller
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private BackpackController backpack;
    
    public override void InstallBindings()
    {
        Container.Bind<Inventory>().FromInstance(inventory).AsSingle();
        Container.Bind<BackpackController>().FromInstance(backpack).AsSingle();
    }
}
