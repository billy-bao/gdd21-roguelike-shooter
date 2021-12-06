using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [System.NonSerialized]
    public GameManager gameManager;
    public LevelData levelData;
    public TileDict tileDict;
    protected Grid grid;
    protected LevelFlags flags;
    protected bool levelClearTriggered = false;
    protected Player player;
    private List<Item> possibleSpawns;

    public virtual void Initialize(LevelFlags flags, Player player, int dir)
    {
        this.flags = flags;
        BlockExits();
        PlacePlayer(player, dir);
        SetDifficulty(flags.diffLevel);
        if(flags.roomCleared)
        {
            //disable WaveManager
            levelData.enemySpawns.state = WaveManager.SpawnState.DONE;
            //clear all enemies
            foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(enemy);
            }
        }
        this.player = player;
    }

    void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    //block off disabled exits
    protected virtual void BlockExits()
    {
        grid = FindObjectOfType<Grid>();
        if (grid == null)
        {
            Debug.Log("No grid found in level! Aborting...");
            return;
        }
        // get the tilemap with a collider
        Tilemap wallTilemap = grid.gameObject.GetComponentInChildren<TilemapCollider2D>().gameObject.GetComponent<Tilemap>();
        Tile t = tileDict.d[(int)TileDict.TileDictId.SideWall];
        if (flags.disableDir[0])
        {
            wallTilemap.SetTile(grid.WorldToCell(levelData.topExit.position), t);
            wallTilemap.SetTransformMatrix(grid.WorldToCell(levelData.topExit.position), Matrix4x4.Rotate(Quaternion.Euler(0, 0, -90)));
        }
        if (flags.disableDir[1])
        {
            wallTilemap.SetTile(grid.WorldToCell(levelData.bottomExit.position), t);
            wallTilemap.SetTransformMatrix(grid.WorldToCell(levelData.bottomExit.position), Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90)));
        }
        if (flags.disableDir[2])
        {
            wallTilemap.SetTile(grid.WorldToCell(levelData.leftExit.position), t);
            //no rotation needed
        }
        if (flags.disableDir[3])
        {
            wallTilemap.SetTile(grid.WorldToCell(levelData.rightExit.position), t);
            wallTilemap.SetTransformMatrix(grid.WorldToCell(levelData.rightExit.position), Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180)));
        }
    }

    protected virtual void PlacePlayer(Player player, int dir)
    {
        Debug.Log("Placing player at dir " + dir);
        try
        {
            switch (dir)
            {
                case -1:
                    {
                        player.transform.position = levelData.itemSpawn.position;
                        break;
                    }
                case 0:
                    {
                        Vector3 v = levelData.topExit.position;
                        v.y -= 1.2f;
                        player.transform.position = v;
                        break;
                    }
                case 1:
                    {
                        Vector3 v = levelData.bottomExit.position;
                        v.y += 1.2f;
                        player.transform.position = v;
                        break;
                    }
                case 2:
                    {
                        Vector3 v = levelData.leftExit.position;
                        v.x += 1.2f;
                        player.transform.position = v;
                        break;
                    }
                case 3:
                    {
                        Vector3 v = levelData.rightExit.position;
                        v.x -= 1.2f;
                        player.transform.position = v;
                        break;
                    }
            }
        }
        catch(UnassignedReferenceException)
        {
            // exit doesn't exist; just spawn at center
            player.transform.position = levelData.itemSpawn.position;
        }
    }

    protected virtual void SetDifficulty(int diff)
    {
        //add mods to wavemanager
        int modsLeft = diff;
        WaveManager spawns = levelData.enemySpawns;
        while(modsLeft > 0)
        {
            modsLeft--;
            int modID = (int)(Random.value * 4);
            switch(modID)
            {
                case 0:
                    {
                        //increase enemy spawns
                        foreach(WaveManager.Wave w in spawns.waves)
                        {
                            w.amount = (int)(w.amount * 1.5) + 1;
                        }
                        break;
                    }
                case 1:
                    {
                        //buff enemy speed
                        foreach (WaveManager.Wave w in spawns.waves)
                        {
                            w.enemy = Instantiate(w.enemy);
                            w.enemy.tag = "Untagged";
                            w.enemy.SetActive(false);
                            Enemy1 e = w.enemy.GetComponent<Enemy1>();
                            if (e != null) e.MoveSpeed += 1;
                        }
                        break;
                    }
                case 2:
                    {
                        //buff enemy damage
                        foreach (WaveManager.Wave w in spawns.waves)
                        {
                            w.enemy = Instantiate(w.enemy);
                            w.enemy.tag = "Untagged";
                            w.enemy.SetActive(false);
                            Enemy1 e = w.enemy.GetComponent<Enemy1>();
                            if (e != null) e.Damage += 1;
                        }
                        break;
                    }
                case 3:
                    {
                        //buff enemy health
                        foreach (WaveManager.Wave w in spawns.waves)
                        {
                            w.enemy = Instantiate(w.enemy);
                            w.enemy.tag = "Untagged";
                            w.enemy.SetActive(false);
                            Enemy1 e = w.enemy.GetComponent<Enemy1>();
                            if (e != null) { e.MaxLife += 3; e.TakeDamage(-3); }
                        }
                        break;
                    }
            }
        }

        //make item reward better
        for (int i = 0; i < diff; i++)
        {
            for (int j = 0; j < levelData.itemDrops.Length; j++)
            {
                Item it = levelData.itemDrops[j];
                it = Instantiate(it);
                it.tag = "Untagged";
                it.gameObject.SetActive(false);
                if (it as HealthRefill != null)
                {
                    (it as HealthRefill).healAmount += 3;
                }
                else if (it as AtkSpdUp != null)
                {
                    (it as AtkSpdUp).incAmount += 0.25f;
                }
                else if (it as MovSpdUp != null)
                {
                    (it as MovSpdUp).incAmount += 1.5f;
                }
                else if (it as AtkDmgUp != null)
                {
                    (it as AtkDmgUp).incAmount += 0.5f;
                }
                // no ProjPierceUp buff - already very good
                levelData.itemDrops[j] = it;
            }
        }
    }

    public virtual Item ChooseRandomItem()
    {
        if(possibleSpawns.Count > 0)
        {
            Item it = MapGenerator.RandChoice(possibleSpawns);
            possibleSpawns.Remove(it);
            return it;
        }
        return null;
    }

    public virtual void SpawnItem(Item it, int pos)
    {
        Vector3 position = levelData.itemSpawn.position;
        position[0] += 2 * pos - 1f;
        it = Instantiate(it, position, Quaternion.identity);
        it.id = pos;
        it.tag = "Item";
        it.gameObject.SetActive(true);
    }

    public virtual void OnEnemyCleared()
    {
        if (player != null) player.areEnemiesCleared = true;
        if (flags == null)
        {
            // single level testing
            possibleSpawns = new List<Item>(levelData.itemDrops);
            if (player.AdjustedMovSpd() >= 10f)
            {
                possibleSpawns.RemoveAll(x => (x as MovSpdUp) != null); //remove move speed up drops
            }
            SpawnItem(ChooseRandomItem(), 0);
            SpawnItem(ChooseRandomItem(), 1);
            return;
        }
        if(!flags.roomCleared)
        {
            flags.roomCleared = true;

            possibleSpawns = new List<Item>(levelData.itemDrops);
            if (player.AdjustedMovSpd() >= 10f)
            {
                possibleSpawns.RemoveAll(x => (x as MovSpdUp) != null); //remove move speed up drops if at cap
            }
            if(flags.diffLevel < 1)
            {
                possibleSpawns.RemoveAll(x => (x as ProjPierceUp) != null); //remove piercing drops if difficulty is 0
            }
            flags.droppedItem[0] = ChooseRandomItem();
            flags.droppedItem[1] = ChooseRandomItem();

            gameManager.OnLevelCleared();
        }
        for (int i = 0; i < flags.droppedItem.Length; i++)
        {
            if (flags.droppedItem[i] != null)
            {
                SpawnItem(flags.droppedItem[i], i);
            }
        }
    }

    public virtual void OnItemPickup(Item item)
    {
        if (flags != null)
        {
            for (int i = 0; i < flags.droppedItem.Length; i++)
            {
                flags.droppedItem[i] = null;
            }
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Item"))
            {
                Item it = obj.GetComponent<Item>();
                if (it == null || it.id >= 0) Destroy(it.gameObject);
            }
        }
    }

    public virtual void ExitLevel(int dir)
    {
        if (player != null) player.areEnemiesCleared = false;
        gameManager.ExitLevel(dir);
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelClearTriggered && levelData.enemySpawns.state == WaveManager.SpawnState.DONE)
        {
            levelClearTriggered = true;
            Debug.Log("All enemies cleared!");
            OnEnemyCleared();
        }
    }
}
