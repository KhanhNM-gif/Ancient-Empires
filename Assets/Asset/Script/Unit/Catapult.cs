using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : Unit
{
    public GameObject rock;
    private void CatapultAttack(Unit unit,float damage)
    {
        GameObject rockClone = Instantiate(rock, firePoint.position, firePoint.rotation);
        RockMove rm = rockClone.GetComponent<RockMove>();
        rm.CreateAttackEffect(unit, damage);
    }
    
    public override void AnimationAttack(Unit unit, float damage)
    {
        CatapultAttack(unit, damage);
    }
}
