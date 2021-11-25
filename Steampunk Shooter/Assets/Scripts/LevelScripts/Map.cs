using System;
using System.Collections.Generic;

public class Map
{
    public List<LevelNode> levels;
    public List<LevelFlags> flags;
    public int startId = 0;

    public Map()
    {
        levels = new List<LevelNode>();
        flags = new List<LevelFlags>();
    }
}
