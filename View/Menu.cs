using View.Properties;

namespace View
{
    public partial class Menu : UserControl
    {
        public event EventHandler<StartEventArgs>? Start;
        private readonly string[] _mapfiles = { "maps/map1.txt", "maps/map2.txt", "maps/map3.txt" };
        private readonly Bitmap[] _map_pictures = { Resources.map1, Resources.map2, Resources.map3 };

        private int _map_index;

        public Menu()
        {
            InitializeComponent();
            _map_index = 0;
            map_picture.Image = _map_pictures[_map_index];
            // start_button.Font = new Font(); // change to Star Wars font
        }
        private void start_button_Click(object sender, EventArgs e)
        {
            Start?.Invoke(sender, new StartEventArgs(_mapfiles[_map_index]));
        }

        private void prev_map_Click(object sender, EventArgs e)
        {
            // apparently % returns a negative value for negative first parameters
            if (_map_index > 0)
            {
                _map_index--;
            }
            else
            {
                _map_index = _mapfiles.Length - 1;
            }
            map_picture.Image = _map_pictures[_map_index];
        }

        private void next_map_Click(object sender, EventArgs e)
        {
            _map_index = (_map_index + 1) % _mapfiles.Length;
            map_picture.Image = _map_pictures[_map_index];
        }
    }
}
