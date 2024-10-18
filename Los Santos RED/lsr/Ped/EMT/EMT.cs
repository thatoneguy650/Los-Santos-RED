using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;

public class EMT : PedExt
{
    public EMT(Ped pedestrian, ISettingsProvideable settings, int health, Agency agency, bool wasModSpawned, ICrimes crimes, IWeapons weapons, string name, IEntityProvideable world) : base(pedestrian,settings,crimes,weapons,name,"EMT",world)
    {
        Health = health;
        AssignedAgency = agency;
        WasModSpawned = wasModSpawned;
        PedReactions.IncludeUnconsciousAsMundane = false;
        PedBrain = new EMTBrain(this, Settings, world, weapons);
    }
    public override ePedAlertType PedAlertTypes { get; set; } = ePedAlertType.UnconsciousBody | ePedAlertType.HelpCry;
    public Agency AssignedAgency { get; set; } = new Agency();
    public override bool KnowsDrugAreas => false;
    public override bool KnowsGangAreas => false;
    public override bool WillCallPolice { get; set; } = true;
    public override bool WillCallPoliceIntense { get; set; } = true;
    public override string BlipName => "EMT";
    public override Color BlipColor => AssignedAgency != null ? AssignedAgency.Color : base.BlipColor;
    public override bool GenerateUnconsciousAlerts { get; set; } = false;
    public bool IsRespondingToInvestigation { get; set; }
    public override void Update(IPerceptable perceptable, IPoliceRespondable policeRespondable, Vector3 placeLastSeen, IEntityProvideable world)
    {
        PlayerToCheck = policeRespondable;
        if(!Pedestrian.Exists())
        {
            return;
        }
        if (Pedestrian.IsAlive)
        {
            if (NeedsFullUpdate)
            {
                IsInWrithe = Pedestrian.IsInWrithe;
                UpdatePositionData();
                PlayerPerception.Update(perceptable, placeLastSeen);
                UpdateVehicleState();
                if (!IsUnconscious && PlayerPerception.DistanceToTarget <= 200f)
                {
                    if (!PlayerPerception.RanSightThisUpdate && !Settings.SettingsManager.PerformanceSettings.EnableIncreasedUpdateMode)
                    {
                        GameFiber.Yield();
                    }
                    if (Settings.SettingsManager.EMSSettings.AllowAlerts)
                    {
                        PedAlerts.Update(policeRespondable, world);
                    }
                }
                GameTimeLastUpdated = Game.GameTime;
            }
        }
        CurrentHealthState.Update(policeRespondable);//has a yield if they get damaged, seems ok 
    }
    public void SetStats(DispatchablePerson dispatchablePerson, IWeapons Weapons, bool addBlip)
    {
        if (!Pedestrian.Exists())
        {
            return;
        }
        if (addBlip)
        {
            AddBlip();
        }
        if (Pedestrian.Exists() && Settings.SettingsManager.CivilianSettings.SightDistance > 60f)
        {
            NativeFunction.Natives.SET_PED_SEEING_RANGE(Pedestrian, Settings.SettingsManager.CivilianSettings.SightDistance);
        }
    }
    protected override string GetPedInfoForDisplay()
    {
        string ExtraItems = base.GetPedInfoForDisplay();
        if (AssignedAgency != null)
        {
            ExtraItems += $"~n~EMT: {AssignedAgency.ShortName}";
        }
        return ExtraItems;
    }
    public override string InteractPrompt(IButtonPromptable player)
    {
        return $"Talk to {FormattedName}";
    }
    public override void MatchPlayerPedType(IPedSwappable Player)
    {
        Player.RemoveAgencyStatus();
        Player.SetAgencyStatus(AssignedAgency);
    }
}