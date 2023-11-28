namespace ModelTest
{
    [TestClass]
    public class GameOverTest : IDisposable
    {
        private Game _game = null!;

        [TestInitialize]
        public void GameTestInit()
        {
            bool[,] walls = {
                {false, false, false, false, false, false }
            };
            Map map = new(walls, 6, 1);
            List<(int, int)> enemies_start = new() { (0, 5) };
            (int, int) player_start = (0, 4);

            _game = new Game(map, enemies_start, player_start);

        }

        [TestCleanup]
        public void GameTestCleanup()
        {
            _game.Dispose();
        }

        // Player Exploded
        [TestMethod]
        public async Task TickBombAndExplodePlayerTets()
        {
            Game.GameOver += PlayerExplodedGameLost;

            _game.PlaceBomb();

            _game.MovePlayer(Direction.Left);

            await Task.Delay(5100);

            Game.GameOver -= PlayerExplodedGameLost;
        }

        private void PlayerExplodedGameLost(object? sender, GameOverEventArgs e)
        {
            Assert.AreEqual(false, e.Player_won);
        }

        // Enemy explodes
        [TestMethod]
        public async Task TickBombAndExplodeEnemyTets()
        {
            Game.GameOver += EnemyExplodedGameWon;

            _game.PlaceBomb();

            for (int i = 0; i < 4; i++)
            {
                _game.MovePlayer(Direction.Left);
            }

            await Task.Delay(5100);

            Game.GameOver -= EnemyExplodedGameWon;
        }

        private void EnemyExplodedGameWon(object? sender, GameOverEventArgs e)
        {
            Assert.AreEqual(true, e.Player_won);
        }

        // Explode at once
        [TestMethod]
        public async Task TickBombAndExplodeEnemyAndPlayerTets()
        {
            Game.GameOver += BothExplodedGameLost;

            _game.PlaceBomb();

            for (int i = 0; i < 3; i++)
            {
                _game.MovePlayer(Direction.Left);
            }

            await Task.Delay(5100);

            Game.GameOver -= BothExplodedGameLost;
        }

        private void BothExplodedGameLost(object? sender, GameOverEventArgs e)
        {
            Assert.AreEqual(false, e.Player_won);
        }


        [TestMethod]
        public void PlayerDestroyedTets()
        {
            Game.GameOver += PlayerDestroyedGameLost;

            _game.MovePlayer(Direction.Right);

            Game.GameOver -= PlayerDestroyedGameLost;
        }

        private void PlayerDestroyedGameLost(object? sender, GameOverEventArgs e)
        {
            Assert.AreEqual(false, e.Player_won);
        }


        public void Dispose()
        {
            _game?.Dispose();
        }
    }
}