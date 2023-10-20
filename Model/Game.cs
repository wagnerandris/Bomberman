namespace Model
{
    public class Game : IDisposable
    {
        private readonly Map _map;
        private readonly List<Enemy> _enemies;
        private readonly PlayerCharacter _playerCharacter;
        private readonly List<Bomb> _bombs;
        private readonly System.Timers.Timer _timer;

        public Game(Map map, List<(int, int)> enemies_start, (int, int) player_start)
        {
            _map = map;
            _enemies = new List<Enemy>();
            foreach (var pos in enemies_start)
            {
                _enemies.Add(new Enemy(pos));
            }
            _playerCharacter = new PlayerCharacter(player_start);
            _bombs = new List<Bomb>();
            
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += Move_Enemies;
        }

        public void StartPause()
        {
            if (_timer.Enabled)
            {
                _timer.Stop();
            } else
            {
                _timer.Start();
            }
        }

        public void Dispose()
        {
            ((IDisposable)_timer).Dispose();
        }

        public void MovePlayer(Direction direction)
        {
            (int, int) position = _playerCharacter.Position;
            switch (direction)
            {
                case Direction.Up:
                    if (position.Item2 == 0) return;
                    position.Item2 -= 1;
                    break;
                case Direction.Down:
                    if (position.Item2 == _map.Height - 1) return;
                    position.Item2 += 1;
                    break;
                case Direction.Left:
                    if (position.Item1 == 0) return;
                    position.Item1 -= 1;
                    break;
                case Direction.Right:
                    if (position.Item1 == _map.Width - 1) return;
                    position.Item1 += 1;
                    break;
            }
            if (_map.Walls[position.Item1, position.Item2]) return;

            if (_enemies.All(e => e.Position != position))
            {
                _playerCharacter.Move(position);
            } else
            {
                // invoke game end event
            }
        }

        public void PlaceBomb()
        {
            throw new NotImplementedException();
        }

        private void Move_Enemies(object? sender, EventArgs e)
        {
            foreach (var enemy in _enemies) enemy.Move();
        }
    }
}