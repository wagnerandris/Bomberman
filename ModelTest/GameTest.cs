namespace ModelTest
{
    [TestClass]
    public class GameTest : IDisposable
    {
        private Game _game = null!;
        private readonly bool[,] _walls = {
            {false, false, false, false, false, false, false, false},
            {false, false, false, true,  false, true,  false, false},
            {false, false, true,  false, false, false, true,  false},
            {false, true,  false, false, true,  false, false, false},
            {false, false, false, true,  false, false, true,  false},
            {false, true,  false, false, false, true,  false, false},
            {false, false, true,  false, true,  false, false, false},
            {false, false, false, false, false, false, false, false}
            };

        [TestInitialize]
        public void GameTestInit()
        {
            Map _map = new(_walls, 8, 8);
            List<(int, int)> enemies_start = new() { (3, 3), (4, 4), (6, 6) };
            (int, int) player_start = (0, 0);

            _game = new Game(_map, enemies_start, player_start);
            _game.timer.Enabled = false;
        }

        [TestCleanup]
        public void GameTestCleanup()
        {
            _game.Dispose();
        }

        [TestMethod]
        public void StartPauseTest()
        {
            Assert.IsFalse(_game.timer.Enabled);

            _game.StartPause();
            Assert.IsTrue(_game.timer.Enabled);

            _game.StartPause();
            Assert.IsFalse(_game.timer.Enabled);
        }

        [TestMethod]
        public async Task TimerTest()
        {
            Assert.AreEqual(1000, _game.timer.Interval);

            _game.timer.Elapsed += Timer_Elapsed;

            _game.timer.Start();

            await Task.Delay(1000);

            _game.timer.Stop();
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Assert.AreEqual(sender, _game.timer);
        }

        int enemies_moved = 0;

        [TestMethod]
        public async Task MoveEnemiesTest()
        {
            Actor.Moved += Enemy_Moved;

            _game.timer.Start();

            await Task.Delay(1500);

            _game.timer.Stop();

            Actor.Moved -= Enemy_Moved;

            Assert.AreEqual(3, enemies_moved);
        }

        private void Enemy_Moved(object? sender, ActorMovedEventArgs e)
        {
            Assert.IsNotNull(sender);
            Assert.IsInstanceOfType(sender, typeof(Enemy));
            enemies_moved++;
        }

        [TestMethod]
        public void MovePlayerTest()
        {
            Actor.Moved += Player_Moved;

            _game.MovePlayer(Direction.Right);

            Actor.Moved -= Player_Moved;
        }

        private void Player_Moved(object? sender, ActorMovedEventArgs e)
        {
            Assert.IsNotNull(sender);
            Assert.IsInstanceOfType(sender, typeof(PlayerCharacter));
            Assert.AreEqual((0, 1), ((PlayerCharacter)sender).Position);
            Assert.AreEqual((0, 0), e.Old_pos);
        }

        [TestMethod]
        public void PlaceBombTest()
        {
            Bomb.Placed += Bomb_Placed;

            _game.PlaceBomb();

            Bomb.Placed -= Bomb_Placed;
        }

        private void Bomb_Placed(object? sender, EventArgs e)
        {
            Assert.IsNotNull(sender);
            Assert.IsInstanceOfType(sender, typeof(Bomb));
            Assert.AreEqual((0, 0), ((Bomb)sender).Position);
        }

        [TestMethod]
        public async Task TickBombAndExplodePlayerTets()
        {
            _game.PlaceBomb();
            _game.timer.Interval = 100;
            Game.GameOver += GameLost;

            _game.timer.Start();

            await Task.Delay(500);

            _game.timer.Stop();

            Game.GameOver -= GameLost;
        }

        private void GameLost(object? sender, GameOverEventArgs e)
        {
            Assert.AreEqual(false, e.Player_won);
        }

        public void Dispose()
        {
            _game?.Dispose();
        }
    }
}