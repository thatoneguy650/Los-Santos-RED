using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RadarDetector
{
    private IPoliceRespondable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private NAudioPlayer AudioPlayer;
    private string ChirpAudio => Settings.SettingsManager.RadarDetectorSettings.SoundFile;
    private bool hasAudio = false;
    private uint gameTimeAudioPlayed = 0;
    private PoliceVehicleExt ClosestCopCar;
    public bool IsTurnedOn { get; private set; } = false;

    public RadarDetector(IPoliceRespondable player, IEntityProvideable world, ISettingsProvideable settings)
    {
        Player = player;
        World = world;
        Settings = settings;
    }

    private float MaxAlertDistance => Settings.SettingsManager.RadarDetectorSettings.MaxAlertDistance;// 100f;////Settings.SettingsManager.ScannerSettings.AlertDistance;
    public void SetState(bool isTurnedOn)
    {
        IsTurnedOn = isTurnedOn;
        EntryPoint.WriteToConsole($"Radar Detector IsTurnedOn {IsTurnedOn}");
    }
    public void Setup()
    {
        AudioPlayer = new NAudioPlayer(Settings);
        if(File.Exists($"Plugins\\LosSantosRED\\audio\\{ChirpAudio}"))
        {
            hasAudio = true;
        }
        else
        {
            EntryPoint.WriteToConsole("RADAR DETECTOR AUDIO MISSING");
        }
    }
    public void Update()
    {
        if (!hasAudio)
        {
            return;
        }
        if (!EntryPoint.ModController.IsRunning)
        {
            return;
        }
        if(!Settings.SettingsManager.RadarDetectorSettings.IsEnabled)
        { 
            return;
        }
        if(!Player.IsInVehicle && Settings.SettingsManager.RadarDetectorSettings.IsVehicleOnly)
        {
            return;
        }
        if (Settings.SettingsManager.RadarDetectorSettings.DisableWithoutItem && !Player.Inventory.Has(typeof(RadarDetectorItem)))
        {
            return;
        }
        if(!IsTurnedOn)
        {
            return;
        }
        ClosestCopCar = World.Vehicles.PoliceVehicles.Where(x => x.DistanceChecker.DistanceToPlayer <= MaxAlertDistance).OrderBy(x => x.DistanceChecker.DistanceToPlayer).FirstOrDefault();
        if (ClosestCopCar == null)
        {
            return;
        }
        float repeatDelayMax = 1.6f;
        float repeatDelayMin = 0.3f;
        float currentDistance = ClosestCopCar.DistanceChecker.DistanceToPlayer;
        float t = currentDistance / (MaxAlertDistance - currentDistance);
        float repeatDelay = MathHelper.Clamp(repeatDelayMin + (repeatDelayMax - repeatDelayMin) * t, repeatDelayMin, repeatDelayMax); //clamp to fix naudio value bug
        uint wait = (uint)((repeatDelay * 1000));
        if (Game.GameTime - gameTimeAudioPlayed > wait && !AudioPlayer.IsAudioPlaying)
        {
            //EntryPoint.WriteToConsole($"RADAR WAIT DELAY {wait} TIME DIFF {Game.GameTime - gameTimeAudioPlayed} {currentDistance}");
            AudioPlayer.Play(ChirpAudio, Settings.SettingsManager.ScannerSettings.AudioVolume, false, Settings.SettingsManager.ScannerSettings.ApplyFilter);
            gameTimeAudioPlayed = Game.GameTime;
        }
    }
    public void Dispose()
    {
        ClosestCopCar = null;
        AudioPlayer?.Abort();
        AudioPlayer = null;
    }
}

