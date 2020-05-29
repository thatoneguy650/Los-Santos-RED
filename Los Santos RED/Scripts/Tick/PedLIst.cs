using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


public static class PedList
{
    private static int MinCivilianHealth = 70;//10;
    private static int MaxCivilianHealth = 100;//20;
    private static int MinCopHealth = 85;//15;
    private static int MaxCopHealth = 125;//25;
    private static int MinCopArmor = 0;//0;
    private static int MaxCopArmor = 50;//25;
    public static List<Cop> CopPeds { get; private set; }
    public static List<Cop> K9Peds { get; set; }
    public static List<Vehicle> PoliceVehicles { get; set; }
    public static List<PedExt> Civilians { get; private set; }
    public static bool IsRunning { get; set; } = true;
    public static int TotalSpawnedCops
    {
        get
        {
            return CopPeds.Where(x => x.WasModSpawned).Count();
        }
    }
    public static string AgenciesChasingPlayer
    {
        get
        {
            return string.Join(" ", CopPeds.Where(x => (x.SeenPlayerSince(10000) || (x.DistanceToPlayer <= 25f && x.DistanceToPlayer >= 1f)) && x.AssignedAgency != null).Select(x => x.AssignedAgency.ColoredInitials).Distinct().ToArray());
        }
    }
    public static void Initialize()
    {
        IsRunning = true;
        CopPeds = new List<Cop>();
        K9Peds = new List<Cop>();
        Civilians = new List<PedExt>();
        PoliceVehicles = new List<Vehicle>();
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
                    if (SearchModeStopping.SpotterCop != null && SearchModeStopping.SpotterCop.Handle == Pedestrian.Handle)
                        continue;

                    if (!CopPeds.Any(x => x.Pedestrian == Pedestrian))
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
        Agency AssignedAgency = Agencies.DetermineAgency(Pedestrian);
        if (AssignedAgency == null && !Pedestrian.Exists())
            return;

        Cop myCop = new Cop(Pedestrian, Pedestrian.Health, AssignedAgency);

        if (Pedestrian.IsInAnyPoliceVehicle && Pedestrian.CurrentVehicle != null && Pedestrian.CurrentVehicle.IsPoliceVehicle)
        {
            Vehicle PoliceCar = Pedestrian.CurrentVehicle;
            if (!PoliceVehicles.Any(x => x.Handle == PoliceCar.Handle))
            {
                Agencies.ChangeLivery(PoliceCar, AssignedAgency);
                PoliceSpawning.UpgradeCruiser(PoliceCar);
                PoliceVehicles.Add(PoliceCar);
            }
        }

        SetPedestrianStats(Pedestrian, true);

        Pedestrian.Inventory.Weapons.Clear();
        myCop.IssuePistol();

        if (General.MySettings.Police.IssuePoliceHeavyWeapons && WantedLevelScript.CurrentPoliceState == WantedLevelScript.PoliceState.DeadlyChase)
            myCop.IssueHeavyWeapon();

        CopPeds.Add(myCop);
    }
    private static void AddCivilian(Ped Pedestrian)
    {
        SetPedestrianStats(Pedestrian,false);
        Civilians.Add(new PedExt(Pedestrian, Pedestrian.Health) { WillCallPolice = General.MyRand.Next(1, 11) <= 4, WillFight = General.MyRand.Next(1, 21) <= 1 });
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

            if (General.MySettings.Police.OverridePoliceAccuracy)
                Pedestrian.Accuracy = General.MySettings.Police.PoliceGeneralAccuracy;

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
    public static void ScanforPoliceVehicles()
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
                        Agencies.ChangeLivery(Veh, Agencies.GetAgencyFromVehicle(Veh));
                        PoliceSpawning.UpgradeCruiser(Veh);
                        PoliceVehicles.Add(Veh);
                    }
                }
            }
        }
    }
    public static void ClearPolice()
    {
        foreach (Cop Cop in CopPeds)
        {
            if (Cop.Pedestrian.Exists())
            {
                if (Cop.Pedestrian.IsInAnyVehicle(false))
                    Cop.Pedestrian.CurrentVehicle.Delete();
                Cop.Pedestrian.Delete();
            }
        }
        CopPeds.Clear();
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
        foreach (Cop Cop in CopPeds.Where(x => x.Pedestrian.Exists() && !x.Pedestrian.IsInAnyVehicle(false)))
        {
            Cop.Pedestrian.Delete();
        }
        foreach (Cop Cop in CopPeds.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsInAnyVehicle(false)))
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
}
