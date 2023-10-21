namespace Model
{
    public class BombExplodedEventArgs
    {
        public (int, int) Position { get; }
        public int Radius { get; }
        public BombExplodedEventArgs((int, int) position, int radius = 3)
        {
            Position = position;
            Radius = radius;
        }
    }
}