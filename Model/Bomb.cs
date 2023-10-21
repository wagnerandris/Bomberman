namespace Model
{
    public class Bomb : IDisposable
    {
        private readonly (int, int) _position;
        private readonly System.Timers.Timer _timer;

        public static event EventHandler? Placed;
        public static event EventHandler<BombExplodedEventArgs>? Exploded;

        public (int, int) Position { get => _position; }

        public Bomb((int, int) pos)
        {
            _position = pos;
            Placed?.Invoke(this, EventArgs.Empty);
            _timer = new System.Timers.Timer(10000);
            _timer.Elapsed += Explode;
            _timer.Start();
        }
        public void Dispose()
        {
            _timer.Dispose();
        }

        private void Explode(object? sender, EventArgs e)
        {
            Exploded?.Invoke(this, new BombExplodedEventArgs(_position));
        }
    }
}
