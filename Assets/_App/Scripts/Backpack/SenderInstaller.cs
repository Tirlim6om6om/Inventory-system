using UnityEngine;
using Zenject;

public class SenderInstaller : MonoInstaller
{
    [SerializeField] private ServerSender serverSender;

    public override void InstallBindings()
    {
        Container.Bind<ServerSender>().FromInstance(serverSender).AsSingle().NonLazy();
    }
}
