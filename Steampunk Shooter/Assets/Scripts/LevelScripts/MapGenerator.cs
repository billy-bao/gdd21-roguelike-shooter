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
        LevelNode n1 = new LevelNode("LongLevel", new Map.Coords(4, 4));
        LevelFlags fl = new LevelFlags();
        map.AddLevel(n1, fl);

        LevelNode n2 = new LevelNode("CustomLevelB", new Map.Coords(4, 5));
        fl = new LevelFlags();
        map.AddLevel(n2, fl);

        LevelNode n3 = new LevelNode("PokemonLevel", new Map.Coords(3, 5));
        fl = new LevelFlags();
        fl.disableTop = true;
        fl.disableLeft = true;
        fl.disableBottom = true;
        map.AddLevel(n3, fl);

        LevelNode n4 = new LevelNode("LongLevel", new Map.Coords(4, 3));
        fl = new LevelFlags();
        fl.disableTop = true;
        map.AddLevel(n4, fl);

        LevelNode n5 = new LevelNode("CustomLevelB", new Map.Coords(5, 5));
        fl = new LevelFlags();
        fl.disableRight = true;
        map.AddLevel(n5, fl);

        LevelNode n6 = new LevelNode("LongLevel", new Map.Coords(5, 4));
        fl = new LevelFlags();
        fl.disableTop = true;
        map.AddLevel(n6, fl);
        return map;
    }
}
