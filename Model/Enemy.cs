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
            (int, int) new_pos;

            switch (direction)
            {
                case Direction.Left:
                    new_pos = Position;
                    new_pos.Item1 -= 1;
                    break;
                case Direction.Right:
                    new_pos = Position;
                    new_pos.Item1 += 1;
                    break;
                case Direction.Up:
                    new_pos = Position;
                    new_pos.Item2 -= 1;
                    break;
                case Direction.Down:
                    new_pos = Position;
                    new_pos.Item2 += 1;
                    break;
            }

            //CheckPosition(new_pos);

            base.Move(direction);
        }

    }
}
