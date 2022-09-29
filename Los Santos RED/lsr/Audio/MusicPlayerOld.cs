using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace MusicGuru
{
    public class MusicPlayerOld
    {

        //System.Media.SoundPlayer Player;
        //WMPLib.WindowsMediaPlayer Player;
        public bool IsBeingPlayed = false;
        private bool IsLooping = false;
        public string FileName;
        public string TrackName;
        //private int Timer = 0;//in minutes
        //BackgroundWorker Player;
        private long lngVolume = 500; //between 0-1000


        public MusicPlayerOld(string fileName)
        {
            this.TrackName = fileName;
            if (fileName.Contains("\\"))
                this.FileName = fileName;
            else
                this.FileName = AppDomain.CurrentDomain.BaseDirectory + fileName;
            //Player = new BackgroundWorker();
            //Player.DoWork += new DoWorkEventHandler(Player_DoWork);

            //Player = new WMPLib.WindowsMediaPlayer();
            //Player.URL = AppDomain.CurrentDomain.BaseDirectory + "local.wav";
            //Player = new System.Media.SoundPlayer(fileName);
        }

        ////play the track for n minutes
        //public void StartPlaying()
        //{
        //    try
        //    {
        //        //this.Timer = timer;
        //        //IsBeingPlayed = true;
        //        PlayLoop();
        //        //ThreadStart ts = new ThreadStart(Loop);
        //        //LoopThread = new Thread(ts);
        //        //LoopThread.Start();

        //        //if (!Player.IsBusy)
        //            //Player.RunWorkerAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        frmMain.WriteLog("Error occured in StartPlaying: " + ex.Message);
        //    }
        //}

        private void PlayWorker()
        {
            StringBuilder sb = new StringBuilder();
            int result = mciSendString("open \"" + FileName + "\" type mpegvideo alias " + this.TrackName, null, 0, IntPtr.Zero); //int result = mciSendString("open \"" + FileName + "\" type waveaudio  alias " + this.TrackName, sb, 0, IntPtr.Zero);
            string command = "setaudio " + this.TrackName + " volume to " + lngVolume.ToString();
            EntryPoint.WriteToConsole($"{command}");
            EntryPoint.WriteToConsole($"Start MusicPlayerOld Loop {lngVolume.ToString()}");

            long err1 = mciSendString(command, null, 0, IntPtr.Zero);
            //long err2 = mciSendString("setaudio " + this.TrackName + " volume to " + lngVolume.ToString(), null, 0, IntPtr.Zero);
            //?FUCK THIS STUPID FUCKING SHIT THAT DOESNT FUCKING WORK PROPERLY, FUCK MCI BULLSHIT FUCK C++ FUCK EM ALL
            if (err1 != 0)
            {
                EntryPoint.WriteToConsole("ERror " + err1);
            }
            //if (err2 != 0)
            //{
            //    EntryPoint.WriteToConsole("ERror " + err2);
            //}

            mciSendString("play " + this.TrackName, sb, 0, IntPtr.Zero);
            IsBeingPlayed = true;
            //loop
            sb = new StringBuilder();
            mciSendString("status " + this.TrackName + " length", sb, 255, IntPtr.Zero);
            int length = Convert.ToInt32(sb.ToString().Length);
            int pos = 0;
            long oldvol = lngVolume;

            //set the initial volume - Changed by Prahlad for phase-2
            //sb = new StringBuilder("................................................................................................................................");


            while (IsBeingPlayed)
            {
                
                sb = new StringBuilder();
                mciSendString("status " + this.TrackName + " position", sb, 255, IntPtr.Zero);
                pos = Convert.ToInt32(sb.ToString().Length);
                if (pos >= length)
                {
                    if (!IsLooping)
                    {
                        EntryPoint.WriteToConsole("BREAKING MUSICPLAYEROLD LOOP");
                        IsBeingPlayed = false;
                        break;
                    }
                    else
                    {
                        mciSendString("play " + this.TrackName + " from 0", sb, 0, IntPtr.Zero);
                    }
                }

                if (oldvol != lngVolume) //volume is changed by user
                {
                    //set new volume - Changed by Prahlad for phase-2
                    sb = new StringBuilder("................................................................................................................................");
                    string cmd = "setaudio " + this.TrackName + " volume to " + lngVolume.ToString();
                    long err = mciSendString(cmd, null, 0, IntPtr.Zero);
                    System.Diagnostics.Debug.Print(cmd);
                    if (err != 0)
                    {
                        EntryPoint.WriteToConsole("ERror " + err);
                    }
                    else
                    {
                        EntryPoint.WriteToConsole("No errors!");
                    }
                    oldvol = lngVolume;
                }
                //Player.openPlayer( AppDomain.CurrentDomain.BaseDirectory + "local.wav");
                //Player.Play();
                Application.DoEvents();
                //Thread.Sleep(500);
            }
            mciSendString("stop " + this.TrackName, sb, 0, IntPtr.Zero);
            mciSendString("close " + this.TrackName, sb, 0, IntPtr.Zero);
        }

        //volume between 0-10
        public int GetVolume()
        {
            return (int)this.lngVolume / 100;
        }

        //volume between 0-10
        public void SetVolume(int newvolume)
        {
            this.lngVolume = newvolume * 100;
            //mciSendString("setaudio " + strAlias + " volume to " & lngVolume, "", 0, 0&);
        }

        public void Play(bool Looping)
        {
            try
            {
                if (IsBeingPlayed)
                {
                    EntryPoint.WriteToConsole("MUSICPLAYEROLD FAIL IsBeingPlayed true");
                    return;
                }
                if (!File.Exists(FileName))
                {
                    IsBeingPlayed = true;
                    EntryPoint.WriteToConsole("MUSICPLAYEROLD FAIL file doesnt exist?");
                    return;
                }
                IsBeingPlayed = true;
                this.IsLooping = Looping;
                ThreadStart ts = new ThreadStart(PlayWorker);
                Thread WorkerThread = new Thread(ts);
                WorkerThread.Start();
                EntryPoint.WriteToConsole("MUSICPLAYEROLD RAN ONCE?");
                //DateTime t = DateTime.Now;
                //PlaySound(FileName, IntPtr.Zero, SoundFlags.SND_FILENAME | SoundFlags.SND_ASYNC | SoundFlags.SND_NODEFAULT | SoundFlags.SND_LOOP);// | SoundFlags.SND_NOSTOP );
                //mciSendString("Open \"" + AppDomain.CurrentDomain.BaseDirectory + "local.wav\" alias local",new StringBuilder(),0,IntPtr.Zero);
                //mciSendString("play local", new StringBuilder(), 0, IntPtr.Zero);
                //PlaySound(null, IntPtr.Zero, SoundFlags.SND_FILENAME | SoundFlags.SND_ASYNC | SoundFlags.SND_LOOP);
                //Timer = 0;//reset the timer
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Error occured in Loop: " + ex.Message);
            }
        }

        public void StopPlaying()
        {
            IsBeingPlayed = false;
            //PlaySound(null, IntPtr.Zero, SoundFlags.SND_FILENAME | SoundFlags.SND_ASYNC | SoundFlags.SND_LOOP);
            //mciSendString("stop local", new StringBuilder(), 0, IntPtr.Zero);
            //mciSendString("close local", new StringBuilder(), 0, IntPtr.Zero);
            //if (LoopThread !=null && LoopThread.ThreadState==ThreadState.Running)
            //    LoopThread.Abort();
        }

        //sound api functions
        [DllImport("winmm.dll")]
        static extern Int32 mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        [DllImport("winmm.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        static extern bool PlaySound(
            string pszSound,
            IntPtr hMod,
            SoundFlags sf);

        // Flags for playing sounds.  For this example, we are reading 
        // the sound from a filename, so we need only specify 
        // SND_FILENAME | SND_ASYNC

        [Flags]
        public enum SoundFlags : int
        {
            SND_SYNC = 0x0000,  // play synchronously (default) 
            SND_ASYNC = 0x0001,  // play asynchronously 
            SND_NODEFAULT = 0x0002,  // silence (!default) if sound not found 
            SND_MEMORY = 0x0004,  // pszSound points to a memory file
            SND_LOOP = 0x0008,  // loop the sound until next sndPlaySound 
            SND_NOSTOP = 0x0010,  // don't stop any currently playing sound 
            SND_PURGE = 0x40, // <summary>Stop Playing Wave</summary>
            SND_NOWAIT = 0x00002000, // don't wait if the driver is busy 
            SND_ALIAS = 0x00010000, // name is a registry alias 
            SND_ALIAS_ID = 0x00110000, // alias is a predefined ID
            SND_FILENAME = 0x00020000, // name is file name 
            SND_RESOURCE = 0x00040004  // name is resource name or atom 
        }

    }
}