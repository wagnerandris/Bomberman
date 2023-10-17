using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    public abstract class Actor : IGameObject
    {
        private (int, int) position;
        public (int, int) Position { get => position; protected set => position = value; }

        public event EventHandler<ActorMovedEventArgs>? ActorMoved;

        public Actor((int, int) pos)
        {
            position = pos;
        }

        public void Move(Direction direction)
        {
            (int, int) old_pos = position;
            switch (direction)
            {
                case Direction.Left:
                    position.Item1 -= 1;
                    break;
                case Direction.Right:
                    position.Item1 += 1;
                    break;
                case Direction.Up:
                    position.Item2 -= 1;
                    break;
                case Direction.Down:
                    position.Item2 += 1;
                    break;
            }
            ActorMoved?.Invoke(this, new ActorMovedEventArgs(old_pos, position));
        }
    }
}
