using UnityEngine;
using Zenject;

public class InjectionDefinition : MonoInstaller<InjectionDefinition>
{
    public override void InstallBindings()
    {
        Container.Bind<IPlayeMovementData>().To<PlayerMovementData>().AsSingle().NonLazy();
    }
}