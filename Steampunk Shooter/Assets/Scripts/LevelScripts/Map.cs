using System;
using System.Collections.Generic;

public class Map
{
    public List<LevelNode> levels;
    public List<LevelFlags> flags;
    public int startId = 0;
    public int[,] mapLayout;
    public int minX, maxX, minY, maxY;

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
        //add level
        lvl.id = levels.Count;
        lvl.flags = flags;
        levels.Add(lvl);
        this.flags.Add(flags);
        mapLayout[lvl.coords.y, lvl.coords.x] = lvl.id;

        //update map boundaries
        if (levels.Count == 1)
        {
            minX = lvl.coords.x;
            maxX = lvl.coords.x;
            minY = lvl.coords.y;
            maxY = lvl.coords.y;
        }
        else
        {
            if (lvl.coords.x < minX) minX = lvl.coords.x;
            if (lvl.coords.x > maxX) maxX = lvl.coords.x;
            if (lvl.coords.y < minY) minY = lvl.coords.y;
            if (lvl.coords.y > maxY) maxY = lvl.coords.y;
        }

        return lvl.id;
    }

    public LevelNode LevelAtCoords(Coords c)
    {
        if (mapLayout[c.y, c.x] == -1) return null;
        return levels[mapLayout[c.y, c.x]];
    }

    public override string ToString()
    {
        string str = "";
        str += "<Map object size " + mapLayout.GetLength(1) + "x" + mapLayout.GetLength(0) + ":\n";
        for (int i = minY; i <= maxY; i++)
        {
            for (int j = minX; j <= maxX; j++)
            {
                str += FormatRoomId(mapLayout[i, j]) + " ";
            }
            str += "\n";
        }
        return str + ">\n";
    }

    private string FormatRoomId(int id)
    {
        string str = id.ToString();
        if (id == -1) str = "x";
        int padToLen = (levels.Count - 1).ToString().Length;
        return str.PadLeft(padToLen);
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
