using ViewModel;

namespace MauiView
{
    public partial class App : Application, IDisposable
    {
        private readonly MenuViewModel _MenuViewModel;
        private readonly GameViewModel _GameViewModel;
        private readonly AppShell _appShell;

        public App()
        {
            InitializeComponent();

            _MenuViewModel = new MenuViewModel();
            _GameViewModel = new GameViewModel();

            _appShell = new AppShell(_MenuViewModel, _GameViewModel)
            {
                BindingContext = _MenuViewModel
            };
            MainPage = _appShell;
        }

        public void Dispose()
        {
            _appShell.Dispose();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = base.CreateWindow(activationState);

            // amikor az alkalmazás fókuszt veszt
            window.Deactivated += (s, e) =>
            {
                Task.Run(() =>
                {
                    if (_GameViewModel.IsRunning) _GameViewModel.KeyDown("Space");
                });
            };

            return window;
        }
    }
}
