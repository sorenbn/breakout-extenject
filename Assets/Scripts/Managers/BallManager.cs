using System;
using UnityEngine;
using Zenject;

public class BallManager : IInitializable, IDisposable
{
    private readonly SignalBus signalBus;
    private readonly Ball.Pool ballPool;

    public BallManager(SignalBus signalBus, Ball.Pool ballPool)
    {
        this.signalBus = signalBus;
        this.ballPool = ballPool;
    }

    public void Initialize()
    {
        signalBus.Subscribe<BallLostSignal>(OnBallLostSignal);

        //TOOD: Move to a "OnGameReset" event or something like that.
        SpawnStartingBall();
    }

    public void Dispose()
    {
        signalBus.Unsubscribe<BallLostSignal>(OnBallLostSignal);
    }

    private void OnBallLostSignal(BallLostSignal signal)
    {
        ballPool.Despawn(signal.Ball);
    }

    private void SpawnStartingBall()
    {
        var ball = ballPool.Spawn();
        ball.transform.position = Vector3.up;
    }
}