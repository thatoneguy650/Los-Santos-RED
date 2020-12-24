using LosSantosRED.lsr;
using LSR;
using NAudio.Wave;
using Rage.Native;
using System;

public class Audio
{
    private static readonly Lazy<Audio> lazy =
    new Lazy<Audio>(() => new Audio());

    public static Audio Instance { get { return lazy.Value; } }

    private Audio()
    {
    }
    private WaveOutEvent outputDevice;
    private AudioFileReader audioFile;
    public bool IsMobileRadioEnabled { get; private set; }
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
        if (Mod.Player.Instance.CurrentVehicle != null && Mod.Player.Instance.CurrentVehicle.Vehicle.IsEngineOn && Mod.Player.Instance.CurrentVehicle.Vehicle.IsPoliceVehicle)
        {
            if (!IsMobileRadioEnabled)
            {
                IsMobileRadioEnabled = true;
                NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", true);
                Debug.Instance.WriteToLog("Audio", "Mobile Radio Enabled");
            }
        }
        else
        {
            if (IsMobileRadioEnabled)
            {
                IsMobileRadioEnabled = false;
                NativeFunction.CallByName<bool>("SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY", false);
                Debug.Instance.WriteToLog("Audio", "Mobile Radio Disabled");
            }
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