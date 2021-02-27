using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR;
using NAudio.Wave;
using Rage;
using Rage.Native;
using System;

public class Audio : IAudioPlayable
{
    private WaveOutEvent outputDevice;
    private AudioFileReader audioFile;
    public Audio()
    {

    } 
    public bool IsAudioPlaying
    {
        get
        {
            return outputDevice != null;
        }
    }
    public void Play(string FileName, float volume)
    {
        try
        {
            if (FileName == "")
            {
                return;
            }
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (audioFile == null)
            {
                audioFile = new AudioFileReader(string.Format("Plugins\\LosSantosRED\\audio\\{0}", FileName))
                {
                    Volume = volume
                };
                outputDevice.Init(audioFile);
            }
            else
            {
                outputDevice.Init(audioFile);
            }
            outputDevice.Play();
        }
        catch (Exception e)
        {
            EntryPoint.WriteToConsole("Audio" + e.StackTrace + e.Message,0);
        }
    }
    public void Abort()
    {
        if (IsAudioPlaying)
        {
            outputDevice.Stop();
        }
    }
    private void OnPlaybackStopped(object sender, StoppedEventArgs args)
    {
        outputDevice.Dispose();
        outputDevice = null;
        if (audioFile != null)
        {
            audioFile.Dispose();
        }
        audioFile = null;
    }

    public void Play(string fileName, int volume)
    {
        Play(fileName, volume * 1.0f);
    }
}