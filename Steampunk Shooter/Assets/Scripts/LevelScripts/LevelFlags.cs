using System;

public class LevelFlags
{
    public bool[] disableDir;
    public bool roomCleared;
    public Item droppedItem;
    public Object customFlags;

    public LevelFlags()
    {
        disableDir = new bool[4];
    }
}
