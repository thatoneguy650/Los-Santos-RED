using LosSantosRED.lsr.Interface;
using Rage;
using Sound;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
//using Microsoft.DirectX.AudioVideoPlayback;


public class WavAudio : IAudioPlayable
{
    [DllImport("winmm.dll")]
    public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

    [DllImport("winmm.dll")]
    public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);
    private SoundPlayer AudioDevice = new SoundPlayer();
    private bool CancelAudio;
    public bool IsAudioPlaying { get; private set; }
    public bool IsScannerPlaying { get; private set; }
    public void Play(string FileName, int volume, bool isScannerPlaying)
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
        SetVolume(volume);
        string AudioFilePath = string.Format("Plugins\\LosSantosRED\\audio\\{0}", FileName);
        AudioDevice.SoundLocation = AudioFilePath;
        IsAudioPlaying = true;
        IsScannerPlaying = isScannerPlaying;
        System.Threading.Tasks.Task.Factory.StartNew(() => { AudioDevice.PlaySync(); IsAudioPlaying = false; IsScannerPlaying = false; });
    }
    public void Play(string FileName, bool isScannerPlaying)
    {
        if (FileName == "")
        {
            return;
        }

        if (AudioDevice == null)
        {
            AudioDevice = new SoundPlayer();
        }

        string AudioFilePath = string.Format("Plugins\\LosSantosRED\\audio\\{0}", FileName);
        AudioDevice.SoundLocation = AudioFilePath;
        IsAudioPlaying = true;
        IsScannerPlaying = isScannerPlaying;
        System.Threading.Tasks.Task.Factory.StartNew(() => { AudioDevice.PlaySync(); IsAudioPlaying = false; IsScannerPlaying = false; });
    }
    public void Play_OLD(string FileName, int volume)
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
        SetVolume(volume);
        string AudioFilePath = string.Format("Plugins\\LosSantosRED\\audio\\{0}", FileName);
        AudioDevice.SoundLocation = AudioFilePath;
        int l = SoundInfo.GetSoundLength(AudioFilePath);

        if (l == 0)
        {
            EntryPoint.WriteToConsole($"WAVAudio: Zero Sound Length! Attempt Alt, {FileName}", 3);
        }
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
            System.Threading.Tasks.Task.Factory.StartNew(() => { AudioDevice.Stop(); });//seems to take 500 ms or so to do it? will lock the game thread 

            //AudioDevice.Stop();
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


