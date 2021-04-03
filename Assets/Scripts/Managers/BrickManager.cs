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
    }

    public void Dispose()
    {
        signalBus.Unsubscribe<BrickDestroyedSignal>(OnBrickDestroyed);
    }

    private void OnBrickDestroyed(BrickDestroyedSignal signal)
    {
        DestroyBrick(signal.Brick);
    }

    private void SpawnBricks()
    {
        spawnedBricks = brickSpawner.SpawnBricks();
    }

    private void DestroyBrick(Brick brick)
    {
        spawnedBricks.Remove(brick);
        brickSpawner.DespawnBrick(brick);
    }

    [Serializable]
    public class Settings
    {
        public int DimensionX;
        public int DimensionY;
    }
}