using ExtensionsMethods;
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public class Pedestrians
{
    private int MaxCivilianHealth = 100;
    private int MaxCopArmor = 50;
    private int MaxCopHealth = 125;
    private int MinCivilianHealth = 70;
    private int MinCopArmor = 0;
    private int MinCopHealth = 85;
    public Pedestrians()
    {
    }
    public string AgenciesChasingPlayer
    {
        get
        {
            return string.Join(" ", Police.Where(x => (x.SeenPlayerFor(10000) || (x.DistanceToPlayer <= 25f && x.DistanceToPlayer >= 1f)) && x.AssignedAgency != null).Select(x => x.AssignedAgency.ColoredInitials).Distinct().ToArray());
        }
    }
    public bool AnyArmyUnitsSpawned
    {
        get
        {
            return Police.Any(x => x.AssignedAgency.Initials == "ARMY" && x.WasModSpawned);
        }
    }
    public bool AnyCopsNearPlayer
    {
        get
        {
            return Police.Any(x => x.DistanceToPlayer <= 150f);
        }
    }
    public bool AnyHelicopterUnitsSpawned
    {
        get
        {
            return Police.Any(x => x.IsInHelicopter && x.WasModSpawned);
        }
    }
    public bool AnyNooseUnitsSpawned
    {
        get
        {
            return Police.Any(x => x.AssignedAgency.Initials == "NOOSE" && x.WasModSpawned);
        }
    }
    public List<PedExt> Civilians { get; private set; } = new List<PedExt>();
    public List<Cop> Police { get; private set; } = new List<Cop>();
    public bool ShouldBustPlayer
    {
        get
        {
            return Police.Any(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive && x.ShouldBustPlayer);
        }
    }
    public int TotalSpawnedCops
    {
        get
        {
            return Police.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
        }
    }
    public bool AnyCopsNearPosition(Vector3 Position, float Distance)
    {
        if (Position != Vector3.Zero && Police.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Position) <= Distance))
            return true;
        else
            return false;
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
    public int CountNearbyCops(Ped Pedestrian)
    {
        return Police.Count(x => Pedestrian.Exists() && x.Pedestrian.Exists() && Pedestrian.Handle != x.Pedestrian.Handle && x.Pedestrian.DistanceTo2D(Pedestrian) >= 3f && x.Pedestrian.DistanceTo2D(Pedestrian) <= 50f);
    }
    public PedExt GetCivilian(uint Handle)
    {
        if (Police.Any(x => x.Pedestrian.Handle == Handle))
            return null;
        else
            return Civilians.FirstOrDefault(x => x.Pedestrian.Handle == Handle);
    }
    public void Prune()
    {
        Police.RemoveAll(x => x.CanRemove);
        Civilians.RemoveAll(x => x.CanRemove);
        foreach (Cop Cop in Police.Where(x => x.Pedestrian.IsDead))
        {
            Mod.World.Instance.MarkNonPersistent(Cop);
        }
        Police.RemoveAll(x => x.CanRemove);
        Civilians.RemoveAll(x => x.CanRemove);
    }
    public void Scan()
    {
        Ped[] GamePeds = Array.ConvertAll(Rage.World.GetEntities(Game.LocalPlayer.Character.Position, 450f, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).Where(x => x is Ped).ToArray(), (x => (Ped)x));//250
        foreach (Ped Pedestrian in GamePeds.Where(s => s.Exists() && !s.IsDead && s.IsVisible && s.IsHuman))
        {
            if (Pedestrian.IsPoliceArmy())
            {
                //if (Mod.Player.Instance.SearchMode.IsSpotterCop(Pedestrian.Handle))
                //    continue;
                if (!Pedestrian.IsVisible)//trying to remove that call with this
                {
                    continue;
                }

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
    private void AddCivilian(Ped Pedestrian)
    {
        SetCivilianStats(Pedestrian);
        bool WillFight = RandomItems.RandomPercent(5);
        bool WillCallPolice = RandomItems.RandomPercent(80);
        if (Pedestrian.Exists())
        {
            if (Pedestrian.IsGangMember())
            {
                WillFight = RandomItems.RandomPercent(95);
                WillCallPolice = false;
            }
            else if (Pedestrian.IsSecurity())
            {
                WillFight = true;
                WillCallPolice = false;
            }
        }
        Civilians.Add(new PedExt(Pedestrian, WillFight, WillCallPolice));
    }
    private void AddCop(Ped Pedestrian)
    {
        Agency AssignedAgency = DataMart.Instance.Agencies.GetAgency(Pedestrian);
        if (AssignedAgency != null && Pedestrian.Exists())
        {
            Cop myCop = new Cop(Pedestrian, Pedestrian.Health, AssignedAgency, false);
            if (DataMart.Instance.Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip && Pedestrian.Exists())
            {
                Blip myBlip = Pedestrian.AttachBlip();
                myBlip.Color = AssignedAgency.AgencyColor;
                myBlip.Scale = 0.6f;
                Mod.World.Instance.AddBlip(myBlip);
            }
            SetCopStats(Pedestrian);
            Pedestrian.Inventory.Weapons.Clear();
            myCop.Loadout.IssueWeapons();
            Police.Add(myCop);
        }
    }
    private void SetCivilianStats(Ped Pedestrian)
    {
        if (DataMart.Instance.Settings.SettingsManager.Police.OverridePoliceAccuracy)
        {
            Pedestrian.Accuracy = DataMart.Instance.Settings.SettingsManager.Police.PoliceGeneralAccuracy;
        }
        int DesiredHealth = RandomItems.MyRand.Next(MinCivilianHealth, MaxCivilianHealth) + 100;
        Pedestrian.MaxHealth = DesiredHealth;
        Pedestrian.Health = DesiredHealth;
        Pedestrian.Armor = 0;
        NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Pedestrian, 281, true);//Can Writhe
        NativeFunction.CallByName<bool>("SET_PED_DIES_WHEN_INJURED", Pedestrian, false);
    }
    private void SetCopStats(Ped Pedestrian)
    {
        if (DataMart.Instance.Settings.SettingsManager.Police.OverridePoliceAccuracy)
        {
            Pedestrian.Accuracy = DataMart.Instance.Settings.SettingsManager.Police.PoliceGeneralAccuracy;
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
}