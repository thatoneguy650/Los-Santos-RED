using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

public class Pedestrians : ITaskerReportable
{
    private IAgencies Agencies;
    private IJurisdictions Jurisdictions;
    private ISettingsProvideable Settings;
    private IZones Zones;
    private INameProvideable Names;
    private IPedGroups RelationshipGroups;
    private List<Entity> WorldPeds = new List<Entity>();
    private IWeapons Weapons;
    private ICrimes Crimes;
    private IShopMenus ShopMenus;
    private IGangs Gangs;
    private uint GameTimeLastCreatedPeds = 0;
    private IGangTerritories GangTerritories;
    private IEntityProvideable World;

    private List<AssignedSeat> SeatAssignments = new List<AssignedSeat>();
    private RelationshipGroup formerPlayer;
    private RelationshipGroup criminalsRG;
    private RelationshipGroup hatesPlayerRG;
    private RelationshipGroup norelationshipRG;

    public Pedestrians(IAgencies agencies, IZones zones, IJurisdictions jurisdictions, ISettingsProvideable settings, INameProvideable names, IPedGroups relationshipGroups, IWeapons weapons, ICrimes crimes, IShopMenus shopMenus, IGangs gangs, IGangTerritories gangTerritories, IEntityProvideable world)
    {
        Agencies = agencies;
        Zones = zones;
        Jurisdictions = jurisdictions;
        Settings = settings;
        Names = names;
        RelationshipGroups = relationshipGroups;
        Weapons = weapons;
        Crimes = crimes;
        ShopMenus = shopMenus;
        Gangs = gangs;
        GangTerritories = gangTerritories;
        World = world;
    }
    public List<PedExt> Civilians { get; private set; } = new List<PedExt>();
    public List<Cop> Police { get; private set; } = new List<Cop>();
    public List<EMT> EMTs { get; private set; } = new List<EMT>();
    public List<Firefighter> Firefighters { get; private set; } = new List<Firefighter>();
    public List<Merchant> Merchants { get; private set; } = new List<Merchant>();
    public List<Zombie> Zombies { get; private set; } = new List<Zombie>();
    public List<GangMember> GangMembers { get; private set; } = new List<GangMember>();
    public List<PedExt> CivilianList => Civilians.Where(x => x.Pedestrian.Exists()).ToList();
    public List<GangMember> GangMemberList => GangMembers.Where(x => x.Pedestrian.Exists()).ToList();
    public List<Zombie> ZombieList => Zombies.Where(x => x.Pedestrian.Exists()).ToList();
    public List<Cop> PoliceList => Police.Where(x => x.Pedestrian.Exists()).ToList();
    public List<EMT> EMTList => EMTs.Where(x => x.Pedestrian.Exists()).ToList();
    public List<Firefighter> FirefighterList => Firefighters.Where(x => x.Pedestrian.Exists()).ToList();
    public List<Merchant> MerchantList => Merchants.Where(x => x.Pedestrian.Exists()).ToList();
    public List<PedExt> DeadPeds { get; private set; } = new List<PedExt>();
    public List<PedExt> LivingPeople
    {
        get
        {
            List<PedExt> myList = new List<PedExt>();
            myList.AddRange(CivilianList);
            myList.AddRange(GangMemberList);
            myList.AddRange(MerchantList);
            myList.AddRange(EMTList);
            myList.AddRange(PoliceList);
            myList.AddRange(FirefighterList);
            return myList;
        }
    }
    public List<PedExt> Citizens
    {
        get
        {
            List<PedExt> myList = new List<PedExt>();
            myList.AddRange(CivilianList);
            myList.AddRange(GangMemberList);
            myList.AddRange(MerchantList);
            return myList;
        }
    }
    public List<PedExt> PedExts
    {
        get
        {
            List<PedExt> myList = new List<PedExt>();
            myList.AddRange(CivilianList);
            myList.AddRange(GangMemberList);
            myList.AddRange(MerchantList);
            myList.AddRange(EMTList);
            myList.AddRange(PoliceList);
            myList.AddRange(FirefighterList);
            myList.AddRange(DeadPeds);
            return myList;
        }
    }
    public bool AnyInjuredPeopleNearPlayer => PedExts.Any(x => (x.IsUnconscious || x.IsInWrithe) && x.DistanceToPlayer <= 150f);
    public bool AnyWantedPeopleNearPlayer => CivilianList.Any(x => x.WantedLevel > 0 && !x.IsBusted && x.DistanceToPlayer <= 150f) || GangMemberList.Any(x => x.WantedLevel > 0 && !x.IsBusted && x.DistanceToPlayer <= 150f) || MerchantList.Any(x => x.WantedLevel > 0 && !x.IsBusted && x.DistanceToPlayer <= 150f);
    public string DebugString { get; set; } = "";
    public bool AnyArmyUnitsSpawned
    {
        get
        {
            return Police.Any(x => x.AssignedAgency.ID == "ARMY" && x.WasModSpawned);
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
            return Police.Any(x => x.AssignedAgency.ID == "NOOSE" && x.WasModSpawned);
        }
    }
    public int TotalSpawnedPolice
    {
        get
        {
            return Police.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
        }
    }
    public int TotalSpawnedEMTs
    {
        get
        {
            return EMTs.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
        }
    }
    public int TotalSpawnedGangMembers
    {
        get
        {
            return GangMembers.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
        }
    }
    public int TotalSpawnedFirefighters => Firefighters.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
    public int TotalSpawnedZombies => Zombies.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
    public void Setup()
    {
        foreach (Gang gang in Gangs.AllGangs)
        {
            RelationshipGroup thisGangGroup = new RelationshipGroup(gang.ID);
           // RelationshipGroup policeGroup = new RelationshipGroup("COP");
            foreach (Gang otherGang in Gangs.AllGangs)
            {
                if (otherGang.ID != gang.ID)
                {
                    if (gang.EnemyGangs.Contains(otherGang.ID))
                    {
                        RelationshipGroup otherGangGroup = new RelationshipGroup(otherGang.ID);
                        otherGangGroup.SetRelationshipWith(thisGangGroup, Relationship.Dislike);
                        thisGangGroup.SetRelationshipWith(otherGangGroup, Relationship.Dislike);
                    }
                    else
                    {
                        RelationshipGroup otherGangGroup = new RelationshipGroup(otherGang.ID);
                        otherGangGroup.SetRelationshipWith(thisGangGroup, Relationship.Neutral);//was like
                        thisGangGroup.SetRelationshipWith(otherGangGroup, Relationship.Neutral);//was like
                    }
                }
            }
            thisGangGroup.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Neutral);//was like
            RelationshipGroup.Cop.SetRelationshipWith(thisGangGroup, Relationship.Neutral);//was like
        }
        NativeFunction.Natives.SET_AMBIENT_PEDS_DROP_MONEY(false);


        formerPlayer = new RelationshipGroup("FORMERPLAYER");
        criminalsRG = new RelationshipGroup("CRIMINALS");
        hatesPlayerRG = new RelationshipGroup("HATES_PLAYER");
        norelationshipRG = new RelationshipGroup("NO_RELATIONSHIP");


    }
    public void Dispose()
    {
        ClearSpawned();
        NativeFunction.Natives.SET_AMBIENT_PEDS_DROP_MONEY(true);
    }
    public void CreateNew()
    {
        WorldPeds = Rage.World.GetEntities(GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).ToList();
        GameFiber.Yield();
        int updated = 0;
        foreach (Ped Pedestrian in WorldPeds.Where(s => s.Exists() && !s.IsDead && s.MaxHealth != 1 && s.Handle != Game.LocalPlayer.Character.Handle))//take 20 is new
        {
            string modelName = Pedestrian.Model.Name.ToLower();
            if (Settings.SettingsManager.WorldSettings.ReplaceVanillaShopKeepers && (modelName == "mp_m_shopkeep_01"))// || modelName == "s_m_y_ammucity_01" || modelName == "s_m_m_ammucountry"))
            {
                Delete(Pedestrian);
                continue;
            }
            uint localHandle = Pedestrian.Handle;

            if (DeadPeds.Any(x => x.Handle == localHandle))
            {
                continue;
            }
            else if (Pedestrian.IsPoliceArmy())
            {
                if (Police.Any(x => x.Handle == localHandle))
                {
                    continue;
                }
                if (Settings.SettingsManager.PoliceSpawnSettings.RemoveNonSpawnedPolice)
                {
                    Delete(Pedestrian);
                    continue;
                }
                else if (Settings.SettingsManager.PoliceSpawnSettings.RemoveAmbientPolice && !Pedestrian.IsPersistent)
                {
                    Delete(Pedestrian);
                    continue;
                }
                AddAmbientCop(Pedestrian);
                GameFiber.Yield();
            }
            else
            {
                if (Pedestrian.IsGangMember())
                {
                    if (GangMembers.Any(x => x.Handle == localHandle))
                    {
                        continue;
                    }
                    if (Settings.SettingsManager.GangSettings.RemoveVanillaSpawnedPeds)// || modelName == "s_m_y_ammucity_01" || modelName == "s_m_m_ammucountry"))
                    {
                        Delete(Pedestrian);
                        continue;
                    }
                    else if (Settings.SettingsManager.GangSettings.RemoveVanillaSpawnedPedsOnFoot && Pedestrian.Exists() && !Pedestrian.IsInAnyVehicle(false))
                    {
                        Delete(Pedestrian);
                        continue;
                    }
                    AddAmbientGangMember(Pedestrian);
                    GameFiber.Yield();
                }
                else if (!Civilians.Any(x => x.Handle == localHandle) && !Merchants.Any(x => x.Handle == localHandle) && !Zombies.Any(x => x.Handle == localHandle) && !GangMembers.Any(x => x.Handle == localHandle) && !Police.Any(x => x.Handle == localHandle))
                {
                    AddAmbientCivilian(Pedestrian);
                    GameFiber.Yield();
                }
            }
            updated++;
            if(updated > 10)
            {
                GameFiber.Yield();
                updated = 0;
            }
        }
        if (Settings.SettingsManager.PerformanceSettings.PrintUpdateTimes)
        {
            EntryPoint.WriteToConsole($"Pedestrians.CreateNew Ran Time Since {Game.GameTime - GameTimeLastCreatedPeds}", 5);
        }
        GameTimeLastCreatedPeds = Game.GameTime;
    }
    public void Prune()
    {
        PruneServicePeds();
        GameFiber.Yield();//TR 29
        PruneGangMembers();
        GameFiber.Yield();//TR 29
        PruneCivilians();
        GameFiber.Yield();//TR 29
        DeadPeds.RemoveAll(x => !x.Pedestrian.Exists());
    }
    public bool AnyCopsNearPosition(Vector3 Position, float Distance)
    {
        if (Position != Vector3.Zero && Police.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Position) <= Distance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool AnyCopsNearCop(Cop cop, int CellsAway)
    {
        if (cop != null && cop.Pedestrian.Exists())
        {
            Vehicle copVehicle = cop.Pedestrian.CurrentVehicle;
            foreach (Cop targetCop in Police)
            {
                if (targetCop.Pedestrian.Exists())
                {
                    if (cop.Pedestrian.Handle != targetCop.Pedestrian.Handle && NativeHelper.IsNearby(cop.CellX, cop.CellY, targetCop.CellX, targetCop.CellY, CellsAway))
                    {
                        if (!targetCop.IsInVehicle)
                        {
                            return true;
                        }
                        else if (copVehicle.Exists() && targetCop.Pedestrian.CurrentVehicle.Exists() && copVehicle.Handle != targetCop.Pedestrian.CurrentVehicle.Handle)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false; 
    }
    public int TotalCopsNearCop(Cop cop, int CellsAway)
    {
        int total = 0;
        if (cop != null && cop.Pedestrian.Exists())
        {
            Vehicle copVehicle = cop.Pedestrian.CurrentVehicle;
            foreach (Cop targetCop in Police)
            {
                if (targetCop.Pedestrian.Exists())
                {
                    if (cop.Pedestrian.Handle != targetCop.Pedestrian.Handle && NativeHelper.IsNearby(cop.CellX, cop.CellY, targetCop.CellX, targetCop.CellY, CellsAway))
                    {
                        if (!targetCop.IsInVehicle)
                        {
                            total++;
                        }
                        else if (copVehicle.Exists() && targetCop.Pedestrian.CurrentVehicle.Exists() && copVehicle.Handle != targetCop.Pedestrian.CurrentVehicle.Handle)
                        {
                            total++;
                        }
                    }
                }
            }
        }
        return total;
    }
    public void ClearPolice()
    {
        foreach (Cop Cop in Police)
        {
            if (Cop.Pedestrian.Exists() && Cop.Pedestrian.Handle != Game.LocalPlayer.Character.Handle)
            {
                Cop.Pedestrian.Delete();
                EntryPoint.PersistentPedsDeleted++;
            }
        }
        Police.Clear();
    }
    public void ClearSpawned()
    {
        ClearPolice();
        foreach (EMT EMT in EMTs)
        {
            if (EMT.Pedestrian.Exists() && EMT.Pedestrian.Handle != Game.LocalPlayer.Character.Handle)
            {
                EMT.Pedestrian.Delete();
                EntryPoint.PersistentPedsDeleted++;
            }
        }
        EMTs.Clear();
        foreach (Firefighter Firefighter in Firefighters)
        {
            if (Firefighter.Pedestrian.Exists() && Firefighter.Pedestrian.Handle != Game.LocalPlayer.Character.Handle)
            {
                Firefighter.Pedestrian.Delete();
                EntryPoint.PersistentPedsDeleted++;
            }
        }
        Firefighters.Clear();
        foreach (Merchant merchant in Merchants)
        {
            if (merchant.Pedestrian.Exists() && merchant.Pedestrian.Handle != Game.LocalPlayer.Character.Handle)
            {
                merchant.Pedestrian.Delete();
            }
        }
        Merchants.Clear();
        foreach (Zombie zombie in Zombies)
        {
            if (zombie.Pedestrian.Exists() && zombie.Pedestrian.Handle != Game.LocalPlayer.Character.Handle)
            {
                zombie.Pedestrian.Delete();
            }
        }
        Zombies.Clear();
        foreach (GangMember gangMember in GangMembers)
        {
            if (gangMember.Pedestrian.Exists() && gangMember.Pedestrian.Handle != Game.LocalPlayer.Character.Handle)
            {
                gangMember.Pedestrian.Delete();
            }
        }
        GangMembers.Clear();
    }
    public void ClearGangMembers()
    {
        foreach (GangMember gangMember in GangMembers)
        {
            if (gangMember.Pedestrian.Exists() && gangMember.Pedestrian.Handle != Game.LocalPlayer.Character.Handle)
            {
                gangMember.Pedestrian.Delete();
            }
        }
        GangMembers.Clear();
    }
    public PedExt GetPedExt(uint Handle)
    {
        return PedExts.FirstOrDefault(x => x.Handle == Handle);
    }
    public GangMember GetGangMember(uint Handle)
    {
        GangMember pedExt = GangMembers.FirstOrDefault(x => x.Handle == Handle);
        if (pedExt != null)
        {
            return pedExt;
        }
        return null;
    }
    private void PruneServicePeds()
    {
        foreach (Cop Cop in Police.Where(x => x.Pedestrian.Exists() && x.CanRemove && x.Pedestrian.IsDead))// && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) >= 200))
        {
            bool hasBlip = false;
            Blip myblip = Cop.Pedestrian.GetAttachedBlip();
            if (myblip.Exists())
            {
                hasBlip = true;
                myblip.Delete();
            }
            Cop.Pedestrian.IsPersistent = false;
            EntryPoint.PersistentPedsNonPersistent++;
            EntryPoint.WriteToConsole($"Pedestrians: Cop {Cop.Pedestrian.Handle} Removed Blip Set Non Persistent hasBlip {hasBlip}", 5);
            if (!DeadPeds.Any(x => x.Handle == Cop.Handle))
            {
                Cop.IsDead = true;
                DeadPeds.Add(Cop);
            }
        }
        foreach (EMT EMT in EMTs.Where(x => x.Pedestrian.Exists() && x.CanRemove && x.Pedestrian.IsDead))// && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) >= 200))
        {
            bool hasBlip = false;
            Blip myblip = EMT.Pedestrian.GetAttachedBlip();
            if (myblip.Exists())
            {
                hasBlip = true;
                myblip.Delete();
            }
            EMT.Pedestrian.IsPersistent = false;
            EntryPoint.PersistentPedsNonPersistent++;
            EntryPoint.WriteToConsole($"Pedestrians: Cop {EMT.Pedestrian.Handle} Removed Blip Set Non Persistent hasBlip {hasBlip}", 5);
            if (!DeadPeds.Any(x => x.Handle == EMT.Handle))
            {
                EMT.IsDead = true;
                DeadPeds.Add(EMT);
            }
        }
        foreach (Firefighter Firefighter in Firefighters.Where(x => x.Pedestrian.Exists() && x.CanRemove && x.Pedestrian.IsDead))// && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) >= 200))
        {
            bool hasBlip = false;
            Blip myblip = Firefighter.Pedestrian.GetAttachedBlip();
            if (myblip.Exists())
            {
                hasBlip = true;
                myblip.Delete();
            }
            Firefighter.Pedestrian.IsPersistent = false;
            EntryPoint.PersistentPedsNonPersistent++;
            EntryPoint.WriteToConsole($"Pedestrians: Cop {Firefighter.Pedestrian.Handle} Removed Blip Set Non Persistent hasBlip {hasBlip}", 5);
            if (!DeadPeds.Any(x => x.Handle == Firefighter.Handle))
            {
                Firefighter.IsDead = true;
                DeadPeds.Add(Firefighter);
            }
        }
        foreach (Merchant Civilian in Merchants.Where(x => x.Pedestrian.Exists() && x.CanRemove && x.Pedestrian.IsDead))
        {
            if (!DeadPeds.Any(x => x.Handle == Civilian.Handle))
            {
                Civilian.IsDead = true;
                DeadPeds.Add(Civilian);
            }
        }
        GameFiber.Yield();
        Police.RemoveAll(x => x.CanRemove);// || x.Handle == Game.LocalPlayer.Character.Handle);
        EMTs.RemoveAll(x => x.CanRemove || x.Handle == Game.LocalPlayer.Character.Handle);
        Firefighters.RemoveAll(x => x.CanRemove || x.Handle == Game.LocalPlayer.Character.Handle);
        Merchants.RemoveAll(x => x.CanRemove || x.Handle == Game.LocalPlayer.Character.Handle);
    }
    private void PruneGangMembers()
    {
        foreach (GangMember GangMember in GangMembers.Where(x => x.Pedestrian.Exists() && x.CanRemove && x.Pedestrian.IsDead))// && x.Pedestrian.IsPersistent))// && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) >= 200))
        {
            bool hasBlip = false;
            Blip myblip = GangMember.Pedestrian.GetAttachedBlip();
            if (myblip.Exists())
            {
                hasBlip = true;
                myblip.Delete();
            }
            GangMember.Pedestrian.IsPersistent = false;

            if (GangMember.Pedestrian.CurrentVehicle.Exists() && GangMember.Pedestrian.CurrentVehicle.IsPersistent)
            {
                GangMember.Pedestrian.CurrentVehicle.IsPersistent = false;
                EntryPoint.PersistentVehiclesNonPersistent++;
            }
            if (!DeadPeds.Any(x => x.Handle == GangMember.Handle))
            {
                GangMember.IsDead = true;
                DeadPeds.Add(GangMember);
            }
            EntryPoint.PersistentPedsNonPersistent++;
        }
        GangMembers.RemoveAll(x => x.CanRemove || x.Handle == Game.LocalPlayer.Character.Handle);
    }
    private void PruneCivilians()
    {
        foreach (PedExt Civilian in Civilians.Where(x => x.IsBusted && x.DistanceToPlayer >= 100f && x.Pedestrian.Exists() && x.Pedestrian.IsPersistent))// && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) >= 200))
        {
            bool hasBlip = false;
            Blip myblip = Civilian.Pedestrian.GetAttachedBlip();
            if (myblip.Exists())
            {
                hasBlip = true;
                myblip.Delete();
            }
            if (!Civilian.WasModSpawned)
            {
                Civilian.Pedestrian.IsPersistent = false;
            }
            EntryPoint.PersistentPedsNonPersistent++;
        }

        //RelationshipGroup formerPlayer = new RelationshipGroup("FORMERPLAYER");
        //RelationshipGroup criminalsRG = new RelationshipGroup("CRIMINALS");
        //RelationshipGroup hatesPlayerRG = new RelationshipGroup("HATES_PLAYER");
        //RelationshipGroup norelationshipRG = new RelationshipGroup("NO_RELATIONSHIP");

        
        foreach (PedExt Civilian in Civilians.Where(x => x.DistanceToPlayer >= 200f && x.Pedestrian.Exists() && x.Pedestrian.IsPersistent))// && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) >= 200))
        {
            if (Civilian.Pedestrian.RelationshipGroup == formerPlayer)
            {
                Civilian.Pedestrian.IsPersistent = false;
                EntryPoint.PersistentPedsNonPersistent++;
            }
            else if (Civilian.IsWanted && !Civilian.WasPersistentOnCreate && !Civilian.WasModSpawned)
            {
                Civilian.Pedestrian.IsPersistent = false;
                EntryPoint.PersistentPedsNonPersistent++;
            }
        }
        foreach (PedExt Civilian in Civilians.Where(x => !x.Pedestrian.Exists()))// && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) >= 200))
        {
            if (Civilian.HasSeenPlayerCommitCrime && (Civilian.WillCallPolice || Civilian.WillCallPoliceIntense) && !Civilian.HasLoggedDeath)
            {
                Civilian.CurrentTask?.Update();
            }
        }
        foreach (PedExt Civilian in Civilians.Where(x => x.Pedestrian.Exists() && x.CanRemove && x.Pedestrian.IsDead))
        {
            if (!DeadPeds.Any(x => x.Handle == Civilian.Handle))
            {
                Civilian.IsDead = true;
                DeadPeds.Add(Civilian);
            }
        }
        Civilians.RemoveAll(x => x.CanRemove || (x.Pedestrian.Exists() && x.Pedestrian.RelationshipGroup == RelationshipGroup.Cop) || (x.Handle == Game.LocalPlayer.Character.Handle));
        DeadPeds.RemoveAll(x => x.Handle == Game.LocalPlayer.Character.Handle);
        foreach (PedExt Civilian in DeadPeds.Where(x => NativeHelper.MaxCellsAway(EntryPoint.FocusCellX,EntryPoint.FocusCellY,x.CellX,x.CellY) >= 3 && x.Pedestrian.Exists() && x.Pedestrian.IsPersistent))// && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) >= 200))
        {
            if (Civilian.Pedestrian.RelationshipGroup == formerPlayer || Civilian.Pedestrian.RelationshipGroup == criminalsRG || Civilian.Pedestrian.RelationshipGroup == hatesPlayerRG || Civilian.Pedestrian.RelationshipGroup == norelationshipRG)
            {
                Civilian.Pedestrian.IsPersistent = false;
                EntryPoint.PersistentPedsNonPersistent++;
            }
            else if (Civilian.IsWanted && !Civilian.WasPersistentOnCreate && !Civilian.WasModSpawned)
            {
                Civilian.Pedestrian.IsPersistent = false;
                EntryPoint.PersistentPedsNonPersistent++;
            }
            else if(Civilian.WasModSpawned)
            {
                Civilian.Pedestrian.IsPersistent = false;
                EntryPoint.PersistentPedsNonPersistent++;
            }
        }
    }
    public void AddEntity(PedExt pedExt)
    {
        if (pedExt != null)
        {
            if (pedExt.GetType() == typeof(Cop))
            {
                if (!Police.Any(x => x.Handle == pedExt.Handle))
                {
                    Police.Add((Cop)pedExt);
                }
            }
            else if (pedExt.GetType() == typeof(EMT))
            {
                if (!EMTs.Any(x => x.Handle == pedExt.Handle))
                {
                    EMTs.Add((EMT)pedExt);
                }
            }
            else if (pedExt.GetType() == typeof(Firefighter))
            {
                if (!Firefighters.Any(x => x.Handle == pedExt.Handle))
                {
                    Firefighters.Add((Firefighter)pedExt);
                }
            }
            else if (pedExt.GetType() == typeof(Merchant))
            {
                if (!Merchants.Any(x => x.Handle == pedExt.Handle))
                {
                    Merchants.Add((Merchant)pedExt);
                }
            }
            else if (pedExt.GetType() == typeof(Zombie))
            {
                if (!Zombies.Any(x => x.Handle == pedExt.Handle))
                {
                    Zombies.Add((Zombie)pedExt);
                }
            }
            else if (pedExt.GetType() == typeof(GangMember))
            {
                if (!GangMembers.Any(x => x.Handle == pedExt.Handle))
                {
                    GangMembers.Add((GangMember)pedExt);
                }
            }
            else
            {
                if (!Civilians.Any(x => x.Handle == pedExt.Handle))
                {
                    Civilians.Add(pedExt);
                }
            }
        }
    }
    public void MarkPedAsRevived(PedExt pedExt)
    {
        if(pedExt.IsDead)
        {
            DeadPeds.Remove(pedExt);
            pedExt.IsDead = false;
            AddEntity(pedExt);
        }
    }
    private ShopMenu GetIllicitMenu()
    {
        if (EntryPoint.FocusZone != null)
        {
            if (EntryPoint.FocusZone.Economy == eLocationEconomy.Rich)
            {
                if (RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.DrugDealerPercentageRichZones))
                {
                    return ShopMenus.GetRandomDrugDealerMenu();
                }
                else if (RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.DrugCustomerPercentageRichZones))
                {
                    return ShopMenus.GetRandomDrugCustomerMenu();
                }
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Middle)
            {
                if (RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.DrugDealerPercentageMiddleZones))
                {
                    return ShopMenus.GetRandomDrugDealerMenu();
                }
                else if (RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.DrugCustomerPercentageMiddleZones))
                {
                    return ShopMenus.GetRandomDrugCustomerMenu();
                }
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Poor)
            {
                if (RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.DrugDealerPercentagePoorZones))
                {
                    return ShopMenus.GetRandomDrugDealerMenu();
                }
                else if (RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.DrugCustomerPercentagePoorZones))
                {
                    return ShopMenus.GetRandomDrugCustomerMenu();
                }
            }
        }
        else
        {
            if (RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.DrugDealerPercentageMiddleZones))
            {
                return ShopMenus.GetRandomDrugDealerMenu();
            }
            else if (RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.DrugCustomerPercentageMiddleZones))
            {
                return ShopMenus.GetRandomDrugCustomerMenu();
            }
        }
        return null;
    }
    private float CivilianCallPercentage()
    {
        if (EntryPoint.FocusZone != null)
        {
            if (EntryPoint.FocusZone.Economy == eLocationEconomy.Rich)
            {
                return Settings.SettingsManager.CivilianSettings.CallPolicePercentageRichZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Middle)
            {
                return Settings.SettingsManager.CivilianSettings.CallPolicePercentageMiddleZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Poor)
            {
                return Settings.SettingsManager.CivilianSettings.CallPolicePercentagePoorZones;
            }
            else
            {
                return Settings.SettingsManager.CivilianSettings.CallPolicePercentageMiddleZones;
            }
        }
        else
        {
            return Settings.SettingsManager.CivilianSettings.CallPolicePercentageMiddleZones;
        }
    }
    private float CivilianSeriousCallPercentage()
    {
        if (EntryPoint.FocusZone != null)
        {
            if (EntryPoint.FocusZone.Economy == eLocationEconomy.Rich)
            {
                return Settings.SettingsManager.CivilianSettings.CallPoliceForSeriousCrimesPercentageRichZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Middle)
            {
                return Settings.SettingsManager.CivilianSettings.CallPoliceForSeriousCrimesPercentageMiddleZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Poor)
            {
                return Settings.SettingsManager.CivilianSettings.CallPoliceForSeriousCrimesPercentagePoorZones;
            }
            else
            {
                return Settings.SettingsManager.CivilianSettings.CallPoliceForSeriousCrimesPercentageMiddleZones;
            }
        }
        else
        {
            return Settings.SettingsManager.CivilianSettings.CallPoliceForSeriousCrimesPercentageMiddleZones;
        }
    }
    private float CivilianFightPercentage()
    {
        if (EntryPoint.FocusZone != null)
        {
            if (EntryPoint.FocusZone.Economy == eLocationEconomy.Rich)
            {
                return Settings.SettingsManager.CivilianSettings.FightPercentageRichZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Middle)
            {
                return Settings.SettingsManager.CivilianSettings.FightPercentageMiddleZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Poor)
            {
                return Settings.SettingsManager.CivilianSettings.FightPercentagePoorZones;
            }
            else
            {
                return Settings.SettingsManager.CivilianSettings.FightPercentageMiddleZones;
            }
        }
        else
        {
            return Settings.SettingsManager.CivilianSettings.FightPercentageMiddleZones;
        }
    }
    private float CivilianFightPolicePercentage()
    {
        if (EntryPoint.FocusZone != null)
        {
            if (EntryPoint.FocusZone.Economy == eLocationEconomy.Rich)
            {
                return Settings.SettingsManager.CivilianSettings.FightPolicePercentageRichZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Middle)
            {
                return Settings.SettingsManager.CivilianSettings.FightPolicePercentageMiddleZones;
            }
            else if (EntryPoint.FocusZone.Economy == eLocationEconomy.Poor)
            {
                return Settings.SettingsManager.CivilianSettings.FightPolicePercentagePoorZones;
            }
            else
            {
                return Settings.SettingsManager.CivilianSettings.FightPolicePercentageMiddleZones;
            }
        }
        else
        {
            return Settings.SettingsManager.CivilianSettings.FightPolicePercentageMiddleZones;
        }
    }






    private void AddAmbientCivilian(Ped Pedestrian)
    {  
        bool WillFight = false;
        bool WillCallPolice = false;
        bool WillCallPoliceIntense = false;
        bool WillFightPolice = false;
        bool IsGangMember = false;
        bool canBeAmbientTasked = true;
        bool WasPersistentOnCreate = false;
        if (Pedestrian.Exists())
        {
            if (Pedestrian.IsSecurity())
            {
                WillFight = RandomItems.RandomPercent(Settings.SettingsManager.CivilianSettings.SecurityFightPercentage);
                WillCallPolice = false;
                WillCallPoliceIntense = false;
                WillFightPolice = false;
            }
            else
            {
                SetCivilianStats(Pedestrian);
                WillFight = RandomItems.RandomPercent(CivilianFightPercentage());
                WillCallPolice = RandomItems.RandomPercent(CivilianCallPercentage());
                WillCallPoliceIntense = RandomItems.RandomPercent(CivilianSeriousCallPercentage());
                WillFightPolice = RandomItems.RandomPercent(CivilianFightPolicePercentage());
            }
            if (Pedestrian.IsPersistent)
            {
                WasPersistentOnCreate = true;
            }
            if (!Settings.SettingsManager.CivilianSettings.TaskMissionPeds && WasPersistentOnCreate)//must have been spawned by another mod?
            {
                WillFight = false;
                WillCallPolice = false;
                WillCallPoliceIntense = false;
                WillFightPolice = false;
                canBeAmbientTasked = false;
            }
            PedGroup myGroup = RelationshipGroups.GetPedGroup(Pedestrian.RelationshipGroup.Name);
            if (myGroup == null)
            {
                myGroup = new PedGroup(Pedestrian.RelationshipGroup.Name, Pedestrian.RelationshipGroup.Name, Pedestrian.RelationshipGroup.Name, false);
            }
            ShopMenu toAdd = GetIllicitMenu();
            PedExt toCreate = new PedExt(Pedestrian, Settings, WillFight, WillCallPolice, IsGangMember, false, Names.GetRandomName(Pedestrian.IsMale), Crimes, Weapons, myGroup.MemberName, World, WillFightPolice) { CanBeAmbientTasked = canBeAmbientTasked , WillCallPoliceIntense = WillCallPoliceIntense, WasPersistentOnCreate = WasPersistentOnCreate };
            toCreate.SetupTransactionItems(toAdd);
                 
            Civilians.Add(toCreate);
            if (Pedestrian.Exists())
            {
                Pedestrian.Money = 0;// toCreate.Money;
                NativeFunction.Natives.SET_PED_SUFFERS_CRITICAL_HITS(Pedestrian, false);
            }
        }
    }
    private void SetCivilianStats(Ped Pedestrian)
    {
        if (Pedestrian.Exists() && !Pedestrian.IsPersistent)
        {
            if (Settings.SettingsManager.CivilianSettings.OverrideAccuracy)
            {
                Pedestrian.Accuracy = Settings.SettingsManager.CivilianSettings.GeneralAccuracy;
            }
            if (Settings.SettingsManager.CivilianSettings.OverrideHealth)
            {
                int DesiredHealth = RandomItems.MyRand.Next(Settings.SettingsManager.CivilianSettings.MinHealth, Settings.SettingsManager.CivilianSettings.MaxHealth) + 100;
                Pedestrian.MaxHealth = DesiredHealth;
                Pedestrian.Health = DesiredHealth;
                Pedestrian.Armor = 0;
            }
            NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Pedestrian, 281, true);//Can Writhe
            NativeFunction.CallByName<bool>("SET_PED_DIES_WHEN_INJURED", Pedestrian, false);
        }
    }
    private void AddAmbientGangMember(Ped Pedestrian)
    {
        if(!Pedestrian.Exists())
        {
            return;
        }
        bool isCarSpawn = Pedestrian.IsInAnyVehicle(false);
        //bool WasPersistentOnCreate = false;
        string relationshipGroupName = Pedestrian.RelationshipGroup.Name;
        Gang MyGang = Gangs.GetGang(relationshipGroupName);
        if(MyGang == null)
        {
            MyGang = new Gang(relationshipGroupName, relationshipGroupName, relationshipGroupName, relationshipGroupName);
        }
        DispatchablePerson gangPerson = null;
        if(MyGang.Personnel != null)
        {
            gangPerson = MyGang.GetSpecificPed(Pedestrian);
        }
        if (Settings.SettingsManager.GangSettings.RemoveVanillaSpawnedPedsOutsideTerritory)
        {
            Zone CurrentZone = Zones.GetZone(Pedestrian.Position);
            if (CurrentZone != null)
            {
                List<ZoneJurisdiction> totalTerritories = GangTerritories.GetGangTerritory(MyGang.ID);
                if (!totalTerritories.Any(x => x.ZoneInternalGameName == CurrentZone.InternalGameName))
                {
                    Delete(Pedestrian);
                    return;
                }
            }
        }
        GangMember gm = new GangMember(Pedestrian, Settings, MyGang, false, Names.GetRandomName(Pedestrian.IsMale), Crimes, Weapons, World);// { CanBeAmbientTasked = canBeAmbientTasked, WasPersistentOnCreate = WasPersistentOnCreate };
        gm.SetStats(gangPerson, ShopMenus, Weapons, Settings.SettingsManager.GangSettings.ShowAmbientBlips);
        if (Pedestrian.IsPersistent)
        {
            gm.WasPersistentOnCreate = true;
        }
        if (!Settings.SettingsManager.CivilianSettings.TaskMissionPeds && gm.WasPersistentOnCreate)//must have been spawned by another mod?
        {
            gm.WillFight = false;
            gm.WillFightPolice = false;
            gm.CanBeAmbientTasked = false;
            gm.SetupTransactionItems(null);
        }
        if (isCarSpawn && Settings.SettingsManager.GangSettings.ForceAmbientCarDocile)
        {
            gm.WillFight = false;
            gm.WillFightPolice = false;
            gm.SetupTransactionItems(null);
            NativeFunction.Natives.REMOVE_ALL_PED_WEAPONS(Pedestrian, false);
        }
        GangMembers.Add(gm);    
    }
    private void AddAmbientCop(Ped Pedestrian)
    {
        var AgencyData = GetAgencyData(Pedestrian, 0);
        Agency AssignedAgency = AgencyData.agency;
        DispatchablePerson AssignedPerson = AgencyData.dispatchablePerson;
        if(AssignedAgency == null || AssignedPerson == null || !Pedestrian.Exists())
        {
            if (Pedestrian.IsPersistent)
            {
                EntryPoint.PersistentPedsDeleted++;
            }
            Delete(Pedestrian);
            EntryPoint.WriteToConsole($"PEDESTRIANS: Add COP FAIL, DELETING", 2);
            return;
        }
        Cop myCop = new Cop(Pedestrian, Settings, Pedestrian.Health, AssignedAgency, false, Crimes, Weapons, Names.GetRandomName(Pedestrian.IsMale), Pedestrian.Model.Name, World);
        myCop.SetStats(AssignedPerson, Weapons, Settings.SettingsManager.PoliceSettings.ShowVanillaBlips, "Lincoln");
        if (!Police.Any(x => x.Pedestrian.Exists() && x.Pedestrian.Handle == Pedestrian.Handle))
        {
            Police.Add(myCop);
            myCop.Pedestrian.IsPersistent = true;
        }
        EntryPoint.WriteToConsole($"PEDESTRIANS: Add COP {Pedestrian.Handle}", 2);
    }
    public (Agency agency, DispatchablePerson dispatchablePerson) GetAgencyData(Ped Cop, int WantedLevel)
    {
        string ZoneName = GetInternalZoneString(Cop.Position);
        List<Agency> ZoneAgencies = new List<Agency>();
        if (ZoneName != "")
        {
            ZoneAgencies = Jurisdictions.GetAgencies(ZoneName, WantedLevel, ResponseType.LawEnforcement);
        }
        if (ZoneAgencies != null)
        {
            DispatchablePerson dispatchablePerson;
            foreach (Agency agency in ZoneAgencies)
            {
                dispatchablePerson = agency.GetSpecificPed(Cop);
                if (dispatchablePerson != null)
                {
                    return (agency, dispatchablePerson);
                }
            }
            foreach (Agency agency in Agencies.GetAgencies())
            {
                dispatchablePerson = agency.GetSpecificPed(Cop);
                if (dispatchablePerson != null)
                {
                    return (agency, dispatchablePerson);
                }
            }
        }
        return (null, null);
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
    private void Delete(PedExt ped)
    {
        if (ped != null && ped.Pedestrian.Exists())
        {
            if (ped.Pedestrian.IsInAnyVehicle(false))
            {
                if (ped.Pedestrian.CurrentVehicle.HasPassengers)
                {
                    foreach (Ped Passenger in ped.Pedestrian.CurrentVehicle.Passengers)
                    {
                        Passenger.Delete();
                        GameFiber.Yield();
                    }
                }
                if (ped.Pedestrian.Exists() && ped.Pedestrian.CurrentVehicle.Exists() && ped.Pedestrian.CurrentVehicle != null)
                {
                    ped.Pedestrian.CurrentVehicle.Delete();
                    GameFiber.Yield();
                }
            }
            if (ped.Pedestrian.Exists())
            {
                ped.Pedestrian.Delete();
                GameFiber.Yield();
            }
        }
    }
    private void Delete(Ped ped)
    {
        GameFiber.Yield();
        if (ped != null && ped.Exists())
        {
            if (ped.IsInAnyVehicle(false))
            {
                if (ped.CurrentVehicle.HasPassengers)
                {
                    foreach (Ped Passenger in ped.CurrentVehicle.Passengers)
                    {
                        Passenger.Delete();
                        GameFiber.Yield();
                    }
                }
                if (ped.Exists() && ped.CurrentVehicle.Exists() && ped.CurrentVehicle != null)
                {
                    ped.CurrentVehicle.Delete();
                    GameFiber.Yield();
                }
            }
            if (ped.Exists())
            {
                ped.Delete();
                GameFiber.Yield();
            }
        }
    }
    public bool IsSeatAssignedToAnyone(VehicleExt vehicleToCheck, int seatToCheck) => SeatAssignments.Any(x => x.Vehicle != null && vehicleToCheck != null && x.Vehicle.Handle == vehicleToCheck.Handle && x.Seat == seatToCheck && x.Ped != null);
    public bool IsSeatAssigned(ISeatAssignable pedToCheck, VehicleExt vehicleToCheck, int seatToCheck) => SeatAssignments.Any(x => x.Vehicle != null && vehicleToCheck != null && x.Vehicle.Handle == vehicleToCheck.Handle && x.Seat == seatToCheck && x.Ped != null && pedToCheck != null && x.Ped.Handle != pedToCheck.Handle);
    public bool AddSeatAssignment(ISeatAssignable ped, VehicleExt vehicle, int seat)
    {
        if (ped == null || !ped.Pedestrian.Exists() || vehicle == null || !vehicle.Vehicle.Exists())
        {
            return false;
        }
        if (SeatAssignments.Any(x => x.Vehicle != null && x.Vehicle.Handle == vehicle.Handle && x.Seat == seat))
        {
            return false;
        }
        SeatAssignments.Add(new AssignedSeat(ped, vehicle, seat));
        return true;
    }
    public void RemoveSeatAssignment(ISeatAssignable ped)
    {
        if (ped != null)
        {
            SeatAssignments.RemoveAll(x => x.Ped != null && x.Ped.Handle == ped.Handle);
        }
    }
    public void ExpireSeatAssignments()
    {
        SeatAssignments.RemoveAll(x => x.Vehicle == null || x.Ped == null || !x.Vehicle.Vehicle.Exists() || !x.Ped.Pedestrian.Exists() || x.Ped.Pedestrian.IsDead);
    }
    public List<AssignedSeat> GetPedsAssignedToVehicle(VehicleExt vehicleToCheck)
    {
        return SeatAssignments.Where(x => x.Vehicle != null && vehicleToCheck != null && x.Vehicle.Handle == vehicleToCheck.Handle).ToList();
    }
}