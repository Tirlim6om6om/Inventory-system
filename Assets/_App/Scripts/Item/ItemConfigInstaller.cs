using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ItemConfigInstaller", menuName = "Installers/ItemConfigInstaller")]
public class ItemConfigInstaller : ScriptableObjectInstaller
{
    [SerializeField] private Item item;
    
    public override void InstallBindings()
    {
        Container.Bind<Item>().FromInstance(item).AsSingle();
    }
}