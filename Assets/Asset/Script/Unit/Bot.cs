using Assets.Asset.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Bot : PlayerHandle
{
    public bool rest=true;
    public Bot(int gold, int goldPerTurn) : base(gold, goldPerTurn)
    {

    }

    private void FindWay(List<Unit> units, List<Unit> ltEnemy)
    {
        ConcurrentDictionary<Unit, List<DestinationCell>> wayDictionary = new ConcurrentDictionary<Unit, List<DestinationCell>>();
        foreach (var item in units)
        {
            Unit.InitiateMovePlatesDelegate dlg = delegate (int x, int y, Queue<IMatrixCoordi> way)
            {
                if (wayDictionary[item] == null) wayDictionary[item] = new List<DestinationCell>();

                List<Unit> enemys = new List<Unit>();
                foreach (var item in ltEnemy) if (Math.Abs(x - item.x) + Math.Abs(y - item.y) <= item.Range) enemys.Add(item);

                DestinationCell DestinationCell = new DestinationCell(x, y, way, enemys);

                wayDictionary[item].Add(DestinationCell);
            };

            item.InitiateMovePlates(dlg);
        }
    }
}

public enum Activity
{
    Attack,
    OccupyCitadel
}