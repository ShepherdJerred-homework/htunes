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
            SetupSongGrid();
            SetupButtonListeners();
        }

        private void SetupSongGrid() {
            SongGrid.ItemsSource = musicLib.Songs.DefaultView;
            SongGridContextMenuPlay.Click += PlayMenuItem_Click;
            SongGridContextMenuDelete.Click += DeleteMenuItem_Click;
        }

        private void SetupButtonListeners() {
            ButtonPrevious.Click += PreviousButton_Click;
            ButtonPlayPause.Click += PlayMenuItem_Click;
            ButtonNext.Click += NextButton_Click;
            ButtonEdit.Click += EditButton_Click;
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e) {

        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e) {
            DataRowView selectedItem = SongGrid.SelectedItem as DataRowView;
            int songId = (int) selectedItem["id"];
        }

        private void NextButton_Click(object sender, RoutedEventArgs e) {

        }

        private void EditButton_Click(object sender, RoutedEventArgs e) {
            // TODO Don't do this if we are in a playlist
            SongGrid.IsReadOnly = !SongGrid.IsReadOnly;
        }

        private void PlayMenuItem_Click(object sender, RoutedEventArgs e) {
            DataRowView selectedItem = SongGrid.SelectedItem as DataRowView;
            int songId = (int) selectedItem["id"];
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e) {
            DataRowView selectedItem = SongGrid.SelectedItem as DataRowView;
            int songId = (int) selectedItem["id"];

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation",
                System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes) {
                
            }
        }

    }
}