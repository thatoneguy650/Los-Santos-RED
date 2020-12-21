using ExtensionsMethods;
using LosSantosRED.lsr;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


public class Pedestrians
{
    private int MinCivilianHealth = 70;//10;
    private int MaxCivilianHealth = 100;//20;
    private int MinCopHealth = 85;//15;
    private int MaxCopHealth = 125;//25;
    private int MinCopArmor = 0;//0;
    private int MaxCopArmor = 50;//25;
    public List<Cop> Police { get; private set; } = new List<Cop>();
    public List<PedExt> Civilians { get; private set; } = new List<PedExt>();
    public int TotalSpawnedCops
    {
        get
        {
            return Police.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
        }
    }
    public bool ShouldBustPlayer
    {
        get
        {
            return Police.Any(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive && x.ShouldBustPlayer);
        }
    }
    public bool AnyCopsNearPlayer
    {
        get
        {
            return Police.Any(x => x.DistanceToPlayer <= 150f);
        }
    }
    public bool AnyNooseUnitsSpawned
    {
        get
        {
            return Police.Any(x => x.AssignedAgency.Initials == "NOOSE" && x.WasModSpawned);
        }
    }
    public bool AnyArmyUnitsSpawned
    {
        get
        {
            return Police.Any(x => x.AssignedAgency.Initials == "ARMY" && x.WasModSpawned);
        }
    }
    public bool AnyHelicopterUnitsSpawned
    {
        get
        {
            return Police.Any(x => x.IsInHelicopter && x.WasModSpawned);
        }
    }
    public string AgenciesChasingPlayer
    {
        get
        {
            return string.Join(" ", Police.Where(x => (x.SeenPlayerFor(10000) || (x.DistanceToPlayer <= 25f && x.DistanceToPlayer >= 1f)) && x.AssignedAgency != null).Select(x => x.AssignedAgency.ColoredInitials).Distinct().ToArray());
        }
    }
    public Pedestrians()
    {

    }
    public void Dispose()
    {
        ClearPolice();
    }
    public void Scan()
    {
        Ped[] GamePeds = Array.ConvertAll(Rage.World.GetEntities(Game.LocalPlayer.Character.Position, 450f, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).Where(x => x is Ped).ToArray(), (x => (Ped)x));//250
        foreach (Ped Pedestrian in GamePeds.Where(s => s.Exists() && !s.IsDead && s.IsVisible && s.IsHuman))
        {
            if (Pedestrian.IsPoliceArmy())
            {
                if (Mod.Player.SearchMode.IsSpotterCop(Pedestrian.Handle))
                    continue;

                if (!Police.Any(x => x.Pedestrian == Pedestrian))
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
    public void Prune()
    {
        Police.RemoveAll(x => x.CanRemove);
        Civilians.RemoveAll(x => x.CanRemove);
        foreach (Cop Cop in Mod.World.Pedestrians.Police.Where(x => x.Pedestrian.IsDead))
        {
            Mod.World.Spawner.MarkNonPersistent(Cop);
        }
        Police.RemoveAll(x => x.CanRemove);
        Civilians.RemoveAll(x => x.CanRemove);
    }
    public void ClearPolice()
    {
        foreach (Cop Cop in Police)
        {
            if (Cop.Pedestrian.Exists())
            {
                Cop.Pedestrian.Delete();
            }
        }
        Police.Clear();
    }
    public PedExt GetCivilian(uint Handle)
    {
        if (Police.Any(x => x.Pedestrian.Handle == Handle))
            return null;
        else
            return Civilians.FirstOrDefault(x => x.Pedestrian.Handle == Handle);
    }
    public bool AnyCopsNearPosition(Vector3 Position, float Distance)
    {
        if (Position != Vector3.Zero && Police.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Position) <= Distance))
            return true;
        else
            return false;
    }
    private void AddCop(Ped Pedestrian)
    {
        Agency AssignedAgency = Mod.DataMart.Agencies.GetAgency(Pedestrian);
        if (AssignedAgency == null || !Pedestrian.Exists())
        {
            return;
        }

        Cop myCop = new Cop(Pedestrian, Pedestrian.Health, AssignedAgency);
        //if (Pedestrian.IsInAnyPoliceVehicle && Pedestrian.CurrentVehicle != null && Pedestrian.CurrentVehicle.IsPoliceVehicle)
        //{
        //    Vehicle PoliceCar = Pedestrian.CurrentVehicle;
        //    if (!Mod.World.Vehicles.PoliceVehicles.Any(x => x.Vehicle.Handle == PoliceCar.Handle))
        //    {
        //        Mod.World.PoliceSpawning.UpdateLivery(PoliceCar, AssignedAgency);
        //        Mod.World.PoliceSpawning.UpgradeCruiser(PoliceCar);
        //        Mod.World.Vehicles.PoliceVehicles.Add(new VehicleExt(PoliceCar));
        //    }
        //}
        if (Mod.DataMart.Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip && Pedestrian.Exists())
        {
            Blip myBlip = Pedestrian.AttachBlip();
            myBlip.Color = AssignedAgency.AgencyColor;
            myBlip.Scale = 0.6f;
            Mod.World.AddBlip(myBlip);
        }
        SetCopStats(Pedestrian);
        Pedestrian.Inventory.Weapons.Clear();
        myCop.Loadout.IssueWeapons();
        Police.Add(myCop);
    }
    private void SetCopStats(Ped Pedestrian)
    {
        if (Mod.DataMart.Settings.SettingsManager.Police.OverridePoliceAccuracy)
        {
            Pedestrian.Accuracy = Mod.DataMart.Settings.SettingsManager.Police.PoliceGeneralAccuracy;
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

        bool WillFight = RandomItems.RandomPercent(5);
        bool WillCallPolice = RandomItems.RandomPercent(80);
        if (Pedestrian.Exists())
        {
            if(Pedestrian.IsGangMember())
            {
                WillFight = RandomItems.RandomPercent(95);
                WillCallPolice = false;
            }
            else if (Pedestrian.IsSecurity())
            {
                WillFight = true;
                WillCallPolice = false;
            }
            //string Nameo = Pedestrian.RelationshipGroup.Name;
            ////Mod.Debug.WriteToLog("Tasking", string.Format("Handle {0}, Nameo !!{1}!!", Pedestrian.Handle, Nameo));
            //if (!string.IsNullOrEmpty(Nameo) && !string.IsNullOrWhiteSpace(Nameo))
            //{
            //    if (Nameo.Contains("GANG"))
            //    {
            //        WillFight = true;
            //        WillCallPolice = false;
            //        int PedType = NativeFunction.CallByName<int>("GET_PED_TYPE", Pedestrian);
            //        string Model = Pedestrian.Model.Name;
            //        string RelationshipGroupName = Pedestrian.RelationshipGroup.Name;
            //       // Mod.Debug.WriteToLog("Tasking", string.Format("Handle {0}, Type {1}, Model {2}, RelationshipGroupName {3}, Nameo {4}", Pedestrian.Handle, PedType, Model, RelationshipGroupName, Nameo));
            //    }
            //}


        }
        Civilians.Add(new PedExt(Pedestrian, WillFight, WillCallPolice));
    }
    private void SetCivilianStats(Ped Pedestrian)
    {
        if (Mod.DataMart.Settings.SettingsManager.Police.OverridePoliceAccuracy)
        {
            Pedestrian.Accuracy = Mod.DataMart.Settings.SettingsManager.Police.PoliceGeneralAccuracy;
        }
        int DesiredHealth = RandomItems.MyRand.Next(MinCivilianHealth, MaxCivilianHealth) + 100;
        Pedestrian.MaxHealth = DesiredHealth;
        Pedestrian.Health = DesiredHealth;
        Pedestrian.Armor = 0;
        NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Pedestrian, 281, true);//Can Writhe
        NativeFunction.CallByName<bool>("SET_PED_DIES_WHEN_INJURED", Pedestrian, false);
    }

}
