using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile
{
    public BaseTile[,] arrTile{ get; set; }
    public int Size { get; set; }
    public MapTile(int width,int height)
    {
        arrTile = new BaseTile[width+1, width+1];
    }

    public static Vector3 GridWordPosition(int x, int y, int z = 0)
    {
        return new Vector3(x,y,z);
    }
}
