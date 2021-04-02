using UnityEngine;
using Zenject;

public class Paddle : MonoBehaviour, ITickable, IBallCollidable
{
    [SerializeField]
    private SpriteRenderer render;

    private CameraController2D cameraController;
    private MapBoundary mapBounds;
    private SignalBus signalBus;

    [Inject]
    public void Bind(CameraController2D cameraController, 
        MapBoundary mapBounds,
        SignalBus signalBus)
    {
        this.cameraController = cameraController;
        this.mapBounds = mapBounds;
        this.signalBus = signalBus;
    }

    public void Tick()
    {
        //Tmp code for now
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            signalBus.Fire<PlayerInputSignal>();
        }

        UpdatePaddlePosition();
    }

    private void UpdatePaddlePosition()
    {
        Vector3 mouseWorld = cameraController.Camera.ScreenToWorldPoint(Input.mousePosition);
        float clampedX = Mathf.Clamp(mouseWorld.x, mapBounds.CameraWorldBounds.min.x + render.bounds.extents.x, mapBounds.CameraWorldBounds.max.x - render.bounds.extents.x);
        transform.position = new Vector3(clampedX, transform.position.y);
    }

    public void OnBallCollided(Ball ball)
    {
        Vector2 dir = (ball.transform.position - transform.position).normalized;

        if (dir.y > 0.0f)
        {
            ball.SetDirection(dir);
        }
    }
}