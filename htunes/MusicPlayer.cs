using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace htunes
{
    class MusicPlayer {
        // https://stackoverflow.com/questions/3502311/how-to-play-a-sound-in-c-net
        void playSong() {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"c:\mywavfile.wav");
            player.Play();
        }
    }
}
