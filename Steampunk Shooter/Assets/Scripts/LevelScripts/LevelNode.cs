using System;
public class LevelNode
{
    public string sceneName;
    public LevelNode[] links;

    public LevelNode(string sceneName)
    {
        this.sceneName = sceneName;
        links = new LevelNode[4];
    }

    public static int oppDir(int dir)
    {
        return dir ^ 1;
    }
}
