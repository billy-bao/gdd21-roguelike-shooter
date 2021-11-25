using UnityEngine;
using System;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    private static MapGenerator instance = null;

    public LevelData[] levelData;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public Map GenerateMap(int size)
    {
        Map map = new Map();
        map.levels.Add(new LevelNode("CustomLevelB"));
        LevelFlags fl = new LevelFlags();
        fl.disableTop = true;
        fl.disableLeft = true;
        fl.disableRight = true;
        map.flags.Add(fl);
        return map;
    }
}
