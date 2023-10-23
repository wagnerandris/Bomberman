using System.Timers;

namespace Model
{
    public class Bomb
    {
        public (int, int) Position { get; }
        private int _tics;

        public static event EventHandler? Placed;
        public static event EventHandler<BombExplodedEventArgs>? Exploded;


        public Bomb((int, int) pos)
        {
            Position = pos;
            _tics = 0;
            Placed?.Invoke(this, EventArgs.Empty);
        }

        public void Tick()
        {
            if (++_tics == 5) Exploded?.Invoke(this, new BombExplodedEventArgs(Position));
        }
    }
}
