using Model;
using Persistence;
using View.Properties;

namespace View
{
    public partial class GameForm : Form
    {
        private Game _game = null!;
        private IMapReader? _mapReader;
        private readonly Random _random = new Random();
        private readonly Bitmap[] _wall_images = { Resources.asteroid1, Resources.asteroid2, Resources.asteroid3 };

        public GameForm()
        {
            InitializeComponent();

            menu.Start += Menu_Start;
        }

        private void Menu_Start(object? sender, StartEventArgs e)
        {
            try
            {
                _mapReader = new TXTMapReader(e.Mapfile);
            } catch (Exception)
            {
                // Popup window
                return;
            }

            // Set up LayoutPanel grid
            tile_map.Controls.Clear();
            tile_map.ColumnCount = _mapReader.Map.Width;
            tile_map.RowCount = _mapReader.Map.Height;

            tile_map.RowStyles.Clear();
            tile_map.ColumnStyles.Clear();

            for (Int32 i = 0; i < tile_map.RowCount; i++)
            {
                tile_map.RowStyles.Add(new RowStyle(SizeType.Percent, 1 / Convert.ToSingle(tile_map.RowCount)));
            }
            for (Int32 j = 0; j < tile_map.ColumnCount; j++)
            {
                tile_map.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1 / Convert.ToSingle(tile_map.ColumnCount)));
            }


            Tile.TileHeight = 512 / _mapReader.Map.Height;
            Tile.TileWidth = 512 / _mapReader.Map.Width;

            // Add walls
            for (int i = 0; i <  _mapReader.Map.Width; i++)
            {
                for (int j = 0; j < _mapReader.Map.Height; j++)
                {
                    if (_mapReader.Map.Walls[i, j])
                    {
                        tile_map.Controls.Add(new Tile(_wall_images[_random.Next(_wall_images.Length)]), i, j);
                    }
                }
            }

            // Add enemies
            foreach ((int, int) coords in _mapReader.Enemies_start)
            {
                tile_map.Controls.Add(new Tile(Resources.TIE), coords.Item1, coords.Item2);
            }

            // Add player
            tile_map.Controls.Add(new Tile(Resources.Slave_I), _mapReader.Player_start.Item1, _mapReader.Player_start.Item2);
            
            // "Switch" to game screen
            menu.Visible = false;
            tile_map.Visible = true;

            // Init game
            _game = new Game(_mapReader.Map, _mapReader.Enemies_start, _mapReader.Player_start);
        }
    }
}