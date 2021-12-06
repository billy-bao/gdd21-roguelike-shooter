using System;

public class LevelFlags
{
    public bool[] disableDir;
    public bool roomCleared;
    public Item[] droppedItem;
    public int diffLevel;
    public Object customFlags;

    public LevelFlags()
    {
        droppedItem = new Item[2];
        disableDir = new bool[4];
    }
}
