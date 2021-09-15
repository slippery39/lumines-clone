using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Logic
{
    public class Square
    {
        private int x;
        public int X { get { return x; } set { x = value; } }
        private int y;
        public int Y { get { return y; } set { y = value; } }
        private int colorValue; //1 or 2;
        public int Color { get { return colorValue; } set { colorValue = value; } }

        public Square(int x, int y, int color)
        {
            this.x = x;
            this.y = y;
            this.colorValue = color;
        }

    }
}
