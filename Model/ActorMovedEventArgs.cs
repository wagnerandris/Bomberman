namespace Model
{
    public class ActorMovedEventArgs
    {
        public (int, int) Old_pos { get; }

        public ActorMovedEventArgs((int, int) old_pos)
        {
            Old_pos = old_pos;
        }
    }
}