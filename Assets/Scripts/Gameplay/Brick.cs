using UnityEngine;
using Zenject;

public class Brick : MonoBehaviour, IBallCollidable
{
    private BrickManager brickManager;

    private void ManualBind(BrickManager brickManager)
    {
        this.brickManager = brickManager;
    }

    public void OnBallCollided(Ball ball)
    {
        brickManager.DestroyBrick(this);
    }

    public class Pool : MonoMemoryPool<Brick>
    {
        [Inject]
        private BrickManager brickManager;

        protected override void OnCreated(Brick item)
        {
            base.OnCreated(item);

            //Manual injection to avoid runtime reflection on every brick, whenever it's created!
            item.ManualBind(brickManager);
        }

        protected override void OnSpawned(Brick item)
        {
            base.OnSpawned(item);
        }

        protected override void OnDespawned(Brick item)
        {
            base.OnDespawned(item);
        }
    }
}