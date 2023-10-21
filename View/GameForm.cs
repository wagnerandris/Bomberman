using Model;
using Persistence;
using View.Properties;

namespace View
{
    public partial class GameForm : Form
    {
        private Game? _game;
        private IMapReader? _mapReader;
        private readonly Random _random = new();
        private readonly Bitmap[] _wall_images = { Resources.asteroid1, Resources.asteroid2, Resources.asteroid3 };

        private bool key_held;

        public GameForm()
        {
            InitializeComponent();

            menu.Start += Menu_Start;
            KeyDown += GameForm_KeyDown;
            KeyUp += GameForm_KeyUp;
            Actor.Moved += Actor_Moved;
            Actor.Destroyed += Actor_Destroyed;
            Bomb.Placed += Bomb_Placed;
            Bomb.Exploded += Bomb_Exploded;
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
            menu.Enabled = false;
            tile_map.Visible = true;

            // Init game
            _game = new Game(_mapReader.Map, _mapReader.Enemies_start, _mapReader.Player_start);
            _game.GameOver += _game_GameOver;
        }

        private void _game_GameOver(object? sender, GameOverEventArgs e)
        {
            // Popup
            throw new NotImplementedException();
        }

        private void Actor_Moved(object? sender, ActorMovedEventArgs e)
        {
            // Bad practice:
            // > It is better to die than to return in failure. -- Klingon proverb
            // > If I had more time, I would have written ~a shorter letter~ better code. -- Blaise Pascal
            if (sender == null) return;

            if (tile_map.InvokeRequired)
            {
                tile_map.Invoke(new Action<object, ActorMovedEventArgs>(Actor_Moved), sender, e);
                return;
            }

            if (sender!.GetType() == typeof(PlayerCharacter))
            {
                tile_map.GetControlFromPosition(((Actor)sender).Position.Item1, ((Actor)sender).Position.Item2).BackgroundImage = Resources.Slave_I;
            }
            else
            {
                tile_map.GetControlFromPosition(((Actor)sender).Position.Item1, ((Actor)sender).Position.Item2).BackgroundImage = Resources.TIE;
            }

            tile_map.GetControlFromPosition(e.Old_pos.Item1, e.Old_pos.Item2).BackgroundImage = null;
        }

        private void Bomb_Placed(object? sender, EventArgs e)
        {
            if (sender == null) return;

            if (tile_map.InvokeRequired)
            {
                tile_map.Invoke(new Action<object, EventArgs>(Bomb_Placed), sender, e);
            }

            tile_map.GetControlFromPosition(((Bomb)sender).Position.Item1, ((Bomb)sender).Position.Item2).BackgroundImage = Resources.seismic_charge;
        }

        private void Bomb_Exploded(object? sender, BombExplodedEventArgs e)
        {
            // TODO: Draw on background instead
            if (sender == null) return;

            if (tile_map.InvokeRequired)
            {
                tile_map.Invoke(new Action<object, BombExplodedEventArgs>(Bomb_Exploded), sender, e);
            }

            tile_map.GetControlFromPosition(e.Position.Item1, e.Position.Item2).BackgroundImage = null;
            // Play explosion animation
        }

        private void Actor_Destroyed(object? sender, ActorDestroyedEventArgs e)
        {
            if (tile_map.InvokeRequired)
            {
                tile_map.Invoke(new Action<object, ActorDestroyedEventArgs>(Actor_Destroyed), sender, e);
            }

            tile_map.GetControlFromPosition(e.Position.Item1, e.Position.Item2).BackgroundImage = null;
        }

        private void GameForm_KeyUp(object? sender, KeyEventArgs e)
        {
            key_held = false;
        }

        private void GameForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (key_held) return;
            key_held = true;

            if (_game == null) return;

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    _game.StartPause();
                    break;
                case Keys.Space:
                    _game.PlaceBomb();
                    break;
                case Keys.Up:
                case Keys.W:
                    _game.MovePlayer(Direction.Up);
                    break;
                case Keys.Down:
                case Keys.S:
                    _game.MovePlayer(Direction.Down);
                    break;
                case Keys.Left:
                case Keys.A:
                    _game.MovePlayer(Direction.Left);
                    break;
                case Keys.Right:
                case Keys.D:
                    _game.MovePlayer(Direction.Right);
                    break;
                default:
                    break;
            }
        }
    }
}