using UnityEngine;
using Zenject;

public class BreakoutInstaller : MonoInstaller
{
    [SerializeField]
    private MapBoundary mapBounds;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MapBoundary>().AsSingle();
    }
}