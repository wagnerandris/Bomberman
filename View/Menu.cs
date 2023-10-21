using View.Properties;

namespace View
{
    public partial class Menu : UserControl
    {
        public event EventHandler<StartEventArgs>? Start;
        private string _mapfile = "maps/map2.txt";

        public Menu()
        {
            InitializeComponent();
            // start_button.Font = new Font(); // change to Star Wars font
        }
        private void start_button_Click(object sender, EventArgs e)
        {
            Start?.Invoke(sender, new StartEventArgs(_mapfile));
        }

        private void prev_map_Click(object sender, EventArgs e)
        {

        }

        private void next_map_Click(object sender, EventArgs e)
        {

        }
    }
}
