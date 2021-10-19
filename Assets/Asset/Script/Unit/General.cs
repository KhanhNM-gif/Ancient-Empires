using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class General : Unit
{
    public GameObject attack;

    private void GeneralAttack()
    {
        
        GameObject rockClone = Instantiate(attack, firePoint.position, firePoint.rotation);
        AttackMove rm = rockClone.GetComponent<AttackMove>();
        rm.SetlastClickedPos(new Vector3(Input.mousePosition.x,Input.mousePosition.y,7));
        rm.SetMoving(true);      
    }
    public override void AnimationAttack()
    {
        GeneralAttack();
    }
    
    
    override public void Start()
    {
        base.Update();
        canOccupiedHouse = true;
        canOccupiedCastle = true;
        isGeneral = true;
    }
    override public void Update()
    {
        base.Update();
    }
}
