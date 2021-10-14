using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public float Lv;
    private float exp;
    private float expRequired;
    public float Attack;
    public float Armor;

    void Start(Unit unit)
    {
        Lv = 1;
        exp = 0;
        expRequired = 100;
        Attack = unit.Attack;
        Armor = unit.Armor;
    }

    void Update()
    {
        Exp();
    }

    // Level Up
    void LvUp()
    {
        Lv += 1;
        exp = exp - expRequired;
        Attack += 5;
        Armor += 2;
        expRequired = 2 * expRequired; 
    }

    void Exp()
    {
        if (exp >= expRequired)
            LvUp();      
    }
}
