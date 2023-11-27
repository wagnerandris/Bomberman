using System.Timers;

namespace Model
{
    public class Game : IDisposable
    {
        private readonly Map _map;
        private readonly List<Enemy> _enemies;
        private readonly List<Bomb> _bombs;
        private readonly PlayerCharacter _player_character;
        
        private readonly System.Timers.Timer _timer;
        public event ElapsedEventHandler TimerElapsed
        {
            add
            {
                _timer.Elapsed += value;
            }
            remove
            {
                _timer.Elapsed -= value;
            }
        }

        public static event EventHandler<GameOverEventArgs>? GameOver;

        public Game(Map map, List<(int, int)> enemies_start, (int, int) player_start)
        {
            _map = map;
            _enemies = new List<Enemy>();
            _bombs = new List<Bomb>();
            _player_character = new PlayerCharacter(player_start, _map, _enemies, _bombs);
            foreach (var pos in enemies_start)
            {
                _enemies.Add(new Enemy(pos, _map, _enemies, _player_character, _bombs));
            }
            
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += Move_And_Tick;
            _timer.Start();

            Bomb.Exploded += Bomb_Exploded;
            Actor.Destroyed += Actor_Destroyed;
        }

        public void Dispose()
        {
            ((IDisposable)_timer).Dispose();
            Bomb.Exploded -= Bomb_Exploded;
            Actor.Destroyed -= Actor_Destroyed;
        }

        private void SendGameOver(bool player_won)
        {
            _timer.Stop();
            Dispose();
            GameOver?.Invoke(this, new GameOverEventArgs(player_won));
        }

        public void StartPause()
        {
            if (_timer.Enabled)
            {
                _timer.Stop();
            }
            else
            {
                _timer.Start();
            }
        }

        public void MovePlayer(Direction direction)
        {
            if (!_timer.Enabled) return;
            _player_character.Move(direction);
        }

        public void PlaceBomb()
        {
            if (!_timer.Enabled) return;
            _player_character.PlaceBomb();
        }

        private void Move_And_Tick(object? sender, EventArgs e)
        {
            lock (_enemies)
            {
                foreach (Enemy enemy in _enemies) enemy.Move();
            }

            lock (_bombs)
            {
                Bomb[] bombs = _bombs.ToArray();
                for (int i = bombs.Length - 1; i >= 0; i--)
                {
                    bombs[i].Tick();
                }
            }

        }

        private void Bomb_Exploded(object? sender, BombExplodedEventArgs e)
        {
            if (sender == null) return;
            lock (_bombs)
            {
                _bombs.Remove((Bomb)sender);
            }

            if (Math.Abs(e.Position.Item1 - _player_character.Position.Item1) <= e.Radius && Math.Abs(e.Position.Item2 - _player_character.Position.Item2) <= e.Radius)
            {
                SendGameOver(false);
                return;
            }

            var exploded_enemies = _enemies.Select(enemy => enemy).Where
                                    (enemy =>
                                    Math.Abs(e.Position.Item1 - enemy.Position.Item1) <= e.Radius &&
                                    Math.Abs(e.Position.Item2 - enemy.Position.Item2) <= e.Radius
                                    ).ToList();

            lock (_enemies)
            {
                foreach (Enemy enemy in exploded_enemies)
                {
                    enemy.InvokeDestroyed();
                    _enemies.Remove(enemy);
                }
            }
        }

        private void Actor_Destroyed(object? sender, ActorDestroyedEventArgs e)
        {
            if (sender == null) return;

            if (sender!.GetType() == typeof(PlayerCharacter))
            {
                SendGameOver(false);
            }
            else
            {
                lock (_enemies)
                {
                    _enemies.Remove((Enemy)sender);
                }
                if (_enemies.Count == 0)
                {
                    SendGameOver(true);
                }
            }
        }
    }
}