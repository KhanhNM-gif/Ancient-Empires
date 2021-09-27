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
            case "06":
                this.GetComponent<SpriteRenderer>().sprite = sprites[0];
                break;
            case "07":
                this.GetComponent<SpriteRenderer>().sprite = sprites[1];
                break;
            case "09":
                this.GetComponent<SpriteRenderer>().sprite = sprites[2];
                break;
            case "10":
                this.GetComponent<SpriteRenderer>().sprite = sprites[5];
                break;
            case "11":
                this.GetComponent<SpriteRenderer>().sprite = sprites[6];
                break;
            case "12":
                this.GetComponent<SpriteRenderer>().sprite = sprites[4];
                break;
        }
    }
}
