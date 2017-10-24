using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using hTunes;

namespace htunes {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private MusicLib musicLib;

        public MainWindow() {
            InitializeComponent();
            musicLib = new MusicLib();
            setupSongGrid();
        }

        public void setupSongGrid() {
            songGrid.ItemsSource = musicLib.Songs.DefaultView;
        }

        private void PlayCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            DataRowView selectedItem = songGrid.SelectedItem as DataRowView;
            int songId = (int) selectedItem["id"];
            
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            Trace.Write("DELETE");
            MessageBoxResult result = MessageBox.Show("Do you want to close this window?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) {
                DataRowView selectedItem = songGrid.SelectedItem as DataRowView;

            }
        }
        
    }
}