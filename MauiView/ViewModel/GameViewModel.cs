using Model;
using Persistence;
using System.Collections.ObjectModel;

namespace ViewModel
{
    public class GameViewModel : ViewModelBase, IDisposable
    {
        private Game? _game;
        private readonly Random _random = new();
        private readonly string[] _wall_image_names = { "asteroid1.png", "asteroid2.png", "asteroid3.png" };

        public bool Player_won { get; private set; }

        public ObservableCollection<BombViewModel> Bombs { get; private set; }
        public ObservableCollection<TileViewModel> Tiles { get; private set; }

        public event EventHandler? GameOver;

        private int _column_count;
        public int ColumnCount
        {
            get => _column_count;
            private set
            {
                _column_count = value;
                OnPropertyChanged(nameof(GameTableColumns));
            }
        }
        public ColumnDefinitionCollection GameTableColumns
        {
            get => new ColumnDefinitionCollection(Enumerable.Repeat(new ColumnDefinition(GridLength.Star), ColumnCount).ToArray());
        }

        private int _row_count;
        public int RowCount
        {
            get => _row_count;
            private set
            {
                _row_count = value;
                OnPropertyChanged(nameof(GameTableRows));
            }
        }

        public RowDefinitionCollection GameTableRows
        {
            get => new RowDefinitionCollection(Enumerable.Repeat(new RowDefinition(GridLength.Star), RowCount).ToArray());
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

        public int IndFromPos((int, int) pos)
        {
            return pos.Item1 * ColumnCount + pos.Item2;
        }

        public DelegateCommand KeyPressCommand { get; set; }

        public GameViewModel()
        {
            Game.GameOver += Game_GameOver;
            Actor.Moved += Actor_Moved;
            Actor.Destroyed += Actor_Destroyed;
            Bomb.Placed += Bomb_Placed;
            Bomb.Exploded += Bomb_Exploded;
            KeyPressCommand = new DelegateCommand(e => KeyDown((string)e!));
            Tiles = new();
            Bombs = new();
        }

        public void Start(string mapfile)
        {
            IMapReader mapReader = new StringMapReader(mapfile);

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
                    Tiles.Add(new TileViewModel(j, i, mapReader.Map.Walls[i, j] ? _wall_image_names[_random.Next(3)] : ""));
                }
            }

            // Add enemies
            foreach ((int, int) coords in mapReader.Enemies_start)
            {
                Tiles[IndFromPos(coords)].Source = "tie.png";
            }

            // Add player
            Tiles[IndFromPos(mapReader.Player_start)].Source = "slave.png";


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

            Player_won = e.Player_won;

            GameOver!.Invoke(this, EventArgs.Empty);

            _game = null;
        }

        private void Actor_Moved(object? sender, ActorMovedEventArgs e)
        {
            // Bad practice:
            // > It is better to die than to return in failure. -- Klingon proverb

            if (sender == null) return;

            Application.Current!.Dispatcher.Dispatch(() =>
            {
                Tiles[IndFromPos(((Actor)sender).Position)].Source = sender.GetType() == typeof(PlayerCharacter) ? "slave.png" : "tie.png";
                Tiles[IndFromPos(e.Old_pos)].Source = "";
            });
        }

        private void Bomb_Placed(object? sender, EventArgs e)
        {
            if (sender == null) return;

            (int, int) pos = ((Bomb)sender).Position;

            Application.Current!.Dispatcher.Dispatch(() =>
            {
                Bombs.Add(new BombViewModel(pos.Item2, pos.Item1));
            });
        }

        private async void Bomb_Exploded(object? sender, BombExplodedEventArgs e)
        {
            // This can only happen if threads are not in sync and a bomb explodes after Game Over
            if (Bombs.Count == 0) return;

            int i = 0;
            // > If I had more time, I would have written ~a shorter letter~ better code. -- Blaise Pascal
            while (i < Bombs.Count && (Bombs[i].X != e.Position.Item2 || Bombs[i].Y != e.Position.Item1)) i++;

            if (i == Bombs.Count) return;

            Application.Current!.Dispatcher.Dispatch(() =>
            {
                Bombs[i].Explode();
            });

            await Task.Delay(1000);

            Application.Current.Dispatcher.Dispatch(() =>
            {
                Bombs.RemoveAt(i);
            });
        }

        private void Actor_Destroyed(object? sender, ActorDestroyedEventArgs e)
        {
            Application.Current!.Dispatcher.Dispatch(() =>
            {
                Tiles[IndFromPos(e.Position)].Source = "";
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

        public void Dispose()
        {
            _game?.Dispose();
        }
    }
}
