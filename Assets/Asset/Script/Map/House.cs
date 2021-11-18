using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : BaseTile
{
    public Transform Smoke1;
    public Transform Smoke2;
    public Transform Smoke3;
    public Transform Smoke4;
    public GameObject smoke1;
    public GameObject smoke2;
    public GameObject smoke3;
    public GameObject smoke4;
    public int isOwnerBy = 0;

    [SerializeField] Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void changeOwner(int owner)
    {
        isOwnerBy = owner;
        switch (isOwnerBy)
        {
            case 1:
                this.GetComponent<SpriteRenderer>().sprite = sprites[0];
                GameObject a = Instantiate(smoke1, Smoke1.position, Smoke1.rotation);
                GameObject b = Instantiate(smoke2, Smoke2.position, Smoke2.rotation);
                GameObject c = Instantiate(smoke3, Smoke3.position, Smoke3.rotation);
                GameObject d = Instantiate(smoke4, Smoke4.position, Smoke4.rotation);
                break;
            case 2:
                this.GetComponent<SpriteRenderer>().sprite = sprites[1];
                GameObject e = Instantiate(smoke1, Smoke1.position, Smoke1.rotation);
                GameObject f = Instantiate(smoke2, Smoke2.position, Smoke2.rotation);
                GameObject g = Instantiate(smoke3, Smoke3.position, Smoke3.rotation);
                GameObject h = Instantiate(smoke4, Smoke4.position, Smoke4.rotation);
                break;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            House c = null;
            int i, j;
            Vector2 vector = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            i = (int)System.Math.Round(vector.x);
            j = (int)System.Math.Round(vector.y);
            if (i != x || j != y)
            {
                return;
            }

            if (MapManager.map.arrTile[i, j].IsHouse)
            {
                c = ((House)MapManager.map.arrTile[i, j]);
            }

            if (c != null && c.isOwnerBy == 1 && GameManager.Instance.GetStatus() == GameManager.eStatus.Turn_Player)
            {
                GameManager.shop.showShop(x, y);
            }
        }
    }
}
