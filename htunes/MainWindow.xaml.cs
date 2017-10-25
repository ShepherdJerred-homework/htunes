using System;
using System.Data;
using System.Windows;
using hTunes;

namespace htunes {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private MusicLib musicLib;
        private MusicPlayer musicPlayer;
        private bool IsPlaylistSelected { get; set; }

        public MainWindow() {
            InitializeComponent();
            musicLib = new MusicLib();
            musicPlayer = new MusicPlayer();
            IsPlaylistSelected = false;
            SetupSongGrid();
            SetupButtonListeners();
        }

        private void SetupSongGrid() {
            SetDefaultItemSource();
            SongGridContextMenuPlay.Click += PlayMenuItem_Click;
            SongGridContextMenuDelete.Click += DeleteMenuItem_Click;
        }

        private void SetDefaultItemSource() {
            SongGrid.ItemsSource = musicLib.Songs.DefaultView;
        }

        private void SetupButtonListeners() {
            ButtonPrevious.Click += PreviousButton_Click;
            ButtonPlayPause.Click += PlayMenuItem_Click;
            ButtonNext.Click += NextButton_Click;
            ButtonEdit.Click += EditButton_Click;
        }

        private void PlaylistListItem_Click(object sender, RoutedEventArgs e) {
            // TODO Set IsPlaylistSelected based on clicked item
            IsPlaylistSelected = false;
            if (IsPlaylistSelected) {
                StopEditing();
                DisableEditButton();
            }
            else {
                EnableEditButton();
                SetDefaultItemSource();
            }
        }

        private void DisableEditButton() {
            ButtonEdit.IsEnabled = false;
        }

        private void EnableEditButton() {
            ButtonEdit.IsEnabled = true;
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e) {
            musicPlayer.PlayPrevious();
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e) {
            // TODO Toggle play/pause
        }

        private void NextButton_Click(object sender, RoutedEventArgs e) {
            musicPlayer.PlayNext();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e) {
            if (!IsPlaylistSelected) {
                SongGrid.IsReadOnly = !SongGrid.IsReadOnly;
                if (SongGrid.IsReadOnly) {
                    StopEditing();
                } else {
                    StartEditing();
                }
            }
        }

        private void StartEditing() {
            ButtonEdit.Content = "Stop Editing";
            SongGrid.IsReadOnly = false;
        }

        private void StopEditing() {
            ButtonEdit.Content = "Start Editing";
            SongGrid.IsReadOnly = true;
        }

        private void PlayMenuItem_Click(object sender, RoutedEventArgs e) {
            DataRowView selectedItem = SongGrid.SelectedItem as DataRowView;
            int songId = (int) selectedItem["title"];

            Song song = musicLib.GetSong(songId);

            musicPlayer.Play(song);
        }

        // https://stackoverflow.com/questions/18315786/confirmation-box-in-c-sharp-wpf
        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e) {
            // TODO Remove from playlist if a playlist is selected
            DataRowView selectedItem = SongGrid.SelectedItem as DataRowView;
            String songTitle = selectedItem["title"] as String;

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(
                "Are you sure you want to delete " + songTitle + "?",
                "Confirm deleting song",
                System.Windows.MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes) {
                // TODO Delete from database
                selectedItem.Delete();
            }
        }

        private void SongGrid_CellEndEdit(object sender, RoutedEventArgs e) {
            // TODO Update database
        }
    }
}