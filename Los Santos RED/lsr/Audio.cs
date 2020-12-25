using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR;
using NAudio.Wave;
using Rage.Native;
using System;

public class Audio : IAudioPlayer
{
    private WaveOutEvent outputDevice;
    private AudioFileReader audioFile;
    public bool CancelAudio { get; set; }
    public bool IsAudioPlaying
    {
        get
        {
            return outputDevice != null;
        }
    }
    public void Update()
    {
        if (DataMart.Instance.Settings.SettingsManager.Police.DisableAmbientScanner)
        {
            NativeFunction.Natives.xB9EFD5C25018725A("PoliceScannerDisabled", true);
        }
        if (DataMart.Instance.Settings.SettingsManager.Police.WantedMusicDisable)
        {
            NativeFunction.Natives.xB9EFD5C25018725A("WantedMusicDisabled", true);
        }

        if (CancelAudio && outputDevice == null)
        {
            CancelAudio = false;
        }
    }
    public void Play(string FileName)
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
                    Volume = DataMart.Instance.Settings.SettingsManager.Police.DispatchAudioVolume
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
            Debug.Instance.WriteToLog("Audio", e.Message);
        }
    }
    public void Abort()
    {
        if (IsAudioPlaying)
        {
            CancelAudio = true;
            outputDevice.Stop();
        }
        if (IsAudioPlaying)
        {
            CancelAudio = true;
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
}