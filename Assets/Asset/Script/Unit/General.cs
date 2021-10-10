using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class General : Unit
{
    public override int cost => Const.ConstGame.COST_GENERAL;

    override public void Update()
    {
        base.Update();
    }
}
