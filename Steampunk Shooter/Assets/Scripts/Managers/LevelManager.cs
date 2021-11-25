using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class LevelManager : MonoBehaviour
{

    public LevelData levelData;
    public TileDict tileDict;

    public virtual void Initialize(LevelFlags flags)
    {
        BlockExits(flags);
    }

    //block off disabled exits
    public virtual void BlockExits(LevelFlags flags)
    {
        Grid grid = FindObjectOfType<Grid>();
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

    // Update is called once per frame
    void Update()
    {

    }
}
