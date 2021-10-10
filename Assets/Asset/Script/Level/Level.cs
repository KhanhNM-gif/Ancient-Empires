using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    public int level =1;
    private int Exp =0;
    private int ExpRequired = 100;

    // Start is called before the first frame update
    void Start()
    {
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
    }
}
