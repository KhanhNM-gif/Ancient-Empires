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



    // Update is called once per frame
    public void changeOwner(int owner)
    {
        if(isOwnerBy == 0)
        {
            isOwnerBy = owner;
            switch (isOwnerBy)
            {
                case 1:
                    this.GetComponent<SpriteRenderer>().sprite = sprites[0];
                    break;
                case 2:
                    this.GetComponent<SpriteRenderer>().sprite = sprites[1];
                    break;
            }
        }

    }

}
