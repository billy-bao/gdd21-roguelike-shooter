using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileDict : MonoBehaviour
{
    public Tile[] d;

    public enum TileDictId
    {
        House = 0,
        Crossarrow,
        UpArrow,
        DownArrow,
        LeftArrow,
        RightArrow,
        VerticalLine,
        HorizontalLine,
        Floor,
        CornerWall,
        SideWall
    }
}
