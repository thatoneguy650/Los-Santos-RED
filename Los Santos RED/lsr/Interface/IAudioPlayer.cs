using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IAudioPlayer
    {
        bool IsAudioPlaying { get; }
        bool CancelAudio { get; set; }
        void Abort();
        void Play(string FileName);
    }
}
