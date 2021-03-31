using UnityEngine;
using Zenject;

public class Paddle : MonoBehaviour, ITickable, IBallCollidable
{
    [SerializeField]
    private SpriteRenderer render;

    private CameraController2D cameraController;
    private MapBoundary mapBounds;

    [Inject]
    public void Bind(CameraController2D cameraController, MapBoundary mapBounds)
    {
        this.cameraController = cameraController;
        this.mapBounds = mapBounds;
    }

    public void Tick()
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