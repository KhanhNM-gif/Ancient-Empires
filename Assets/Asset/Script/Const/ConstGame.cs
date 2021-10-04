using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Const
{
    public static class ConstGame
    {
        public const int FRAME = 60;
        public const int MAX_UNIT = 8;
    }
    public static class NameUnit
    {
        public const string BLUE_ARCHER = "BlueArcher";
    }

    public static class Unit
    {
       public static Tuple<int, int>[] STEP_MOVE = new Tuple<int, int>[]
        {
            new Tuple<int, int>(1,0),
            new Tuple<int, int>(-1,0),
            new Tuple<int, int>(0,1),
            new Tuple<int, int>(0,-1),
        };
    }
}
