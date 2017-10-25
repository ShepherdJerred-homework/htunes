using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Media;

namespace htunes {
    class MusicPlayer {

        private MediaPlayer MediaPlayer;
        private List<Song> songList;
        public List<Song> SongList {
            get => songList;
            set {
                songList = value;
                ResetCurrentSong();
            }
        }
        public int CurrentSong { get; private set; }
        public bool IsPlaying { get; private set; }
        public bool IsPaused { get; private set; }

        public MusicPlayer() {
            MediaPlayer = new MediaPlayer();
            MediaPlayer.MediaEnded += MediaPlayer_AfterSong;
        }

        // http://www.wpf-tutorial.com/audio-video/playing-audio/
        public void Play() {
            IsPaused = false;
            IsPlaying = true;
            MediaPlayer.Stop();
            MediaPlayer.Open(new Uri(SongList[CurrentSong].Filename));
            MediaPlayer.Play();
        }

        public void Play(Song song) {
            SongList = new List<Song> { song };
            Play();
        }

        public void Pause() {
            IsPaused = true;
            IsPlaying = false;
            MediaPlayer.Pause();
        }

        public void Resume() {
            IsPaused = false;
            IsPlaying = true;
            MediaPlayer.Play();
        }

        public void Stop() {
            IsPaused = false;
            IsPlaying = false;
            MediaPlayer.Stop();
        }

        public void PlayNext() {
            if (IsPlaying) {
                if (CurrentSong + 1 < SongList.Count) {
                    CurrentSong++;
                    Play();
                }
                else {
                    Stop();
                }
            }
        }

        public void PlayPrevious() {
            if (IsPlaying) {
                if (CurrentSong != 0) {
                    CurrentSong--;
                    Play();
                }
                else {
                    Stop();
                }
            }
        }

        public void ResetCurrentSong() {
            CurrentSong = 0;
        }
        
        public void MediaPlayer_AfterSong (object sender, EventArgs e) {
            PlayNext();
        }
    }
}
