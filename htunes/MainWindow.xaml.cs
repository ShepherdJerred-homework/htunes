using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using hTunes;
using Microsoft.Win32;
using System.Windows.Input;
using System.Linq;
using System.Windows.Navigation;

namespace htunes {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        private MusicLib musicLib;
        private MusicPlayer musicPlayer;
        private bool IsPlaylistSelected { get; set; }
        private String CurrentPlaylist { get; set; }
        private Point startPoint;

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
            if (PlaylistList.SelectedItems.Count > 0) {
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
            
            int songId;

            if (IsPlaylistSelected) {
                songId = Int32.Parse(selectedItem["id"] as string);
                int position = Int32.Parse(selectedItem["position"] as string);
                musicLib.RemoveSongFromPlaylist(position, songId, CurrentPlaylist);
                UpdateSongList(musicLib.SongsForPlaylist(CurrentPlaylist).DefaultView);
            }
            else {
                songId = (int) selectedItem["id"];
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
            ButtonEdit.Content = "Disable Editing";
            SongGrid.IsReadOnly = false;
        }

        private void StopEditing() {
            ButtonEdit.Content = "Enable Editing";
            SongGrid.IsReadOnly = true;
        }

        // TODO Doesn't seem to save last edit
        private void SaveDatabase() {
            musicLib.Save();
        }

        // https://github.com/fmccown/MiniPlayerWpf/blob/master/MiniPlayerWpf/MainWindow.xaml.cs
        // Author: fmccown
        private void AddSongButton_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.FileName = "";
            openFileDialog.DefaultExt = "*.wma;*.wav;*mp3";
            openFileDialog.Filter =
                "Media files|*.mp3;*.m4a;*.wma;*.wav|MP3 (*.mp3)|*.mp3|M4A (*.m4a)|*.m4a|Windows Media Audio (*.wma)|*.wma|Wave files (*.wav)|*.wav|All files|*.*";

            bool? result = openFileDialog.ShowDialog();

            if (result == true) {
                musicLib.AddSong(openFileDialog.FileName);
                SongGrid.SelectedIndex = SongGrid.Items.Count - 1;
                SongGrid.Focus();
            }
        }

        private void NewPlaylistButton_Click(object sender, RoutedEventArgs e) {
            NewPlaylistForm newPlaylistForm = new NewPlaylistForm();
            newPlaylistForm.NewNameTextBox.Focus();
            bool? result = newPlaylistForm.ShowDialog();
            if (result == true) {
                musicLib.AddPlaylist(newPlaylistForm.NewNameTextBox.Text);
                SetupPlaylistList();
            }
        }

        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void RenameMenuItem_Click(object sender, RoutedEventArgs e) {
            NewPlaylistForm newPlaylistForm = new NewPlaylistForm();
            newPlaylistForm.NewNameTextBox.Text = PlaylistList.SelectedItem.ToString();
            newPlaylistForm.NewNameTextBox.Focus();
            newPlaylistForm.NewNameTextBox.SelectAll();
            bool? result = newPlaylistForm.ShowDialog();
            if (result == true) {
                musicLib.RenamePlaylist(PlaylistList.SelectedItem.ToString(), newPlaylistForm.NewNameTextBox.Text);
                SetupPlaylistList();
            }
        }

        private void DeletePlaylistMenuItem_Click(object sender, RoutedEventArgs e)
        {
            musicLib.DeletePlaylist(PlaylistList.SelectedItem.ToString());
            SetupPlaylistList();
        }

        private void SongGrid_MouseMove(object sender, MouseEventArgs e) {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            // Start the drag-drop if mouse has moved far enough
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)) {
                DataRowView selectedSong = SongGrid.SelectedItem as DataRowView;

                if (selectedSong != null) {
                    string songId = selectedSong.Row.ItemArray[0].ToString();
                    DragDrop.DoDragDrop(SongGrid, songId, DragDropEffects.Copy);
                }
            }
        }

        private void SongGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            startPoint = e.GetPosition(null);
        }

        private void PlaylistList_Drop(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.StringFormat)) {
                TextBlock playlistTextBlock = e.OriginalSource as TextBlock;
                string songId = (string) e.Data.GetData(DataFormats.StringFormat);
                string playlistName = playlistTextBlock.DataContext.ToString();
                
                Song s;

                try {
                    s = musicLib.GetSong(int.Parse(songId));
                }
                catch (Exception) {
                    s = null;
                }
                if (s != null) {
                    musicLib.AddSongToPlaylist(s.Id, playlistName);
                }
            }
        }

        private void PlaylistList_DragOver(object sender, DragEventArgs e) {
            e.Effects = DragDropEffects.None;

            if (e.Data.GetDataPresent(DataFormats.StringFormat)) {
                string songId = e.Data.GetData(DataFormats.StringFormat) as string;

                if (musicLib.SongIds.Contains(songId)) {
                    e.Effects = DragDropEffects.Copy;
                }
            }
        }

        // https://stackoverflow.com/questions/10238694/example-using-hyperlink-in-wpf
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e) {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}