using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ViewModel
{
    public class MenuViewModel : ViewModelBase
    {


        public event EventHandler<string>? Start;
        private readonly string[] _mapfiles = { "maps/map1.txt", "maps/map2.txt", "maps/map3.txt" };
        private readonly string[] _map_pictures = { "Map1", "Map2", "Map3" };

        private int _map_index;
        public int MapIndex
        {
            get => _map_index;
            set
            {
                _map_index = value;
                OnPropertyChanged(nameof(MapImage));
            }
        }

        public BitmapImage MapImage
        {
            get
            {
                return (BitmapImage)Application.Current.Resources[_map_pictures[_map_index]];
            }
        }

        public DelegateCommand StartCommand { get; set; }
        public DelegateCommand PrevMapCommand { get; set; }
        public DelegateCommand NextMapCommand { get; set; }

        public MenuViewModel()
        {
            MapIndex = 0;

            StartCommand = new DelegateCommand (_ =>
                Start?.Invoke(this, _mapfiles[MapIndex])
            );

            PrevMapCommand = new DelegateCommand (_ =>
            {
                // apparently % returns a negative value for negative first parameters
                if (MapIndex > 0) MapIndex--;
                else MapIndex = _mapfiles.Length - 1;
            }
            );

            NextMapCommand = new DelegateCommand (_ =>
                MapIndex = (MapIndex + 1) % _mapfiles.Length
            );
        }
    }
}
