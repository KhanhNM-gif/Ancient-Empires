using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : Unit
{
    public RockMove rockmove;
    public GameObject rock;
    public void OnMouseDown()
    {
        rock.SetActive(true);
        rockmove.move();
    }
}
