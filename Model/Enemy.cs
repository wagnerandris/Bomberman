using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Enemy : Actor
    {
        private Direction direction;
        private readonly Array values;
        private readonly Random random;

        public Enemy((int, int) pos) : base(pos)
        {
            values = Enum.GetValues(typeof(Direction));
            random = new Random();

            direction = (Direction)values.GetValue(random.Next(values.Length))!;

        }

        public void Move()
        {
            (int, int) new_position = Position;

            base.Move(new_position);
        }

    }
}
