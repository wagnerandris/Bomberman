namespace ModelTest
{
    [TestClass]
    public class GameTest : IDisposable
    {
        private Game _game = null!;

        [TestInitialize]
        public void GameTestInit()
        {
            bool[,] walls = {
                {false, false, false },
                {false, true, false },
                { false, false, false }
            };
            Map map = new(walls, 3, 3);
            List<(int, int)> enemies_start = new() { (0, 2), (2, 0) };
            (int, int) player_start = (0, 0);

            _game = new Game(map, enemies_start, player_start);
            _game.StartPause();
        }

        [TestCleanup]
        public void GameTestCleanup()
        {
            _game.Dispose();
        }

        int timer_elapsed = 0;

        [TestMethod]
        public async Task TimerTest()
        {
            _game.TimerElapsed += Timer_Elapsed;

            Assert.AreEqual(timer_elapsed, 0);

            _game.StartPause();

            await Task.Delay(1100);

            _game.StartPause();

            Assert.AreEqual(timer_elapsed, 1);

            _game.TimerElapsed -= Timer_Elapsed;
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            timer_elapsed++;
        }

        int enemies_moved = 0;

        [TestMethod]
        public async Task MoveEnemiesTest()
        {
            Actor.Moved += Enemy_Moved;

            _game.StartPause();

            await Task.Delay(1100);

            _game.StartPause();

            Actor.Moved -= Enemy_Moved;

            Assert.AreEqual(2, enemies_moved);
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

        public void Dispose()
        {
            _game?.Dispose();
        }
    }
}