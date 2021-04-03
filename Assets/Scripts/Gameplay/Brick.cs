using UnityEngine;
using Zenject;

public class Brick : MonoBehaviour, IBallCollidable
{
    private SignalBus signalBus;

    private void ManualBind(SignalBus signalBus)
    {
        this.signalBus = signalBus;
    }

    public void OnBallCollided(Ball ball)
    {
        signalBus.Fire(new BrickDestroyedSignal 
        { 
            Brick = this,
        });
    }

    public class Pool : MonoMemoryPool<Brick>
    {
        private SignalBus signalBus;

        public Pool(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        protected override void OnCreated(Brick item)
        {
            base.OnCreated(item);
            item.ManualBind(signalBus);
        }
    }
}