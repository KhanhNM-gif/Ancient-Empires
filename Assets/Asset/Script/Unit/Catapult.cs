using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : Unit
{
    public GameObject rock;

    private void Shoot()
    {
        
        GameObject rockClone = Instantiate(rock, firePoint.position, firePoint.rotation);
        RockMove rm = rockClone.GetComponent<RockMove>();
        rm.SetlastClickedPos(new Vector3(Input.mousePosition.x,Input.mousePosition.y,7));
        rm.SetMoving(true);      
    }
    public override void AnimationAttack()
    {
        Shoot();
    }
}
