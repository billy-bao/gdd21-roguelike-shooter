using UnityEngine;
using System;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    private static MapGenerator instance = null;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    public Map GenerateMap(int size)
    {
        Map map = new Map(10, 10);
        LevelNode n1 = new LevelNode("CustomLevelB", new Map.Coords(4, 4));
        LevelFlags fl = new LevelFlags();
        fl.disableTop = true;
        fl.disableRight = true;
        map.AddLevel(n1, fl);

        LevelNode n2 = new LevelNode("CustomLevelB", new Map.Coords(3, 4));
        fl = new LevelFlags();
        fl.disableTop = true;
        fl.disableLeft = true;
        map.AddLevel(n2, fl);

        return map;
    }
}
