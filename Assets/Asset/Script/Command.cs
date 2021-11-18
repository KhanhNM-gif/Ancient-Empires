using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Asset.Script
{
    static class Command
    {
        public static bool IsNull<T>(this T myObject) where T : class
        {
            if (myObject is UnityEngine.Object obj)
            {
                return false;
            }
            else
            {
                return myObject == null;
            }
        }
    }
}
