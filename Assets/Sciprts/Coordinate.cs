using System;

public enum TileTypeID
{
    Ground = 0,
    Wall = 1,
    Cliff = 2,
}

[Serializable]
public class Coordinate
{
    public int X;
    public int Y;

    public int TileTypeID;
}
