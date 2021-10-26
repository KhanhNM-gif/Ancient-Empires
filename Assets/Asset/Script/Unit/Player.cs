using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Player : PlayerHandle
{
    public Player(int gold, int goldPerTurn) : base(gold, goldPerTurn)
    {

    }
    public override void StartTurn()
    {
        base.StartTurn();
        SkipTurn.Instance.btn_SkipTurn.SetActive(true);
    }
}
