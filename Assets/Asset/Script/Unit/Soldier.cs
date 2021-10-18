using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Soldier : Unit
{

    override public void Start()
    {
        base.Update();
        canOccupiedHouse = true;
    }

    override public void Update()
    {
        base.Update();
    }
}
