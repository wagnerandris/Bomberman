using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Bomb
    {
        private readonly (int, int) _position;

        public (int, int) Position { get => _position; }

        public Bomb((int, int) pos)
        {
            _position = pos;
            _ = new Timer(Explode(), null, 3000, Timeout.Infinite);
        }

        private TimerCallback Explode()
        {
            throw new NotImplementedException();
        }
    }
}
