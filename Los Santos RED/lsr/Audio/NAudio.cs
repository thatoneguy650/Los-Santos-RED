using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR;
using NAudio.Dsp;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Rage;
using Rage.Native;
using System;

public class NAudioPlayer : IAudioPlayable
{
    private WaveOutEvent outputDevice;
    private AudioFileReader audioFile;
    private ISettingsProvideable Settings;
    public bool IsPlayingLowPriority { get; set; }
    public NAudioPlayer(ISettingsProvideable settings)
    {
        Settings = settings;
    }
    public bool IsScannerPlaying { get; private set; }
    public bool IsAudioPlaying
    {
        get
        {
            return outputDevice != null;
        }
    }
    public void Play(string FileName, bool isLowPriority)
    {
        Play(FileName, 1.0f, isLowPriority);
    }
    public void Play(string FileName, float volume, bool isLowPriority)
    {
        try
        {
            if (FileName == "")
            {
                return;
            }
            IsPlayingLowPriority = isLowPriority;
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }

            if (volume > 1.0f)
            {
                volume = 1.0f;
            }
            else if (volume < 0.0f)
            {
                volume = 0.0f;
            }

            if (audioFile == null)
            {
                audioFile = new AudioFileReader(string.Format("Plugins\\LosSantosRED\\audio\\{0}", FileName))
                {
                    Volume = volume
                };
                //toPlay = audioFile;
                //using (var reader2 = new AudioFileReader(string.Format("Plugins\\LosSantosRED\\audio\\{0}", "SpeedQuotes\\samllOther.wav")))
                //{
                //    MixingSampleProvider mixer = new MixingSampleProvider(new[] { audioFile, reader2 });

                //    //toPlay = mixer;
                //    //outputDevice.Init(mixer);

                //    WaveFileWriter.CreateWaveFile16("mixed.wav", mixer);


                //    audioFile = new AudioFileReader("mixed.wav");
                //    toPlay = audioFile;
                //}
            }

            if (Settings.SettingsManager.ScannerSettings.ApplyFilter)
            {
                MyWaveProvider filter = new MyWaveProvider(audioFile, 2000);
                outputDevice.Init(filter);
            }
            else
            {
                outputDevice.Init(audioFile);
            }

            outputDevice.Play();
        }
        catch (Exception e)
        {
            EntryPoint.WriteToConsole("NAudio: " + e.StackTrace + e.Message, 0);
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
        IsPlayingLowPriority = false;
    }

    public void Play(string fileName, int volume, bool isScannerPlaying)
    {
        float realVolume = (float)volume / 10.0f;
        if (realVolume > 1.0f)
        {
            realVolume = 1.0f;
        }
        else if (realVolume < 0.0f)
        {
            realVolume = 0.0f;
        }
        Play(fileName, realVolume, isScannerPlaying);
    }

    class MyWaveProvider : ISampleProvider
    {
        private ISampleProvider sourceProvider;
        private float cutOffFreq;
        private int channels;
        private BiQuadFilter[] filters;

        public MyWaveProvider(ISampleProvider sourceProvider, int cutOffFreq)
        {
            this.sourceProvider = sourceProvider;
            this.cutOffFreq = cutOffFreq;

            channels = sourceProvider.WaveFormat.Channels;
            filters = new BiQuadFilter[channels];
            CreateLowFilters();
            CreateHighFilters();
        }

        private void CreateLowFilters()
        {
            for (int n = 0; n < channels; n++)
                if (filters[n] == null)
                    filters[n] = BiQuadFilter.LowPassFilter(44100, cutOffFreq, 1);
                else
                    filters[n].SetLowPassFilter(44100, cutOffFreq, 1);
        }
        private void CreateHighFilters()
        {
            for (int n = 0; n < channels; n++)
                if (filters[n] == null)
                    filters[n] = BiQuadFilter.HighPassFilter(44100, cutOffFreq, 1);
                else
                    filters[n].SetHighPassFilter(44100, cutOffFreq, 1);
        }

        public WaveFormat WaveFormat { get { return sourceProvider.WaveFormat; } }

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = sourceProvider.Read(buffer, offset, count);

            for (int i = 0; i < samplesRead; i++)
                buffer[offset + i] = filters[(i % channels)].Transform(buffer[offset + i]);

            return samplesRead;
        }
    }
}