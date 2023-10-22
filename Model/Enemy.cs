using System.Runtime.CompilerServices;

namespace Model
{
    public class Enemy : Actor
    {
        private Direction _direction;
        private readonly List<Direction> _tried_directions = new();
        private readonly PlayerCharacter _player;

        private static readonly Random _random = new();
        private static readonly Array _possible_directions = Enum.GetValues(typeof(Direction));

        public Enemy((int, int) pos, Map map, List<Enemy> enemies, PlayerCharacter player, List<Bomb> bombs) : base(pos, map, enemies, bombs)
        {
            _player = player;

            _direction = (Direction)_possible_directions.GetValue(_random.Next(_possible_directions.Length))!;
        }

        private void ChangeDirection()
        {
            // Find new direction not already tried in this step
            _tried_directions.Add(_direction);
            int i = _random.Next(_possible_directions.Length);
            int j = 0;
            do
            {
                _direction = (Direction)_possible_directions.GetValue(i)!;
                i = (i + 1) % _possible_directions.Length;
                j++;
            }
            while (_tried_directions.Contains(_direction) && j < _possible_directions.Length);
        }

        protected override (int, int)? NewPosition(Direction direction)
        {
            (int, int)? new_position = base.NewPosition(direction);
            if (new_position == null) return null;

            // Enemies may not step on each other or bombs
            if (!_enemies.All(e => e.Position != new_position) || !_bombs.All(e => e.Position != new_position)) return null;

            return new_position;
        }

        public void Move()
        {
            // keep moving in the same direction
            (int, int)? new_position = NewPosition(_direction);

            int i = 0;
            bool changed = false;
            // try to find a new position in another direction,
            // if there is no valid position in the current direction
            while (new_position == null && i < 3)
            {
                ChangeDirection();
                changed = true;
                new_position = NewPosition(_direction);
                i++;
            }

            // reset tried directions
            if (changed)
            {
                _tried_directions.Clear();
            }

            // it's possible that none of the four directions is available
            if (new_position == null) return;

            base.Move(((int, int))new_position!);

            // If the player is stepped on it's destroyed
            if (_player.Position == new_position)
            {
                InvokeDestroyed(_player);
            }
        }
    }
}
