using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Way : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void changeSprite(string s)
    {
        switch (s)
        {
            case "a":
                this.GetComponent<SpriteRenderer>().sprite = sprites[0];
                break;
            case "b":
                this.GetComponent<SpriteRenderer>().sprite = sprites[1];
                break;
            case "c":
                this.GetComponent<SpriteRenderer>().sprite = sprites[2];
                break;
            case "d":
                this.GetComponent<SpriteRenderer>().sprite = sprites[5];
                break;
            case "e":
                this.GetComponent<SpriteRenderer>().sprite = sprites[6];
                break;
            case "f":
                this.GetComponent<SpriteRenderer>().sprite = sprites[4];
                break;
        }
    }
}
