using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour, IInitializable, IFixedTickable, IPoolable
{
    [SerializeField]
    private float speed = 2.0f;

    [SerializeField]
    private SpriteRenderer render;

    private Rigidbody2D body;
    private MapBoundary mapBounds;
    private SignalBus signalBus;

    public void ManualBind(MapBoundary mapBounds, SignalBus signalBus)
    {
        this.mapBounds = mapBounds;
        this.signalBus = signalBus;
    }

    public void Initialize()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void OnSpawned()
    {
        signalBus.Subscribe<PlayerInputSignal>(OnPlayerInput);
        SetSimulated(false);
    }

    public void OnDespawned()
    {
        signalBus.Unsubscribe<PlayerInputSignal>(OnPlayerInput);
        SetSimulated(false);
    }

    public void FixedTick()
    {
        if (InDeadZone())
        {
            signalBus.Fire(new BallLostSignal { Ball = this });
        }

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
        return body.position.y > mapBounds.CameraWorldBounds.max.y - render.bounds.extents.y && body.velocity.y > 0;
    }

    private bool InDeadZone()
    {
        return body.position.y < mapBounds.CameraWorldBounds.min.y + render.bounds.extents.y && body.velocity.y < 0;
    }

    private Vector2 GetRandomStartVector()
    {
        Vector2 rndDirection = Random.insideUnitCircle;
        rndDirection.y = 1.0f;

        return rndDirection;
    }

    private void OnPlayerInput()
    {
        if (!body.simulated)
        {
            SetDirection(GetRandomStartVector());
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

    public class Pool : MonoMemoryPool<Ball>
    {
        private readonly MapBoundary mapBounds;
        private readonly SignalBus signalBus;
        private readonly TickableManager tickManager;

        public Pool(MapBoundary mapBounds, 
            SignalBus signalBus, 
            TickableManager tickManager)
        {
            this.mapBounds = mapBounds;
            this.signalBus = signalBus;
            this.tickManager = tickManager;
        }

        protected override void OnCreated(Ball item)
        {
            base.OnCreated(item);

            item.ManualBind(mapBounds, signalBus);
            item.Initialize();
        }

        protected override void OnSpawned(Ball item)
        {
            base.OnSpawned(item);

            item.OnSpawned();
            tickManager.AddFixed(item);
        }

        protected override void OnDespawned(Ball item)
        {
            base.OnDespawned(item);
            
            tickManager.RemoveFixed(item);
            item.OnDespawned();
        }
    }
}