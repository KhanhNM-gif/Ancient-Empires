using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus
{
    public int damage;
    public int defense;
    public int move;
    public int attackrange;

    // Start is called before the first frame update
    public PlayerStatus()
    {
        damage = 50;
        defense = 30;
        move = 3;
        attackrange = 2;
    }
}
