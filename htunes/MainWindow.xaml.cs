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
        private String CurrentPlaylist { get; set; }

        public MainWindow() {
            InitializeComponent();
            musicLib = new MusicLib();
            musicPlayer = new MusicPlayer();
            IsPlaylistSelected = false;
            SetupSongGrid();
            SetupButtonListeners();
            SetupPlaylistList();
        }

        private void SetupPlaylistList() {
            List<String> PlaylistListItems = new List<string>();
            PlaylistListItems.Add("All Music");
            PlaylistListItems.AddRange(musicLib.Playlists);
            PlaylistList.ItemsSource = PlaylistListItems;

            PlaylistList.SelectedIndex = 0;

            PlaylistList.SelectionChanged += PlaylistListItem_Click;
        }

        private void SetupSongGrid() {
            UpdateSongList(musicLib.Songs.DefaultView);
            SongGrid.CellEditEnding += SongGrid_CellEndEdit;
            SongGridContextMenuPlay.Click += PlayMenuItem_Click;
            SongGridContextMenuDelete.Click += DeleteMenuItem_Click;
        }

        private void UpdateSongList(DataView data) {
            SongGrid.ItemsSource = data;

            List<Song> songs = new List<Song>();

            foreach (DataRowView songRow in data) {
                int id;
                if (IsPlaylistSelected) {
                    id = Convert.ToInt32((string) songRow.Row["id"]);
                }
                else {
                    id = (int) songRow.Row["id"];
                }
                Song song = musicLib.GetSong(id);
                songs.Add(song);
            }

            musicPlayer.SongList = songs;
        }

        private void SetupButtonListeners() {
            ButtonPrevious.Click += PreviousButton_Click;
            ButtonPlayPause.Click += PlayPauseButton_Click;
            ButtonStop.Click += StopButton_Click;
            ButtonNext.Click += NextButton_Click;
            ButtonEdit.Click += EditButton_Click;
        }

        private void PlaylistListItem_Click(object sender, RoutedEventArgs e) {
            
            CurrentPlaylist = (string) PlaylistList.SelectedItems[0];
            IsPlaylistSelected = CurrentPlaylist != "All Music";

            if (IsPlaylistSelected) {
                StopEditing();
                DisableEditButton();
                UpdateSongList(musicLib.SongsForPlaylist(CurrentPlaylist).DefaultView);
                PositionColumn.Visibility = Visibility.Visible;
            }
            else {
                EnableEditButton();
                UpdateSongList(musicLib.Songs.DefaultView);
                PositionColumn.Visibility = Visibility.Hidden;
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

        private void StopButton_Click(object sender, RoutedEventArgs e) {
            musicPlayer.Stop();
            if (!musicPlayer.IsPlaying) {
                ButtonPlayPause.Content = "Play";
            }
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

        private void PlayMenuItem_Click(object sender, RoutedEventArgs e) {
            DataRowView selectedItem = SongGrid.SelectedItem as DataRowView;
            int songId = (int) selectedItem["id"];

            Song song = musicLib.GetSong(songId);

            musicPlayer.Play(song);
        }

        // https://stackoverflow.com/questions/18315786/confirmation-box-in-c-sharp-wpf
        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e) {
            DataRowView selectedItem = SongGrid.SelectedItem as DataRowView;
            int songId = (int) selectedItem["id"];

            if (IsPlaylistSelected) {
                // TODO Remove from playlist
                // musicLib.RemoveSongFromPlaylist(position, songId, CurrentPlaylist);
            }
            else {
                String songTitle = selectedItem["title"] as String;
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(
                    "Are you sure you want to delete " + songTitle + "?",
                    "Confirm deleting song",
                    System.Windows.MessageBoxButton.YesNo);

                if (messageBoxResult == MessageBoxResult.Yes) {
                    musicLib.DeleteSong(songId);
                }
            }
            selectedItem.Delete();
            SaveDatabase();
        }

        private void SongGrid_CellEndEdit(object sender, DataGridCellEditEndingEventArgs e) {
            SaveDatabase();
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

        private void StartEditing() {
            ButtonEdit.Content = "Stop Editing";
            SongGrid.IsReadOnly = false;
        }

        private void StopEditing() {
            ButtonEdit.Content = "Start Editing";
            SongGrid.IsReadOnly = true;
        }

        // TODO Doesn't seem to save last edit
        private void SaveDatabase() {
            musicLib.Save();
        }
    }
}