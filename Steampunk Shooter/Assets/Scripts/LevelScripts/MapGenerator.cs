using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    private static MapGenerator instance = null;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    public LevelData[] levels;

    public static T RandChoice<T>(IReadOnlyList<T> list)
    {
        return list[(int)(Random.value * list.Count)];
    }

    private LevelData FindData(string sceneName)
    {
        foreach(LevelData d in levels)
        {
            if (d.sceneName == sceneName) return d;
        }
        return null;
    }

    public Map GenerateMap(int size)
    {
        // generate lists of levels with a given direction open
        List<LevelData>[] dirLevels = new List<LevelData>[4];
        for (int i = 0; i < 4; i++) dirLevels[i] = new List<LevelData>();
        foreach(LevelData d in levels)
        {
            bool[] dirs = d.openDirs();
            for (int i = 0; i < 4; i++)
                if (dirs[i])
                {
                    Debug.Log("Scene " + d.sceneName + " has open dir " + i);
                    dirLevels[i].Add(d);
                }
        }

        Map map = new Map(size * 2 + 1, size * 2 + 1);
        List<Map.Coords> nextCoords = new List<Map.Coords>();
        nextCoords.Add(new Map.Coords(size, size));

        for(int lvlnum = 0; lvlnum < size; lvlnum++)
        {
            if (nextCoords.Count == 0) break;
            Map.Coords curCoords = RandChoice(nextCoords);
            nextCoords.Remove(curCoords);

            //find adjacent placed rooms w/ open doors and closed doors
            List<int> adjDirs = new List<int>();
            List<int> closedDirs = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                LevelNode lvl = map.LevelAtCoords(Map.MoveDir(curCoords, i));
                int oppDir = LevelNode.oppDir(i);
                if(lvl != null && FindData(lvl.sceneName).openDirs()[oppDir] && !lvl.flags.disableDir[oppDir])
                {
                    Debug.Log("Level " + lvlnum + ": adj dir " + i);
                    adjDirs.Add(i);
                }
                else if(lvl != null)
                {
                    Debug.Log("Level " + lvlnum + ": closed dir " + i);
                    closedDirs.Add(i);
                }
            }
            //satisfy one random adjacency (so this room is reachable)
            LevelData curLvl;
            if(adjDirs.Count > 0)
            {
                int adjDir = RandChoice(adjDirs);
                Debug.Log("Level " + lvlnum + ": using dir list " + adjDir);
                curLvl = RandChoice(dirLevels[adjDir]);
                Debug.Log("Level " + lvlnum + ": picked scene " + curLvl.sceneName);
                adjDirs.Remove(adjDir);
                LevelNode curNode = new LevelNode(curLvl.sceneName, curCoords);
                LevelFlags curFlags = new LevelFlags();

                //handle other adjacencies: if cur room is open, then leave open; otherwise close adjacent room door
                foreach(int dir in adjDirs)
                {
                    if(!curLvl.openDirs()[dir])
                    {
                        Debug.Log("Level " + lvlnum + ": closing adj door in dir " + dir);
                        map.LevelAtCoords(Map.MoveDir(curCoords, dir)).flags.disableDir[LevelNode.oppDir(dir)] = true;
                    }
                }

                //handle closed adjacencies: close cur room's door
                foreach(int dir in closedDirs)
                {
                    if(curLvl.openDirs()[dir])
                    {
                        Debug.Log("Level " + lvlnum + ": closing own door in dir " + dir);
                        curFlags.disableDir[dir] = true;
                    }
                }

                map.AddLevel(curNode, curFlags);
            }
            else
            {
                //first room - pick any level
                curLvl = RandChoice(levels);
                LevelNode curNode = new LevelNode(curLvl.sceneName, curCoords);
                map.AddLevel(curNode, new LevelFlags());
            }

            //add adjacent empty directions to nextCoords
            for(int i = 0; i < 4; i++)
            {
                Map.Coords nextCoord = Map.MoveDir(curCoords, i);
                if (curLvl.openDirs()[i] && map.LevelAtCoords(nextCoord) is null && !nextCoords.Contains(nextCoord))
                {
                    Debug.Log("Level " + lvlnum + ": empty dir " + i);
                    nextCoords.Add(nextCoord);
                }
            }
        }

        //final pass: close all doors leading to coords with no level placed
        foreach(LevelNode lvl in map.levels)
        {
            for(int i = 0; i < 4; i++)
            {
                if (FindData(lvl.sceneName).openDirs()[i] && map.LevelAtCoords(Map.MoveDir(lvl.coords, i)) is null)
                {
                    lvl.flags.disableDir[i] = true;
                }
            }
        }
        Debug.Log(map);

        // rare case: generator blocked itself off and placed a lot less rooms than requested - retry
        if(map.levels.Count < (int)(0.9 * size))
        {
            Debug.Log("size too small, retry map generation");
            return GenerateMap(size);
        }

        map.startId = (int)(Random.value * map.levels.Count);
        Debug.Log("Starting room is room #" + map.startId);

        //increase difficulty of farther rooms
        int[] dists = new int[map.levels.Count];
        for (int i = 0; i < dists.Length; i++) dists[i] = -1;
        Queue<int> bfs = new Queue<int>();
        bfs.Enqueue(map.startId);
        dists[map.startId] = 0;
        while (bfs.Count > 0)
        {
            int curRoom = bfs.Dequeue();
            for(int i = 0; i < 4; i++)
            {
                LevelNode adjRoom = map.LevelAtCoords(Map.MoveDir(map.levels[curRoom].coords, i));
                if(adjRoom != null && dists[adjRoom.id] == -1)
                {
                    dists[adjRoom.id] = dists[curRoom] + 1;
                    bfs.Enqueue(adjRoom.id);
                }
            }
        }
        for(int i = 0; i < dists.Length; i++)
        {
            map.flags[i].diffLevel = Mathf.Min((int)(Random.value * dists[i]), (int)Mathf.Sqrt(2*dists[i]));
        }

        return map;
    }
}
