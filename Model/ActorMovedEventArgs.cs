namespace Model
{
    public class ActorMovedEventArgs
    {
        public (int, int) Old_pos { get; }
        public (int, int) New_pos { get; }

        public ActorMovedEventArgs((int, int) old_pos, (int, int) new_pos)
        {
            Old_pos = old_pos;
            New_pos = new_pos;
        }
    }
}