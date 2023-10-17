using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    internal class Bomb : IGameObject
    {
        private readonly (int, int) position;

        public (int, int) Position { get => position; }

        public Bomb((int, int) pos)
        {
            position = pos;
            _ = new Timer(Explode(), null, 3000, Timeout.Infinite);
        }

        private TimerCallback Explode()
        {
            throw new NotImplementedException();
        }
    }
}
