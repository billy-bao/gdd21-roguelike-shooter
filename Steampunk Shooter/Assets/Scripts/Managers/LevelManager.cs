using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [System.NonSerialized]
    public GameManager gameManager;
    public LevelData levelData;
    public TileDict tileDict;
    protected Grid grid;
    protected LevelFlags flags;
    protected bool levelClearTriggered = false;
    public virtual void Initialize(LevelFlags flags, Player player, int dir)
    {
        this.flags = flags;
        BlockExits();
        PlacePlayer(player, dir);
        if(flags.roomCleared)
        {
            //disable WaveManager
            levelData.enemySpawns.state = WaveManager.SpawnState.DONE;
        }

    }

    //block off disabled exits
    public virtual void BlockExits()
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

    public virtual void PlacePlayer(Player player, int dir)
    {
        Debug.Log("Placing player at dir " + dir);
        switch(dir)
        {
            case -1:
                {
                    player.transform.position = levelData.itemSpawn.position;
                    break;
                }
            case 0:
                {
                    Vector3 v = levelData.topExit.position;
                    v.y -= 1;
                    player.transform.position = v;
                    break;
                }
            case 1:
                {
                    Vector3 v = levelData.bottomExit.position;
                    v.y += 1;
                    player.transform.position = v;
                    break;
                }
            case 2:
                {
                    Vector3 v = levelData.leftExit.position;
                    v.x += 1;
                    player.transform.position = v;
                    break;
                }
            case 3:
                {
                    Vector3 v = levelData.rightExit.position;
                    v.x -= 1;
                    player.transform.position = v;
                    break;
                }
        }
    }

    public virtual void OnEnemyCleared()
    {
        if(flags == null)
        {
            // single level testing
            Instantiate(levelData.itemDrops[Random.Range(0, levelData.itemDrops.Length)], levelData.itemSpawn.position, Quaternion.identity);
            return;
        }
        if(!flags.roomCleared)
        {
            flags.roomCleared = true;
            flags.droppedItem = levelData.itemDrops[Random.Range(0, levelData.itemDrops.Length)];
        }
        if(flags.droppedItem != null)
        {
            Instantiate(flags.droppedItem, levelData.itemSpawn.position, Quaternion.identity);
        }
    }

    public virtual void OnItemPickup(Item i)
    {
        if (flags != null)
        {
            flags.droppedItem = null;
        }
    }

    public virtual void ExitLevel(int dir)
    {
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
