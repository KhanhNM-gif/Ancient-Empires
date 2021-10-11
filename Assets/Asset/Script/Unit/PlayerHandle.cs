using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerHandle
{
    public int NumberUnitLimit { get; set; } = Const.PlayerHandle.NUMBER_UNIT_LIMIT; 
    public int NumberUnit { get => arrListUnit.Count; }
    public int Gold { get; set; }
    public int GoldPerTurn { get; set; }
    public List<Unit> arrListUnit = new List<Unit>();
    public PlayerHandle(int gold, int goldPerTurn, List<Unit> arrListUnit)
    {
        Gold = gold;
        GoldPerTurn = goldPerTurn;
        this.arrListUnit = arrListUnit;
    }
    public bool CheckLimitUnit() => NumberUnit < NumberUnitLimit;
    public virtual void StartTurn()
    {
        Gold += GoldPerTurn;
        foreach (var item in arrListUnit)
        {
            item.SetIsAttack(true);
            item.SetIsMove(true);
            item.EnableUnit();
        }
    }
    public virtual void AddUnit(Unit unit)
    {
        arrListUnit.Add(unit);
    }
}
