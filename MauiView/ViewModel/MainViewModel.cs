using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ViewModel
{

    public class MainViewModel : ViewModelBase, IDisposable
    {

        private readonly MenuViewModel _menu_view_model;
        private readonly GameViewModel _game_view_model;

        private ViewModelBase _current_veiw_model = null!;

        public ViewModelBase CurrentViewModel
        {
            get => _current_veiw_model;
            set
            {
                _current_veiw_model = value;
                OnPropertyChanged();
            }
        }
        public DelegateCommand KeyDownCommand { get; private set; }

        public MainViewModel()
        {
            _menu_view_model = new MenuViewModel();
            _game_view_model = new GameViewModel();

            KeyDownCommand = new DelegateCommand(param => _game_view_model.KeyDown(param!.ToString()!));

            _menu_view_model.Start += Menu_Start;
            _game_view_model.GameOver += Game_Over;

            CurrentViewModel = _menu_view_model;
        }

        private void Menu_Start(object? sender, string e)
        {
            CurrentViewModel = _game_view_model;
            _game_view_model.Start(e);
        }

        private void Game_Over(object? sender, EventArgs e)
        {
            CurrentViewModel = _menu_view_model;
        }

        public void Dispose()
        {
            _game_view_model.Dispose();
        }
    }
}
