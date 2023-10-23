namespace Model
{
    public class Game : IDisposable
    {
        private readonly Map _map;
        private readonly List<Enemy> _enemies;
        private readonly List<Bomb> _bombs;
        private readonly PlayerCharacter _player_character;
        
        public readonly System.Timers.Timer timer;

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
            
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += Move_And_Tick;
            timer.Start();

            Bomb.Exploded += Bomb_Exploded;
            Actor.Destroyed += Actor_Destroyed;
        }

        public void StartPause()
        {
            if (timer.Enabled)
            {
                timer.Stop();
            }
            else
            {
                timer.Start();
            }
        }

        public void Dispose()
        {
            ((IDisposable)timer).Dispose();
            Bomb.Exploded -= Bomb_Exploded;
            Actor.Destroyed -= Actor_Destroyed;
        }

        public void MovePlayer(Direction direction)
        {
            if (!timer.Enabled) return;
            _player_character.Move(direction);
        }

        public void PlaceBomb()
        {
            if (!timer.Enabled) return;
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
        }

        private void SendGameOver(bool player_won)
        {
            timer.Stop();
            Dispose();
            GameOver?.Invoke(this, new GameOverEventArgs(player_won));
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