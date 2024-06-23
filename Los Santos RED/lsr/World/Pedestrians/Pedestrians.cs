using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.ArrayExtensions;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

public class Pedestrians : ITaskerReportable
{
    private IAgencies Agencies;
    private IJurisdictions Jurisdictions;
    private ISettingsProvideable Settings;
    private IZones Zones;
    private INameProvideable Names;
    private IPedGroups RelationshipGroups;
    private List<Ped> WorldPeds = new List<Ped>();
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


    public List<PedExt> PossibleTargets { get; private set; } = new List<PedExt>();
    public Cop ClosestCopToPlayer { get; private set; }

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
    public List<CanineUnit> PoliceCanines { get; private set; } = new List<CanineUnit>();
    public List<SecurityGuard> SecurityGuards { get; private set; } = new List<SecurityGuard>();
    public List<Firefighter> Firefighters { get; private set; } = new List<Firefighter>();
    public List<Merchant> Merchants { get; private set; } = new List<Merchant>();
    public List<Teller> Tellers { get; private set; } = new List<Teller>();
    public List<Zombie> Zombies { get; private set; } = new List<Zombie>();
    public List<GangMember> GangMembers { get; private set; } = new List<GangMember>();

    public List<TaxiDriver> TaxiDrivers { get; private set; } = new List<TaxiDriver>();

    public List<PedExt> DeadPeds { get; private set; } = new List<PedExt>();
    public List<PedExt> CivilianList => Civilians.Where(x => x.Pedestrian.Exists()).ToList();
    public List<GangMember> GangMemberList => GangMembers.Where(x => x.Pedestrian.Exists()).ToList();
    public List<Zombie> ZombieList => Zombies.Where(x => x.Pedestrian.Exists()).ToList();
    public List<Cop> PoliceList => Police.Where(x => x.Pedestrian.Exists()).ToList();
    public List<CanineUnit> PoliceCanineList => PoliceCanines.Where(x => x.Pedestrian.Exists()).ToList();
    public List<EMT> EMTList => EMTs.Where(x => x.Pedestrian.Exists()).ToList();
    public List<SecurityGuard> SecurityGuardList => SecurityGuards.Where(x => x.Pedestrian.Exists()).ToList();
    public List<Firefighter> FirefighterList => Firefighters.Where(x => x.Pedestrian.Exists()).ToList();
    public List<Merchant> MerchantList => Merchants.Where(x => x.Pedestrian.Exists()).ToList();
    public List<Teller> TellerList => Tellers.Where(x => x.Pedestrian.Exists()).ToList();
    public List<TaxiDriver> TaxiDriverList => TaxiDrivers.Where(x => x.Pedestrian.Exists()).ToList();
    public List<PedExt> LivingPeople
    {
        get
        {
            List<PedExt> myList = new List<PedExt>();
            myList.AddRange(CivilianList);
            myList.AddRange(GangMemberList);
            myList.AddRange(MerchantList);
            myList.AddRange(TellerList);
            myList.AddRange(EMTList);
            myList.AddRange(PoliceList);
            myList.AddRange(FirefighterList);
            myList.AddRange(SecurityGuardList);
            myList.AddRange(TaxiDriverList);
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
            myList.AddRange(TellerList);
            myList.AddRange(SecurityGuardList);
            myList.AddRange(TaxiDriverList);
            return myList;
        }
    }
    public List<PedExt> ServiceWorkers
    {
        get
        {
            List<PedExt> myList = new List<PedExt>();
            myList.AddRange(MerchantList);
            myList.AddRange(TellerList);
            myList.AddRange(TaxiDriverList);
            return myList;
        }
    }
    public List<PedExt> GeneralCitizenGroup
    {
        get
        {
            List<PedExt> myList = new List<PedExt>();
            myList.AddRange(MerchantList);
            myList.AddRange(TellerList);
            myList.AddRange(CivilianList);
            myList.AddRange(SecurityGuardList);
            myList.AddRange(TaxiDriverList);
            return myList;
        }
    }
    public List<Cop> AllPoliceList
    {
        get
        {
            List<Cop> myList = new List<Cop>();
            myList.AddRange(PoliceList);
            myList.AddRange(PoliceCanineList);
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
            myList.AddRange(TellerList);
            myList.AddRange(EMTList);
            myList.AddRange(PoliceList);
            myList.AddRange(FirefighterList);
            myList.AddRange(DeadPeds);
            myList.AddRange(SecurityGuardList);
            myList.AddRange(PoliceCanineList);
            myList.AddRange(TaxiDriverList);
            return myList;
        }
    }
    public bool AnyInjuredPeopleNearPlayer => PedExts.Any(x => !x.IsDead && (x.IsUnconscious || x.IsInWrithe) && x.DistanceToPlayer <= 150f);
    public bool AnyWantedPeopleNearPlayer => CivilianList.Any(x => x.WantedLevel > 0 && !x.IsBusted && x.DistanceToPlayer <= 150f) || GangMemberList.Any(x => x.WantedLevel > 0 && !x.IsBusted && x.DistanceToPlayer <= 150f) || MerchantList.Any(x => x.WantedLevel > 0 && !x.IsBusted && x.DistanceToPlayer <= 150f) || TellerList.Any(x => x.WantedLevel > 0 && !x.IsBusted && x.DistanceToPlayer <= 150f);
    public string DebugString { get; set; } = "";
    public bool AnyArmyUnitsSpawned => Police.Any(x => x.AssignedAgency.ID == "ARMY" && x.WasModSpawned);
    public bool AnyHelicopterUnitsSpawned => Police.Any(x => x.IsInHelicopter && x.WasModSpawned);
    public bool AnyNooseUnitsSpawned => Police.Any(x => x.AssignedAgency.ID == "NOOSE" && x.WasModSpawned);
    public int TotalSpawnedPolice => Police.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
    public int TotalSpawnedAmbientPolice => Police.Where(x => x.WasModSpawned && !x.IsLocationSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
    public int TotalSpawnedLocationPolice => Police.Where(x => x.WasModSpawned && x.IsLocationSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
    public int TotalSpawnedPoliceCanines => PoliceCanines.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
    public int TotalSpawnedAmbientPoliceCanines => PoliceCanines.Where(x => x.WasModSpawned && !x.IsLocationSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
    public int TotalSpawnedEMTs => EMTs.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
    public int TotalSpawnedAmbientEMTs => EMTs.Where(x => x.WasModSpawned && !x.IsLocationSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
    public int TotalSpawnedAmbientFirefighterss => Firefighters.Where(x => x.WasModSpawned && !x.IsLocationSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
    public int TotalSpawnedGangMembers => GangMembers.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();


    public int TotalSpawnedSecurityGuards => SecurityGuards.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();

    public int TotalSpawnedAmbientGangMembers => GangMembers.Where(x => x.WasModSpawned && !x.IsLocationSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
    public int TotalSpawnedFirefighters => Firefighters.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
    public int TotalSpawnedZombies => Zombies.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();

    public int TotalSpawnedTaxiDrivers => TaxiDrivers.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
    public int TotalSpawnedAmbientTaxiDrivers => TaxiDrivers.Where(x => x.WasModSpawned && !x.IsLocationSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();


    public int TotalSpawnedMerchants => Merchants.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();
    public int TotalSpawnedTellers => Tellers.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();



    public int TotalSpawnedServiceWorkers => ServiceWorkers.Where(x => x.WasModSpawned && x.Pedestrian.Exists() && x.Pedestrian.IsAlive).Count();

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

            thisGangGroup.SetRelationshipWith(RelationshipGroup.SecurityGuard, Relationship.Neutral);//was like
            RelationshipGroup.SecurityGuard.SetRelationshipWith(thisGangGroup, Relationship.Neutral);//was like
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
        WorldPeds = Rage.World.GetAllPeds().ToList();// EntryPoint.ModController.AllPeds.ToList();// Rage.World.GetAllPeds().ToList();// Rage.World.GetEntities(GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).ToList();
        GameFiber.Yield();
        int updated = 0;
        foreach (Ped Pedestrian in WorldPeds.Where(s => s.Exists() && !s.IsDead && s.MaxHealth != 1 && s.Handle != Game.LocalPlayer.Character.Handle))//take 20 is new
        {
            uint localHandle = Pedestrian.Handle;
            string modelName = Pedestrian.Model.Name.ToLower();
            if (Settings.SettingsManager.WorldSettings.ReplaceVanillaShopKeepers && !ServiceWorkers.Any(x => x.Handle == localHandle) && (modelName == "mp_m_shopkeep_01" || modelName == "s_f_m_fembarber" || modelName == "s_m_m_hairdress_01"))// || modelName == "s_m_y_ammucity_01" || modelName == "s_m_m_ammucountry"))
            {
                Delete(Pedestrian);
                continue;
            }

            int PedType = NativeFunction.Natives.GET_PED_TYPE<int>(Pedestrian);
            if (DeadPeds.Any(x => x.Handle == localHandle))
            {
                continue;
            }
            else if (PedType == 28)//Animal
            {
                continue;//dont do anything yet, let the game just do nothing with them, what about killing animals? no big deal for now i guess
            }
            else if (Pedestrian.IsPoliceArmy(PedType))
            {
                if (Police.Any(x => x.Handle == localHandle))
                {
                    continue;
                }
                if (Pedestrian.IsArmy(PedType) || Pedestrian.Model.Name.ToLower() == "s_m_m_prisguard_01")
                {
                    AddAmbientCop(Pedestrian);
                    GameFiber.Yield();
                    continue;
                }
                else
                {

                    if (Settings.SettingsManager.PoliceSpawnSettings.RemoveNonSpawnedPolice || (Settings.SettingsManager.PoliceSpawnSettings.RemoveAmbientPolice && !Pedestrian.IsPersistent))
                    {
                        Delete(Pedestrian);
                        continue;
                    }
                    AddAmbientCop(Pedestrian);
                    GameFiber.Yield();
                }
            }
            else
            {
                if(Pedestrian.IsSecurity())
                {
                    if (SecurityGuards.Any(x => x.Handle == localHandle))
                    {
                        continue;
                    }
                    if (Settings.SettingsManager.SecuritySettings.RemoveNonSpawnedSecurity || (Settings.SettingsManager.SecuritySettings.RemoveAmbientSecurity && !Pedestrian.IsPersistent))
                    {
                        Delete(Pedestrian);
                        continue;
                    }
                    AddAmbientSecurityGuard(Pedestrian);
                    GameFiber.Yield();
                }
                else if (Pedestrian.IsGangMember())
                {
                    if (GangMembers.Any(x => x.Handle == localHandle))
                    {
                        continue;
                    }
                    if (Settings.SettingsManager.GangSettings.RemoveNonSpawnedGangMembers)// || modelName == "s_m_y_ammucity_01" || modelName == "s_m_m_ammucountry"))
                    {
                        Delete(Pedestrian);
                        continue;
                    }
                    else if (Settings.SettingsManager.GangSettings.RemoveNonSpawnedGangMembersOnFoot && Pedestrian.Exists() && !Pedestrian.IsInAnyVehicle(false))
                    {
                        //if (Settings.SettingsManager.GangSettings.RemoveNonSpawnedGangMembersOnFoot_Extra)
                        //{
                            Vector3 blockPos = Pedestrian.Position;


                        //Vector3 Corner1 = NativeHelper.GetOffsetPosition(blockPos, Pedestrian.Heading, 1.0f);

                        //Vector3 Corner2 = NativeHelper.GetOffsetPosition(blockPos, Pedestrian.Heading, -1.0f);

                        new ScenarioBlock(blockPos, "") { Distance = 20f }.Block();
                        EntryPoint.WriteToConsole($"SETTING NEW SCENARIO BLOCK AT {blockPos}");


                        //NativeFunction.Natives.SET_PED_NON_CREATION_AREA(Corner1.X, Corner1.Y, Corner1.Z +1.0f, Corner2.X, Corner2.Y, Corner2.Z-1.0f);


                        //}
                        Delete(Pedestrian);

                        //MoveToHell(Pedestrian);
                        continue;
                    }
                    AddAmbientGangMember(Pedestrian);
                    GameFiber.Yield();
                }
                else if (Pedestrian.Model.Name.ToLower() == "s_m_y_baywatch_01" || Pedestrian.Model.Name.ToLower() == "s_f_y_baywatch_01" || Pedestrian.Model.Name.ToLower() == "s_m_y_uscg_01")//lifeguards& coast guard
                {
                    if (Police.Any(x => x.Handle == localHandle))
                    {
                        continue;
                    }
                    NativeFunction.Natives.SET_PED_AS_COP(Pedestrian);

                    Pedestrian.RelationshipGroup = "COP";
                    AddAmbientCop(Pedestrian);
                }
                else if (!Civilians.Any(x => x.Handle == localHandle) && !ServiceWorkers.Any(x => x.Handle == localHandle) 
                    && !Zombies.Any(x => x.Handle == localHandle) && !GangMembers.Any(x => x.Handle == localHandle) 
                    && !Police.Any(x => x.Handle == localHandle) && !EMTs.Any(x => x.Handle == localHandle) && !Firefighters.Any(x => x.Handle == localHandle) && !SecurityGuards.Any(x => x.Handle == localHandle))
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

    private void MoveToHell(Ped pedestrian)
    {
       if(pedestrian.Exists())
        {
            pedestrian.Position = new Vector3(136.5146f, -2203.149f, 7.30914f);
            AddAmbientGangMember(pedestrian);
        }
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
    public void UpdateDead()
    {
        int updated = 0;
        foreach(PedExt deadPed in DeadPeds.ToList())
        {
            if(!deadPed.Pedestrian.Exists())
            {
                continue;
            }
            deadPed.UpdatePositionData();
            updated++;
            if(updated >= 10)
            {
                GameFiber.Yield();
            }
        }
    }
    public bool AnyCopsNearPosition(Vector3 Position, float Distance) => Position != Vector3.Zero && Police.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Position) <= Distance);
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
        foreach (CanineUnit Canine in PoliceCanines)
        {
            if (Canine.Pedestrian.Exists() && Canine.Pedestrian.Handle != Game.LocalPlayer.Character.Handle)
            {
                Canine.Pedestrian.Delete();
                EntryPoint.PersistentPedsDeleted++;
            }
        } 
        PoliceCanines.Clear();
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
        foreach (PedExt serviceWorker in ServiceWorkers)
        {
            if (serviceWorker.Pedestrian.Exists() && serviceWorker.Pedestrian.Handle != Game.LocalPlayer.Character.Handle)
            {
                serviceWorker.Pedestrian.Delete();
            }
        }
        Merchants.Clear();
        Tellers.Clear();
        TaxiDrivers.Clear();
        foreach (Zombie zombie in Zombies)
        {
            if (zombie.Pedestrian.Exists() && zombie.Pedestrian.Handle != Game.LocalPlayer.Character.Handle)
            {
                zombie.Pedestrian.Delete();
            }
        }
        Zombies.Clear();
        ClearGangMembers();
        foreach (SecurityGuard securityGuard in SecurityGuards)
        {
            if (securityGuard.Pedestrian.Exists() && securityGuard.Pedestrian.Handle != Game.LocalPlayer.Character.Handle)
            {
                securityGuard.Pedestrian.Delete();
            }
        }
        SecurityGuards.Clear();
        foreach (PedExt deadPed in DeadPeds)
        {
            if (deadPed.Pedestrian.Exists() && deadPed.Pedestrian.Handle != Game.LocalPlayer.Character.Handle)
            {
                deadPed.Pedestrian.Delete();
            }
        }
        DeadPeds.Clear();
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
        foreach (Cop Cop in Police.Where(x => x.Pedestrian.Exists() && x.CanRemove))// && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) >= 200))
        {
            Cop.DeleteBlip();
            Cop.Pedestrian.IsPersistent = false;
            EntryPoint.PersistentPedsNonPersistent++;
            EntryPoint.WriteToConsole($"Pedestrians: Cop {Cop.Pedestrian.Handle} Removed Blip Set Non Persistent", 5);
            if (Cop.Pedestrian.IsDead && !DeadPeds.Any(x => x.Handle == Cop.Handle))
            {
                Cop.IsDead = true;
                DeadPeds.Add(Cop);
            }
        }
        Police.RemoveAll(x => x.CanRemove);
        GameFiber.Yield();
        foreach (CanineUnit CanineUnit in PoliceCanines.Where(x => x.Pedestrian.Exists() && x.CanRemove))// && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) >= 200))
        {
            CanineUnit.DeleteBlip();
            CanineUnit.Pedestrian.IsPersistent = false;
            EntryPoint.PersistentPedsNonPersistent++;
            EntryPoint.WriteToConsole($"Pedestrians: CanineUnit {CanineUnit.Pedestrian.Handle} Removed Blip Set Non Persistent", 5);
            if (CanineUnit.Pedestrian.IsDead && !DeadPeds.Any(x => x.Handle == CanineUnit.Handle))
            {
                CanineUnit.IsDead = true;
                DeadPeds.Add(CanineUnit);
            }
        }
        PoliceCanines.RemoveAll(x => x.CanRemove);
        GameFiber.Yield();
        foreach (SecurityGuard SecurityGuard in SecurityGuards.Where(x => x.Pedestrian.Exists() && x.CanRemove))// && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) >= 200))
        {
            SecurityGuard.DeleteBlip();
            SecurityGuard.Pedestrian.IsPersistent = false;
            EntryPoint.PersistentPedsNonPersistent++;
            EntryPoint.WriteToConsole($"Pedestrians: SecurityGuard {SecurityGuard.Pedestrian.Handle} Removed Blip Set Non Persistent", 5);
            if (SecurityGuard.Pedestrian.IsDead && !DeadPeds.Any(x => x.Handle == SecurityGuard.Handle))
            {
                SecurityGuard.IsDead = true;
                DeadPeds.Add(SecurityGuard);
            }
        }
        SecurityGuards.RemoveAll(x => x.CanRemove);
        GameFiber.Yield();
        foreach (EMT EMT in EMTs.Where(x => x.Pedestrian.Exists() && x.CanRemove))// && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) >= 200))
        {
            EMT.DeleteBlip();
            EMT.Pedestrian.IsPersistent = false;
            EntryPoint.PersistentPedsNonPersistent++;
            EntryPoint.WriteToConsole($"Pedestrians: EMT {EMT.Pedestrian.Handle} Removed Blip Set Non Persistent", 5);
            if (EMT.Pedestrian.IsDead && !DeadPeds.Any(x => x.Handle == EMT.Handle))
            {
                EMT.IsDead = true;
                DeadPeds.Add(EMT);
            }
        }
        EMTs.RemoveAll(x => x.CanRemove || x.Handle == Game.LocalPlayer.Character.Handle);
        GameFiber.Yield();
        foreach (Firefighter Firefighter in Firefighters.Where(x => x.Pedestrian.Exists() && x.CanRemove))// && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) >= 200))
        {
            Firefighter.DeleteBlip();
            Firefighter.Pedestrian.IsPersistent = false;
            EntryPoint.PersistentPedsNonPersistent++;
            EntryPoint.WriteToConsole($"Pedestrians: Firefighter {Firefighter.Pedestrian.Handle} Removed Blip Set Non Persistent", 5);
            if (Firefighter.Pedestrian.IsDead && !DeadPeds.Any(x => x.Handle == Firefighter.Handle))
            {
                Firefighter.IsDead = true;
                DeadPeds.Add(Firefighter);
            }
        }
        Firefighters.RemoveAll(x => x.CanRemove || x.Handle == Game.LocalPlayer.Character.Handle);
        GameFiber.Yield();
        foreach (PedExt Civilian in ServiceWorkers.Where(x => x.Pedestrian.Exists() && x.CanRemove))
        {
            Civilian.DeleteBlip();
            if (Civilian.Pedestrian.IsDead && !DeadPeds.Any(x => x.Handle == Civilian.Handle))
            {
                Civilian.IsDead = true;
                DeadPeds.Add(Civilian);
            }
        }
        Merchants.RemoveAll(x => x.CanRemove || x.Handle == Game.LocalPlayer.Character.Handle);
        Tellers.RemoveAll(x => x.CanRemove || x.Handle == Game.LocalPlayer.Character.Handle);
        TaxiDrivers.RemoveAll(x => x.CanRemove || x.Handle == Game.LocalPlayer.Character.Handle);
        GameFiber.Yield();
        foreach (PedExt Civilian in LivingPeople.Where(x => x.IsUnconscious && x.DistanceToPlayer >= 100f && x.Pedestrian.Exists()))
        {
            Civilian.DeleteBlip();
            Civilian.Pedestrian.IsPersistent = false;
            EntryPoint.PersistentPedsNonPersistent++;
            EntryPoint.WriteToConsole($"Pedestrians: LivingPeople {Civilian.Pedestrian.Handle} Removed Blip Set Non Persistent since they are uncoscious", 5);
        }
    }
    private void PruneGangMembers()
    {
        foreach (GangMember GangMember in GangMembers.Where(x => x.Pedestrian.Exists() && !x.IsManuallyDeleted && x.CanRemove && x.Pedestrian.IsDead))// && x.Pedestrian.IsPersistent))// && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) >= 200))
        {
            GangMember.DeleteBlip();
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
            else if (pedExt.GetType() == typeof(CanineUnit))
            {
                if (!PoliceCanines.Any(x => x.Handle == pedExt.Handle))
                {
                    PoliceCanines.Add((CanineUnit)pedExt);
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
            else if (pedExt.GetType() == typeof(SecurityGuard))
            {
                EntryPoint.WriteToConsole($"SECURITY CHECK IF EXISTS {pedExt.Handle}");
                if (!SecurityGuards.Any(x => x.Handle == pedExt.Handle))
                {
                    SecurityGuards.Add((SecurityGuard)pedExt);
                    EntryPoint.WriteToConsole($"SECURITY WAS ADDED AS NEW {pedExt.Handle}");
                }
            }
            else if (pedExt.GetType() == typeof(Teller))
            {
                if (!Tellers.Any(x => x.Handle == pedExt.Handle))
                {
                    Tellers.Add((Teller)pedExt);
                }
            }
            else if (pedExt.GetType() == typeof(TaxiDriver))
            {
                if (!TaxiDrivers.Any(x => x.Handle == pedExt.Handle))
                {
                    TaxiDrivers.Add((TaxiDriver)pedExt);
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
    private void AddAmbientCivilian(Ped Pedestrian)
    {  
        bool WasPersistentOnCreate = false;
        if(!Pedestrian.Exists())
        {
            return;
        }
        Pedestrian.Money = 0;// toCreate.Money;
        if (Pedestrian.IsPersistent)
        {
            WasPersistentOnCreate = true;
        }
        PedGroup myGroup = RelationshipGroups.GetPedGroup(Pedestrian.RelationshipGroup.Name);
        if (myGroup == null)
        {
            myGroup = new PedGroup(Pedestrian.RelationshipGroup.Name, Pedestrian.RelationshipGroup.Name, Pedestrian.RelationshipGroup.Name, false);
        }
        PedExt createdPedExt;
        if (Pedestrian.CurrentVehicle.Exists() && Pedestrian.CurrentVehicle.Model.Hash == 3338918751) //Taxi
        {
            createdPedExt = new TaxiDriver(Pedestrian, Settings, Crimes, Weapons, Names.GetRandomName(Pedestrian.IsMale), myGroup.MemberName, World, false)
            {
                WasPersistentOnCreate = WasPersistentOnCreate,
            };
           // EntryPoint.WriteToConsole($"CREATE AMBIENT TAXI PED {createdPedExt.Handle}");
        }
        else
        {
            createdPedExt = new PedExt(Pedestrian, Settings, Crimes, Weapons, Names.GetRandomName(Pedestrian.IsMale), myGroup.MemberName, World)
            {
                WasPersistentOnCreate = WasPersistentOnCreate,
            };
        }
        AddEntity(createdPedExt);
        
        createdPedExt.SetBaseStats(null, ShopMenus, Weapons, false);  
        if (!Pedestrian.Exists())
        {
            return;
        }
        if (Settings.SettingsManager.CivilianSettings.OverrideAccuracy)
        {
            Pedestrian.Accuracy = Settings.SettingsManager.CivilianSettings.GeneralAccuracy;
        }
        if (Settings.SettingsManager.CivilianSettings.OverrideHealth)
        {
            int DesiredHealth = RandomItems.MyRand.Next(Settings.SettingsManager.CivilianSettings.MinHealth, Settings.SettingsManager.CivilianSettings.MaxHealth) + 100;
            Pedestrian.MaxHealth = DesiredHealth;
            Pedestrian.Health = DesiredHealth;
        }

        if (Settings.SettingsManager.CivilianSettings.DisableWrithe)
        {
            NativeFunction.Natives.SET_PED_CONFIG_FLAG(Pedestrian, (int)281, true);
        }
        if (Settings.SettingsManager.CivilianSettings.DisableWritheShooting)
        {
            NativeFunction.Natives.SET_PED_CONFIG_FLAG(Pedestrian, (int)327, true);
        }



        if (Pedestrian.IsPersistent)
        {
            createdPedExt.WasPersistentOnCreate = true;
        }
        if (!Settings.SettingsManager.CivilianSettings.TaskMissionPeds && createdPedExt.WasPersistentOnCreate)//must have been spawned by another mod?
        {
            createdPedExt.WillCallPolice = false;
            createdPedExt.WillCallPoliceIntense = false;
            createdPedExt.WillFight = false;
            createdPedExt.WillFightPolice = false;
            createdPedExt.WillAlwaysFightPolice = false;
            createdPedExt.CanBeAmbientTasked = false;
            createdPedExt.SetupTransactionItems(null, false);
        }
        else
        {
            createdPedExt.SetupTransactionItems(EntryPoint.FocusZone?.GetIllicitMenu(Settings, ShopMenus), false);
        }

        //Civilians.Add(createdPedExt);
        //EntryPoint.WriteToConsole("ADD AMBIENT FINAL");
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
        gm.SetStats(gangPerson, ShopMenus, Weapons, Settings.SettingsManager.GangSettings.ShowAmbientBlips, false, false, false);
        if(!Pedestrian.Exists())
        {
            return;
        }
        if (Pedestrian.IsPersistent)
        {
            gm.WasPersistentOnCreate = true;
        }
        if (!Settings.SettingsManager.CivilianSettings.TaskMissionPeds && gm.WasPersistentOnCreate)//must have been spawned by another mod?
        {
            gm.WillFight = false;
            gm.WillFightPolice = false;
            gm.WillAlwaysFightPolice = false;
            gm.CanBeAmbientTasked = false;
            gm.SetupTransactionItems(null, false);
        }
        if (isCarSpawn && Settings.SettingsManager.GangSettings.ForceAmbientCarDocile)
        {
            gm.WillFight = false;
            gm.WillFightPolice = false;
            gm.WillAlwaysFightPolice = false;
            gm.SetupTransactionItems(null, false);
            NativeFunction.Natives.REMOVE_ALL_PED_WEAPONS(Pedestrian, false);
        }
        GangMembers.Add(gm);    
    }
    private void AddAmbientCop(Ped Pedestrian)
    {
        if (!Pedestrian.Exists())
        {
            return;
        }
        var AgencyData = GetAgencyData(Pedestrian, 0, ResponseType.LawEnforcement);
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
        myCop.SetStats(AssignedPerson, Weapons, Settings.SettingsManager.PoliceSettings.AttachBlipsToAmbientPeds, "Lincoln", Settings.SettingsManager.PoliceSettings.SightDistance);
        if(!Pedestrian.Exists())
        {
            return;
        }
        //Pedestrian.IsPersistent = true;
        if (!Police.Any(x => x.Pedestrian.Exists() && x.Pedestrian.Handle == Pedestrian.Handle))
        {
            Police.Add(myCop);
        }
        //EntryPoint.WriteToConsole($"PEDESTRIANS: Add COP {Pedestrian.Handle}", 2);
    }
    private void AddAmbientSecurityGuard(Ped Pedestrian)
    {
        if(!Pedestrian.Exists())
        {
            return;
        }
        var AgencyData = GetAgencyData(Pedestrian, 0, ResponseType.Security);
        Agency AssignedAgency = AgencyData.agency;
        DispatchablePerson AssignedPerson = AgencyData.dispatchablePerson;
        if (AssignedAgency == null || AssignedPerson == null || !Pedestrian.Exists())
        {
            if (Pedestrian.IsPersistent)
            {
                EntryPoint.PersistentPedsDeleted++;
            }
            Delete(Pedestrian);
            EntryPoint.WriteToConsole($"PEDESTRIANS: Add SECURITY FAIL, DELETING", 2);
            return;
        }
        SecurityGuard mySecurityGuard = new SecurityGuard(Pedestrian, Settings, Pedestrian.Health, AssignedAgency, false, Crimes, Weapons, Names.GetRandomName(Pedestrian.IsMale), Pedestrian.Model.Name, World);
        mySecurityGuard.SetStats(AssignedPerson, Weapons, Settings.SettingsManager.SecuritySettings.AttachBlipsToAmbientPeds);
        if (!Pedestrian.Exists())
        {
            return;
        }
        //Pedestrian.IsPersistent = true;
        if (!SecurityGuards.Any(x => x.Pedestrian.Exists() && x.Pedestrian.Handle == Pedestrian.Handle))
        {
            SecurityGuards.Add(mySecurityGuard);
        }
        //EntryPoint.WriteToConsole($"PEDESTRIANS: Add SECURITY {Pedestrian.Handle}", 2);        
    }
    public (Agency agency, DispatchablePerson dispatchablePerson) GetAgencyData(Ped ped, int WantedLevel, ResponseType responseType)
    {
        string ZoneName = GetInternalZoneString(ped.Position);
        List<Agency> ZoneAgencies = new List<Agency>();
        if (ZoneName != "")
        {
            ZoneAgencies = Jurisdictions.GetAgencies(ZoneName, WantedLevel, responseType);
        }
        DispatchablePerson dispatchablePerson;
        if (ZoneAgencies != null)
        {
            foreach (Agency agency in ZoneAgencies)
            {
                dispatchablePerson = agency.GetSpecificPed(ped);
                if (dispatchablePerson != null)
                {
                    return (agency, dispatchablePerson);
                }
            }
        }
        foreach (Agency agency in Agencies.GetAgenciesByResponse(responseType))
        {
            dispatchablePerson = agency.GetSpecificPed(ped);
            if (dispatchablePerson != null)
            {
                return (agency, dispatchablePerson);
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
                        if(!ped.Exists() || !ped.CurrentVehicle.Exists())
                        {
                            break;
                        }
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
    public void SetPossiblePoliceTargets()
    {
        if (World.IsZombieApocalypse)
        {
            List<PedExt> TotalList = new List<PedExt>();
            TotalList.AddRange(World.Pedestrians.CivilianList);
            TotalList.AddRange(World.Pedestrians.GangMemberList);
            TotalList.AddRange(World.Pedestrians.ServiceWorkers);
            TotalList.AddRange(World.Pedestrians.ZombieList);
            PossibleTargets = TotalList.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive && !x.IsUnconscious && (x.IsWanted || (x.IsBusted && !x.IsArrested)) && x.DistanceToPlayer <= 200f).ToList();//150f
        }
        else
        {
            List<PedExt> TotalList = new List<PedExt>();
            TotalList.AddRange(World.Pedestrians.CivilianList);
            TotalList.AddRange(World.Pedestrians.GangMemberList);
            TotalList.AddRange(World.Pedestrians.ServiceWorkers);
            PossibleTargets = TotalList.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive && !x.IsUnconscious && (x.IsWanted || (x.IsBusted && !x.IsArrested)) && x.DistanceToPlayer <= 200f).ToList();//150f
        }
        ClosestCopToPlayer = World.Pedestrians.PoliceList.Where(x => x.Pedestrian.Exists() && !x.IsInVehicle && x.DistanceToPlayer <= 30f && x.Pedestrian.IsAlive).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
    }
    public void CleanupAmbient()
    {
        //if (Civilians.Count() < 50)
        //{
        //    return;
        //}
        //PedExt ped = Civilians.Where(x => x.Pedestrian.Exists() && !x.WasModSpawned && !x.Pedestrian.IsPersistent && !x.Pedestrian.IsOnScreen).FirstOrDefault();
        //if (ped == null)
        //{
        //    return;
        //}
        //EntryPoint.WriteToConsole($"CleanupAmbient RAN DELETED CIVILIAN PED");
        //ped.FullyDelete();
    }

}