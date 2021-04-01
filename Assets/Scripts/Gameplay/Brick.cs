using UnityEngine;
using Zenject;

public class Brick : MonoBehaviour, IInitializable, IBallCollidable
{
    private BrickManager brickManager;

    [Inject]
    public void Bind(BrickManager brickManager)
    {
        this.brickManager = brickManager;
    }

    public void Initialize()
    {

    }

    public void OnBallCollided(Ball ball)
    {
        brickManager.DestroyBrick(this);
    }

    public class Factory : PlaceholderFactory<Brick>
    {
    }
}