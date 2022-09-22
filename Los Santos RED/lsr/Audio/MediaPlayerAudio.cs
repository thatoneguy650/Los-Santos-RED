using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Media;

public class MediaPlayerAudio : IAudioPlayable
{
    private MediaPlayer AudioDevice = new MediaPlayer();
    private ISettingsProvideable Settings;
    public MediaPlayerAudio(ISettingsProvideable settings)
    {
        Settings = settings;

    }

    public bool IsAudioPlaying { get; private set; }
    public bool IsPlayingLowPriority { get; set; } = false;
    public void Abort()
    {
        if (IsAudioPlaying)
        {
            EntryPoint.WriteToConsole("ABORT RAN");
            System.Threading.Tasks.Task.Factory.StartNew(() => { AudioDevice.Stop(); IsPlayingLowPriority = false; });//seems to take 500 ms or so to do it? will lock the game thread
        }
    }
    public void Play(string FileName, int volume, bool isLowPriority)
    {
        Play(FileName, isLowPriority);
    }

    private void AudioDevice_MediaEnded(object sender, EventArgs e)
    {
        EntryPoint.WriteToConsole("MEDIA ENDED RAN");
        IsAudioPlaying = false; 
        IsPlayingLowPriority = false;
    }

    public void Play(string FileName, bool isLowPriority)
    {
        try
        {
            if (FileName == "")
            {
                return;
            }
            
            IsPlayingLowPriority = isLowPriority;


            //System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => {

            EntryPoint.WriteToConsole("PLAY RAN !!!");
            System.Threading.Tasks.Task.Factory.StartNew(() => {
                EntryPoint.WriteToConsole("PLAY RAN 111");
                AudioDevice = new MediaPlayer();

                AudioDevice.MediaEnded += AudioDevice_MediaEnded;

                //CreateAudioDevice();

                Uri cool = new Uri(@"LosSantosRED\audio\01_assistance_required\0x0D3F2889.wav", UriKind.Relative);

                AudioDevice.Open(cool);
            if (Settings.SettingsManager.ScannerSettings.SetVolume)
            {
                AudioDevice.Volume = (float)Settings.SettingsManager.ScannerSettings.AudioVolume.Clamp(0, 10) / 10f;
            }
            IsAudioPlaying = true;
                EntryPoint.WriteToConsole("PLAY RAN 22222");
                AudioDevice.Play();
                
                //
                  });//seems to take 500 ms or so to do it? will lock the game thread
            GameFiber.Sleep(1000);
          //  }));

            //Dispatcher.Run();
        }
        catch(Exception ex)
        {
            Game.DisplayNotification(ex.Message);
            EntryPoint.WriteToConsole($"Error {ex.Message} {ex.StackTrace}");
        }
    }
    private void CreateAudioDevice()
    {
        if (AudioDevice == null)
        {
            AudioDevice = new MediaPlayer();
            AudioDevice.MediaEnded += AudioDevice_MediaEnded;
        }
    }
}