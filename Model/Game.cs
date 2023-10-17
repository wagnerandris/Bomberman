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
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += Move_Enemies;
            _bombs = new List<Bomb>();
        }

        public void Dispose()
        {
            ((IDisposable)_timer).Dispose();
        }

        /*
        public bool CheckPosition((int, int) position)
        {
        }
        */
        private void Move_Enemies(object? sender, EventArgs e)
        {
            foreach (var enemy in _enemies) enemy.Move();
        }
    }
}