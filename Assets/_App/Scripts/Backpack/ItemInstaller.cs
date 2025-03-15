using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ItemInstaller : MonoInstaller
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ItemEventSystem itemEventSystem;
    [SerializeField] private ItemDragHandler itemDragHandler;
    [SerializeField] private ItemLocker locker;
    
    public override void InstallBindings()
    {
        Container.Bind<Rigidbody>().FromInstance(rb).AsSingle();
        Container.Bind<ItemEventSystem>().FromInstance(itemEventSystem).AsSingle();
        Container.Bind<ItemLocker>().FromInstance(locker).AsSingle();
        Container.Bind<IDraggable>().FromInstance(itemDragHandler).AsSingle();
    }
}
