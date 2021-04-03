using UnityEngine;
using Zenject;

public class BreakoutInstaller : MonoInstaller
{
    [SerializeField]
    private Brick brickPrefab;

    public override void InstallBindings()
    {
        InstallSignals();

        Container.BindInterfacesAndSelfTo<CursorManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<MapBoundary>().AsSingle();

        Container.BindInterfacesAndSelfTo<BrickManager>().AsSingle();
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
    }
}