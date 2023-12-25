namespace ViewModel
{
    public class TileViewModel : ViewModelBase
    {
        private int x;
        private int y;
        private string? image;

        //TODO explosion
        public int X
        {
            get => x;
            set
            {
                x = value;
                OnPropertyChanged();
            }
        }
        public int Y
        {
            get => y;
            set
            {
                y = value;
                OnPropertyChanged();
            }
        }
        public string? Source
        {
            get => image;
            set
            {
                image = value;
                OnPropertyChanged();
            }
        }

        public TileViewModel(int x, int y, string source = "")
        {
            X = x;
            Y = y;
            Source = source;
        }
    }
}