using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

public class Pedestrians
{
    private int MaxCivilianHealth = 100;
    private int MaxCopArmor = 50;
    private int MaxCopHealth = 125;
    private int MinCivilianHealth = 70;
    private int MinCopArmor = 0;
    private int MinCopHealth = 85;
    private IAgencies Agencies;
    private IZoneJurisdictions ZoneJurisdictions;
    private ISettingsProvideable Settings;
    private IZones Zones;
    private INameProvideable Names;
    private IPedGroups RelationshipGroups;
    public Pedestrians(IAgencies agencies, IZones zones, IZoneJurisdictions zoneJurisdictions, ISettingsProvideable settings, INameProvideable names, IPedGroups relationshipGroups)
    {
        Agencies = agencies;
        Zones = zones;
        ZoneJurisdictions = zoneJurisdictions;
        Settings = settings;
        Names = names;
        RelationshipGroups = relationshipGroups;
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
    public bool AnyPoliceShouldBustPlayer
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
        PedExt CopPed = Police.FirstOrDefault(x => x.Pedestrian.Handle == Handle);
        if (CopPed != null)
        {
            return CopPed;
        }
        else
        {
            return Civilians.FirstOrDefault(x => x.Pedestrian.Handle == Handle);
        }
    }
    public void Prune()
    {
        Police.RemoveAll(x => x.CanRemove);
        Civilians.RemoveAll(x => x.CanRemove);
        foreach (Cop Cop in Police.Where(x => x.Pedestrian.IsDead))
        {
            Cop.Pedestrian.IsPersistent = false;
        }
        Police.RemoveAll(x => x.CanRemove);
        Civilians.RemoveAll(x => x.CanRemove);
    }
    public void Scan()
    {
        int PedsCreated = 0;
        Ped[] GamePeds = Array.ConvertAll(Rage.World.GetEntities(Game.LocalPlayer.Character.Position, 250f, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).Where(x => x is Ped).ToArray(), (x => (Ped)x));//250//450
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
                    PedsCreated++;
                    AddCop(Pedestrian);
                }
            }
            else
            {
                if (!Civilians.Any(x => x.Pedestrian.Handle == Pedestrian.Handle))
                {
                    PedsCreated++;
                    AddCivilian(Pedestrian);
                }
            }
            if(PedsCreated >= 10)
            {
                return;
            }
        }
    }
    private void AddCivilian(Ped Pedestrian)
    {
        SetCivilianStats(Pedestrian);
        bool WillFight = RandomItems.RandomPercent(5);
        bool WillCallPolice = RandomItems.RandomPercent(80);
        bool IsGangMember = false;
        if (Pedestrian.Exists())
        {
            if (Pedestrian.IsGangMember())
            {
                IsGangMember = true;
                WillFight = RandomItems.RandomPercent(95);
                WillCallPolice = false;
            }
            else if (Pedestrian.IsSecurity())
            {
                WillFight = true;
                WillCallPolice = false;
            }
        }
        Civilians.Add(new PedExt(Pedestrian, WillFight, WillCallPolice, IsGangMember, Names.GetRandomName(Pedestrian.IsMale), RelationshipGroups.GetPedGroup(Pedestrian.RelationshipGroup.Name)));
    }
    private void AddCop(Ped Pedestrian)
    {
        Agency AssignedAgency = GetAgency(Pedestrian, 0);//maybe need the actual wanted level here?
        if (AssignedAgency != null && Pedestrian.Exists())
        {
            Cop myCop = new Cop(Pedestrian, Pedestrian.Health, AssignedAgency, false);
            myCop.IssueWeapons();
            if (Settings.SettingsManager.Police.SpawnedAmbientPoliceHaveBlip && Pedestrian.Exists())
            {
                Blip myBlip = Pedestrian.AttachBlip();
                myBlip.Color = AssignedAgency.AgencyColor;
                myBlip.Scale = 0.6f;
                //WorldLogger.AddEntity(myBlip);
            }
            SetCopStats(Pedestrian);
            Police.Add(myCop);
        }
    }
    public Agency GetAgency(Ped Cop, int WantedLevel)
    {
        if (!Cop.IsPoliceArmy())
        {
            return null;
        }
        if (Cop.IsArmy())
        {
            return Agencies.GetRandomMilitaryAgency();//return AgenciesList.Where(x => x.AgencyClassification == Classification.Military).FirstOrDefault();
        }
        else if (Cop.IsPolice())
        {
            Agency ToReturn;
            List<Agency> ModelMatchAgencies = Agencies.GetAgencies(Cop);
            if (ModelMatchAgencies.Count > 1)
            {
                string ZoneName = GetInternalZoneString(Cop.Position);
                if (ZoneName != "")
                {
                    //Game.Console.Print(string.Format("GetAgencyFromPed! ZoneName {0}", ZoneName));
                    if(ZoneJurisdictions == null)
                    {
                        //Game.Console.Print("GetAgencyFromPed! ZoneJurisdictions is null!!!!!");
                    }
                    List<Agency> ZoneAgencies = ZoneJurisdictions.GetAgencies(ZoneName, WantedLevel);
                    if (ZoneAgencies != null)
                    {
                        foreach (Agency ZoneAgency in ZoneAgencies)
                        {
                            if (ModelMatchAgencies.Any(x => x.Initials == ZoneAgency.Initials))
                            {
                                return ZoneAgency;
                            }
                        }
                    }
                }
            }
            ToReturn = ModelMatchAgencies.FirstOrDefault();
            if (ToReturn == null)
            {
                //Game.Console.Print(string.Format("GetAgencyFromPed! Couldnt get agency from {0} ped deleting", Cop.Model.Name));
                Cop.Delete();
            }
            return ToReturn;
        }
        else
        {
            return null;
        }
    }
    private void SetCivilianStats(Ped Pedestrian)
    {
        if (Settings.SettingsManager.Police.OverridePoliceAccuracy)
        {
            Pedestrian.Accuracy = Settings.SettingsManager.Police.PoliceGeneralAccuracy;
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
        if (Settings.SettingsManager.Police.OverridePoliceAccuracy)
        {
            Pedestrian.Accuracy = Settings.SettingsManager.Police.PoliceGeneralAccuracy;
        }
        int DesiredHealth = RandomItems.MyRand.Next(MinCopHealth, MaxCopHealth) + 100;
        int DesiredArmor = RandomItems.MyRand.Next(MinCopArmor, MaxCopArmor);
        Pedestrian.MaxHealth = DesiredHealth;
        Pedestrian.Health = DesiredHealth;
        Pedestrian.Armor = DesiredArmor;
        NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Pedestrian, 281, true);//Can Writhe
        NativeFunction.CallByName<bool>("SET_PED_DIES_WHEN_INJURED", Pedestrian, false);
       //not needed if i am tasking everything?, messing up the enter vehicle task?
        //NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 7, false);//No commandeering//https://gtaforums.com/topic/833391-researchguide-combat-behaviour-flags/
    }
    private string GetInternalZoneString(Vector3 ZonePosition)
    {
        string zoneName;
        unsafe
        {
            IntPtr ptr = Rage.Native.NativeFunction.CallByName<IntPtr>("GET_NAME_OF_ZONE", ZonePosition.X, ZonePosition.Y, ZonePosition.Z);

            zoneName = Marshal.PtrToStringAnsi(ptr);
        }
        return zoneName;
    }
}