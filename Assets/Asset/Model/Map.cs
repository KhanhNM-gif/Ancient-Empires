using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Asset.Model
{
    public class Map
    {
        public Cell[,] arrCell { get; set; }
        public int Size { get; set; }
        public Map(int size, string filepath)
        {
            Size = size;
            arrCell = new Cell[size, size];
            ReadFile(filepath);
        }

        public void ReadFile(string filepath)
        {
            int i = 0;
            foreach (var s in File.ReadLines(filepath))
            {
                string[] str = s.Trim().Split(' ');
                if (str.Count() > 0)
                {
                    for (int j = 0; j < str.Count(); j++)
                    {
                        arrCell[j, i] = new Cell(str[j].Equals("1") ? true : false, i, j);
                    }

                }
                i++;
            }
        }
        public static Vector3 GridWordPosition(int x, int y, int z = 0)
        {
            return new Vector3(x - 0.5f, 8 - 1.5f - y, z);
        }
    }
}
