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
        GameObject arrowClone = Instantiate(arrow, firePoint.position, firePoint.rotation);
        ArrowMove af = arrowClone.GetComponent<ArrowMove>();
        af.SetlastClickedPos(new Vector3(Input.mousePosition.x,Input.mousePosition.y,7));
        af.SetMoving(true);         
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
