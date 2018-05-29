using Camera;
using Implementation.Data;
using UnityEngine;
using Zenject;

public class InjectionDefinition : MonoInstaller<InjectionDefinition>
{
    public override void InstallBindings()
    {
        Container.Bind<IMovementData>().To<MovementData>().AsTransient().NonLazy();
        Container.Bind<ICameraData>().To<CameraData>().AsSingle().NonLazy();
        Container.Bind<IMechanicsData>().To<MechanicsData>().AsTransient().NonLazy();
        Container.Bind<IEnemyData>().To<EnemyData>().AsSingle().NonLazy();
    }
}