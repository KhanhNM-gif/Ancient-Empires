using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : BaseTile
{
    [SerializeField] Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
       
    }


    public void updateForest()
    {
        var ran = Random.Range(0, 2);
        if (ran == 1)
        {
            this.GetComponent<SpriteRenderer>().sprite = sprites[0];
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
    }
}
