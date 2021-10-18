using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class General : Unit
{


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
