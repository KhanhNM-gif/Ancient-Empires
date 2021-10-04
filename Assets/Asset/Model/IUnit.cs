using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Asset.Model
{
    interface IUnit
    {
        float CurrentHP { get; set; }
        float HP { get; set; }
        float Attack { get; set; }
        float Armor { get; set; }
        int Range { get; set; }
        int Move { get; set; }
    }
}
