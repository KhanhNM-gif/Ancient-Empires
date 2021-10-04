using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{

    public int damage;
    public int defense;
    public int move;
    public int attackrange;

    // Start is called before the first frame update
    void Start()
    {
        damage = 50;
        defense = 30;
        move = 3;
        attackrange = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
