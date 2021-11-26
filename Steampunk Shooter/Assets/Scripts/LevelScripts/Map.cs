using System;
using System.Collections.Generic;

public class Map
{
    public List<LevelNode> levels;
    public List<LevelFlags> flags;
    public int startId = 0;
    public int[,] mapLayout;

    public Map(int width, int height)
    {
        levels = new List<LevelNode>();
        flags = new List<LevelFlags>();
        mapLayout = new int[height, width];
        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                mapLayout[i, j] = -1;
            }
        }
    }

    public int AddLevel(LevelNode lvl, LevelFlags flags)
    {
        lvl.id = levels.Count;
        levels.Add(lvl);
        this.flags.Add(flags);
        mapLayout[lvl.coords.y, lvl.coords.x] = lvl.id;
        return lvl.id;
    }

    public LevelNode LevelAtCoords(Coords c)
    {
        if (mapLayout[c.y, c.x] == -1) return null;
        return levels[mapLayout[c.y, c.x]];
    }

    public readonly struct Coords
    {
        public readonly int x, y;
        public Coords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    // returns the direction b is in relation to a, or -1 if non-adjacent.
    public static int AdjacentDir(Coords a, Coords b)
    {
        if (a.x == b.x && a.y - 1 == b.y) return 0;
        if (a.x == b.x && a.y + 1 == b.y) return 1;
        if (a.y == b.y && a.x - 1 == b.y) return 2;
        if (a.y == b.y && a.x + 1 == b.y) return 3;
        return -1;
    }

    public static Coords MoveDir(Coords a, int dir)
    {
        if (dir == 0) return new Coords(a.x, a.y - 1);
        if (dir == 1) return new Coords(a.x, a.y + 1);
        if (dir == 2) return new Coords(a.x - 1, a.y);
        if (dir == 3) return new Coords(a.x + 1, a.y);
        throw new ArgumentOutOfRangeException("Invalid dir in MoveDir: " + dir);
    }
}
