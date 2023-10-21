namespace Model
{
    public class Game : IDisposable
    {
        private readonly Map _map;
        private readonly List<Enemy> _enemies;
        private readonly PlayerCharacter _player_character;
        private readonly List<Bomb> _bombs;
        private readonly System.Timers.Timer _timer;

        public Game(Map map, List<(int, int)> enemies_start, (int, int) player_start)
        {
            _map = map;
            _enemies = new List<Enemy>();
            _player_character = new PlayerCharacter(player_start, _map, _enemies);
            _bombs = new List<Bomb>();
            foreach (var pos in enemies_start)
            {
                _enemies.Add(new Enemy(pos, _map, _enemies, _player_character, _bombs));
            }
            
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
            _player_character.Move(direction);
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