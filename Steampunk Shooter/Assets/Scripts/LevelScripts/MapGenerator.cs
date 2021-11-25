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
        map.flags.Add(new LevelFlags());
        return map;
    }
}
