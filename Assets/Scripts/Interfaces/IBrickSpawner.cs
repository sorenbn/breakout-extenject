using System.Collections.Generic;

public interface IBrickSpawner
{
    public List<Brick> SpawnBricks();
    public void DespawnBrick(Brick brick);
}