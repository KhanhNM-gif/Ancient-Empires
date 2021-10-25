using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : BaseTile
{
    public int isOwnerBy = 0;

    [SerializeField] Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        if(x==4 && y==4)
            {
            changeOwner(1);
        }
        if(x==10 && y==9)
        {
            changeOwner(2);
        }
    }


    public void changeOwner(int owner)
    {
        if (isOwnerBy == 0)
        {
            isOwnerBy = owner;
            switch (isOwnerBy)
            {
                case 1:
                    this.GetComponent<SpriteRenderer>().sprite = sprites[0];
                    break;
                case 2:
                    this.GetComponent<SpriteRenderer>().sprite = sprites[2];
                    break;
            }
        }

    }
    void Update()
    {
        Castle c = null;


        int i, j;

        Vector2 vector = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        i = (int)System.Math.Round(vector.x);
        j = (int)System.Math.Round(vector.y);
        if (MapManager.map.arrTile[i, j].IsCastle)
        {
            c = ((Castle)MapManager.map.arrTile[i, j]);
        }

        if (c != null && c.isOwnerBy == 1 && Input.GetMouseButtonDown(1) && GameManager.Instance.GetStatus() == GameManager.eStatus.Turn_Player)
        {
            GameManager.shop.showShop(x, y);
        }
    }
}
