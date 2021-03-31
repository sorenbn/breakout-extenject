using UnityEngine;
using Zenject;

/*
 * Powerups:
    - Full boundary
    - Ball splitter
    - Sticky to paddle
*/

public class Ball : MonoBehaviour, IInitializable, IFixedTickable
{
    [SerializeField]
    private float speed = 2.0f;

    [SerializeField]
    private SpriteRenderer render;

    private Rigidbody2D body;
    private MapBoundary mapBounds;

    [Inject]
    public void Inject(MapBoundary mapBounds)
    {
        this.mapBounds = mapBounds;
    }

    public void Initialize()
    {
        body = GetComponent<Rigidbody2D>();
        SetDirection(Random.insideUnitCircle);
    }

    public void FixedTick()
    {
        if (OutsideHorizontalBounds())
        {
            ReflectX();
        }

        if (OutsideVerticalBounds())
        {
            ReflectY();
        }
    }

    public void SetDirection(Vector2 direction)
    {
        body.velocity = direction.normalized * speed;
    }

    private void ReflectX()
    {
        body.velocity = new Vector2(body.velocity.x * -1.0f, body.velocity.y);
    }

    private void ReflectY()
    {
        body.velocity = new Vector2(body.velocity.x, body.velocity.y * -1.0f);
    }

    private bool OutsideHorizontalBounds()
    {
        return (body.position.x > mapBounds.CameraWorldBounds.max.x - render.bounds.extents.x && body.velocity.x > 0)
                    || (body.position.x < mapBounds.CameraWorldBounds.min.x + render.bounds.extents.x && body.velocity.x < 0);
    }

    private bool OutsideVerticalBounds()
    {
        return (body.position.y > mapBounds.CameraWorldBounds.max.y - render.bounds.extents.y && body.velocity.y > 0)
                    || (body.position.y < mapBounds.CameraWorldBounds.min.y + render.bounds.extents.y && body.velocity.y < 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out IBallCollidable collidable))
        {
            collidable.OnBallCollided(this);
        }
    }
}
