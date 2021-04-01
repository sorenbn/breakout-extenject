using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SettingsInstaller", menuName = "Installers/SettingsInstaller")]
public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
{
    [SerializeField]
    private MapBoundary.Settings mapBoundarySettings;

    [SerializeField]
    private BrickManager.Settings brickManagerSettings;

    public override void InstallBindings()
    {
        Container.BindInstances(mapBoundarySettings, 
            brickManagerSettings);
    }
}