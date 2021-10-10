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
        public const int COST_ARCHER = 250;
        public const int COST_SOLDIER = 150;
        public const int COST_GENERAL = 1000;
        public const int COST_CATAPULT = 700;
    }
    public static class NameUnit
    {
        public const string BLUE_ARCHER = "BlueArcher";
        public const string BLUE_CATAPULT = "BlueCatapult";
        public const string BLUE_GENERAL = "BlueGeneral";
        public const string BLUE_SOLDIER = "BlueSoldier";

        public const string RED_ARCHER = "RedArcher";
        public const string RED_CATAPULT = "RedCatapult";
        public const string RED_GENERAL = "RedGeneral";
        public const string RED_SOLDIER = "RedSoldier";
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
    public static class PlayerHandle
    {
        public const int NUMBER_UNIT_LIMIT = 8;
    }
}
