using NAudio.Wave;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


    internal static class DispatchAudioSystem
    {
        private static WaveOutEvent outputDevice;
        private static AudioFileReader audioFile;
        public static bool IsRunning { get; set; } = true;
        public static void Initialize()
        {
            MainLoop();
        }
        public static void MainLoop()
        {
            GameFiber.StartNew(delegate
            {
                while (IsRunning)
                {                       

                    //if (Game.IsKeyDown(Keys.NumPad5))
                    //{
                    //    List<string> myList = new List<string>(new string[] { Scanner.Resident.DISPATCH_INTRO_01.Value, Scanner.AssistanceRequired.AssistanceRequiredRandom(), Scanner.Crimes.CRIME_10_99_DAVID_01.Value, Scanner.Resident.OUTRO_01.Value });
                    //    PlayAudioList(myList);
                    //}

                    GameFiber.Yield();
                }
            });
        }
    private static void PlayAudio(String _Audio)
    {
        if (outputDevice == null)
        {
            outputDevice = new WaveOutEvent();
            outputDevice.PlaybackStopped += OnPlaybackStopped;
        }
        if (audioFile == null)
        {
            audioFile = new AudioFileReader(String.Format("Plugins\\InstantAction\\audio\\scanner\\{0}.wav", _Audio));
            audioFile.Volume = 0.4f;
            outputDevice.Init(audioFile);
        }
        outputDevice.Play();
    }
    private static void PlayAudioList(List<String> SoundsToPlay)
    {
        //GameFiber.StartNew(delegate
        //{
            while (outputDevice != null)
                GameFiber.Yield();
            foreach (String audioname in SoundsToPlay)
            {
                PlayAudio(audioname);
                while (outputDevice != null)
                    GameFiber.Yield();
            }
        //});
    }
    private static void OnPlaybackStopped(object sender, StoppedEventArgs args)
    {
        outputDevice.Dispose();
        outputDevice = null;
        audioFile.Dispose();
        audioFile = null;
    }
}

