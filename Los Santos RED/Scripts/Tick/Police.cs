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

public static class Police
{
    public static bool IsRunning { get; set; } = true;
    public static bool AnyCanSeePlayer { get; set; }
    public static bool AnyCanRecognizePlayer { get; set; }
    public static bool AnyRecentlySeenPlayer { get; set; }
    public static bool AnySeenPlayerCurrentWanted { get; set; }
    public static Vector3 PlaceLastSeenPlayer { get; set; }
    public static Vector3 PlayerLastSeenForwardVector { get; set; }
    public static bool IsNightTime { get; set; }
    public static int PreviousWantedLevel { get; set; }  
    public static bool WasPlayerLastSeenInVehicle { get; set; }
    public static float PlayerLastSeenHeading { get; set; }
    public static bool InSearchMode
    {
        get
        {
            if (PlayerState.AreStarsGreyedOut && PedList.CopPeds.All(x => !x.RecentlySeenPlayer()))
                return true;
            else
                return false;
        }
    }
    public static float ActiveDistance
    {
        get
        {
            return PlayerState.WantedLevel * 500f;
        }
    }
    private static float TimeToRecognizePlayer
    {
        get
        {
            if (PlayerState.IsNightTime)
                return 3500;
            else if (PlayerState.IsInVehicle)
                return 750;
            else
                return 2000;
        }
    }
    public static void Initialize()
    {
        AnyCanSeePlayer = false;
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
        PedList.CopPeds.RemoveAll(x => !x.Pedestrian.Exists());
        PedList.K9Peds.RemoveAll(x => !x.Pedestrian.Exists() || x.Pedestrian.IsDead);
        foreach (Cop Cop in PedList.CopPeds)
        {
            Cop.Update();
            Cop.SetChaseStatus();
        }
        foreach (Cop Cop in PedList.CopPeds.Where(x => x.Pedestrian.IsDead))
        {
            PoliceSpawning.MarkNonPersistent(Cop);
        }
        PedList.CopPeds.RemoveAll(x => x.Pedestrian.IsDead);
        PedList.PoliceVehicles.RemoveAll(x => !x.Exists());
    }
    private static void UpdateRecognition()
    {
        AnyCanSeePlayer = PedList.CopPeds.Any(x => x.CanSeePlayer);

        if (AnyCanSeePlayer)
            AnyRecentlySeenPlayer = true;
        else
            AnyRecentlySeenPlayer = PedList.CopPeds.Any(x => x.SeenPlayerSince(General.MySettings.Police.PoliceRecentlySeenTime));

        AnyCanRecognizePlayer = PedList.CopPeds.Any(x => x.HasSeenPlayerFor >= TimeToRecognizePlayer || (x.CanSeePlayer && x.DistanceToPlayer <= 20f) || (x.DistanceToPlayer <= 7f && x.DistanceToPlayer > 0f));

        if (!AnySeenPlayerCurrentWanted && AnyRecentlySeenPlayer)
            AnySeenPlayerCurrentWanted = true;

        if (!AnySeenPlayerCurrentWanted)
            PlaceLastSeenPlayer = WantedLevelScript.PlaceWantedStarted;
        else if (!PlayerState.AreStarsGreyedOut)
            PlaceLastSeenPlayer = Game.LocalPlayer.Character.Position;

        NativeFunction.CallByName<bool>("SET_PLAYER_WANTED_CENTRE_POSITION", Game.LocalPlayer, PlaceLastSeenPlayer.X, PlaceLastSeenPlayer.Y, PlaceLastSeenPlayer.Z);
    }
}

