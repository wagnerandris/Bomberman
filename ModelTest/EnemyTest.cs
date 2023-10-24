namespace ModelTest
{
    [TestClass]
    public class EnemyTest
    {
        private readonly bool[,] walls = {
            { false, false, false },
            { true,  false, true  },
            { false, false, true  },
        };
        private readonly Map _map;
        private List<Enemy> _enemies = null!;
        private List<Bomb> _bombs = null!;
        private PlayerCharacter _player = null!;
        private Enemy _enemy = null!;

        public EnemyTest()
        {
            _map = new(walls, 3, 3);
        }


        [TestInitialize]
        public void Init()
        {
            _enemies = new List<Enemy>();
            _bombs = new List<Bomb>();
            _player = new PlayerCharacter((1, 10), _map, _enemies, _bombs);
            _enemy = new((0, 0), _map, _enemies, _player, _bombs);
        }

        [TestMethod]
        public void MoveTest()
        {
            Assert.AreEqual((0, 0), _enemy.Position);

            _enemy.Move();

            Assert.AreEqual((0, 1), _enemy.Position);

            _enemy.Move();

            Assert.AreEqual((0, 2), _enemy.Position);

            _enemy.Move();

            Assert.AreEqual((0, 1), _enemy.Position);
        }

        [TestMethod]
        public void AvoidBombTest()
        {
            _bombs.Add(new Bomb((0, 2)));

            _enemy.Move();
            _enemy.Move();

            Assert.AreNotEqual((0, 2), _enemy.Position);
        }

        [TestMethod]
        public void AvoidEnemyTest()
        {
            _enemies.Add( new Enemy((0, 2), _map, _enemies, _player, _bombs) );

            _enemy.Move();
            _enemy.Move();

            Assert.AreNotEqual((0, 2), _enemy.Position);
        }

        [TestMethod]
        public void StuckTest()
        {
            _bombs.Add(new Bomb((0, 1)));

            _enemy.Move();

            Assert.AreEqual((0, 0), _enemy.Position);

            _bombs.Clear();

            _enemy.Move();

            Assert.AreEqual((0, 1), _enemy.Position);
        }

        [TestMethod]
        public void DestroyPlayerTest()
        {
            _player = new PlayerCharacter((0, 1), _map, _enemies, _bombs);
            Actor.Destroyed += Player_Destroyed;

            _enemy.Move();

            Actor.Destroyed -= Player_Destroyed;

            Assert.AreEqual((0,1), _enemy.Position);
        }

        private void Player_Destroyed(object? sender, ActorDestroyedEventArgs e)
        {
            Assert.IsNotNull(sender);
            Assert.AreSame(_player, (PlayerCharacter)sender);
            Assert.AreEqual(_player.Position, e.Position);
        }
    }
}
