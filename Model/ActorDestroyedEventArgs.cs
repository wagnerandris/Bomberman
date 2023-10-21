namespace Model
{
    public class ActorDestroyedEventArgs
    {
        public (int, int) Position { get; }

        public ActorDestroyedEventArgs((int, int) position)
        {
            Position = position;
        }
    }
}