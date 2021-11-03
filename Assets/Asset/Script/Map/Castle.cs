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
    }


    public void changeOwner(int owner)
    {
        if(isOwnerBy != 0)
        {
            if (isOwnerBy == 1 && owner == 2) GameManager.Instance.player.CountOccupiedCastle--;
            if (isOwnerBy == 2 && owner == 1) GameManager.Instance.bot.CountOccupiedCastle--;
        }

        isOwnerBy = owner;
        switch (isOwnerBy)
        {
            case 1:
                GameManager.Instance.player.CountOccupiedCastle++;
                this.GetComponent<SpriteRenderer>().sprite = sprites[0];
                break;
            case 2:
                GameManager.Instance.bot.CountOccupiedCastle++;
                this.GetComponent<SpriteRenderer>().sprite = sprites[2];
                break;
        }
        GameManager.Instance.EndGame();

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Castle c = null;
            int i, j;
            Vector2 vector = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            i = (int)System.Math.Round(vector.x);
            j = (int)System.Math.Round(vector.y);
            if (i != x || j != y)
            {
                return;
            }

            if (MapManager.map.arrTile[i, j].IsCastle)
            {
                c = ((Castle)MapManager.map.arrTile[i, j]);
            }

            if (c != null && c.isOwnerBy == 1 && GameManager.Instance.GetStatus() == GameManager.eStatus.Turn_Player)
            {
                GameManager.shop.showShop(x, y);
            }
        }

    }
}
