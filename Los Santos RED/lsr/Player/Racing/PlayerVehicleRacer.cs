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
    public string CurrentLapDisplay { get; private set; } = "";
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

        // Initialize HUD and UI
        TotalRacers = vehicleRace.VehicleRacers.Count();
        BigMessage = new BigMessageThread(true);
        UpdateLapDisplay(vehicleRace);

        // Initialize World Markers
        CheckpointBlip = new Blip(TargetCheckpoint.Position, 20f) { Color = EntryPoint.LSRedColor };
        vehicleRace.AddBlip(CheckpointBlip);
        CreateCheckpoint(vehicleRace);

        Player.GPSManager.AddGPSRoute("Checkpoint", TargetCheckpoint.Position, false);

        StartHudFiber(vehicleRace);
    }

    private void StartHudFiber(VehicleRace vehicleRace)
    {
        GameFiber.StartNew(delegate
        {
            while (vehicleRace.IsActive && !HasFinishedRace)
            {
                if (GameTimeFinishedRace == 0 && CurrentPosition > 0)
                {
                    DrawRacingHUD();
                }
                GameFiber.Yield();
            }
        }, "RaceHUD_Fiber");
    }

    private void DrawRacingHUD()
    {
        LSRHUDSettings hudSettings = Settings.SettingsManager.LSRHUDSettings;
        Color hudColor = Color.FromName(hudSettings.RacingPositionColor);
        GTAFont font = (GTAFont)hudSettings.RacingPositionFont;
        GTATextJustification justify = (GTATextJustification)hudSettings.RacingPositionJustificationID;

        // Position (e.g., 1st, 2nd)
        NativeHelper.DisplayTextOnScreen(NativeHelper.AddOrdinal(CurrentPosition),
            hudSettings.RacingPositionPositionY, hudSettings.RacingPositionPositionX,
            hudSettings.RacingPositionScale, hudColor, font, justify, false);

        // Time
        NativeHelper.DisplayTextOnScreen(CurrentTime,
            hudSettings.RacingPositionPositionTimeY, hudSettings.RacingPositionPositionX,
            hudSettings.RacingPositionSmallerScale, hudColor, font, justify, false);

        // Lap
        NativeHelper.DisplayTextOnScreen(CurrentLapDisplay,
            hudSettings.RacingPositionPositionLapY, hudSettings.RacingPositionPositionX,
            hudSettings.RacingPositionSmallerScale, hudColor, font, justify, false);
    }

    public override void Update(VehicleRace vehicleRace)
    {
        if (HasFinishedRace) return;

        base.Update(vehicleRace);
        CalculatePositionAndTimer(vehicleRace);
    }

    private void CalculatePositionAndTimer(VehicleRace vehicleRace)
    {
        if (TargetCheckpoint == null || vehicleRace?.VehicleRacers == null) return;

        // Efficient LINQ positioning
        CurrentPosition = 1 + vehicleRace.VehicleRacers.Count(x =>
            (x.CurrentLap > CurrentLap) ||
            (x.CurrentLap == CurrentLap && x.TargetCheckpoint != null && x.TargetCheckpoint.Order > TargetCheckpoint.Order) ||
            (x.CurrentLap == CurrentLap && x.TargetCheckpoint != null && x.TargetCheckpoint.Order == TargetCheckpoint.Order && x.DistanceToCheckpoint < DistanceToCheckpoint)
        );

        CurrentTime = ConvertMSToTime(Game.GameTime - GameTimeStartedRace);
    }

    private void UpdateLapDisplay(VehicleRace vehicleRace)
    {
        CurrentLapDisplay = $"Lap {CurrentLap}/{vehicleRace.NumberOfLaps}";
    }

    private string ConvertMSToTime(uint totalMS)
    {
        TimeSpan t = TimeSpan.FromMilliseconds(totalMS);
        // Correctly pads Minutes to 2 digits, Seconds to 2 digits, and Milliseconds to 3 digits
        return $"{t.Minutes:00}:{t.Seconds:00}.{t.Milliseconds:000}";
    }

    private void CreateCheckpoint(VehicleRace vehicleRace)
    {
        if (TargetCheckpoint == null || vehicleRace == null) return;

        Vector3 toPointAt = AfterTargetCheckpoint?.Position ?? TargetCheckpoint.Position;
        int checkpointType = 0; // Default cylinder

        if (TargetCheckpoint.IsFinish)
        {
            checkpointType = (CurrentLap == vehicleRace.NumberOfLaps) ? 4 : 3;
        }

        CheckpointID = NativeFunction.Natives.CREATE_CHECKPOINT<int>(
            checkpointType, TargetCheckpoint.Position, toPointAt, 10f,
            EntryPoint.LSRedColor.R, EntryPoint.LSRedColor.G, EntryPoint.LSRedColor.B, 100, TargetCheckpoint.Order);
    }

    public override void OnReachedCheckpoint(VehicleRace vehicleRace)
    {
        if (CheckpointBlip.Exists()) CheckpointBlip.Position = TargetCheckpoint.Position;

        Player.GPSManager.AddGPSRoute("Checkpoint", TargetCheckpoint.Position, false);
        NativeFunction.Natives.DELETE_CHECKPOINT(CheckpointID);

        UpdateLapDisplay(vehicleRace); // Recalculate string only when lap/checkpoint changes
        CreateCheckpoint(vehicleRace);
        PlayCheckpointSound();

        base.OnReachedCheckpoint(vehicleRace);
    }
    public override void SetRaceStart(VehicleRace vehicleRace)
    {
        // Unlocks the player controls when the race starts
        Player.IsSetDisabledControlsWithCamera = false;
        Player.RacingManager.StartRacing(vehicleRace);

        base.SetRaceStart(vehicleRace);
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
        if (CheckpointBlip.Exists()) CheckpointBlip.Delete();
        NativeFunction.Natives.DELETE_CHECKPOINT(CheckpointID);
        Player.GPSManager.Reset();
    }

    private void ShowFinishUI(int finalPosition)
    {
        string header = finalPosition == 1 ? "Winner" : $"Position: {finalPosition}";
        HudColor bgColor = finalPosition == 1 ? HudColor.GreenDark : HudColor.RedLight;

        ShowMessage(header, ConvertMSToTime(GameTimeFinishedRace - GameTimeStartedRace), HudColor.Black, bgColor, 3000);
    }

    public override void Dispose()
    {
        RemoveCheckpoints();
        BigMessage?.Fiber?.Abort();
        BigMessage?.MessageInstance?.Dispose();
        Player.RacingManager.StopRacing();
        base.Dispose();
    }

    private void PlayCheckpointSound()
    {
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET", 0);
    }

    public void ShowMessage(string msg, string sub, HudColor c1, HudColor c2, int time)
    {
        if (BigMessage?.MessageInstance == null) return;

        BigMessage.MessageInstance.ShowColoredShard(msg, sub, c1, c2, time);
        PlayCheckpointSound();
    }

    public void ShowMessage(string msg, string sub)
    {
        // Redirects to the main method with default "Success" colors (Black & Green)
        ShowMessage(msg, sub, HudColor.Black, HudColor.GreenDark, 2000);
    }
}



