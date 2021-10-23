using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Soldier : Unit
{
    public GameObject attack;

    private void SoldierAttack()
    {
        GameObject attackClone = Instantiate(attack, firePoint.position, firePoint.rotation);
        AttackMove rm = attackClone.GetComponent<AttackMove>();
        rm.SetlastClickedPos(new Vector3(Input.mousePosition.x,Input.mousePosition.y,7));
        rm.SetMoving(true);      
    }
    public override void AnimationAttack()
    {
        SoldierAttack();
    }
    
    override public void Start()
    {
        canOccupiedHouse = true;
    }
    
    override public void Update()
    { 
        base.Update();
    }
    


    

}
