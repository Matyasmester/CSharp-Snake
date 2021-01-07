using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class ConsoleChar
    {
        public char value;
        public int x;
        public int y;

        public ConsoleChar(char value, int x, int y)
        {
            this.value = value;
            this.x = x;
            this.y = y;
        }

        public void SetCoordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
