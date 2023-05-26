using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;
using System.Drawing;

public class Firefighter : PedExt
{
    private uint GameTimeSpawned;
    public Firefighter(Ped pedestrian, ISettingsProvideable settings, int health, Agency agency, bool wasModSpawned, ICrimes crimes, IWeapons weapons, string name, IEntityProvideable world) : base(pedestrian,settings, crimes, weapons, name, "FireFighter", world)
    {
        Health = health;
        AssignedAgency = agency;
        WasModSpawned = wasModSpawned;
        if (WasModSpawned)
        {
            GameTimeSpawned = Game.GameTime;
        }
    }
    public Agency AssignedAgency { get; set; } = new Agency();
    public uint HasBeenSpawnedFor => Game.GameTime - GameTimeSpawned;
    public override bool KnowsDrugAreas => false;
    public override bool KnowsGangAreas => false;
    public override Color BlipColor => AssignedAgency != null ? AssignedAgency.Color : base.BlipColor;
    public override bool WillCallPolice { get; set; } = true;
    public override bool WillCallPoliceIntense { get; set; } = true;
    public override void Update(IPerceptable perceptable, IPoliceRespondable policeRespondable, Vector3 placeLastSeen, IEntityProvideable world)
    {
        PlayerToCheck = policeRespondable;
        if (!Pedestrian.Exists())
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
                if (Settings.SettingsManager.PerformanceSettings.EMSUpdatePerformanceMode1 && !PlayerPerception.RanSightThisUpdate)
                {
                    GameFiber.Yield();//TR TEST 30
                }
                if (Settings.SettingsManager.PerformanceSettings.IsEMSYield1Active)
                {
                    GameFiber.Yield();//TR TEST 30
                }
                UpdateVehicleState();
                if (Settings.SettingsManager.PerformanceSettings.IsEMSYield2Active)
                {
                    GameFiber.Yield();
                }
                if (Settings.SettingsManager.PerformanceSettings.EMSUpdatePerformanceMode2 && !PlayerPerception.RanSightThisUpdate)
                {
                    GameFiber.Yield();//TR TEST 30
                }
                if (Settings.SettingsManager.EMSSettings.AllowEMTsToCallEMTsOnBodies)
                {
                    PedAlerts.LookForUnconsciousPeds(world);
                }
                GameTimeLastUpdated = Game.GameTime;
            }
        }
        CurrentHealthState.Update(policeRespondable);//has a yield if they get damaged, seems ok 
    }

    public void SetStats(DispatchablePerson dispatchablePerson, IShopMenus shopMenus, IWeapons Weapons, bool addBlip)
    {
        dispatchablePerson.SetPedExtPermanentStats(this, Settings.SettingsManager.CivilianSettings.OverrideHealth, false, Settings.SettingsManager.CivilianSettings.OverrideAccuracy);
        if (!Pedestrian.Exists())
        {
            return;
        }
        if (string.IsNullOrEmpty(AssignedAgency.MemberName) && AssignedAgency.MemberName != "")
        {
            GroupName = AssignedAgency.MemberName;
        }
        if (addBlip)
        {
            AddBlip();
        }
    }
    //public bool WasModSpawned { get; private set; }
}