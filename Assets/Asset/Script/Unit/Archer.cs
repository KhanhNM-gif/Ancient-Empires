using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Archer : Unit
{
    public GameObject arrow;

    private void ArcherAttack(Unit unit, float damage)
    {
        GameObject arrowClone = Instantiate(arrow, firePoint.position, firePoint.rotation);
        ArrowMove af = arrowClone.GetComponent<ArrowMove>();
        af.CreateAttackEffect(unit, damage);
    }
    public override void AnimationAttack(Unit unit, float damage)
    {
        ArcherAttack(unit, damage);
    }
    override public void Update()
    {
        base.Update();
    }
}
