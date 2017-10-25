using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace htunes
{
    class MusicPlayer {

        private SoundPlayer Player;
        public Queue<Song> SongQueue { get; set; }
        public Song CurrentSong { get; private set; }
        public bool IsPlaying { get; private set; }

        // https://stackoverflow.com/questions/3502311/how-to-play-a-sound-in-c-net
        public void Play() {
            // TODO Play next song after song finishes
            CurrentSong = SongQueue.Dequeue();
            Player = new System.Media.SoundPlayer(CurrentSong.Filename);
            Player.Play();
        }

        public void Play(Song song) {
            CurrentSong = song;
            Player = new System.Media.SoundPlayer(CurrentSong.Filename);
            Player.Play();
        }

        public void Pause() {
            
        }

        public void PlayNext() {
            
        }

        public void PlayPrevious() {
            
        }
    }
}
