using Assets.Asset.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManger : MonoBehaviour
{
    public GameObject pt;
    private Sprite[] sprites;
    public static Map map;
    void Start()
    {
        map = new Map(9, @"C:\Users\minhk\Desktop\map1.txt");
        sprites = Resources.LoadAll<Sprite>("tiles");
        for (int i = 0; i < map.Size; i++)
            for (int j = 0; j < map.Size; j++)
            {
                SpawmTile(i, j, map.arrCell[i, j].isCome ? 18 : 0);
            }
    }
    private void SpawmTile(int x, int y, int numberSprite)
    {
        SpriteRenderer sr = Instantiate(pt, Map.GridWordPosition(x, y), Quaternion.identity).GetComponent<SpriteRenderer>();
        sr.sprite = sprites[numberSprite];
        sr.name = $"({x},{y})";
    }

}
