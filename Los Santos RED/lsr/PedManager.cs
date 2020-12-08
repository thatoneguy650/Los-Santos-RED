using ExtensionsMethods;
using LosSantosRED.lsr;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


public class PedManager
{
    private int MinCivilianHealth = 70;//10;
    private int MaxCivilianHealth = 100;//20;
    private int MinCopHealth = 85;//15;
    private int MaxCopHealth = 125;//25;
    private int MinCopArmor = 0;//0;
    private int MaxCopArmor = 50;//25;
    public List<Cop> Cops { get; private set; }
    public List<Cop> K9Peds { get; private set; }
    public List<PedExt> Civilians { get; private set; }
    public int TotalSpawnedCops
    {
        get
        {
            return Cops.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
        }
    }
    public bool AnyCopsNearPlayer
    {
        get
        {
            return Cops.Any(x => x.DistanceToPlayer <= 150f);
        }
    }
    public bool AnyNooseUnitsSpawned
    {
        get
        {
            return Cops.Any(x => x.AssignedAgency.Initials == "NOOSE" && x.WasModSpawned);
        }
    }
    public bool AnyArmyUnitsSpawned
    {
        get
        {
            return Cops.Any(x => x.AssignedAgency.Initials == "ARMY" && x.WasModSpawned);
        }
    }
    public bool AnyHelicopterUnitsSpawned
    {
        get
        {
            return Cops.Any(x => x.IsInHelicopter && x.WasModSpawned);
        }
    }
    public string AgenciesChasingPlayer
    {
        get
        {
            return string.Join(" ", Cops.Where(x => (x.SeenPlayerSince(10000) || (x.DistanceToPlayer <= 25f && x.DistanceToPlayer >= 1f)) && x.AssignedAgency != null).Select(x => x.AssignedAgency.ColoredInitials).Distinct().ToArray());
        }
    }
    public PedManager()
    {
        Cops = new List<Cop>();
        K9Peds = new List<Cop>();
        Civilians = new List<PedExt>();
    }
    public void Dispose()
    {
        ClearPolice();
    }
    public void Tick()
    {
        Ped[] Pedestrians = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 450f, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).Where(x => x is Ped).ToArray(), (x => (Ped)x));//250
        foreach (Ped Pedestrian in Pedestrians.Where(s => s.Exists() && !s.IsDead && s.IsVisible && s.IsHuman))
        {
            if (Pedestrian.IsPoliceArmy())
            {
                if (SearchModeStoppingManager.SpotterCop != null && SearchModeStoppingManager.SpotterCop.Handle == Pedestrian.Handle)
                    continue;

                if (!Cops.Any(x => x.Pedestrian == Pedestrian))
                {
                    AddCop(Pedestrian);
                }
            }
            else
            {
                if (!Civilians.Any(x => x.Pedestrian.Handle == Pedestrian.Handle))
                {
                    AddCivilian(Pedestrian);
                }
            }
        }    
    }
    public void ClearPolice()
    {
        foreach (Cop Cop in Cops)
        {
            if (Cop.Pedestrian.Exists())
            {
                Cop.Pedestrian.Delete();
            }
        }
        Cops.Clear();
    }
    public PedExt GetCivilian(uint Handle)
    {
        if (Cops.Any(x => x.Pedestrian.Handle == Handle))
            return null;
        else
            return Civilians.FirstOrDefault(x => x.Pedestrian.Handle == Handle);
    }
    public bool AnyCopsNearPosition(Vector3 Position, float Distance)
    {
        if (Position != Vector3.Zero && Cops.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Position) <= Distance))
            return true;
        else
            return false;
    }
    private void AddCop(Ped Pedestrian)
    {
        Agency AssignedAgency = AgencyManager.GetAgency(Pedestrian);
        if (AssignedAgency == null && !Pedestrian.Exists())
            return;

        Cop myCop = new Cop(Pedestrian, Pedestrian.Health, AssignedAgency);

        if (Pedestrian.IsInAnyPoliceVehicle && Pedestrian.CurrentVehicle != null && Pedestrian.CurrentVehicle.IsPoliceVehicle)
        {
            Vehicle PoliceCar = Pedestrian.CurrentVehicle;
            if (!VehicleManager.PoliceVehicles.Any(x => x.Handle == PoliceCar.Handle))
            {
                PoliceSpawningManager.UpdateLivery(PoliceCar, AssignedAgency);
                PoliceSpawningManager.UpgradeCruiser(PoliceCar);
                VehicleManager.PoliceVehicles.Add(PoliceCar);
            }
        }

        if (SettingsManager.MySettings.Police.SpawnedAmbientPoliceHaveBlip && Pedestrian.Exists())
        {
            Blip myBlip = Pedestrian.AttachBlip();
            myBlip.Color = AssignedAgency.AgencyColor;
            myBlip.Scale = 0.6f;
            Mod.Map.AddBlip(myBlip);
        }

        SetCopStats(Pedestrian);

        Pedestrian.Inventory.Weapons.Clear();
        PoliceEquipmentManager.IssueWeapons(myCop);

        Cops.Add(myCop);
    }
    private void SetCopStats(Ped Pedestrian)
    {
        if (SettingsManager.MySettings.Police.OverridePoliceAccuracy)
        {
            Pedestrian.Accuracy = SettingsManager.MySettings.Police.PoliceGeneralAccuracy;
        }     
        int DesiredHealth = RandomItems.MyRand.Next(MinCopHealth, MaxCopHealth) + 100;
        int DesiredArmor = RandomItems.MyRand.Next(MinCopArmor, MaxCopArmor);
        Pedestrian.MaxHealth = DesiredHealth;
        Pedestrian.Health = DesiredHealth;
        Pedestrian.Armor = DesiredArmor;
        NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Pedestrian, 281, true);//Can Writhe
        NativeFunction.CallByName<bool>("SET_PED_DIES_WHEN_INJURED", Pedestrian, false);
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 7, false);//No commandeering//https://gtaforums.com/topic/833391-researchguide-combat-behaviour-flags/
    }
    private void AddCivilian(Ped Pedestrian)
    {
        SetCivilianStats(Pedestrian);
        Civilians.Add(new PedExt(Pedestrian, RandomItems.RandomPercent(10), RandomItems.RandomPercent(70)));
    }
    private void SetCivilianStats(Ped Pedestrian)
    {
        if (SettingsManager.MySettings.Police.OverridePoliceAccuracy)
        {
            Pedestrian.Accuracy = SettingsManager.MySettings.Police.PoliceGeneralAccuracy;
        }
        int DesiredHealth = RandomItems.MyRand.Next(MinCivilianHealth, MaxCivilianHealth) + 100;           
        Pedestrian.MaxHealth = DesiredHealth;
        Pedestrian.Health = DesiredHealth;   
        Pedestrian.Armor = 0;
        NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Pedestrian, 281, true);//Can Writhe
        NativeFunction.CallByName<bool>("SET_PED_DIES_WHEN_INJURED", Pedestrian, false);
    }

}
