using LosSantosRED.lsr.Interface;
using System;
using System.Media;
using System.Runtime.InteropServices;

public class WavAudioPlayer : IAudioPlayable
{
    private SoundPlayer AudioDevice = new SoundPlayer();
    public bool IsAudioPlaying { get; private set; }
    public bool IsPlayingLowPriority { get; set; } = false;
    [DllImport("winmm.dll")]
    public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

    [DllImport("winmm.dll")]
    public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);
    public void Abort()
    {
        if (IsAudioPlaying)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() => { AudioDevice.Stop(); IsPlayingLowPriority = false; });//seems to take 500 ms or so to do it? will lock the game thread
        }
    }
    public void Play(string FileName, int volume, bool isLowPriority, bool applyFilter)
    {
        if (FileName == "")
        {
            return;
        }

        if (AudioDevice == null)
        {
            AudioDevice = new SoundPlayer();
        }

        if (volume > 10)
        {
            volume = 10;
        }
        else if (volume <= 0)
        {
            volume = 1;
        }
        IsPlayingLowPriority = isLowPriority;
       //SetVolume(volume);
        string AudioFilePath = string.Format("Plugins\\LosSantosRED\\audio\\{0}", FileName);
        AudioDevice.SoundLocation = AudioFilePath;
        IsAudioPlaying = true;
        System.Threading.Tasks.Task.Factory.StartNew(() => { AudioDevice.PlaySync(); IsAudioPlaying = false; IsPlayingLowPriority = false; });
    }
    public void Play(string FileName, bool isLowPriority, bool applyFilter)
    {
        if (FileName == "")
        {
            return;
        }

        if (AudioDevice == null)
        {
            AudioDevice = new SoundPlayer();
        }
        IsPlayingLowPriority = isLowPriority;
        string AudioFilePath = string.Format("Plugins\\LosSantosRED\\audio\\{0}", FileName);
        AudioDevice.SoundLocation = AudioFilePath;
        IsAudioPlaying = true;
        System.Threading.Tasks.Task.Factory.StartNew(() => { AudioDevice.PlaySync(); IsAudioPlaying = false; IsPlayingLowPriority = false; });
    }

    public void Play(string fileName, float volume, bool isLowPriority, bool applyFilter)
    {
        Play(fileName, isLowPriority, applyFilter);
    }

    private int GetVolume()
    {
        return -1;
        //uint CurrVol;
        //waveOutGetVolume(IntPtr.Zero, out CurrVol);
        //ushort CalcVol = (ushort)(CurrVol & 0x0000ffff);
        //return (ushort)(CalcVol / (ushort.MaxValue / 10));
    }
    public void SetVolume(int VolumeToSet)
    {
        //int NewVolume = ((ushort.MaxValue / 10) * VolumeToSet);
        //uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
        //waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
        //GetVolume();
    }
}