using Model;
using Persistence;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ViewModel
{
    public class GameViewModel : ViewModelBase
    {
        private Game? _game;
        private readonly Random _random = new();
        private readonly string[] _wall_image_names = { "Wall1", "Wall2", "Wall3" };

        public ObservableCollection<BombViewModel> Bombs { get; private set; }
        public ObservableCollection<BitmapImage> Tiles { get; private set; }

        public event EventHandler? GameOver;

        private int _column_count;
        public int ColumnCount
        {
            get => _column_count;
            private set
            {
                _column_count = value;
                OnPropertyChanged();
            }
        }

        private int _row_count;
        public int RowCount
        {
            get => _row_count;
            private set
            {
                _row_count = value;
                OnPropertyChanged();
            }
        }

        private int _game_time = 0;
        public int GameTime
        {
            get => _game_time;
            set
            {
                _game_time = value;
                OnPropertyChanged(nameof(GameTimeString));
            }
        }

        public string GameTimeString
        {
            get => $"{GameTime / 60}:{GameTime % 60}";
        }

        private int _destroyed_enemies = 0;
        public int DestroyedEnemies
        {
            get => _destroyed_enemies;
            set
            {
                _destroyed_enemies = value;
                OnPropertyChanged();
            }
        }

        private bool _key_held;

        private int _tile_width;
        public int TileWidth
        {
            get => _tile_width;
            set
            {
                _tile_width = value;
                OnPropertyChanged();
            }
        }

        private int _tile_height;
        public int TileHeight
        {
            get => _tile_height;
            set
            {
                _tile_height = value;
                OnPropertyChanged();
            }
        }

        // I guess coordinates are switched compared to Windows Forms
        public int IndFromPos((int, int) pos)
        {
            return pos.Item2 * ColumnCount + pos.Item1;
        }

        public GameViewModel()
        {
            Game.GameOver += Game_GameOver;
            Actor.Moved += Actor_Moved;
            Actor.Destroyed += Actor_Destroyed;
            Bomb.Placed += Bomb_Placed;
            Bomb.Exploded += Bomb_Exploded;
            Tiles = new();
            Bombs = new();
        }

        public void Start(string mapfile)
        {
            IMapReader mapReader;
            try
            {
                mapReader = new TXTMapReader(mapfile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Mapfile Error");
                return;
            }

            // Reset variables representing game specific data
            DestroyedEnemies = 0;
            GameTime = 0;

            // Reset collections binded to visuals
            Bombs.Clear();
            Tiles.Clear();

            // Set up tiles
            ColumnCount = mapReader.Map.Width;
            RowCount = mapReader.Map.Height;

            TileWidth = 512 / mapReader.Map.Width;
            TileHeight = 512 / mapReader.Map.Height;


            for (int i = 0; i < mapReader.Map.Width; i++)
            {
                for (int j = 0; j < mapReader.Map.Height; j++)
                {
                    // I guess coordinates are switched compared to Windows Forms
                    Tiles.Add(mapReader.Map.Walls[j, i] ? ((BitmapImage)Application.Current.Resources[_wall_image_names[_random.Next(3)]]) : null!);
                }
            }

            // Add enemies
            foreach ((int, int) coords in mapReader.Enemies_start)
            {
                Tiles[IndFromPos(coords)] = (BitmapImage)Application.Current.Resources["Enemy"];
            }

            // Add player
            Tiles[IndFromPos(mapReader.Player_start)] = (BitmapImage)Application.Current.Resources["Player"];


            // Init game
            _game = new Game(mapReader.Map, mapReader.Enemies_start, mapReader.Player_start);
            _game.TimerElapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            GameTime++;
        }

        private void Game_GameOver(object? sender, GameOverEventArgs e)
        {
            // So that if multiple game over signals come i.e.: player and last enemy destoyed at the same time, only the first registeres
            if (_game != null)
            {
                _game.TimerElapsed -= Timer_Elapsed;
            }

            MessageBox.Show(String.Format("{0} Game Time: {1} Destroyed Enemies: {2}", e.Player_won ? "Victory!" : "Defeat!", GameTime, DestroyedEnemies), "Game Over");

            GameOver.Invoke(this, EventArgs.Empty);

            _game = null;
        }

        private void Actor_Moved(object? sender, ActorMovedEventArgs e)
        {
            // Bad practice:
            // > It is better to die than to return in failure. -- Klingon proverb

            if (sender == null) return;

            BitmapImage image;

            if (sender.GetType() == typeof(PlayerCharacter))
            {
                image = (BitmapImage)Application.Current.Resources["Player"];
            }
            else
            {
                image = (BitmapImage)Application.Current.Resources["Enemy"];
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                Tiles[IndFromPos(((Actor)sender).Position)] = image;
                Tiles[IndFromPos(e.Old_pos)] = null!;
            });
        }

        private void Bomb_Placed(object? sender, EventArgs e)
        {
            if (sender == null) return;

            (int, int) pos = ((Bomb)sender).Position;

            Application.Current.Dispatcher.Invoke(() =>
            {
                Bombs.Add(new BombViewModel(
                    pos.Item1 * TileWidth, pos.Item2 * TileHeight,
                    TileWidth, TileHeight
                    ));
            });
        }

        private async void Bomb_Exploded(object? sender, BombExplodedEventArgs e)
        {
            // This can only happen if threads are not in sync and a bomb explodes after Game Over
            if (Bombs.Count == 0) return;

            int i = 0;
            // > If I had more time, I would have written ~a shorter letter~ better code. -- Blaise Pascal
            while (i < Bombs.Count && (Bombs[i].X != e.Position.Item1 * TileWidth || Bombs[i].Y != e.Position.Item2 * TileHeight)) i++;

            if (i == Bombs.Count) return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                Bombs[i].explode();
            });

            await Task.Delay(1000);

            Application.Current.Dispatcher.Invoke(() =>
            {
                Bombs.RemoveAt(i);
            });
        }

        private void Actor_Destroyed(object? sender, ActorDestroyedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Tiles[IndFromPos(e.Position)] = null!;
            });

            if (sender!.GetType() == typeof(Enemy))
            {
                DestroyedEnemies++;
            }
        }

        public void KeyDown(string e)
        {
            if (_game == null) return;

            switch (e)
            {
                case "Esc":
                    _game.StartPause();
                    break;
                case "Space":
                    _game.PlaceBomb();
                    break;
                case "Up":
                    _game.MovePlayer(Direction.Up);
                    break;
                case "Down":
                    _game.MovePlayer(Direction.Down);
                    break;
                case "Left":
                    _game.MovePlayer(Direction.Left);
                    break;
                case "Right":
                    _game.MovePlayer(Direction.Right);
                    break;
                default:
                    break;
            }
        }
    }
}
