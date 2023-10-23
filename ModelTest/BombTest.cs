namespace ModelTest
{
    [TestClass]
    public class BombTest
    {
        private bool exploded = false;
        private Bomb _bomb = null!;

        [TestMethod]
        public void ExplosionTest()
        {
            _bomb = new((0, 0));
            Bomb.Exploded += Bomb_Exploded;

            for (int i = 0; i < 4; i++)
            {
                _bomb.Tick();
            }
            Assert.IsFalse(exploded);

            _bomb.Tick();

            Assert.IsTrue(exploded);

            Bomb.Exploded -= Bomb_Exploded;
        }

        private void Bomb_Exploded(object? sender, BombExplodedEventArgs e)
        {
            Assert.IsFalse(exploded);
            Assert.IsNotNull(sender);
            Assert.AreSame(_bomb, (Bomb)sender);
            Assert.AreEqual(_bomb.Position, e.Position);
            
            exploded = true;
        }
    }
}
