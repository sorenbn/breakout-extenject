using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

/*
 * TODO: 
 * Make into a MonoMemoryPool, to be able to spawn N amount of balls through powerup
 */

public class Ball : MonoBehaviour, IInitializable, IFixedTickable, IDisposable
{
    [SerializeField]
    private float speed = 2.0f;

    [SerializeField]
    private SpriteRenderer render;

    private Rigidbody2D body;
    private MapBoundary mapBounds;
    private SignalBus signalBus;

    [Inject]
    public void Inject(MapBoundary mapBounds, SignalBus signalBus)
    {
        this.mapBounds = mapBounds;
        this.signalBus = signalBus;
    }

    public void Initialize()
    {
        body = GetComponent<Rigidbody2D>();

        SetSimulated(false);
        SetDirection(GetRandomStartVector());

        signalBus.Subscribe<PlayerInputSignal>(OnPlayerInput);
    }

    public void Dispose()
    {
        signalBus.Unsubscribe<PlayerInputSignal>(OnPlayerInput);
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

    public void SetSimulated(bool value)
    {
        body.simulated = value;
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

    private void OnPlayerInput()
    {
        if (!body.simulated)
        {
            SetSimulated(true);
        }   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out IBallCollidable collidable))
        {
            collidable.OnBallCollided(this);
        }
    }

    private Vector2 GetRandomStartVector()
    {
        Vector2 rndDirection = Random.insideUnitCircle;
        rndDirection.y = 1.0f;

        return rndDirection;
    }
}
