using Model;
using Persistence;
using View.Properties;

namespace View
{
    public partial class GameForm : Form
    {
        private Game? _game;
        private IMapReader? _mapReader;
        private readonly Random _random = new Random();
        private readonly Bitmap[] _wall_images = { Resources.asteroid1, Resources.asteroid2, Resources.asteroid3 };

        public GameForm()
        {
            InitializeComponent();

            menu.Start += Menu_Start;
            Actor.ActorMoved += Actor_ActorMoved;
        }

        private void Menu_Start(object? sender, StartEventArgs e)
        {
            try
            {
                _mapReader = new TXTMapReader(e.Mapfile);
            }
            catch (Exception)
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


            int TileHeight = 512 / _mapReader.Map.Height;
            int TileWidth = 512 / _mapReader.Map.Width;

            for (int i = 0; i < _mapReader.Map.Width; i++)
            {
                for (int j = 0; j < _mapReader.Map.Height; j++)
                {
                    // Populating the map with tiles
                    tile_map.Controls.Add(new Panel()
                        {
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Height = TileHeight,
                        Width = TileWidth,
                        Margin = new Padding(0)
                        },
                        i, j);

                    if (_mapReader.Map.Walls[i, j])
                    {
                        tile_map.GetControlFromPosition(i, j).BackgroundImage = _wall_images[_random.Next(3)];
                    }
                }
            }

            // Add enemies
            foreach ((int, int) coords in _mapReader.Enemies_start)
            {
                tile_map.GetControlFromPosition(coords.Item1, coords.Item2).BackgroundImage = Resources.TIE;
            }

            // Add player
            tile_map.GetControlFromPosition(_mapReader.Player_start.Item1, _mapReader.Player_start.Item2).BackgroundImage = Resources.Slave_I;

            // "Switch" to game screen
            menu.Visible = false;
            tile_map.Visible = true;

            // Init game
            _game = new Game(_mapReader.Map, _mapReader.Enemies_start, _mapReader.Player_start);
        }

        private void Actor_ActorMoved(object? sender, ActorMovedEventArgs e)
        {
            if (tile_map.InvokeRequired)
            {
                tile_map.Invoke(new Action<object?, ActorMovedEventArgs>(Actor_ActorMoved), sender, e);
                return;
            }

            Control old_tile = tile_map.GetControlFromPosition(e.Old_pos.Item1, e.Old_pos.Item2);
            tile_map.GetControlFromPosition(e.New_pos.Item1, e.New_pos.Item2).BackgroundImage = old_tile.BackgroundImage;
            old_tile.BackgroundImage = null;
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_game == null) return false;
            switch (keyData)
            {
                case Keys.Escape:
                    _game.StartPause();
                    break;
                case Keys.Space:
                    _game.PlaceBomb();
                    break;
                case Keys.Up:
                    _game.MovePlayer(Direction.Up);
                    break;
                case Keys.Down:
                    _game.MovePlayer(Direction.Down);
                    break;
                case Keys.Left:
                    _game.MovePlayer(Direction.Left);
                    break;
                case Keys.Right:
                    _game.MovePlayer(Direction.Right);
                    break;
                default:
                    break;
            }
            return true;
        }
    }
}