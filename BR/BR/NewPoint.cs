using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BR
{
    public struct NewPoint : IComparable<NewPoint>
    {
        public int x;
        public int y;

        public NewPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int CompareTo(NewPoint other)
        {
            if (x < other.x)
                return -1;
            else if (x > other.x)
                return +1;
            else if (y < other.y)
                return -1;
            else if (y > other.y)
                return +1;
            else
                return 0;
        }
    }
}
