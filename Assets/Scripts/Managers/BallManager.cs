using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BallManager : IInitializable, IDisposable
{
    private readonly SignalBus signalBus;
    private readonly Ball.Pool ballPool;
    private readonly RoutineRunner runner;

    private List<Ball> spawnedBalls = new List<Ball>();

    public BallManager(SignalBus signalBus, Ball.Pool ballPool, RoutineRunner runner)
    {
        this.signalBus = signalBus;
        this.ballPool = ballPool;
        this.runner = runner;
    }

    public void Initialize()
    {
        signalBus.Subscribe<BallLostSignal>(OnBallLostSignal);
        signalBus.Subscribe<PlayerWonSignal>(OnPlayerWonOrLostSignal);
        signalBus.Subscribe<PlayerLostSignal>(OnPlayerWonOrLostSignal);

        SpawnBall();
    }

    public void Dispose()
    {
        signalBus.Unsubscribe<BallLostSignal>(OnBallLostSignal);
        signalBus.Unsubscribe<PlayerWonSignal>(OnPlayerWonOrLostSignal);
        signalBus.Unsubscribe<PlayerLostSignal>(OnPlayerWonOrLostSignal);
    }

    private void SpawnBall(float delay = 0.0f)
    {
        if (delay == 0.0f)
        {
            var ball = ballPool.Spawn();
            ball.transform.position = Vector3.up;

            spawnedBalls.Add(ball);
        }
        else
        {
            runner.StartCoroutine(DelayedSpawn(delay));
        }
    }

    private void ResetBalls()
    {
        for (int i = spawnedBalls.Count - 1; i >= 0; i--)
        {
            ballPool.Despawn(spawnedBalls[i]);
            spawnedBalls.Remove(spawnedBalls[i]);
        }
    }

    private void OnBallLostSignal(BallLostSignal signal)
    {
        ballPool.Despawn(signal.Ball);
        spawnedBalls.Remove(signal.Ball);

        if (spawnedBalls.Count == 0)
        {
            signalBus.Fire<PlayerLostSignal>();
        }
    }

    private void OnPlayerWonOrLostSignal()
    {
        ResetBalls();
        SpawnBall(0.5f);
    }

    private IEnumerator DelayedSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnBall(0.0f);
    }
}