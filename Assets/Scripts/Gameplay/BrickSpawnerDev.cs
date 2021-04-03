using System.Collections.Generic;
using UnityEngine;

public class BrickSpawnerDev : IBrickSpawner
{
    private Brick.Pool brickPool;

    private readonly MapBoundary mapBounds;
    private readonly BrickManager.Settings settings;

    public BrickSpawnerDev(BrickManager.Settings settings, 
        MapBoundary mapBounds, 
        Brick.Pool brickPool)
    {
        this.settings = settings;
        this.mapBounds = mapBounds;
        this.brickPool = brickPool;
    }

    public List<Brick> SpawnBricks()
    {
        var bricks = new List<Brick>();

        for (int y = 0; y < settings.DimensionY; y++)
        {
            for (int x = 0; x < settings.DimensionX; x++)
            {
                var brick = brickPool.Spawn();
                Vector2 position = mapBounds.CameraWorldBounds.center
                    - new Vector3(mapBounds.CameraWorldBounds.extents.x / 2, 0)
                    + new Vector3(x, y);

                brick.transform.position = position;
                bricks.Add(brick);
            }
        }

        return bricks;
    }

    public void DespawnBrick(Brick brick)
    {
        brickPool.Despawn(brick);
    }
}