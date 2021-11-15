using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class General : Unit
{
    public GameObject attack;
    private void GeneralAttack(Unit unit, float damage)
    {
        GameObject attackClone = Instantiate(attack, firePoint.position, firePoint.rotation);
        AttackMove am = attackClone.GetComponent<AttackMove>();
        am.CreateAttackEffect(unit, damage);
    }
    public override void AnimationAttack(Unit unit, float damage)
    {
        GeneralAttack(unit, damage);
    }


    override public void Start()
    {
        base.Start();
        canOccupiedHouse = true;
        canOccupiedCastle = true;
        isGeneral = true;
    }
    override public void Update()
    {
        base.Update();
    }
}
