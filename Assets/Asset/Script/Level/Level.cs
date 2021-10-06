using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    public int level;
    private int Exp;
    private int ExpRequired;

    public float attack;
    public float armor;

    // Start is called before the first frame update
    void Start(Unit unit)
    {
        level = 1;
        Exp = 0;
        ExpRequired = 100;
        attack = unit.Attack;
        armor = unit.Armor;
    }

    // Update is called once per frame
    void Update()
    {
        LvUp();
    
    }

    void LvUp()
    {
        if (Exp >= ExpRequired)
        {
            level += 1;
            Exp = Exp - ExpRequired;
            ExpRequired += 100;
        }

        attack += 5;
        armor += 2;
    }
}
