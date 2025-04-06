using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public class PlayerVehicleRacer : VehicleRacer
{
    private int CheckpointID;
    private Blip CheckpointBlip;
    private int TotalRacers;
    private BigMessageThread BigMessage;
    private IRaceable Player;
    private ISettingsProvideable Settings;
    public int CurrentPosition { get; private set; }
    public string CurrentTime { get; private set; } = "";
    public override string RacerName => Player.PlayerName;
    public override bool IsPlayer => true;
    public PlayerVehicleRacer(VehicleExt vehicleExt, IRaceable player, ISettingsProvideable settings) : base(vehicleExt)
    {
        Player = player;
        Settings = settings;
    }
    public override void SetupRace(VehicleRace vehicleRace)
    {
        base.SetupRace(vehicleRace);
        CheckpointBlip = new Blip(TargetCheckpoint.Position, 20f) { Color = EntryPoint.LSRedColor };
        vehicleRace.AddBlip(CheckpointBlip);
        CreateCheckpoint();
        Player.GPSManager.AddGPSRoute("Checkpoint", TargetCheckpoint.Position, false);
        TotalRacers = vehicleRace.VehicleRacers.Count();
        BigMessage = new BigMessageThread(true);
        GameFiber raceGameFiber = GameFiber.StartNew(delegate
        {
            while (vehicleRace.IsActive && !HasFinishedRace)
            {
                string finalPos = $"{NativeHelper.AddOrdinal(CurrentPosition)}";
                if (GameTimeFinishedRace == 0 && CurrentPosition > 0)
                {
                    NativeHelper.DisplayTextOnScreen(finalPos,
                        Settings.SettingsManager.LSRHUDSettings.RacingPositionPositionY,
                        Settings.SettingsManager.LSRHUDSettings.RacingPositionPositionX,
                        Settings.SettingsManager.LSRHUDSettings.RacingPositionScale,
                        Color.FromName(Settings.SettingsManager.LSRHUDSettings.RacingPositionColor),
                        (GTAFont)Settings.SettingsManager.LSRHUDSettings.RacingPositionFont,
                        (GTATextJustification)Settings.SettingsManager.LSRHUDSettings.RacingPositionJustificationID,
                        false);
                }
                GameFiber.Yield();
            }
        }, "RaceGameFiber");
    }
    public override void Update(VehicleRace vehicleRace)
    {
        base.Update(vehicleRace);
        CalculatePosition(vehicleRace);
    }
    private void CalculatePosition(VehicleRace vehicleRace)
    {
        if(TargetCheckpoint == null || vehicleRace == null || vehicleRace.VehicleRacers == null)
        {
            return;
        }
        CurrentPosition = 1+vehicleRace.VehicleRacers.Where(x => x.TargetCheckpoint != null && x.TargetCheckpoint.Order > TargetCheckpoint.Order || (x.TargetCheckpoint.Order == TargetCheckpoint.Order && x.DistanceToCheckpoint < DistanceToCheckpoint)).Count();
        uint currentTime = Game.GameTime - GameTimeStartedRace;
        try
        {          
            CurrentTime = ConvertMSToTime(Game.GameTime - GameTimeStartedRace);
            Player.RacingManager.RaceTimer.Text = CurrentTime;
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"currentTime {currentTime}");
            EntryPoint.WriteToConsole(ex.Message);
            EntryPoint.WriteToConsole(ex.StackTrace);
            Game.DisplayHelp(ex.Message);
        }
    }
    private string ConvertMSToTime(uint TotalGameTime)
    {
        TimeSpan t = TimeSpan.FromMilliseconds(TotalGameTime);
        string answer = string.Format("{0:000}:{1:000}.{2:000}",
                                t.Minutes,
                                t.Seconds,
                                t.Milliseconds);
        return answer;
    }
    private void CreateCheckpoint()
    {
        if(TargetCheckpoint == null)
        {
            return;
        }
        Vector3 toPointAt = TargetCheckpoint.Position;
        if (AfterTargetCheckpoint != null)
        {
            toPointAt = AfterTargetCheckpoint.Position;
        }
        int checkpointType = TargetCheckpoint.IsFinish ? 4 : 0;
        CheckpointID = NativeFunction.Natives.CREATE_CHECKPOINT<int>(checkpointType, TargetCheckpoint.Position, toPointAt, 10f, EntryPoint.LSRedColor.R, EntryPoint.LSRedColor.G, EntryPoint.LSRedColor.B, 100, TargetCheckpoint.Order);      
    }
    public override void Dispose()
    {
        if(CheckpointBlip.Exists())
        {
            CheckpointBlip.Delete();
        }
        NativeFunction.Natives.DELETE_CHECKPOINT(CheckpointID);
        BigMessage.Fiber?.Abort();
        if (BigMessage != null && BigMessage.MessageInstance != null)
        {
            BigMessage.MessageInstance.Dispose();
        }
        Player.RacingManager.StopRacing();
    }
    public override void OnReachedCheckpoint()
    {
        if (CheckpointBlip.Exists())
        {
            CheckpointBlip.Position = TargetCheckpoint.Position;
        }
        Player.GPSManager.AddGPSRoute("Checkpoint", TargetCheckpoint.Position, false);
        NativeFunction.Natives.DELETE_CHECKPOINT(CheckpointID);
        CreateCheckpoint();
        PlayCheckpointSound();
        base.OnReachedCheckpoint();
    }
    public override void OnFinishedRace(int finalPosition, VehicleRace vehicleRace)
    {
        RemoveCheckpoints();
        ShowFinishUI(finalPosition);
        vehicleRace.OnPlayerFinishedRace();



        base.OnFinishedRace(finalPosition, vehicleRace);
    }
    private void RemoveCheckpoints()
    {
        if (CheckpointBlip.Exists())
        {
            CheckpointBlip.Delete();
        }
        NativeFunction.Natives.DELETE_CHECKPOINT(CheckpointID);
        PlayCheckpointSound();
        Player.GPSManager.Reset();
    }
    private void ShowFinishUI(int finalPosition)
    {
        string toShow = "";
        HudColor toShowColor = HudColor.Black;
        if (finalPosition == 1)
        {
            toShow = $"Winner";
            toShowColor = HudColor.GreenDark;
        }
        else
        {
            toShow = $"Position: {finalPosition}";
            toShowColor = HudColor.RedLight;
        }
        try
        {
            EntryPoint.WriteToConsole($"Finish Race Total MS {GameTimeFinishedRace - GameTimeStartedRace}");
            ShowMessage(toShow, ConvertMSToTime(GameTimeFinishedRace - GameTimeStartedRace), HudColor.Black, toShowColor, 3000);
        }
        catch (Exception ex)
        {
            Game.DisplayHelp(ex.Message);
        }
    }
    private void PlayCheckpointSound()
    {
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET", 0);
    }
    public void ShowMessage(string v, string v1, HudColor color1, HudColor color2, int time)
    {
        BigMessage.MessageInstance.ShowColoredShard(v, v1, color1, color2, time);
        PlayCheckpointSound();
    }
    public void ShowMessage(string v, string v1)
    {
        BigMessage.MessageInstance.ShowColoredShard(v, v1, HudColor.Black, HudColor.GreenDark, 1000);
        PlayCheckpointSound();
    }
    public override void SetRaceStart(VehicleRace vehicleRace)
    {
        Player.IsSetDisabledControls = false;
        Player.RacingManager.StartRacing(vehicleRace);
        base.SetRaceStart(vehicleRace);
    }

}

