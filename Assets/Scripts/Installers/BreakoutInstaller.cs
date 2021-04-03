using UnityEngine;
using Zenject;

public class BreakoutInstaller : MonoInstaller
{
    [SerializeField]
    private Brick brickPrefab;

    [SerializeField]
    private Ball ballPrefab;

    public override void InstallBindings()
    {
        InstallSignals();

        Container.Bind<RoutineRunner>().FromNewComponentOnNewGameObject().AsSingle();

        Container.BindInterfacesAndSelfTo<CursorManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<MapBoundary>().AsSingle();

        Container.BindInterfacesAndSelfTo<BrickManager>().AsSingle();
        Container.Bind<IBrickSpawner>().To<BrickSpawnerDev>().AsSingle();

        Container.BindInterfacesAndSelfTo<BallManager>().AsSingle();

        Container.BindMemoryPool<Brick, Brick.Pool>()
            .WithInitialSize(60)
            .ExpandByDoubling()
            .FromComponentInNewPrefab(brickPrefab)
#if UNITY_EDITOR
            .UnderTransformGroup("Bricks")
#endif
            ;

        Container.BindMemoryPool<Ball, Ball.Pool>()
            .WithInitialSize(10)
            .ExpandByDoubling()
            .FromComponentInNewPrefab(ballPrefab)
#if UNITY_EDITOR
            .UnderTransformGroup("Balls")
#endif
            ;
    }

    private void InstallSignals()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<PlayerInputSignal>().OptionalSubscriber();
        Container.DeclareSignal<BallLostSignal>();
        Container.DeclareSignal<BrickDestroyedSignal>();
    }
}