using System;
using System.Collections.Generic;
using Zenject;

public class BrickManager : IInitializable, IDisposable
{
    private readonly SignalBus signalBus;
    private readonly IBrickSpawner brickSpawner;

    private List<Brick> spawnedBricks;

    public BrickManager(SignalBus signalBus, IBrickSpawner brickSpawner)
    {
        this.signalBus = signalBus;
        this.brickSpawner = brickSpawner;
    }

    public void Initialize()
    {
        SpawnBricks();

        signalBus.Subscribe<BrickDestroyedSignal>(OnBrickDestroyed);
        signalBus.Subscribe<PlayerLostSignal>(OnPlayerLostSignal);
    }

    public void Dispose()
    {
        signalBus.Unsubscribe<BrickDestroyedSignal>(OnBrickDestroyed);
        signalBus.Unsubscribe<PlayerLostSignal>(OnPlayerLostSignal);
    }

    private void SpawnBricks()
    {
        spawnedBricks = brickSpawner.SpawnBricks();
    }

    private void DestroyBrick(Brick brick)
    {
        spawnedBricks.Remove(brick);
        brickSpawner.DespawnBrick(brick);

        if (spawnedBricks.Count == 0)
        {
            ResetBricks();
            signalBus.Fire<PlayerWonSignal>();

            SpawnBricks();
        }
    }

    private void ResetBricks()
    {
        for (int i = spawnedBricks.Count - 1; i >= 0; i--)
        {
            brickSpawner.DespawnBrick(spawnedBricks[i]);
            spawnedBricks.Remove(spawnedBricks[i]);
        }

        spawnedBricks.Clear();
    }

    private void OnBrickDestroyed(BrickDestroyedSignal signal)
    {
        DestroyBrick(signal.Brick);
    }

    private void OnPlayerLostSignal()
    {
        ResetBricks();
        SpawnBricks();
    }

    [Serializable]
    public class Settings
    {
        public int DimensionX;
        public int DimensionY;
    }
}