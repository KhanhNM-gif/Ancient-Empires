using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderMap : BaseTile
{

    [SerializeField] Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void changeBorderMap(int i, int j, int width, int height)
    {
        switch (changeBorder(i, j, width, height))
        {
            case 1:
                this.GetComponent<SpriteRenderer>().sprite = sprites[0];
                break;
            case 2:
                this.GetComponent<SpriteRenderer>().sprite = sprites[5];
                break;
            case 3:
                this.GetComponent<SpriteRenderer>().sprite = sprites[6];
                break;
            case 4:
                this.GetComponent<SpriteRenderer>().sprite = sprites[7];
                break;
            case 5:
                this.GetComponent<SpriteRenderer>().sprite = sprites[4];
                break;
            case 6:
                this.GetComponent<SpriteRenderer>().sprite = sprites[1];
                break;
            case 7:
                this.GetComponent<SpriteRenderer>().sprite = sprites[2];
                break;
            case 8:
                this.GetComponent<SpriteRenderer>().sprite = sprites[3];
                break;
            default:
                break;
        }
    }


    private static int changeBorder(int i, int j, int width, int height)
    {
        if (i == 0 && j == height - 1)
        {
            return 1;
        }
        else if (i == 0 && j != 0)
        {
            return 2;
        }
        else if (i == 0 && j == 0)
        {
            return 3;
        }
        else if (i != width - 1 && j == 0)
        {
            return 4;
        }
        else if (i == width - 1 && j == 0)
        {
            return 5;
        }
        else if (i == width - 1 && j != height - 1)
        {
            return 6;
        }
        else if (i == width - 1 && j == height - 1)
        {
            return 7;
        }
        else if (j == height - 1)
        {
            return 8;
        }
        return 0;
    }
}
