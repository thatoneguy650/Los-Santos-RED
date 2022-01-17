using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Drawing;

public class Investigation
{
    private uint GameTimeLastInvestigationExpired;
    private uint GameTimeStartedInvestigation;
    private Blip InvestigationBlip;
    private IPoliceRespondable Player;
    private ISettingsProvideable Settings;
    private IEntityProvideable World;
    private bool HavePlayerDescription = false;
    private uint GameTimeLastUpdatedInvestigation;

    private Color blipColor => IsSuspicious? Color.Orange : Color.Yellow;
    public Investigation(IPoliceRespondable player, ISettingsProvideable settings, IEntityProvideable world)
    {
        Player = player;
        Settings = settings;
        World = world;
    }
    public string DebugText => $"Invest: IsActive {IsActive} IsSus {IsSuspicious} Distance {Distance} Position {Position}";
    public float Distance => Settings.SettingsManager.InvestigationSettings.ActiveDistance;
    public bool IsActive { get; private set; }
    public bool IsSuspicious => IsActive && IsNearPosition && HavePlayerDescription;
    public Vector3 Position { get; private set; }
    private bool IsOutsideInvestigationRange { get; set; }
    private bool IsTimedOut => GameTimeStartedInvestigation != 0 && Game.GameTime - GameTimeStartedInvestigation >= Settings.SettingsManager.InvestigationSettings.TimeLimit;//60000;//short for testing was 180000
    public bool IsNearPosition { get; private set; }
    public void Dispose()
    {
        if (InvestigationBlip.Exists())
        {
            InvestigationBlip.Delete();
        }
    }
    public void Reset()
    {
        IsActive = false;
        HavePlayerDescription = false;
        GameTimeStartedInvestigation = 0;
        GameTimeLastInvestigationExpired = 0;
        if (InvestigationBlip.Exists())
        {
            InvestigationBlip.Delete();
        }
    }
    public void Start(Vector3 postionToInvestigate, bool havePlayerDescription)
    {
        if (Player.IsNotWanted)
        {
            Position = NativeHelper.GetStreetPosition(postionToInvestigate);
            GameFiber.Yield();
            if (havePlayerDescription)
            {
                HavePlayerDescription = havePlayerDescription;
            }
            if (!IsActive)
            {
                IsActive = true;
                GameTimeStartedInvestigation = Game.GameTime;
                if (Settings.SettingsManager.InvestigationSettings.CreateBlip)
                {
                    InvestigationBlip = new Blip(Position, 250f)
                    {
                        Name = "Investigation Center",
                        Color = blipColor,
                        Alpha = 0.25f
                    };
                    NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
                    NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME("Investigation Center");
                    NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(InvestigationBlip);
                    NativeFunction.Natives.SET_BLIP_AS_SHORT_RANGE((uint)InvestigationBlip.Handle, true);
                    GameFiber.Yield();
                }
                EntryPoint.WriteToConsole($"PLAYER EVENT: INVESTIGATION START", 3);
            }
        }
    }
    public void Update()
    {
        IsNearPosition = Position != Vector3.Zero && Player.Character.DistanceTo2D(Position) <= Settings.SettingsManager.InvestigationSettings.SuspiciousDistance;
        IsOutsideInvestigationRange = Position == Vector3.Zero || Game.LocalPlayer.Character.DistanceTo2D(Position) > Settings.SettingsManager.InvestigationSettings.MaxDistance;
        if (IsActive && Player.IsNotWanted)
        {
            if ((IsTimedOut && !World.AnyWantedCiviliansNearPlayer) || IsOutsideInvestigationRange) //remove after 3 minutes
            {
                Expire();
            }
            if (IsSuspicious && Player.AnyPoliceCanRecognizePlayer && Player.PoliceResponse.HasBeenNotWantedFor >= 5000)
            {
                Player.PoliceResponse.ApplyReportedCrimes();
            }
        }
        UpdateBlip();
        if (Settings.SettingsManager.DebugSettings.PrintUpdateTimes)
        {
            EntryPoint.WriteToConsole($"Investigation Update (Secondary Tasks) 250ms? Ran Time Since {Game.GameTime - GameTimeLastUpdatedInvestigation}", 5);
        }
        GameTimeLastUpdatedInvestigation = Game.GameTime;
    }
    private void UpdateBlip()
    {
        if (IsActive && Player.IsNotWanted && Settings.SettingsManager.InvestigationSettings.CreateBlip)
        {
            if (!InvestigationBlip.Exists())
            {
                InvestigationBlip = new Blip(Position, 250f)
                {
                    Name = "Investigation Center",
                    Color = blipColor,//Color.Yellow,
                    Alpha = 0.25f
                };
                NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
                NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME("Investigation Center");
                NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(InvestigationBlip);
                NativeFunction.Natives.SET_BLIP_AS_SHORT_RANGE((uint)InvestigationBlip.Handle, true);
                GameFiber.Yield();
            }
            else
            {
                InvestigationBlip.Position = Position;
                InvestigationBlip.Color = blipColor;
            }
        }
        else
        {
            if (InvestigationBlip.Exists())
            {
                InvestigationBlip.Delete();
            }
        }
    }
    private void Expire()
    {
        IsActive = false;
        HavePlayerDescription = false;
        GameTimeStartedInvestigation = 0;
        GameTimeLastInvestigationExpired = Game.GameTime;
        if (InvestigationBlip.Exists())
        {
            InvestigationBlip.Delete();
        }
        Player.OnInvestigationExpire();
    }
}

