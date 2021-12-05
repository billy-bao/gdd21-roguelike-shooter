using UnityEngine;
using System.Collections;

public class LevelData : MonoBehaviour
{
    public string sceneName;
    public Transform topExit;
    public Transform bottomExit;
    public Transform leftExit;
    public Transform rightExit;
    public Transform itemSpawn;
    public Item[] itemDrops;
    public WaveManager enemySpawns;
    private bool[] dirs;

    public bool[] openDirs()
    {
        if (this.dirs.Length == 4) return this.dirs;
        bool[] dirs = new bool[4];
        if (topExit != null) dirs[0] = true;
        if (bottomExit != null) dirs[1] = true;
        if (leftExit != null) dirs[2] = true;
        if (rightExit != null) dirs[3] = true;
        this.dirs = dirs;
        return dirs;
    }
}
