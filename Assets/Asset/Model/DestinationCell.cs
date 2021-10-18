using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Asset.Model
{
    class DestinationCell : MatrixCoordi
    {
        public DestinationCell(int x, int y, Queue<MatrixCoordi> way, List<Unit> ltUnitCanAttack)
        {
            this.x = x;
            this.y = y;
            this.way = way;
            this.ltUnitCanAttack = ltUnitCanAttack;
        }
        public int x { get; set; }
        public int y { get; set; }
        Queue<MatrixCoordi> way { get; set; }
        public List<Unit> ltUnitCanAttack { get; set; }

    }
}
