using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Archer : Unit
{
    public GameObject arrow;

    private void ArcherAttack()
    {
        
        GameObject rockClone = Instantiate(arrow, firePoint.position, firePoint.rotation);
        RockMove rm = rockClone.GetComponent<RockMove>();
        rm.SetlastClickedPos(new Vector3(Input.mousePosition.x,Input.mousePosition.y,7));
        rm.SetMoving(true);      
    }
    public override void AnimationAttack()
    {
        ArcherAttack();
    }
    override public void Update()
    {
        base.Update();
    }
}
