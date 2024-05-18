using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using LSR;
using NAudio.Dsp;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;

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
    public void Play(string FileName, bool isLowPriority, bool applyFilter)
    {
        Play(FileName, 1.0f, isLowPriority, applyFilter);
    }
    public void Play(string FileName, float volume, bool isLowPriority, bool applyFilter)
    {
        try
        {
            if (FileName == "")
            {
                return;
            }
            //EntryPoint.WriteToConsole($"ACTUAL PLAY 1", 5);
            IsPlayingLowPriority = isLowPriority;
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            //EntryPoint.WriteToConsole($"ACTUAL PLAY 2", 5);
            if (volume > 1.0f)
            {
                volume = 1.0f;
            }
            else if (volume < 0.0f)
            {
                volume = 0.0f;
            }
            //EntryPoint.WriteToConsole($"ACTUAL PLAY 3", 5);
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
            //EntryPoint.WriteToConsole($"ACTUAL PLAY 4", 5);
            if (applyFilter)
            {
                MyWaveProvider filter = new MyWaveProvider(audioFile, 1400);
                //MyWaveProvider filter = new MyWaveProvider(audioFile, 2000, Settings);
                outputDevice.Init(filter);
            }
            else
            {
                outputDevice.Init(audioFile);
            }
            //EntryPoint.WriteToConsole($"ACTUAL PLAY 5", 5);
            outputDevice.Play();
            //EntryPoint.WriteToConsole($"ACTUAL PLAY 6", 5);
        }
        catch (Exception e)
        {
            EntryPoint.WriteToConsole("NAudio: " + e.StackTrace + e.Message, 0);
        }
    }
    public void Abort()
    {
        try
        {
            if (IsAudioPlaying && outputDevice != null)
            {
                outputDevice.Stop();
            }
        }
        catch (Exception e)
        {
            EntryPoint.WriteToConsole("NAudio: " + e.StackTrace + e.Message, 0);
        }
    }
    private void OnPlaybackStopped(object sender, StoppedEventArgs args)
    {
        try
        { 
            outputDevice?.Dispose();
            outputDevice = null;
            if (audioFile != null)
            {
                audioFile.Dispose();
            }
            audioFile = null;
            IsPlayingLowPriority = false;
        }
        catch (Exception e)
        {
            EntryPoint.WriteToConsole("NAudio: " + e.StackTrace + e.Message, 0);
        }
    }

    public void Play(string fileName, int volume, bool isScannerPlaying, bool applyFilter)
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
        Play(fileName, realVolume, isScannerPlaying, applyFilter);
    }


    public void Setup()
    {
        //if (outputDevice == null)
        //{
        //    outputDevice = new WaveOutEvent();
        //    outputDevice.PlaybackStopped += OnPlaybackStopped;
        //}
        //audioFile = new AudioFileReader(string.Format("Plugins\\LosSantosRED\\audio\\{0}", ""))
        //{
        //    Volume = 0.0f
        //};
        //outputDevice.Init(audioFile);
    }

    //class MyWaveProvider_New : ISampleProvider
    //{
    //    private ISampleProvider sourceProvider;
    //    private float cutOffFreq;
    //    private int channels;
    //    private BiQuadFilter[] filters;
    //    private ISettingsProvideable Settings;
    //    public List<EqualizerBand> EqualizerBands { get; }

    //    public MyWaveProvider_New(ISampleProvider sourceProvider, int cutOffFreq, ISettingsProvideable settings)
    //    {
    //        Settings = settings;
    //        this.sourceProvider = sourceProvider;
    //        this.cutOffFreq = cutOffFreq;

    //        channels = sourceProvider.WaveFormat.Channels;
    //        filters = new BiQuadFilter[channels];

    //        EqualizerBands = new List<EqualizerBand>() 
    //           {
    //                    new EqualizerBand {Bandwidth = 1.0f, Frequency = 100, Gain = -30f},
    //                    new EqualizerBand {Bandwidth = 1.0f, Frequency = 200, Gain = -30f},
    //                    new EqualizerBand {Bandwidth = 1.0f, Frequency = 300, Gain = -30f},
    //                    new EqualizerBand {Bandwidth = 1.0f, Frequency = 400, Gain = 0f},
    //                    new EqualizerBand {Bandwidth = 1.0f, Frequency = 500, Gain = 0f},
    //                    new EqualizerBand {Bandwidth = 1.0f, Frequency = 600, Gain = 3f},
    //                    new EqualizerBand {Bandwidth = 1.0f, Frequency = 700, Gain = 4f},
    //                    new EqualizerBand {Bandwidth = 3.0f, Frequency = 1000, Gain = 9f},
    //                    new EqualizerBand {Bandwidth = 3.0f, Frequency = 2000, Gain = 18f},
    //                    new EqualizerBand {Bandwidth = 3.0f, Frequency = 3000, Gain = 24f},
    //                    new EqualizerBand {Bandwidth = 3.0f, Frequency = 4000, Gain = 9f},
    //                    new EqualizerBand {Bandwidth = 1.0f, Frequency = 5000, Gain = -3f}
    //                };

    //        if (Settings.SettingsManager.ScannerSettings.ApplyEQFilter)
    //        {
    //            CreateEQFilters();
    //        }
    //        if (Settings.SettingsManager.ScannerSettings.ApplyLowFilter)
    //        {
    //            CreateLowFilters();
    //        }
    //        if (Settings.SettingsManager.ScannerSettings.ApplyHighFilter)
    //        {
    //            CreateHighFilters();
    //        }

    //    }
    //    private void CreateEQFilters()
    //    {
    //        foreach (EqualizerBand eb in EqualizerBands)
    //        {
    //            for (int n = 0; n < channels; n++)
    //                if (filters[n] == null)
    //                    filters[n] = BiQuadFilter.PeakingEQ(44100, eb.Frequency, eb.Bandwidth, eb.Gain);
    //                else
    //                    filters[n].SetPeakingEq(44100, eb.Frequency,eb.Bandwidth,eb.Gain);
    //        }
    //    }
    //    private void CreateLowFilters()
    //    {
    //        int Cutoff = Settings.SettingsManager.ScannerSettings.LowCutoffFrequency;

    //        for (int n = 0; n < channels; n++)
    //            if (filters[n] == null)
    //                filters[n] = BiQuadFilter.LowPassFilter(44100, Cutoff, 1);
    //            else
    //                filters[n].SetLowPassFilter(44100, Cutoff, 1);
    //    }
    //    private void CreateHighFilters()
    //    {
    //        int Cutoff = Settings.SettingsManager.ScannerSettings.HighCutoffFrequency;
    //        for (int n = 0; n < channels; n++)
    //            if (filters[n] == null)
    //                filters[n] = BiQuadFilter.HighPassFilter(44100, Cutoff, 1);
    //            else
    //                filters[n].SetHighPassFilter(44100, Cutoff, 1);
    //    }

    //    public WaveFormat WaveFormat { get { return sourceProvider.WaveFormat; } }

    //    public int Read(float[] buffer, int offset, int count)
    //    {
    //        int samplesRead = sourceProvider.Read(buffer, offset, count);

    //        for (int i = 0; i < samplesRead; i++)
    //            buffer[offset + i] = filters[(i % channels)].Transform(buffer[offset + i]);

    //        return samplesRead;
    //    }
    //}

    class EqualizerBand
    {
        public float Frequency { get; set; }
        public float Gain { get; set; }
        public float Bandwidth { get; set; }
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