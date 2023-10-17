namespace View
{
    public class StartEventArgs : EventArgs
    {
        public string Mapfile { get; }
        public StartEventArgs(string mapfile)
        {
            Mapfile = mapfile;
        }
    }
}