using Microsoft.Maui.Graphics;

namespace ViewModel
{
    public class BombViewModel : ViewModelBase
    {
        private readonly int _cc;
        private readonly int _rc;
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
            }
        }
        public int Y
        {
            get => y;
            set
            {
                y = value;
            }
        }
        public int Width
        {
            get => width;
            set
            {
                width = value;
            }
        }
        public int Height
        {
            get => height;
            set
            {
                height = value;
            }
        }

        public Rect LayoutBounds
        {
            get => new(X / (float)(_cc - Width), Y / (float)(_rc - Height), Width / (float)_cc, Height / (float)_rc);
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

        public BombViewModel(int x_, int y_, int ColumnCount, int RowCount)
        {
            _cc = ColumnCount;
            _rc = RowCount;
            Width = 1;
            Height = 1;
            X = x_;
            Y = y_;
            Source = "seismic_charge.png";
            OnPropertyChanged(nameof(LayoutBounds));
        }

        public void Explode()
        {
            X -= 3;
            Y -= 3;
            Width = 7;
            Height = 7;
            Source = "seismic_wave.png";
            OnPropertyChanged(nameof(LayoutBounds));
        }
    }
}