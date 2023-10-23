﻿namespace Model
{
    public abstract class Actor
    {
        protected readonly Map _map;
        protected readonly List<Enemy> _enemies;
        protected readonly List<Bomb> _bombs;

        private (int, int) _position;
        public (int, int) Position { get => _position; protected set => _position = value; }

        public static event EventHandler<ActorMovedEventArgs>? Moved;
        public static event EventHandler<ActorDestroyedEventArgs>? Destroyed;

        protected Actor((int, int) start_position, Map map, List<Enemy> enemies, List<Bomb> bombs)
        {
            _position = start_position;
            _map = map;
            _enemies = enemies;
            _bombs = bombs;

            Bomb.Exploded += Bomb_Exploded;
        }

        protected virtual (int, int)? NewPosition(Direction direction)
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
            Moved?.Invoke(this, new ActorMovedEventArgs(old_position));
        }

        private void Bomb_Exploded(object? sender, BombExplodedEventArgs e)
        {
            if (Math.Abs(e.Position.Item1 - _position.Item1) <= e.Radius && Math.Abs(e.Position.Item2 - _position.Item2) <= e.Radius)
            {
                Destroyed?.Invoke(this, new ActorDestroyedEventArgs(_position));

                // To prevent dying to exposion when already dead, but not yet garbage collected.
                // Not very effective.
                Bomb.Exploded -= Bomb_Exploded;
            }
        }

        protected void InvokeDestroyed(Actor actor)
        {
            Destroyed?.Invoke(actor, new ActorDestroyedEventArgs(_position));
        }
    }
}
