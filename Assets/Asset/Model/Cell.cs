using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Asset.Model
{
    public class Cell
    {
        public Cell(bool isCome, int x, int y)
        {
            this.isCome = isCome;
            this.x = y;
            this.y = x;
            //Width = width;
            //Height = height;
        }

        public bool isCome { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int ammo { get; set; }
        //public float Width { get; set; }
        //public float Height { get; set; }

    }
}
