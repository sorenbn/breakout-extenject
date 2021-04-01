using UnityEngine;
using Zenject;

public class BreakoutInstaller : MonoInstaller
{
    [SerializeField]
    private Brick brickPrefab;

    public override void InstallBindings()
    {
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
}