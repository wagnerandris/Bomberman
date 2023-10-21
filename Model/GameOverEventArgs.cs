namespace Model
{
    public class GameOverEventArgs
    {
        public bool Player_won { get; }

        public GameOverEventArgs(bool player_won)
        {
            Player_won = player_won;
        }
    }
}