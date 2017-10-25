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

        private void setupSongGrid() {
            SongGrid.ItemsSource = musicLib.Songs.DefaultView;
            SongGridContextMenuPlay.Click += playMenuItem_Click;
            SongGridContextMenuDelete.Click += deleteMenuItem_Click;
        }

        private void setupButtonListeners() {
            ButtonPrevious.Click += previousButton_Click;
            ButtonPlayPause.Click += playMenuItem_Click;
            ButtonNext.Click += nextButton_Click;
            ButtonEdit.Click += editButton_Click;
        }

        private void previousButton_Click(object sender, RoutedEventArgs e) {

        }

        private void playPauseButton_Click(object sender, RoutedEventArgs e) {
            DataRowView selectedItem = SongGrid.SelectedItem as DataRowView;
            int songId = (int) selectedItem["id"];
        }

        private void nextButton_Click(object sender, RoutedEventArgs e) {

        }

        private void editButton_Click(object sender, RoutedEventArgs e) {
            // TODO Don't do this if we are in a playlist
            SongGrid.IsReadOnly = !SongGrid.IsReadOnly;
        }

        private void playMenuItem_Click(object sender, RoutedEventArgs e) {
            DataRowView selectedItem = SongGrid.SelectedItem as DataRowView;
            int songId = (int) selectedItem["id"];
        }

        private void deleteMenuItem_Click(object sender, RoutedEventArgs e) {
            DataRowView selectedItem = SongGrid.SelectedItem as DataRowView;
            int songId = (int) selectedItem["id"];

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation",
                System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes) {
                
            }
        }

    }
}