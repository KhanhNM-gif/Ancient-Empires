using System;
using System.Collections.Generic;

namespace Assets.Asset.Model
{
    class DestinationCell
    {
        public DestinationCell(int x, int y, Queue<IMatrixCoordi> way, List<Unit> ltUnitCanAttack)
        {
            this.way = way;
            this.ltUnitCanAttack = ltUnitCanAttack;
        }

        Queue<IMatrixCoordi> way { get; set; }
        public List<Unit> ltUnitCanAttack { get; set; }

        public int Distance(IMatrixCoordi mc)
        {
            throw new NotImplementedException();
        }
    }
}
