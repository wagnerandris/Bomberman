namespace ViewModel
{
    public class MenuViewModel : ViewModelBase
    {


        public event EventHandler<string>? Start;
        private readonly string[] _mapfiles = { "map1.txt", "map2.txt", "map3.txt" };
        private readonly string[] _map_pictures = { "map1.jpg", "map2.jpg", "map3.jpg" };

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

        public string MapImage
        {
            get
            {
                return _map_pictures[_map_index];
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
