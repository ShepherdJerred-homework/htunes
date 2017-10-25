using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
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
            UpdateSongList(musicLib.Songs.DefaultView);
            SongGridContextMenuPlay.Click += PlayMenuItem_Click;
            SongGridContextMenuDelete.Click += DeleteMenuItem_Click;
        }

        private void UpdateSongList(DataView data) {
            // TODO We should order this by playlist position or song ID
            // Add data to the position column
            SongGrid.ItemsSource = data;

            List<Song> songs = new List<Song>();

            foreach (DataRowView songRow in data) {
                int id = (int) songRow.Row["id"];
                Song song = musicLib.GetSong(id);
                songs.Add(song);
            }

            musicPlayer.SongList = songs;
        }

        private void SetupButtonListeners() {
            ButtonPrevious.Click += PreviousButton_Click;
            ButtonPlayPause.Click += PlayPauseButton_Click;
            ButtonNext.Click += NextButton_Click;
            ButtonEdit.Click += EditButton_Click;
        }

        private void PlaylistListItem_Click(object sender, RoutedEventArgs e) {
            // TODO Set IsPlaylistSelected based on clicked item
            IsPlaylistSelected = false;
            if (IsPlaylistSelected) {
                StopEditing();
                DisableEditButton();
                // TODO Get the new data source from Playlist
                UpdateSongList(null);
            }
            else {
                EnableEditButton();
                UpdateSongList(musicLib.Songs.DefaultView);
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
            CheckQueueOutOfBounds();
        }

        private void CheckQueueOutOfBounds() {
            if (!musicPlayer.IsPlaying) {
                ButtonPlayPause.Content = "Play";
                musicPlayer.ResetCurrentSong();
            }
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e) {
            // TODO Toggle play/pause
            if (musicPlayer.IsPlaying) {
                PauseSong();
            }
            else {
                if (musicPlayer.IsPaused) {
                    ResumeSong();
                }
                else {
                    PlaySong();
                }
            }
        }

        private void PlaySong() {
            ButtonPlayPause.Content = "Pause";
            musicPlayer.Play();
        }

        private void PauseSong() {
            ButtonPlayPause.Content = "Play";
            musicPlayer.Pause();
        }

        private void ResumeSong() {
            ButtonPlayPause.Content = "Pause";
            musicPlayer.Resume();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e) {
            musicPlayer.PlayNext();
            CheckQueueOutOfBounds();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e) {
            if (!IsPlaylistSelected) {
                SongGrid.IsReadOnly = !SongGrid.IsReadOnly;
                if (SongGrid.IsReadOnly) {
                    StopEditing();
                }
                else {
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