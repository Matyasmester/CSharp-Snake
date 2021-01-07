using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Player
    {
        public Direction Direction = Direction.Forward;
        public ConsoleChar Head;
        public List<ConsoleChar> Body = new List<ConsoleChar>();
        public bool IsAlive = true;

        public Player(ConsoleChar Head)
        {
            this.Head = Head;
        }

        public void AddBodyPart(ConsoleChar c)
        {
            Body.Add(c);
        }

        public void Kill()
        {
            IsAlive = false;
        }
    }
}
