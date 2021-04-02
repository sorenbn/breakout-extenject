using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BrickManager : IInitializable
{
    [Inject]
    private Brick.Pool brickPool;

    private readonly Settings settings;
    private readonly MapBoundary mapBounds;

    private List<Brick> spawnedBricks;

    public BrickManager(Settings settings, MapBoundary mapBounds)
    {
        this.settings = settings;
        this.mapBounds = mapBounds;

        spawnedBricks = new List<Brick>();
    }

    public void Initialize()
    {
        SpawnBricks();
    }

    public void DestroyBrick(Brick brick)
    {
        spawnedBricks.Remove(brick);
        brickPool.Despawn(brick);
    }

    private void SpawnBricks()
    {
        for (int y = 0; y < settings.DimensionY; y++)
        {
            for (int x = 0; x < settings.DimensionX; x++)
            {
                var brick = brickPool.Spawn();
                Vector2 position = mapBounds.CameraWorldBounds.center
                    - new Vector3(mapBounds.CameraWorldBounds.extents.x / 2, 0)
                    + new Vector3(x, y);

                brick.transform.position = position;

                spawnedBricks.Add(brick);
            }
        }
    }

    private void ClearBricks()
    {
        for (int i = spawnedBricks.Count - 1; i >= 0; i--)
        {
            brickPool.Despawn(spawnedBricks[i]);
        }

        brickPool.Clear();
    }

    [System.Serializable]
    public class Settings
    {
        public int DimensionX;
        public int DimensionY;
    }
}