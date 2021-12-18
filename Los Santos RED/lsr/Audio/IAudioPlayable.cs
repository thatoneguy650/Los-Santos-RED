using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IAudioPlayable
    {
        bool IsAudioPlaying { get; }
        bool IsScannerPlaying { get; }

        void Abort();
        void Play(string fileName, int volume, bool isScannerPlaying);
    }
}
