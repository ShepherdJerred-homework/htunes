using System.Data;
using System.Linq;
using System.Windows;
using hTunes;

namespace htunes {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MusicLib musicLib;

        public MainWindow() {
            InitializeComponent();
            musicLib = new MusicLib();
            setupSongGrid();
        }

        public void setupSongGrid()
        {
            songGrid.ItemsSource = musicLib.Songs.DefaultView;
        }
    }
}
