namespace ModelTest
{
    [TestClass]
    public class PlayerCharacterTest
    {
        private readonly bool[,] walls = {
            // coordinates are (x,y), sot his is visually transposed
            { false, false, false }, // first column x = 0
            { true,  false, true  },
            { false, false, true  },
        };
        private readonly Map _map;
        private List<Enemy> _enemies = null!;
        private List<Bomb> _bombs = null!;
        private PlayerCharacter _player = null!;

        public PlayerCharacterTest()
        {
            _map = new(walls, 3, 3);
        }

        [TestInitialize]
        public void Init()
        {
            _enemies = new List<Enemy>();
            _bombs = new List<Bomb>();
            _player = new PlayerCharacter((0, 0), _map, _enemies, _bombs);
        }


        [TestMethod]
        public void MoveTest()
        {
            Assert.AreEqual((0, 0), _player.Position);

            _player.Move(Direction.Down);
            Assert.AreEqual((0, 1), _player.Position);

            _player.Move(Direction.Right);
            Assert.AreEqual((1, 1), _player.Position);

            _player.Move(Direction.Left);
            Assert.AreEqual((0, 1), _player.Position);

            _player.Move(Direction.Up);
            Assert.AreEqual((0, 0), _player.Position);
        }

        [TestMethod]
        public void MapBoundryTest()
        {
            _player.Move(Direction.Up);
            Assert.AreEqual((0, 0), _player.Position);

            _player.Move(Direction.Left);
            Assert.AreEqual((0, 0), _player.Position);

            _player = new PlayerCharacter((2, 2), _map, _enemies, _bombs);

            _player.Move(Direction.Down);
            Assert.AreEqual((2, 2), _player.Position);

            _player.Move(Direction.Right);
            Assert.AreEqual((2, 2), _player.Position);
        }

        [TestMethod]
        public void WallTest()
        {
            _player.Move(Direction.Right);
            Assert.AreEqual((0, 0), _player.Position);

            _player = new PlayerCharacter((1, 1), _map, _enemies, _bombs);

            _player.Move(Direction.Up);
            Assert.AreEqual((1, 1), _player.Position);

            _player.Move(Direction.Down);
            Assert.AreEqual((1, 1), _player.Position);

            _player = new PlayerCharacter((2, 2), _map, _enemies, _bombs);

            _player.Move(Direction.Left);
            Assert.AreEqual((2, 2), _player.Position);
        }

        [TestMethod]
        public void PlaceBombTest()
        {
            _player.PlaceBomb();

            Assert.AreEqual(1, _bombs.Count);
            Assert.AreEqual(_player.Position, _bombs[0].Position);
        }

        [TestMethod]
        public void ExplodedTest()
        {
            Actor.Destroyed += Actor_Exploded;

            Bomb bomb = new((0, 1));

            for (int i = 0; i < 5; i++)
            {
                bomb.Tick();
            }

            Actor.Destroyed -= Actor_Exploded;
        }

        private void Actor_Exploded(object? sender, ActorDestroyedEventArgs e)
        {
            Assert.IsNotNull(sender);
            Assert.AreSame(_player, (PlayerCharacter)sender);
            Assert.AreEqual(_player.Position, e.Position);
        }

        [TestMethod]
        public void EnemyCollisionTest()
        {
            _enemies.Add(new Enemy((0, 1), _map, _enemies, _player, _bombs));
            Actor.Destroyed += Player_Destroyed;

            _player.Move(Direction.Right);

            Actor.Destroyed -= Player_Destroyed;
        }

        private void Player_Destroyed(object? sender, ActorDestroyedEventArgs e)
        {
            Assert.IsNotNull(sender);
            Assert.AreSame(_player, (PlayerCharacter)sender);
            Assert.AreEqual(_player.Position, e.Position);
        }
    }
}
