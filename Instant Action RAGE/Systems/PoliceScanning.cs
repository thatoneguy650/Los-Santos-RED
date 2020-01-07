using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


public static class PoliceScanning
{
    public static List<GTACop> CopPeds { get; private set; }
    public static List<GTACop> K9Peds { get; set; }
    public static List<Vehicle> PoliceVehicles { get; set; }
    public static List<GTAPed> Civilians { get; private set; }
    public static string AgenciesChasingPlayer
    {
        get
        {
            return string.Join(" ", CopPeds.Where(x => x.SeenPlayerSince(10000)).Select(x => (x.isInHelicopter ? "~b~LSPD-ASD~s~" : x.AssignedAgency.ColoredInitials)).Distinct().ToArray());
        }
    }
    public static void Initialize()
    {
        CopPeds = new List<GTACop>();
        K9Peds = new List<GTACop>();
        Civilians = new List<GTAPed>();
        PoliceVehicles = new List<Vehicle>();
    }
    public static void Tick()
    {
        ScanForPolice();
    }
    public static void Dispose()
    {
        ClearPolice();
    }
    public static void ScanForPolice()
    {
        Ped[] Pedestrians = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 450f, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).Where(x => x is Ped).ToArray(), (x => (Ped)x));//250
        foreach (Ped Pedestrian in Pedestrians.Where(s => s.Exists() && !s.IsDead && s.IsVisible))
        {
            if(Pedestrian.IsPoliceArmy())
            {
                if (SearchModeStopping.SpotterCop != null && SearchModeStopping.SpotterCop.Handle == Pedestrian.Handle)
                    continue;

                if (!CopPeds.Any(x => x.Pedestrian == Pedestrian))
                {
                    bool canSee = false;
                    if (Pedestrian.PlayerIsInFront() && Pedestrian.IsInRangeOf(Game.LocalPlayer.Character.Position, 55f) && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", Pedestrian, Game.LocalPlayer.Character))
                        canSee = true;
                    Agency AssignedAgency = Agencies.GetAgencyFromPed(Pedestrian, true);
                    GTACop myCop = new GTACop(Pedestrian, canSee, canSee ? Game.GameTime : 0, canSee ? Game.LocalPlayer.Character.Position : new Vector3(0f, 0f, 0f), Pedestrian.Health, AssignedAgency);
                    Pedestrian.IsPersistent = false;
                    if (Settings.OverridePoliceAccuracy)
                        Pedestrian.Accuracy = Settings.PoliceGeneralAccuracy;
                    Pedestrian.Inventory.Weapons.Clear();
                    Police.IssueCopPistol(myCop);
                    NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 7, false);//No commandeering//https://gtaforums.com/topic/833391-researchguide-combat-behaviour-flags/


                    if (Pedestrian.IsInAnyPoliceVehicle && Pedestrian.CurrentVehicle != null && Pedestrian.CurrentVehicle.IsPoliceVehicle)
                    {
                        Vehicle PoliceCar = Pedestrian.CurrentVehicle;

                        if (!PoliceVehicles.Any(x => x.Handle == PoliceCar.Handle))
                        {
                            PoliceSpawning.CheckandChangeLivery(PoliceCar, AssignedAgency);
                            PoliceSpawning.UpgradeCruiser(PoliceCar);
                            PoliceVehicles.Add(PoliceCar);
                        }
                    }   
                    CopPeds.Add(myCop);

                    if (Settings.IssuePoliceHeavyWeapons && Police.CurrentPoliceState == Police.PoliceState.DeadlyChase)
                        Police.IssueCopHeavyWeapon(myCop);
                }
            }
            else
            {
                if (!Civilians.Any(x => x.Pedestrian.Handle == Pedestrian.Handle))
                {
                    Civilians.Add(new GTAPed(Pedestrian, false, Pedestrian.Health));
                }
            }
        }  
            //Police.UpdatedCopsStats();
    }
    public static void ScanforPoliceVehicles()
    {
        Vehicle[] Vehicles = Array.ConvertAll(World.GetEntities(Game.LocalPlayer.Character.Position, 250f, GetEntitiesFlags.ConsiderGroundVehicles).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));//250
        foreach (Vehicle Veh in Vehicles.Where(s => s.Exists()))
        {
            if (Veh.IsPoliceVehicle)
            {
                if (!PoliceVehicles.Any(x => x.Handle == Veh.Handle))
                {
                    Agency AssignedAgency = Agencies.GetAgencyFromEmptyVehicle(Veh);
                    Debugging.WriteToLog("LiveryChanger", "CheckingLivery");
                    PoliceSpawning.CheckandChangeLivery(Veh, AssignedAgency);
                    PoliceSpawning.UpgradeCruiser(Veh);
                    PoliceVehicles.Add(Veh);
                }
            }

        }
    }
    public static void ClearPoliceAroundArea(Vector3 Location,float Radius)
    {
        foreach (GTACop Cop in CopPeds.Where(x => x.Pedestrian.DistanceTo2D(Location) <= Radius))
        {
            if (Cop.Pedestrian.Exists())
            {
                if (Cop.Pedestrian.IsInAnyVehicle(false))
                    Cop.Pedestrian.CurrentVehicle.Delete();
                Cop.Pedestrian.Delete();
            }
        }
        Vehicle[] Vehicles = Array.ConvertAll(World.GetEntities(Location, Radius, GetEntitiesFlags.ConsiderAllVehicles | GetEntitiesFlags.ExcludePlayerVehicle).Where(x => x is Vehicle).ToArray(), (x => (Vehicle)x));
        foreach (Vehicle MyVehicle in Vehicles.Where(s => s.Exists()))
        {
            if(MyVehicle.IsPoliceVehicle)
            {
                MyVehicle.Delete();
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
        //Tasking.UntaskAll(true);
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
