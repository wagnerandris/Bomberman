using Model;
using Persistence;
using System.Timers;
using View.Properties;

namespace View
{
    public partial class GameForm : Form
    {
        private Game? _game;
        private readonly Random _random = new();
        private readonly Bitmap[] _wall_images = { Resources.asteroid1, Resources.asteroid2, Resources.asteroid3 };
        private readonly Dictionary<(int, int), PictureBox> _bombs = new();

        private int _tile_width;
        private int _tile_height;

        private int _gametime = 0;
        private int _destroyed_enemies = 0;
        private bool key_held;

        public GameForm()
        {
            InitializeComponent();

            menu.Start += Menu_Start;
            KeyDown += GameForm_KeyDown;
            KeyUp += GameForm_KeyUp;
            Game.GameOver += Game_GameOver;
            Actor.Moved += Actor_Moved;
            Actor.Destroyed += Actor_Destroyed;
            Bomb.Placed += Bomb_Placed;
            Bomb.Exploded += Bomb_Exploded;
        }

        private void Menu_Start(object? sender, StartEventArgs e)
        {
            IMapReader mapReader;
            try
            {
                mapReader = new TXTMapReader(e.Mapfile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mapfile Error", MessageBoxButtons.OK);
                return;
            }

            tile_map.UseWaitCursor = true;

            // Set up LayoutPanel grid
            tile_map.Controls.Clear();
            tile_map.ColumnCount = mapReader.Map.Width;
            tile_map.RowCount = mapReader.Map.Height;

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


            _tile_width = 512 / mapReader.Map.Width;
            _tile_height = 512 / mapReader.Map.Height;


            for (int i = 0; i < mapReader.Map.Width; i++)
            {
                for (int j = 0; j < mapReader.Map.Height; j++)
                {
                    // Populating the map with tiles
                    tile_map.Controls.Add(new Panel()
                    {
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Height = _tile_height,
                        Width = _tile_width,
                        Margin = new Padding(0)
                    },
                        i, j);

                    if (mapReader.Map.Walls[i, j])
                    {
                        tile_map.GetControlFromPosition(i, j).BackgroundImage = _wall_images[_random.Next(3)];
                    }
                }
            }

            // Add enemies
            foreach ((int, int) coords in mapReader.Enemies_start)
            {
                tile_map.GetControlFromPosition(coords.Item1, coords.Item2).BackgroundImage = Resources.TIE;
            }

            // Add player
            tile_map.GetControlFromPosition(mapReader.Player_start.Item1, mapReader.Player_start.Item2).BackgroundImage = Resources.Slave_I;

            // "Switch" to game screen
            menu.Visible = false;
            menu.Enabled = false;
            tile_map.Visible = true;

            // needed to prevent bell sound when pressing space
            // without this, focus remains on the start game button, which gets disabled
            tile_map.Focus();

            // Reset variables representing game specific data
            _destroyed_enemies = 0;
            _gametime = 0;
            _bombs.Clear();

            // Init game
            _game = new Game(mapReader.Map, mapReader.Enemies_start, mapReader.Player_start);
            _game.timer.Elapsed += Timer_Elapsed;

            tile_map.UseWaitCursor = false;
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (status_strip.InvokeRequired)
            {
                status_strip.Invoke(new Action<object, System.Timers.ElapsedEventArgs>(Timer_Elapsed), sender, e);
                return;
            }
            _gametime++;
            time.Text = $"{_gametime / 60}:{_gametime % 60}";
        }

        private void Game_GameOver(object? sender, GameOverEventArgs e)
        {
            if (tile_map.InvokeRequired)
            {
                tile_map.Invoke(new Action<object, GameOverEventArgs>(Game_GameOver), sender, e);
                return;
            }

            // So that if multiple game over signals come i.e.: player and last enemy destoyed at the same time, only the first registeres
            if (_game != null)
            {
                _game.timer.Elapsed -= Timer_Elapsed;
            }

            tile_map.Visible = false;
            foreach (var bomb in _bombs)
            {
                Controls.Remove(bomb.Value);
            }

            MessageBox.Show(String.Format("{0} Game Time: {1} Destryed Enemies: {2}", e.Player_won ? "Victory!" : "Defeat!", _gametime, _destroyed_enemies), "Game Over", MessageBoxButtons.OK);

            _game = null;
            destroyed.Text = "0";
            time.Text = "0:0";

            menu.Visible = true;
            menu.Enabled = true;
            menu.Focus();
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

            (int, int) pos = ((Bomb)sender).Position;

            _bombs.Add(
                pos,
                new PictureBox()
                {
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Location = new Point(pos.Item1 * _tile_width + 1, pos.Item2 * _tile_height + 1),
                    Margin = new Padding(0),
                    BackColor = Color.Transparent,
                    BackgroundImage = null,
                    Size = new Size(_tile_width, _tile_height),
                    Image = Resources.seismic_charge
                }
                );

            Controls.Add(_bombs[pos]);
            _bombs[pos].BringToFront();

            // So that bombs don't explode over status strip
            status_strip.BringToFront();

        }

        private async void Bomb_Exploded(object? sender, BombExplodedEventArgs e)
        {
            // This can only happen if threads are not in sync and a bomb explodes after Game Over
            if (_bombs.Count == 0) return;

            if (InvokeRequired)
            {
                Invoke(new Action<object, BombExplodedEventArgs>(Bomb_Exploded), sender, e);
                return;
            }

            PictureBox bomb = _bombs[e.Position];
            bomb.Visible = false;
            bomb.Left -= _tile_width * 3;
            bomb.Top -= _tile_height * 3;
            bomb.Size = new Size(_tile_width * 7, _tile_height * 7);
            bomb.Image = Resources.seismic_wave;
            bomb.Visible = true;

            await Task.Delay(1000);

            Controls.Remove(_bombs[e.Position]);
            _bombs.Remove(e.Position);
        }

        private void Actor_Destroyed(object? sender, ActorDestroyedEventArgs e)
        {
            if (tile_map.InvokeRequired)
            {
                tile_map.Invoke(new Action<object, ActorDestroyedEventArgs>(Actor_Destroyed), sender, e);
                return;
            }

            tile_map.GetControlFromPosition(e.Position.Item1, e.Position.Item2).BackgroundImage = null;

            if (sender!.GetType() == typeof(Enemy))
            {
                _destroyed_enemies++;
                destroyed.Text = _destroyed_enemies.ToString();
            }

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