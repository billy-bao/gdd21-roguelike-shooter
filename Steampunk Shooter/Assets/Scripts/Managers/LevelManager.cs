using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [System.NonSerialized]
    public GameManager gameManager;
    public LevelData levelData;
    public TileDict tileDict;
    private Grid grid;

    public virtual void Initialize(LevelFlags flags, Player player, int dir)
    {
        BlockExits(flags);
        PlacePlayer(player, dir);
    }

    //block off disabled exits
    public virtual void BlockExits(LevelFlags flags)
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
        if (flags.disableTop)
        {
            wallTilemap.SetTile(grid.WorldToCell(levelData.topExit.position), t);
            wallTilemap.SetTransformMatrix(grid.WorldToCell(levelData.topExit.position), Matrix4x4.Rotate(Quaternion.Euler(0, 0, -90)));
        }
        if (flags.disableBottom)
        {
            wallTilemap.SetTile(grid.WorldToCell(levelData.bottomExit.position), t);
            wallTilemap.SetTransformMatrix(grid.WorldToCell(levelData.bottomExit.position), Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90)));
        }
        if (flags.disableLeft)
        {
            wallTilemap.SetTile(grid.WorldToCell(levelData.leftExit.position), t);
            //no rotation needed
        }
        if (flags.disableRight)
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

    public virtual void ExitLevel(int dir)
    {
        gameManager.ExitLevel(dir);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
