using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    public int level;
    private int Exp;
    private int ExpRequired;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        Exp = 0;
        ExpRequired = 100;
    }

    // Update is called once per frame
    void Update()
    {
        EXP();
        if(Input.GetKeyDown(KeyCode.E))
        {
            Exp += 20;
        }

    }

    void RankUp()
    {
        level += 1;
        Exp = Exp - ExpRequired;
        ExpRequired += 100;
    }

    void EXP()
    {
        if (Exp >= ExpRequired)
            RankUp();
    }
}
