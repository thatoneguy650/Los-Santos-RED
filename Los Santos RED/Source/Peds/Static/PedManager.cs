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
    public static List<Cop> K9Peds { get; set; }
    public static List<Vehicle> PoliceVehicles { get; set; }
    public static List<VehicleExt> CivilianVehicles { get; set; }
    public static List<PedExt> Civilians { get; private set; }
    public static bool IsRunning { get; set; } = true;
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
    public static bool AnyCopsNearPosition(Vector3 Position,float Distance)
    {
        if (Position != Vector3.Zero && Cops.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Position) <= Distance))
            return true;
        else
            return false;
    }
    public static bool CanSpawnHelicopter
    {
        get
        {
            return PoliceVehicles.Count(x => x.IsHelicopter) < SettingsManager.MySettings.Police.HelicopterLimit;
        }
    }
    public static bool CanSpawnBoat
    {
        get
        {
            return PedManager.PoliceVehicles.Count(x => x.IsBoat) < SettingsManager.MySettings.Police.BoatLimit;
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
        PoliceVehicles = new List<Vehicle>();
        CivilianVehicles = new List<VehicleExt>();
    }
    public static void Dispose()
    {
        IsRunning = false;
        ClearPolice();
    }
    public static void ScanForPeds()
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
    private static void AddCop(Ped Pedestrian)
    {
        Agency AssignedAgency = AgencyManager.GetAgency(Pedestrian);
        if (AssignedAgency == null && !Pedestrian.Exists())
            return;

        Cop myCop = new Cop(Pedestrian, Pedestrian.Health, AssignedAgency);

        if (Pedestrian.IsInAnyPoliceVehicle && Pedestrian.CurrentVehicle != null && Pedestrian.CurrentVehicle.IsPoliceVehicle)
        {
            Vehicle PoliceCar = Pedestrian.CurrentVehicle;
            if (!PoliceVehicles.Any(x => x.Handle == PoliceCar.Handle))
            {
                PoliceSpawningManager.UpdateLivery(PoliceCar, AssignedAgency);
                PoliceSpawningManager.UpgradeCruiser(PoliceCar);
                PoliceVehicles.Add(PoliceCar);
            }
        }

        if (SettingsManager.MySettings.Police.SpawnedAmbientPoliceHaveBlip && Pedestrian.Exists())
        {
            Blip myBlip = Pedestrian.AttachBlip();
            myBlip.Color = AssignedAgency.AgencyColor;
            myBlip.Scale = 0.6f;
            General.CreatedBlips.Add(myBlip);
        }

        SetPedestrianStats(Pedestrian, true);

        Pedestrian.Inventory.Weapons.Clear();
        PoliceEquipmentManager.IssueWeapons(myCop);

        Cops.Add(myCop);
    }
    private static void AddCivilian(Ped Pedestrian)
    {
        SetPedestrianStats(Pedestrian,false);
        Civilians.Add(new PedExt(Pedestrian, General.RandomPercent(15), General.RandomPercent(50)));
    }
    private static void SetPedestrianStats(Ped Pedestrian,bool IsCop)
    {
        int DesiredHealth;
        int DesiredArmor;
        if (IsCop)
        {
            if (Pedestrian.Armor > 0)
                DesiredArmor = General.MyRand.Next(MinCopArmor, MaxCopArmor);
            else
                DesiredArmor = 0;
            DesiredHealth = General.MyRand.Next(MinCopHealth, MaxCopHealth);

            if (SettingsManager.MySettings.Police.OverridePoliceAccuracy)
                Pedestrian.Accuracy = SettingsManager.MySettings.Police.PoliceGeneralAccuracy;

            NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 7, false);//No commandeering//https://gtaforums.com/topic/833391-researchguide-combat-behaviour-flags/
        }
        else
        {
            DesiredArmor = 0;
            DesiredHealth = General.MyRand.Next(MinCivilianHealth, MaxCivilianHealth);
        }
        DesiredHealth = DesiredHealth + 100;
       // Debugging.WriteToLog("SetPedestrianStats", string.Format("IsCop {0}, Current Health {1}, Max Health {2}, Desired {3}, Armor {4}, DesiredArmor {5}",IsCop, Pedestrian.Health, Pedestrian.MaxHealth, DesiredHealth,Pedestrian.Armor, DesiredArmor));
        Pedestrian.Health = DesiredHealth;
        Pedestrian.MaxHealth = DesiredHealth;
        Pedestrian.Armor = DesiredArmor;
        NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Pedestrian, 281, true);//Can Writhe
        //NativeFunction.CallByName<bool>("SET_PED_SUFFERS_CRITICAL_HITS", Pedestrian, true);
        NativeFunction.CallByName<bool>("SET_PED_DIES_WHEN_INJURED", Pedestrian, false);
    }
    public static void ScanForVehicles()
    {
        if (IsRunning)
        {
            Vehicle[] Vehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 450f, GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));//250
            foreach (Vehicle Veh in Vehicles.Where(s => s.Exists()))
            {
                if (Veh.IsPoliceVehicle)
                {
                    if (!PoliceVehicles.Any(x => x.Handle == Veh.Handle))
                    {
                        PoliceSpawningManager.UpdateLivery(Veh);
                        PoliceSpawningManager.UpgradeCruiser(Veh);
                        PoliceVehicles.Add(Veh);
                    }
                }
                else
                {
                    if (!CivilianVehicles.Any(x => x.VehicleEnt.Handle == Veh.Handle))
                    {
                        AddCivilianVehicle(Veh);
                    }
                }
            }
        }
    }
    private static void AddCivilianVehicle(Vehicle Car)
    {
        CivilianVehicles.Add(new VehicleExt(Car));
    }
    public static void ClearPolice()
    {
        foreach (Cop Cop in Cops)
        {
            if (Cop.Pedestrian.Exists())
            {
                if (Cop.Pedestrian.IsInAnyVehicle(false))
                    Cop.Pedestrian.CurrentVehicle.Delete();
                Cop.Pedestrian.Delete();
            }
        }
        Cops.Clear();
        foreach (Cop Cop in K9Peds)
        {
            if (Cop.Pedestrian.Exists())
            {
                if (Cop.Pedestrian.IsInAnyVehicle(false))
                    Cop.Pedestrian.CurrentVehicle.Delete();
                Cop.Pedestrian.Delete();
            }
        }
        K9Peds.Clear();
    }
    public static void ClearPoliceCompletely()
    {
        foreach (Cop Cop in K9Peds.Where(x => x.Pedestrian.Exists() && !x.Pedestrian.IsInHelicopter))
        {
            Cop.Pedestrian.Delete();
        }
        foreach (Cop Cop in Cops.Where(x => x.Pedestrian.Exists() && !x.Pedestrian.IsInAnyVehicle(false)))
        {
            Cop.Pedestrian.Delete();
        }
        foreach (Cop Cop in Cops.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsInAnyVehicle(false)))
        {
            Cop.Pedestrian.CurrentVehicle.Delete();
            Cop.Pedestrian.Delete();
        }
        Ped[] closestPed = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 400f, GetEntitiesFlags.ExcludePlayerPed | GetEntitiesFlags.ConsiderAnimalPeds).Where(x => x is Ped).ToArray(), (x => (Ped)x));
        foreach (Ped dog in closestPed)
        {
            dog.Delete();
        }
        Vehicle[] PoliceCars = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 500f, GetEntitiesFlags.ExcludePlayerVehicle | GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        foreach (Vehicle Veh in PoliceCars.Where(x => x.Exists() && x.IsPoliceVehicle))
        {
            Veh.Delete();
        }
    }
    public static PedExt GetCivilian(uint Handle)
    {
        if (Cops.Any(x => x.Pedestrian.Handle == Handle))
            return null;
        else
            return Civilians.FirstOrDefault(x => x.Pedestrian.Handle == Handle);
    }
}
