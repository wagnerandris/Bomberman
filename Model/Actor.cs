using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    public abstract class Actor
    {
        protected readonly Map _map;
        protected readonly List<Enemy> _enemies;

        private (int, int) _position;
        public (int, int) Position { get => _position; protected set => _position = value; }

        public static event EventHandler<ActorMovedEventArgs>? ActorMoved;

        public Actor((int, int) start_position, Map map, List<Enemy> enemies)
        {
            _position = start_position;
            _map = map;
            _enemies = enemies;
        }

        public (int, int)? NewPosition(Direction direction)
        {
            (int, int) new_position = Position;
            switch (direction)
            {
                case Direction.Up:
                    if (new_position.Item2 == 0) return null;
                    new_position.Item2 -= 1;
                    break;
                case Direction.Down:
                    if (new_position.Item2 == _map.Height - 1) return null;
                    new_position.Item2 += 1;
                    break;
                case Direction.Left:
                    if (new_position.Item1 == 0) return null;
                    new_position.Item1 -= 1;
                    break;
                case Direction.Right:
                    if (new_position.Item1 == _map.Width - 1) return null;
                    new_position.Item1 += 1;
                    break;
            }

            if (_map.Walls[new_position.Item1, new_position.Item2]) return null;
            return new_position;
        }

        public void Move((int, int) new_position)
        {
            (int, int) old_position = _position;
            _position = new_position;
            ActorMoved?.Invoke(this, new ActorMovedEventArgs(old_position, new_position));
        }
    }
}
