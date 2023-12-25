using ViewModel;

namespace MauiView
{
    public partial class AppShell : Shell, IDisposable
    {
        private readonly MenuViewModel _menu_view_model;
        private readonly GameViewModel _game_view_model;
        public DelegateCommand KeyDownCommand { get; private set; }

        public AppShell(MenuViewModel MenuViewModel_, GameViewModel GameViewModel_)
        {
            InitializeComponent();

            _menu_view_model = MenuViewModel_;
            _game_view_model = GameViewModel_;

            KeyDownCommand = new DelegateCommand(param => _game_view_model.KeyDown(param!.ToString()!));

            _menu_view_model.Start += Menu_Start;
            _game_view_model.GameOver += Game_Over;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
            //return base.OnBackButtonPressed();
        }

        public async Task<string> ReadMapFile(string filePath)
        {
            using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(filePath);
            using StreamReader reader = new(fileStream);

            return await reader.ReadToEndAsync();
        }

        private async void Menu_Start(object? sender, string e)
        {

            string mapstring;

            try
            {
                mapstring = await ReadMapFile(e);
                _game_view_model.Start(mapstring);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", String.Format("Could not load map: {0}", ex.Message), "Ok");
                return;
            }

            await Navigation.PushAsync(new GamePage
            {
                BindingContext = _game_view_model
            });
        }

        private async void Game_Over(object? sender, EventArgs _)
        {
            await DisplayAlert("Game Over", String.Format("{0}\tGame Time: {1} s\tDestroyed Enemies: {2}", _game_view_model.Player_won ? "Victory!" : "Defeat!", _game_view_model.GameTime, _game_view_model.DestroyedEnemies), "Ok");

            await Navigation.PopAsync();
        }

        public void Dispose()
        {
            _game_view_model.Dispose();
        }
    }
}
