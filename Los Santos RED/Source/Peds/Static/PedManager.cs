using ExtensionsMethods;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


public static class PedManager
{
    private static int MinCivilianHealth = 70;//10;
    private static int MaxCivilianHealth = 100;//20;
    private static int MinCopHealth = 85;//15;
    private static int MaxCopHealth = 125;//25;
    private static int MinCopArmor = 0;//0;
    private static int MaxCopArmor = 50;//25;
    public static List<Cop> Cops { get; private set; }
    public static List<Cop> K9Peds { get; private set; }
    public static List<PedExt> Civilians { get; private set; }
    public static bool IsRunning { get; set; }
    public static int TotalSpawnedCops
    {
        get
        {
            return Cops.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
        }
    }
    public static bool AnyCopsNearPlayer
    {
        get
        {
            return Cops.Any(x => x.DistanceToPlayer <= 150f);
        }
    }
    public static bool AnyNooseUnitsSpawned
    {
        get
        {
            return Cops.Any(x => x.AssignedAgency.Initials == "NOOSE" && x.WasModSpawned);
        }
    }
    public static bool AnyArmyUnitsSpawned
    {
        get
        {
            return Cops.Any(x => x.AssignedAgency.Initials == "ARMY" && x.WasModSpawned);
        }
    }
    public static bool AnyHelicopterUnitsSpawned
    {
        get
        {
            return Cops.Any(x => x.IsInHelicopter && x.WasModSpawned);
        }
    }
    public static string AgenciesChasingPlayer
    {
        get
        {
            return string.Join(" ", Cops.Where(x => (x.SeenPlayerSince(10000) || (x.DistanceToPlayer <= 25f && x.DistanceToPlayer >= 1f)) && x.AssignedAgency != null).Select(x => x.AssignedAgency.ColoredInitials).Distinct().ToArray());
        }
    }
    public static void Initialize()
    {
        IsRunning = true;
        Cops = new List<Cop>();
        K9Peds = new List<Cop>();
        Civilians = new List<PedExt>();
    }
    public static void Dispose()
    {
        IsRunning = false;
        ClearPolice();
    }
    public static void Tick()
    {
        if (IsRunning)
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
    }
    public static void ClearPolice()
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
    public static PedExt GetCivilian(uint Handle)
    {
        if (Cops.Any(x => x.Pedestrian.Handle == Handle))
            return null;
        else
            return Civilians.FirstOrDefault(x => x.Pedestrian.Handle == Handle);
    }
    public static bool AnyCopsNearPosition(Vector3 Position, float Distance)
    {
        if (Position != Vector3.Zero && Cops.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Position) <= Distance))
            return true;
        else
            return false;
    }
    private static void AddCop(Ped Pedestrian)
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
            General.CreatedBlips.Add(myBlip);
        }

        SetCopStats(Pedestrian);

        Pedestrian.Inventory.Weapons.Clear();
        PoliceEquipmentManager.IssueWeapons(myCop);

        Cops.Add(myCop);
    }
    private static void SetCopStats(Ped Pedestrian)
    {
        if (SettingsManager.MySettings.Police.OverridePoliceAccuracy)
        {
            Pedestrian.Accuracy = SettingsManager.MySettings.Police.PoliceGeneralAccuracy;
        }     
        int DesiredHealth = General.MyRand.Next(MinCopHealth, MaxCopHealth) + 100;
        int DesiredArmor = General.MyRand.Next(MinCopArmor, MaxCopArmor);
        Pedestrian.MaxHealth = DesiredHealth;
        Pedestrian.Health = DesiredHealth;
        Pedestrian.Armor = DesiredArmor;
        NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Pedestrian, 281, true);//Can Writhe
        NativeFunction.CallByName<bool>("SET_PED_DIES_WHEN_INJURED", Pedestrian, false);
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 7, false);//No commandeering//https://gtaforums.com/topic/833391-researchguide-combat-behaviour-flags/
    }
    private static void AddCivilian(Ped Pedestrian)
    {
        SetCivilianStats(Pedestrian);
        Civilians.Add(new PedExt(Pedestrian, General.RandomPercent(10), General.RandomPercent(70)));
    }
    private static void SetCivilianStats(Ped Pedestrian)
    {
        if (SettingsManager.MySettings.Police.OverridePoliceAccuracy)
        {
            Pedestrian.Accuracy = SettingsManager.MySettings.Police.PoliceGeneralAccuracy;
        }
        int DesiredHealth = General.MyRand.Next(MinCivilianHealth, MaxCivilianHealth) + 100;           
        Pedestrian.MaxHealth = DesiredHealth;
        Pedestrian.Health = DesiredHealth;   
        Pedestrian.Armor = 0;
        NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Pedestrian, 281, true);//Can Writhe
        NativeFunction.CallByName<bool>("SET_PED_DIES_WHEN_INJURED", Pedestrian, false);
    }

}
