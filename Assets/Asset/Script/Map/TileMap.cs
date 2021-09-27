using Assets.Asset.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public Map map;
    void Start()
    {
        map = new Map(9, @"C:\Users\minhk\Desktop\map1.txt");
    }
}
