using System;
public class LevelNode
{
    public int id;
    public string sceneName;
    public Map.Coords coords;
    public LevelFlags flags;

    public LevelNode(string sceneName, Map.Coords coords)
    {
        this.sceneName = sceneName;
        this.coords = coords;
    }

    public static int oppDir(int dir)
    {
        return dir ^ 1;
    }
}
