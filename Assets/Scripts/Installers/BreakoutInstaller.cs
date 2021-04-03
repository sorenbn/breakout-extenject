using UnityEngine;
using Zenject;

public class BreakoutInstaller : MonoInstaller
{
    [SerializeField]
    private Brick brickPrefab;

    public override void InstallBindings()
    {
        InstallSignals();

        Container.Bind<RoutineRunner>().FromNewComponentOnNewGameObject().AsSingle();

        Container.BindInterfacesAndSelfTo<CursorManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<MapBoundary>().AsSingle();

        Container.BindInterfacesAndSelfTo<BrickManager>().AsSingle();
        Container.Bind<IBrickSpawner>().To<BrickSpawnerDev>().AsSingle();

        Container.BindMemoryPool<Brick, Brick.Pool>()
            .WithInitialSize(60)
            .ExpandByDoubling()
            .FromComponentInNewPrefab(brickPrefab)
#if UNITY_EDITOR
            .UnderTransformGroup("Bricks")
#endif
            ;
    }

    private void InstallSignals()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<PlayerInputSignal>();
        Container.DeclareSignal<BallLostSignal>();
        Container.DeclareSignal<BrickDestroyedSignal>();
    }
}