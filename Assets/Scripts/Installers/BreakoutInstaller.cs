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
        Container.BindFactory<Brick, Brick.Factory>()
            .FromComponentInNewPrefab(brickPrefab)
            .UnderTransformGroup("Bricks");
    }
}