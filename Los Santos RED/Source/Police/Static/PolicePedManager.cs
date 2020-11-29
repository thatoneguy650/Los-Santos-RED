using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PolicePedManager

{
    public static bool IsRunning { get; set; } = true;
    public static bool AnyCanSeePlayer { get; set; }
    public static bool AnyCanHearPlayerShooting { get; set; }
    public static bool AnyCanRecognizePlayer { get; set; }
    public static bool AnyRecentlySeenPlayer { get; set; }
    public static bool AnySeenPlayerCurrentWanted { get; set; }
    public static Vector3 PlaceLastSeenPlayer { get; set; }
    public static Vector3 PlayerLastSeenForwardVector { get; set; }
    public static bool IsNightTime { get; set; }
    public static int PreviousWantedLevel { get; set; }  
    public static bool WasPlayerLastSeenInVehicle { get; set; }
    public static float PlayerLastSeenHeading { get; set; }
    //public static bool InSearchMode
    //{
    //    get
    //    {
    //        if (PlayerState.AreStarsGreyedOut && PedList.Cops.All(x => !x.RecentlySeenPlayer))
    //            return true;
    //        else
    //            return false;
    //    }
    //}
    public static float ActiveDistance
    {
        get
        {
            return 400f + (PlayerStateManager.WantedLevel * 200f);//500f
        }
    }
    private static float TimeToRecognizePlayer
    {
        get
        {
            if (PlayerStateManager.IsNightTime)
                return 3500;
            else if (PlayerStateManager.IsInVehicle)
                return 750;
            else
                return 2000;
        }
    }
    public static void Initialize()
    {
        AnyCanSeePlayer = false;
        AnyCanHearPlayerShooting = false;
        AnyCanRecognizePlayer  = false;
        AnyRecentlySeenPlayer = false;
        AnySeenPlayerCurrentWanted = false;
        PlaceLastSeenPlayer = default;
        WasPlayerLastSeenInVehicle = false;
        PlayerLastSeenHeading = 0f;
        PlayerLastSeenForwardVector = default;
        IsNightTime = false;
        PreviousWantedLevel = 0;
        IsRunning = true;
    }
    public static void Dispose()
    {
        IsRunning = false;    
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            UpdateCops();
            UpdateRecognition();
        }
    }
    private static void UpdateCops()
    {
        PedManager.Cops.RemoveAll(x => !x.Pedestrian.Exists());
        PedManager.K9Peds.RemoveAll(x => !x.Pedestrian.Exists());
        foreach (Cop Cop in PedManager.Cops)
        {
            Cop.Update();
            if (Cop.ShouldBustPlayer)
            {
                PlayerStateManager.StartManualArrest();
            }
        }
        foreach (Cop Cop in PedManager.Cops.Where(x => x.Pedestrian.IsDead))
        {
            PoliceSpawningManager.MarkNonPersistent(Cop);
        }
        PedManager.Cops.RemoveAll(x => x.Pedestrian.IsDead);
        PedManager.PoliceVehicles.RemoveAll(x => !x.Exists());
    }
    private static void UpdateRecognition()
    {
        AnyCanSeePlayer = PedManager.Cops.Any(x => x.CanSeePlayer);
        AnyCanHearPlayerShooting = PedManager.Cops.Any(x => x.WithinWeaponsAudioRange);

        if (AnyCanSeePlayer)
            AnyRecentlySeenPlayer = true;
        else
            AnyRecentlySeenPlayer = PedManager.Cops.Any(x => x.SeenPlayerSince(SettingsManager.MySettings.Police.PoliceRecentlySeenTime));

        AnyCanRecognizePlayer = PedManager.Cops.Any(x => x.TimeContinuoslySeenPlayer >= TimeToRecognizePlayer || (x.CanSeePlayer && x.DistanceToPlayer <= 20f) || (x.DistanceToPlayer <= 7f && x.DistanceToPlayer > 0.01f));

        if (!AnySeenPlayerCurrentWanted && AnyRecentlySeenPlayer)
            AnySeenPlayerCurrentWanted = true;

        if (AnyRecentlySeenPlayer)
        {
            if (!AnySeenPlayerCurrentWanted)
                PlaceLastSeenPlayer = WantedLevelManager.PlaceWantedStarted;
            else if (!PlayerStateManager.AreStarsGreyedOut)
                PlaceLastSeenPlayer = Game.LocalPlayer.Character.Position;
        }

        NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, PlaceLastSeenPlayer.X, PlaceLastSeenPlayer.Y, PlaceLastSeenPlayer.Z);
    }
    
}

