using LosSantosRED.lsr.Interface;
using Rage;
using Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


public class AudioNew : IAudioPlayable
{
    [DllImport("winmm.dll")]
    public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

    [DllImport("winmm.dll")]
    public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);
    private SoundPlayer AudioDevice = new SoundPlayer();
    private bool CancelAudio;
    public bool IsAudioPlaying { get; private set; }
    public void Play(string FileName, int volume)
    {
        if (FileName == "")
        {
            return;
        }

        if (AudioDevice == null)
        {
            AudioDevice = new SoundPlayer();
        }

        if(volume > 10)
        {
            volume = 10;
        }    
        else if (volume <= 0)
        {
            volume = 1;
        }
        SetVolume(volume);
        string AudioFilePath = string.Format("Plugins\\LosSantosRED\\audio\\{0}", FileName);
        AudioDevice.SoundLocation = AudioFilePath;
        int l = SoundInfo.GetSoundLength(AudioFilePath);
        IsAudioPlaying = true;
        Thread t2 = new Thread(delegate ()
        {
            AudioDevice.Play();
            while (l > 0 && !CancelAudio)
            {
                Thread.Sleep(100);
                l -= 100;
            }
            IsAudioPlaying = false;
            CancelAudio = false;
        });
        t2.Start();     
    }
    public void Abort()
    {
        if (IsAudioPlaying)
        {
            CancelAudio = true;
            AudioDevice.Stop();    
        }
    }
    private int GetVolume()
    {
        uint CurrVol;
        waveOutGetVolume(IntPtr.Zero, out CurrVol);
        ushort CalcVol = (ushort)(CurrVol & 0x0000ffff);
        return (ushort)(CalcVol / (ushort.MaxValue / 10));
    }
    private void SetVolume(int VolumeToSet)
    {
        int NewVolume = ((ushort.MaxValue / 10) * VolumeToSet);
        uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
        waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
        GetVolume();
    }
}


