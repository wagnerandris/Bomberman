using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Map
    {
        public int Width { get; }
        public int Height { get; }
        public bool[,] Walls { get; }

        public Map(bool[,] walls, int width, int heigth)
        {
            Walls = walls;
            Width = width;
            Height = heigth;
        }
    }
}
