using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : Unit
{
    public GameObject rock;

    public void Shoot()
    {
        GameObject rockClone = Instantiate(rock, firePoint.position, firePoint.rotation);
        RockMove rm = rockClone.GetComponent<RockMove>();
        rm.SetlastClickedPos(Input.mousePosition);
        rm.SetMoving(true);      
    }
}
