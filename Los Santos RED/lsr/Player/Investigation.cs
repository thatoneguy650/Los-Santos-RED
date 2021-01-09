﻿using LosSantosRED.lsr;
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
    private float NearInvestigationDistance = 250f;
    private IPoliceRespondable Player;
    private Blip InvestigationBlip;
    public Investigation(IPoliceRespondable player)
    {
        Player = player;
    }
    public float Distance { get; private set; } = 800f;
    public float DistanceToInvestigationPosition => !IsActive || Position == Vector3.Zero ? 9999f : Game.LocalPlayer.Character.DistanceTo2D(Position);
    public bool HaveDescription { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsSuspicious => IsActive && NearInvestigationPosition && HaveDescription;
    public bool LastInvestigationRecentlyExpired => GameTimeLastInvestigationExpired != 0 && Game.GameTime - GameTimeLastInvestigationExpired <= 5000;
    public Vector3 Position { get; private set; }
    private bool IsTimedOut => GameTimeStartedInvestigation != 0 && Game.GameTime - GameTimeStartedInvestigation >= 180000;
    private bool NearInvestigationPosition => Position != Vector3.Zero && Game.LocalPlayer.Character.DistanceTo2D(Position) <= NearInvestigationDistance;
    public void Reset()
    {
        IsActive = false;
        HaveDescription = false;
        GameTimeStartedInvestigation = 0;
        GameTimeLastInvestigationExpired = 0;
        if (InvestigationBlip.Exists())
        {
            InvestigationBlip.Delete();
        }
    }
    public void Dispose()
    {
        if (InvestigationBlip.Exists())
        {
            InvestigationBlip.Delete();
        }
    }
    public void Start()
    {
        if (Player.IsNotWanted && !IsActive)
        {
            SetActive();
        }
        else if (IsActive)
        {
            Position = Natives.GetStreetPosition(Player.PoliceResponse.PlaceLastReportedCrime);
            HaveDescription = Player.PoliceResponse.PoliceHaveDescription;
        }
    }
    public void Update()
    {
        if (IsActive && Player.IsNotWanted)
        {
            if (IsTimedOut) //remove after 3 minutes
            {
                Expire();
            }
            if (NearInvestigationPosition && HaveDescription && Player.AnyPoliceCanRecognizePlayer && Player.PoliceResponse.HasBeenNotWantedFor >= 5000)
            {
                Player.PoliceResponse.ApplyReportedCrimes();
            }
        }
    }
    private void Expire()
    {
        IsActive = false;
        HaveDescription = false;
        GameTimeStartedInvestigation = 0;
        GameTimeLastInvestigationExpired = Game.GameTime;
        if (InvestigationBlip.Exists())
        {
            InvestigationBlip.Delete();
        }
    }
    private void SetActive()
    {
        IsActive = true;
        Position = Natives.GetStreetPosition(Player.PoliceResponse.PlaceLastReportedCrime);
        HaveDescription = Player.PoliceResponse.PoliceHaveDescription;
        GameTimeStartedInvestigation = Game.GameTime;
        InvestigationBlip = new Blip(Position, 250f)
        {
            Name = "Investigation Center",
            Color = Color.Yellow,
            Alpha = 0.25f
        };
        NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)InvestigationBlip.Handle, true);
    }
}
