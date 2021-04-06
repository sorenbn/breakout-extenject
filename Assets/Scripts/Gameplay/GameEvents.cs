public class PlayerInputSignal
{
}

public class PlayerWonSignal
{
}

public class PlayerLostSignal
{
}

public class BallLostSignal
{
    public Ball Ball
    {
        get; set;
    }
}

public class BrickDestroyedSignal
{
    public Brick Brick
    {
        get; set;
    }
}