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

public static class Investigation
{
    private static bool PrevIsInInvestigationMode;
    private static Vector3 PrevInvestigationPosition;
    private static uint GameTimeStartedInvestigation;
    private static uint GameTimeLastInvestigationExpired;
    private static Blip InvestigationBlip;

    public static float InvestigationDistance { get; set; }
    public static Vector3 InvestigationPosition { get; set; }
    public static float NearInvestigationDistance { get; set; }
    public static bool InInvestigationMode { get; set; }
    public static bool HavePlayerDescription { get; set; }
    public static bool IsRunning { get; set; } = true;
    public static bool InvestigationModeExpired
    {
        get
        {
            if (GameTimeStartedInvestigation == 0)
                return false;
            else if (Game.GameTime - GameTimeStartedInvestigation >= 180000)
                return true;
            else
                return false;
        }
    }
    public static bool IsSuspicious
    {
        get
        {
            if (!InInvestigationMode)
                return false;
            else if (InInvestigationMode && NearInvestigationPosition && HavePlayerDescription)
                return true;
            else
                return false;
        }
    }
    public static bool LastInvestigationRecentlyExpired
    {
        get
        {
            if (GameTimeLastInvestigationExpired == 0)
                return false;
            else if (Game.GameTime - GameTimeLastInvestigationExpired <= 5000)
                return true;
            else
                return false;
        }
    }

    public static bool NearInvestigationPosition
    {
        get
        {
            return InvestigationPosition != Vector3.Zero && Game.LocalPlayer.Character.DistanceTo2D(InvestigationPosition) <= NearInvestigationDistance;
        }
    }
    public static float DistanceToInvestigationPosition
    {
        get
        {
            if (!InInvestigationMode || InvestigationPosition == Vector3.Zero)
                return 9999f;
            else
                return Game.LocalPlayer.Character.DistanceTo2D(InvestigationPosition);
        }
    }
    public static void Initialize()
    {
        InvestigationPosition = Vector3.Zero;
        InvestigationDistance = 800f;//350f;
        PrevInvestigationPosition = Vector3.Zero;
        NearInvestigationDistance = 250f;
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
            InvestigationTick();
        }
    }
    private static void InvestigationTick()
    {
        if (PlayerState.IsWanted)
            InInvestigationMode = false;

        if (InvestigationModeExpired) //remove after 3 minutes
            InInvestigationMode = false;

        if (PrevInvestigationPosition != InvestigationPosition)
            InvestigationPositionChanged();

        if (PrevIsInInvestigationMode != InInvestigationMode)
            PoliceInInvestigationModeChanged();


        if (PlayerState.IsNotWanted && InInvestigationMode && NearInvestigationPosition && HavePlayerDescription && Police.AnyCanRecognizePlayer && WantedLevelScript.HasBeenNotWantedFor >= 5000)
        {
             WantedLevelScript.ApplyReportedCrimes();
        }
    }
    private static void InvestigationPositionChanged()
    {
        UpdateInvestigationUI();
        Debugging.WriteToLog("ValueChecker", string.Format("InvestigationPosition Changed to: {0}", InvestigationPosition));
        PrevInvestigationPosition = InvestigationPosition;
    }
    private static void PoliceInInvestigationModeChanged()
    {
        if (InInvestigationMode) //added
        {
            UpdateInvestigationUI();
            GameTimeStartedInvestigation = Game.GameTime;
        }
        else //removed
        {
            if (InvestigationBlip.Exists())
                InvestigationBlip.Delete();
            if (PlayerState.IsNotWanted)
            {
                HavePlayerDescription = false;
            }
            GameTimeStartedInvestigation = 0;
            GameTimeLastInvestigationExpired = Game.GameTime;
        }
        Debugging.WriteToLog("ValueChecker", string.Format("PoliceInInvestigationMode Changed to: {0}", InInvestigationMode));
        PrevIsInInvestigationMode = InInvestigationMode;
    }
    private static void UpdateInvestigationUI()
    {
        UpdateInvestigationPosition();
        AddUpdateInvestigationBlip(InvestigationPosition, NearInvestigationDistance);
    }
    private static void UpdateInvestigationPosition()
    {
        Vector3 SpawnLocation = Vector3.Zero;
        General.GetStreetPositionandHeading(InvestigationPosition, out SpawnLocation, out float Heading, false);
        if (SpawnLocation != Vector3.Zero)
            InvestigationPosition = SpawnLocation;
    } 
    private static void AddUpdateInvestigationBlip(Vector3 Position, float Size)
    {
        if (Position == Vector3.Zero)
        {
            if (InvestigationBlip.Exists())
                InvestigationBlip.Delete();
            return;
        }
        if (!InInvestigationMode)
        {
            if (InvestigationBlip.Exists())
                InvestigationBlip.Delete();
            return;
        }
        if (!InvestigationBlip.Exists())
        {
            InvestigationBlip = new Blip(Position, Size)
            {
                Name = "Investigation Center",
                Color = Color.Orange,
                Alpha = 0.25f
            };

            NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)InvestigationBlip.Handle, true);
            General.CreatedBlips.Add(InvestigationBlip);
        }
        if (InvestigationBlip.Exists())
            InvestigationBlip.Position = Position;
    } 
}

