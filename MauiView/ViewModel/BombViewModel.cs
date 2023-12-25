namespace ViewModel
{
    public class BombViewModel : ViewModelBase
    {
        private int x;
        private int y;
        private int width;
        private int height;
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
        public int Width
        {
            get => width;
            set
            {
                width = value;
                OnPropertyChanged();
            }
        }
        public int Height
        {
            get => height;
            set
            {
                height = value;
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

        public BombViewModel(int x, int y)
        {
            X = x;
            Y = y;
            Width = 1;
            Height = 1;
            Source = "Bomb";
        }

        public void Explode()
        {
            //X -= 3;
            //Y -= 3;
            Width = 7;
            Height = 7;
            Source = "Explosion";
        }
    }
}