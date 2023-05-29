using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;
using System.Drawing;

public class Firefighter : PedExt, IWeaponIssuable
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
        WeaponInventory = new WeaponInventory(this, Settings);
        PedBrain = new FirefighterBrain(this, Settings, world, weapons);
    }
    public IssuableWeapon GetRandomMeleeWeapon(IWeapons weapons) => AssignedAgency.GetRandomMeleeWeapon(weapons);
    public IssuableWeapon GetRandomWeapon(bool v, IWeapons weapons) => AssignedAgency.GetRandomWeapon(v, weapons);
    public Agency AssignedAgency { get; set; } = new Agency();
    public uint HasBeenSpawnedFor => Game.GameTime - GameTimeSpawned;
    public override bool KnowsDrugAreas => false;
    public override bool KnowsGangAreas => false;
    public override Color BlipColor => AssignedAgency != null ? AssignedAgency.Color : base.BlipColor;
    public override bool WillCallPolice { get; set; } = true;
    public override bool WillCallPoliceIntense { get; set; } = true;
    public WeaponInventory WeaponInventory { get; private set; }
    public bool HasTaser { get; set; } = false;
    public bool IsUsingMountedWeapon { get; set; } = false;

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
                UpdateVehicleState();
                if (!IsUnconscious && PlayerPerception.DistanceToTarget <= 200f)
                {
                    if (!PlayerPerception.RanSightThisUpdate)
                    {
                        GameFiber.Yield();
                    }
                    UpdateAlerts(perceptable, policeRespondable, world);
                }
                GameTimeLastUpdated = Game.GameTime;
            }
        }
        CurrentHealthState.Update(policeRespondable);//has a yield if they get damaged, seems ok 
    }
    protected override void UpdateAlerts(IPerceptable perceptable, IPoliceRespondable policeRespondable, IEntityProvideable world)
    {
        if (Settings.SettingsManager.FireSettings.AllowToCallEMTsOnBodies)
        {
            PedAlerts.LookForUnconsciousPeds(world);
        }
        if (PedAlerts.HasSeenUnconsciousPed)
        {
            perceptable.AddMedicalEvent(PedAlerts.PositionLastSeenUnconsciousPed);
            PedAlerts.HasSeenUnconsciousPed = false;
        }
    }
    public void SetStats(DispatchablePerson dispatchablePerson, IShopMenus shopMenus, IWeapons Weapons, bool addBlip)
    {
        dispatchablePerson.SetPedExtPermanentStats(this, Settings.SettingsManager.CivilianSettings.OverrideHealth, false, Settings.SettingsManager.CivilianSettings.OverrideAccuracy);
        if (!Pedestrian.Exists())
        {
            return;
        }
        if (!IsAnimal)
        {
            WeaponInventory.IssueWeapons(Weapons, true, false, false, dispatchablePerson);
            GameFiber.Yield();
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
}