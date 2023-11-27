using System.Windows;
using System.Windows.Media.Imaging;

namespace ViewModel
{
    public class BombViewModel : ViewModelBase
    {
        private int x;
        private int y;
        private int width;
        private int height;
        private BitmapImage image;

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
        public BitmapImage Image
        {
            get => image;
            set
            {
                image = value;
                OnPropertyChanged();
            }
        }

        public BombViewModel(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Image = (BitmapImage)Application.Current.Resources["Bomb"];
        }

        public void explode()
        {
            X -= 3 * Width;
            Y -= 3 * Height;
            Width *= 7;
            Height *= 7;
            Image = (BitmapImage)Application.Current.Resources["Explosion"];
        }
    }
}