using UnityEngine;
using Zenject;

public class MapBoundary : IInitializable, ITickable
{
    private Settings settings;
    private CameraController2D cameraController;

    public Bounds CameraWorldBounds
    {
        get; private set;
    }

    [Inject]
    public void Bind(Settings settings, CameraController2D cameraController)
    {
        this.settings = settings;
        this.cameraController = cameraController;
    }

    public void Initialize()
    {
        CalculateCameraWorldBounds();
    }

    private void CalculateCameraWorldBounds()
    {
        CameraWorldBounds = cameraController.Camera.OrthographicBounds();
    }

    public void Tick()
    {
        if (settings.DrawBounds)
        {
            DrawBounds(CameraWorldBounds);
        }
    }

    private void DrawBounds(Bounds b)
    {
        // bottom
        var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
        var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
        var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
        var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

        Debug.DrawLine(p1, p2, Color.blue);
        Debug.DrawLine(p2, p3, Color.red);
        Debug.DrawLine(p3, p4, Color.yellow);
        Debug.DrawLine(p4, p1, Color.magenta);

        // top
        var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
        var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
        var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
        var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

        Debug.DrawLine(p5, p6, Color.blue);
        Debug.DrawLine(p6, p7, Color.red);
        Debug.DrawLine(p7, p8, Color.yellow);
        Debug.DrawLine(p8, p5, Color.magenta);

        // sides
        Debug.DrawLine(p1, p5, Color.white);
        Debug.DrawLine(p2, p6, Color.gray);
        Debug.DrawLine(p3, p7, Color.green);
        Debug.DrawLine(p4, p8, Color.cyan);
    }

    [System.Serializable]
    public class Settings
    {
        public bool DrawBounds;
    }
}