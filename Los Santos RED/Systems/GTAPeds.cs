using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


public static class GTAPeds
{
    private static int MinCivilianHealth = 70;//10;
    private static int MaxCivilianHealth = 100;//20;
    private static int MinCopHealth = 85;//15;
    private static int MaxCopHealth = 125;//25;
    private static int MinCopArmor = 0;//0;
    private static int MaxCopArmor = 50;//25;
    public static List<GTACop> CopPeds { get; private set; }
    public static List<GTACop> K9Peds { get; set; }
    public static List<Vehicle> PoliceVehicles { get; set; }
    public static List<GTAPed> Civilians { get; private set; }
    public static List<GTAPed> PlayerKilledCivilians { get; private set; }
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
        CopPeds = new List<GTACop>();
        K9Peds = new List<GTACop>();
        Civilians = new List<GTAPed>();
        PlayerKilledCivilians = new List<GTAPed>();
        PoliceVehicles = new List<Vehicle>();
    }
    public static void Dispose()
    {
        ClearPolice();
    }
    public static void ScanForPeds()
    {
        Ped[] Pedestrians = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 450f, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).Where(x => x is Ped).ToArray(), (x => (Ped)x));//250
        foreach (Ped Pedestrian in Pedestrians.Where(s => s.Exists() && !s.IsDead && s.IsVisible && s.IsHuman))
        {
            if(Pedestrian.IsPoliceArmy())
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
    private static void AddCop(Ped Pedestrian)
    {
        bool canSee = false;
        if (Pedestrian.PlayerIsInFront() && Pedestrian.IsInRangeOf(Game.LocalPlayer.Character.Position, 55f) && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Pedestrian, Game.LocalPlayer.Character))
            canSee = true;

        SetPedestrianStats(Pedestrian, true);

        Agency AssignedAgency = Agencies.GetAgencyFromPed(Pedestrian);

        if (AssignedAgency == null && !Pedestrian.Exists())
            return;

        GTACop myCop = new GTACop(Pedestrian, canSee, canSee ? Game.GameTime : 0, canSee ? Game.LocalPlayer.Character.Position : new Vector3(0f, 0f, 0f), Pedestrian.Health, AssignedAgency);
        Pedestrian.IsPersistent = false;
        if (LosSantosRED.MySettings.Police.OverridePoliceAccuracy)
            Pedestrian.Accuracy = LosSantosRED.MySettings.Police.PoliceGeneralAccuracy;
        Pedestrian.Inventory.Weapons.Clear();
        Police.IssueCopPistol(myCop);
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 7, false);//No commandeering//https://gtaforums.com/topic/833391-researchguide-combat-behaviour-flags/

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
        CopPeds.Add(myCop);

        if (LosSantosRED.MySettings.Police.IssuePoliceHeavyWeapons && Police.CurrentPoliceState == Police.PoliceState.DeadlyChase)
            Police.IssueCopHeavyWeapon(myCop);
    }
    private static void AddCivilian(Ped Pedestrian)
    {
        SetPedestrianStats(Pedestrian,false);
        Civilians.Add(new GTAPed(Pedestrian, false, Pedestrian.Health) { WillCallPolice = LosSantosRED.MyRand.Next(1, 11) <= 4, WillFight = LosSantosRED.MyRand.Next(1, 21) <= 1 });
    }
    private static void SetPedestrianStats(Ped Pedestrian,bool IsCop)
    {
        int DesiredHealth;
        int DesiredArmor;
        if (IsCop)
        {
            if (Pedestrian.Armor > 0)
                DesiredArmor = LosSantosRED.MyRand.Next(MinCopArmor, MaxCopArmor);
            else
                DesiredArmor = 0;
            DesiredHealth = LosSantosRED.MyRand.Next(MinCopHealth, MaxCopHealth);
        }
        else
        {
            DesiredArmor = 0;
            DesiredHealth = LosSantosRED.MyRand.Next(MinCivilianHealth, MaxCivilianHealth);
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
        Vehicle[] Vehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 450f, GetEntitiesFlags.ConsiderAllVehicles).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));//250
        foreach (Vehicle Veh in Vehicles.Where(s => s.Exists()))
        {
            if (Veh.IsPoliceVehicle)
            {
                if (!PoliceVehicles.Any(x => x.Handle == Veh.Handle))
                {
                    Agencies.ChangeLiveryAtZone(Veh, Zones.GetZoneAtLocation(Veh.Position));
                    PoliceSpawning.UpgradeCruiser(Veh);
                    PoliceVehicles.Add(Veh);
                }
            }
        }
    }
    public static void ClearPolice()
    {
        foreach (GTACop Cop in CopPeds)
        {
            if (Cop.Pedestrian.Exists())
            {
                if (Cop.Pedestrian.IsInAnyVehicle(false))
                    Cop.Pedestrian.CurrentVehicle.Delete();
                Cop.Pedestrian.Delete();
            }
        }
        CopPeds.Clear();
        foreach (GTACop Cop in K9Peds)
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
        foreach (GTACop Cop in K9Peds.Where(x => x.Pedestrian.Exists() && !x.Pedestrian.IsInHelicopter))
        {
            Cop.Pedestrian.Delete();
        }
        foreach (GTACop Cop in CopPeds.Where(x => x.Pedestrian.Exists() && !x.Pedestrian.IsInAnyVehicle(false) && !x.Pedestrian.IsInHelicopter))
        {
            Cop.Pedestrian.Delete();
        }
        foreach (GTACop Cop in CopPeds.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsInAnyVehicle(false) && !x.Pedestrian.IsInHelicopter))
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
