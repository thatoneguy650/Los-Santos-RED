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
    public override bool AutoCallsInUnconsciousPeds { get; set; } = true;
    public override ePedAlertType PedAlertTypes { get; set; } = ePedAlertType.UnconsciousBody | ePedAlertType.HelpCry;
    public IssuableWeapon GetRandomMeleeWeapon(IWeapons weapons) => AssignedAgency.GetRandomMeleeWeapon(weapons);
    public IssuableWeapon GetRandomWeapon(bool v, IWeapons weapons) => AssignedAgency.GetRandomWeapon(v, weapons);
    public Agency AssignedAgency { get; set; } = new Agency();
    public uint HasBeenSpawnedFor => Game.GameTime - GameTimeSpawned;
    public override bool KnowsDrugAreas => false;
    public override bool KnowsGangAreas => false;
    public override string BlipName => "Firefighter";
    public override Color BlipColor => AssignedAgency != null ? AssignedAgency.Color : base.BlipColor;
    public override bool WillCallPolice { get; set; } = true;
    public override bool WillCallPoliceIntense { get; set; } = true;
    public WeaponInventory WeaponInventory { get; private set; }
    public bool HasTaser { get; set; } = false;
    public bool IsUsingMountedWeapon { get; set; } = false;
    public bool IsRespondingToInvestigation { get; set; }
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
                    if (Settings.SettingsManager.FireSettings.AllowAlerts)
                    {
                        PedAlerts.Update(policeRespondable, world);
                    }
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
    protected override string GetPedInfoForDisplay()
    {
        string ExtraItems = base.GetPedInfoForDisplay();
        if (AssignedAgency != null)
        {
            ExtraItems += $"~n~Firefighter: {AssignedAgency.ShortName}";
        }
        return ExtraItems;
    }
    public override string InteractPrompt(IButtonPromptable player)
    {
        return $"Talk to {FormattedName}";
    }

}