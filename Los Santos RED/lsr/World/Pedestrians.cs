using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

public class Pedestrians
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
    public Pedestrians(IAgencies agencies, IZones zones, IJurisdictions jurisdictions, ISettingsProvideable settings, INameProvideable names, IPedGroups relationshipGroups, IWeapons weapons, ICrimes crimes, IShopMenus shopMenus, IGangs gangs, IGangTerritories gangTerritories)
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
    public void ClearSpawned()
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




       // PedExt pedExt = Police.FirstOrDefault(x => x.Handle == Handle);
       // if (pedExt != null)
       // {
       //     return pedExt;
       //}
       // pedExt = EMTs.FirstOrDefault(x => x.Handle == Handle);
       // if (pedExt != null)
       // {
       //     return pedExt;
       // }
       // pedExt = Firefighters.FirstOrDefault(x => x.Handle == Handle);
       // if (pedExt != null)
       // {
       //     return pedExt;
       // }
       // pedExt = Merchants.FirstOrDefault(x => x.Handle == Handle);
       // if (pedExt != null)
       // {
       //     return pedExt;
       // }
       // pedExt = GangMembers.FirstOrDefault(x => x.Handle == Handle);
       // if (pedExt != null)
       // {
       //     return pedExt;
       // }
       // return Civilians.FirstOrDefault(x => x.Handle == Handle);

    }
    public void Prune()
    {
        PruneServicePeds();
        PruneGangMembers();
        PruneCivilians();
        DeadPeds.RemoveAll(x => !x.Pedestrian.Exists());
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
        Police.RemoveAll(x => x.CanRemove);
        EMTs.RemoveAll(x => x.CanRemove);
        Firefighters.RemoveAll(x => x.CanRemove);
        Merchants.RemoveAll(x => x.CanRemove);
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
            EntryPoint.WriteToConsole($"Pedestrians: GANG MEMBER {GangMember.Pedestrian.Handle} Removed Blip Set Non Persistent hasBlip {hasBlip}", 5);
        }
        GangMembers.RemoveAll(x => x.CanRemove);
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
            EntryPoint.WriteToConsole($"Pedestrians: CIVILIAN {Civilian.Pedestrian.Handle} Removed Blip Set Non Persistent hasBlip {hasBlip}", 5);
        }
        RelationshipGroup formerPlayer = new RelationshipGroup("FORMERPLAYER");
        foreach (PedExt Civilian in Civilians.Where(x => x.DistanceToPlayer >= 200f && x.Pedestrian.Exists() && x.Pedestrian.IsPersistent))// && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) >= 200))
        {
            if (Civilian.Pedestrian.RelationshipGroup == formerPlayer)
            {
                Civilian.Pedestrian.IsPersistent = false;
                EntryPoint.WriteToConsole($"Pedestrians: CIVILIAN {Civilian.Pedestrian.Handle} SET NON PERISISTENT, FORMER PLAYER", 5);
                EntryPoint.PersistentPedsNonPersistent++;
            }
            else if (Civilian.IsWanted && !Civilian.WasPersistentOnCreate && !Civilian.WasModSpawned)
            {
                Civilian.Pedestrian.IsPersistent = false;
                EntryPoint.WriteToConsole($"Pedestrians: CIVILIAN {Civilian.Pedestrian.Handle} SET NON PERISISTENT, WAS NOT PERSISTENTLY CREATED", 5);
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
        Civilians.RemoveAll(x => x.CanRemove);
        Civilians.RemoveAll(x => x.Pedestrian.Exists() && x.Pedestrian.RelationshipGroup == RelationshipGroup.Cop);
        PedExts.RemoveAll(x => x.Pedestrian.Exists() && x.Pedestrian.Handle == Game.LocalPlayer.Character.Handle);
        PedExts.RemoveAll(x => x.Handle == Game.LocalPlayer.Character.Handle);


        PedExts.RemoveAll(x => x.Handle == (uint)Game.LocalPlayer.Character.Handle);

        //DeadPeds.RemoveAll(x => x.Handle == Game.LocalPlayer.Character.Handle);
    }
    public void Setup()
    {
        //foreach(Gang gang in Gangs.AllGangs)
        //{
        //    RelationshipGroup thisGangGroup = new RelationshipGroup(gang.ID);
        //    RelationshipGroup policeGroup = new RelationshipGroup("COP");
        //    foreach (Gang otherGang in Gangs.AllGangs)
        //    {
        //        if(otherGang.ID != gang.ID)
        //        {
        //            RelationshipGroup otherGangGroup = new RelationshipGroup(otherGang.ID);
        //            otherGangGroup.SetRelationshipWith(thisGangGroup, Relationship.Neutral);
        //            thisGangGroup.SetRelationshipWith(otherGangGroup, Relationship.Neutral);
        //        }
        //    }
        //    thisGangGroup.SetRelationshipWith(policeGroup, Relationship.Neutral);
        //    policeGroup.SetRelationshipWith(thisGangGroup, Relationship.Neutral);
        //}
        foreach (Gang gang in Gangs.AllGangs)
        {
            RelationshipGroup thisGangGroup = new RelationshipGroup(gang.ID);
            RelationshipGroup policeGroup = new RelationshipGroup("COP");
            foreach (Gang otherGang in Gangs.AllGangs)
            {
                if (otherGang.ID != gang.ID)
                {
                    if(gang.EnemyGangs.Contains(otherGang.ID))
                    {
                        RelationshipGroup otherGangGroup = new RelationshipGroup(otherGang.ID);
                        otherGangGroup.SetRelationshipWith(thisGangGroup, Relationship.Neutral);
                        thisGangGroup.SetRelationshipWith(otherGangGroup, Relationship.Neutral);
                    }
                    else
                    {
                        RelationshipGroup otherGangGroup = new RelationshipGroup(otherGang.ID);
                        otherGangGroup.SetRelationshipWith(thisGangGroup, Relationship.Like);
                        thisGangGroup.SetRelationshipWith(otherGangGroup, Relationship.Like);
                    }

                }
            }
            thisGangGroup.SetRelationshipWith(policeGroup, Relationship.Like);
            policeGroup.SetRelationshipWith(thisGangGroup, Relationship.Like);
        }
        NativeFunction.Natives.SET_AMBIENT_PEDS_DROP_MONEY(false);
    }
    public void Dispose()
    {
        ClearSpawned();
        NativeFunction.Natives.SET_AMBIENT_PEDS_DROP_MONEY(true);
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
    public void CreateNew()
    {
        WorldPeds = Rage.World.GetEntities(GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).ToList();
        GameFiber.Yield();
        foreach (Ped Pedestrian in WorldPeds.Where(s => s.Exists() && !s.IsDead && s.MaxHealth != 1 && s.Handle != Game.LocalPlayer.Character.Handle))//take 20 is new
        {
            string modelName = Pedestrian.Model.Name.ToLower();
            if (Settings.SettingsManager.WorldSettings.ReplaceVanillaShopKeepers && (modelName == "mp_m_shopkeep_01"))// || modelName == "s_m_y_ammucity_01" || modelName == "s_m_m_ammucountry"))
            {
                Delete(Pedestrian);
                //Pedestrian.Delete();
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
                if (Settings.SettingsManager.PoliceSettings.RemoveVanillaSpawnedPeds)
                {
                    Delete(Pedestrian);
                    continue;
                }
                AddAmbientCop(Pedestrian);
                GameFiber.Yield();
            }
            else
            {
                if(Pedestrian.IsGangMember())
                {
                    if(GangMembers.Any(x => x.Handle == localHandle))
                    {
                        continue;
                    }
                    if (Settings.SettingsManager.GangSettings.RemoveVanillaSpawnedPeds)// || modelName == "s_m_y_ammucity_01" || modelName == "s_m_m_ammucountry"))
                    {
                        Delete(Pedestrian);
                        continue;
                    }
                    else if(Settings.SettingsManager.GangSettings.RemoveVanillaSpawnedPedsOnFoot && Pedestrian.Exists() && !Pedestrian.IsInAnyVehicle(false))
                    {
                        EntryPoint.WriteToConsole("RemoveVanillaSpawnedPedsOnFoot DELETED PED");
                        Delete(Pedestrian);
                        continue;
                    }
                    AddAmbientGangMember(Pedestrian);
                    GameFiber.Yield();
                }
                else if (!Civilians.Any(x => x.Handle == localHandle) && !Merchants.Any(x=> x.Handle == localHandle) && !Zombies.Any(x => x.Handle == localHandle) && !GangMembers.Any(x=> x.Handle == localHandle) && !Police.Any(x => x.Handle == localHandle))
                {
                    AddAmbientCivilian(Pedestrian);
                    GameFiber.Yield();
                }
            }
        }
        if (Settings.SettingsManager.DebugSettings.PrintUpdateTimes)
        {
            EntryPoint.WriteToConsole($"Pedestrians.CreateNew Ran Time Since {Game.GameTime - GameTimeLastCreatedPeds}", 5);
        }
        GameTimeLastCreatedPeds = Game.GameTime;
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
    private void AddAmbientCivilian(Ped Pedestrian)
    {  
        bool WillFight = false;
        bool WillCallPolice = false;
        bool WillCallPoliceIntense = false;
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
            }
            else
            {
                SetCivilianStats(Pedestrian);
                WillFight = RandomItems.RandomPercent(CivilianFightPercentage());
                WillCallPolice = RandomItems.RandomPercent(CivilianCallPercentage());
                WillCallPoliceIntense = RandomItems.RandomPercent(CivilianSeriousCallPercentage());
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
                canBeAmbientTasked = false;
            }
            //EntryPoint.WriteToConsole($"Added Ambient Civilian {Pedestrian.Handle}");
            PedGroup myGroup = RelationshipGroups.GetPedGroup(Pedestrian.RelationshipGroup.Name);
            if (myGroup == null)
            {
                myGroup = new PedGroup(Pedestrian.RelationshipGroup.Name, Pedestrian.RelationshipGroup.Name, Pedestrian.RelationshipGroup.Name, false);
            }
            ShopMenu toAdd = GetIllicitMenu();
            PedExt toCreate = new PedExt(Pedestrian, Settings, WillFight, WillCallPolice, IsGangMember, false, Names.GetRandomName(Pedestrian.IsMale), Crimes, Weapons, myGroup.MemberName) { CanBeAmbientTasked = canBeAmbientTasked , ShopMenu = toAdd, WillCallPoliceIntense = WillCallPoliceIntense, WasPersistentOnCreate = WasPersistentOnCreate };
            Civilians.Add(toCreate);
            if (Pedestrian.Exists())
            {
                Pedestrian.Money = 0;// toCreate.Money;
                NativeFunction.Natives.SET_PED_SUFFERS_CRITICAL_HITS(Pedestrian, false);
            }
        }
    }
    private void AddAmbientGangMember(Ped Pedestrian)
    {
        if (Pedestrian.Exists())
        {
            bool WasPersistentOnCreate = false;
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
            bool WillFight = RandomItems.RandomPercent(MyGang.FightPercentage);
            bool canBeAmbientTasked = true;
            if (Pedestrian.IsPersistent)
            {
                WasPersistentOnCreate = true;
            }
            if (!Settings.SettingsManager.CivilianSettings.TaskMissionPeds && WasPersistentOnCreate)//must have been spawned by another mod?
            {
                WillFight = false;
                canBeAmbientTasked = false;
            }
            ShopMenu toAdd = null;
            if (RandomItems.RandomPercent(MyGang.DrugDealerPercentage))
            {
                toAdd = ShopMenus.GetRandomMenu(MyGang.DealerMenuGroup);
                if (toAdd == null)
                {
                    toAdd = ShopMenus.GetRandomDrugDealerMenu();
                }
            }
            GangMember gm = new GangMember(Pedestrian, Settings, MyGang, false, WillFight, false, Names.GetRandomName(Pedestrian.IsMale), Crimes, Weapons) { CanBeAmbientTasked = canBeAmbientTasked, ShopMenu = toAdd,WasPersistentOnCreate = WasPersistentOnCreate };
            if (Pedestrian.Exists())
            {
                gm.Money = gm.Money;
                gm.Pedestrian.Money = 0;// gm.Money;
                NativeFunction.Natives.SET_PED_SUFFERS_CRITICAL_HITS(Pedestrian, false);
                gm.WeaponInventory.IssueWeapons(Weapons, RandomItems.RandomPercent(MyGang.PercentageWithMelee), RandomItems.RandomPercent(MyGang.PercentageWithSidearms), RandomItems.RandomPercent(MyGang.PercentageWithLongGuns));
            }
            bool withPerson = false;
            if (gangPerson != null)
            {
                if (Settings.SettingsManager.GangSettings.OverrideHealth)
                {
                    int health = RandomItems.GetRandomNumberInt(gangPerson.HealthMin, gangPerson.HealthMax) + 100;
                    Pedestrian.MaxHealth = health;
                    Pedestrian.Health = health;
                }
                if (Settings.SettingsManager.GangSettings.OverrideArmor)
                {
                    int armor = RandomItems.GetRandomNumberInt(gangPerson.ArmorMin, gangPerson.ArmorMax);
                    Pedestrian.Armor = armor;
                }
                gm.Accuracy = RandomItems.GetRandomNumberInt(gangPerson.AccuracyMin, gangPerson.AccuracyMax);
                gm.ShootRate = RandomItems.GetRandomNumberInt(gangPerson.ShootRateMin, gangPerson.ShootRateMax);
                gm.CombatAbility = RandomItems.GetRandomNumberInt(gangPerson.CombatAbilityMin, gangPerson.CombatAbilityMax);

                if (Settings.SettingsManager.GangSettings.OverrideAccuracy)
                {
                    Pedestrian.Accuracy = gm.Accuracy;
                    NativeFunction.Natives.SET_PED_SHOOT_RATE(Pedestrian, gm.ShootRate);
                    NativeFunction.Natives.SET_PED_COMBAT_ABILITY(Pedestrian, gm.CombatAbility);
                }
                withPerson = true;
            }
            else
            {
                if (Settings.SettingsManager.GangSettings.OverrideAccuracy)
                {
                    Pedestrian.Accuracy = gm.Accuracy;
                    NativeFunction.Natives.SET_PED_SHOOT_RATE(Pedestrian, gm.ShootRate);
                    NativeFunction.Natives.SET_PED_COMBAT_ABILITY(Pedestrian, gm.CombatAbility);
                }
                //EntryPoint.WriteToConsole($"PEDESTRIANS: COULD NOT LOOKUP GANG MEMBER GOING WITH DEFAULT", 2);
            }
            //EntryPoint.WriteToConsole($"PEDESTRIANS: Add GANG MEMBER {Pedestrian.Handle} withPerson lookup? {withPerson}", 2);
            GangMembers.Add(gm);
        }
    }
    private void AddAmbientCop(Ped Pedestrian)
    {
        var AgencyData = GetAgencyData(Pedestrian, 0);
        Agency AssignedAgency = AgencyData.agency;
        DispatchablePerson AssignedPerson = AgencyData.dispatchablePerson;

        if (AssignedAgency != null && Pedestrian.Exists() && AssignedPerson != null)
        {
            if (Settings.SettingsManager.PoliceSettings.OverrideHealth)
            {
                int health = RandomItems.GetRandomNumberInt(AssignedPerson.HealthMin, AssignedPerson.HealthMax) + 100;
                Pedestrian.MaxHealth = health;
                Pedestrian.Health = health;
            }
            if (Settings.SettingsManager.PoliceSettings.OverrideArmor)
            {
                int armor = RandomItems.GetRandomNumberInt(AssignedPerson.ArmorMin, AssignedPerson.ArmorMax);
                Pedestrian.Armor = armor;
            }

            Cop myCop = new Cop(Pedestrian, Settings, Pedestrian.Health, AssignedAgency, false, Crimes, Weapons, Names.GetRandomName(Pedestrian.IsMale), Pedestrian.Model.Name);
            myCop.WeaponInventory.IssueWeapons(Weapons, true,true,true);

            int accuracy = RandomItems.GetRandomNumberInt(AssignedPerson.AccuracyMin, AssignedPerson.AccuracyMax);
            int shootRate = RandomItems.GetRandomNumberInt(AssignedPerson.ShootRateMin, AssignedPerson.ShootRateMax);
            int combatAbility = RandomItems.GetRandomNumberInt(AssignedPerson.CombatAbilityMin, AssignedPerson.CombatAbilityMax);

            int tazerAccuracy = RandomItems.GetRandomNumberInt(AssignedPerson.TaserAccuracyMin, AssignedPerson.TaserAccuracyMax);
            int tazerShootRate = RandomItems.GetRandomNumberInt(AssignedPerson.TaserShootRateMin, AssignedPerson.TaserShootRateMax);

            int vehicleAccuracy = RandomItems.GetRandomNumberInt(AssignedPerson.VehicleAccuracyMin, AssignedPerson.VehicleAccuracyMax);
            int vehicleShootRate = RandomItems.GetRandomNumberInt(AssignedPerson.VehicleShootRateMin, AssignedPerson.VehicleShootRateMax);

            myCop.Accuracy = accuracy;
            myCop.ShootRate = shootRate;
            myCop.CombatAbility = combatAbility;
            myCop.TaserAccuracy = tazerAccuracy;
            myCop.TaserShootRate = tazerShootRate;
            myCop.VehicleAccuracy = vehicleAccuracy;
            myCop.VehicleShootRate = vehicleShootRate;
            if (AssignedAgency.Division != -1)
            {
                myCop.Division = AssignedAgency.Division;
                myCop.UnityType = "Lincoln";
                myCop.BeatNumber = AssignedAgency.GetNextBeatNumber();
                myCop.GroupName = $"{AssignedAgency.ID} {myCop.Division}-{myCop.UnityType}-{myCop.BeatNumber}";
            }
            else if (AssignedAgency.GroupName != "")
            {
                myCop.GroupName = AssignedAgency.GroupName;
            }
            if (!Police.Any(x => x.Pedestrian.Exists() && x.Pedestrian.Handle == Pedestrian.Handle))
            {
                Police.Add(myCop);
                myCop.Pedestrian.IsPersistent = true;
            }
            EntryPoint.WriteToConsole($"PEDESTRIANS: Add COP {Pedestrian.Handle}", 2);
        }
        else
        {
            if (Pedestrian.IsPersistent)
            {
                EntryPoint.PersistentPedsDeleted++;
            }
            Pedestrian.Delete();
            EntryPoint.WriteToConsole($"PEDESTRIANS: Add COP FAIL, DELETING", 2);
        }
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
           // NativeFunction.Natives.SET_DRIVER_ABILITY(Pedestrian, 100f);
        }
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
}