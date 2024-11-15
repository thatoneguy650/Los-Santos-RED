using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using System;
using System.Linq;

public class PedSwap : IPedSwap
{
    private ICrimes Crimes;
    private Model CurrentModelPlayerIs;
    private Ped CurrentPed;
    private bool CurrentPedIsBusted;
    private bool CurrentPedIsDead;
    private string CurrentPedName;
    private Vector3 CurrentPedPosition;
    private Vehicle CurrentPedVehicle;
    private int CurrentPedVehicleSeat;
    private IEntityProvideable Entities;
    private Model InitialPlayerModel;
    private PedVariation InitialPlayerVariation;
    private INameProvideable Names;
    private IPedSwappable Player;
    private ISettingsProvideable Settings;
    private bool TargetPedInVehicle;
    private bool TargetPedIsMale;
    private Model TargetPedModel;
    private string TargetPedModelName;
    private Vector3 TargetPedPosition;
    private RelationshipGroup TargetPedRelationshipGroup;
    private bool TargetPedUsingScenario;
    private PedVariation TargetPedVariation;
    private Vehicle TargetPedVehicle;
    private IWeapons Weapons;
    private ITimeControllable Time;
    private IModItems ModItems;
    private IEntityProvideable World;
    private IPedGroups PedGroups;
    private IShopMenus ShopMenus;
    private IDispatchablePeople DispatchablePeople;
    private IHeads Heads;
    private IClothesNames ClothesNames;
    private IGangs Gangs;
    private IAgencies Agencies;
    private ITattooNames TattooNames;
    private IGameSaves GameSaves;
    private ISavedOutfits SavedOutfits;
    private IInteractionable Interactionable;
    private bool HasSetOffset;
    private bool IsDisposed;

    public PedSwap(ITimeControllable time, IPedSwappable player, ISettingsProvideable settings, IEntityProvideable entities, IWeapons weapons, ICrimes crimes, INameProvideable names, IModItems modItems, IEntityProvideable world, 
        IPedGroups pedGroups, IShopMenus shopMenus, IDispatchablePeople dispatchablePeople, IHeads heads, IClothesNames clothesNames, IGangs gangs, IAgencies agencies, ITattooNames tattooNames, IGameSaves gameSaves, ISavedOutfits savedOutfits, IInteractionable interactionable)
    {
        Time = time;
        Player = player;
        Settings = settings;
        Entities = entities;
        Weapons = weapons;
        Crimes = crimes;
        Names = names;
        ModItems = modItems;
        World = world;
        PedGroups = pedGroups;
        ShopMenus = shopMenus;
        DispatchablePeople = dispatchablePeople;
        Heads = heads;
        ClothesNames = clothesNames;
        Gangs = gangs;
        Agencies = agencies;
        TattooNames = tattooNames;
        GameSaves = gameSaves;
        SavedOutfits = savedOutfits;
        Interactionable = interactionable;
    }
    public int CurrentPedMoney { get; private set; }
    public void AddOffset()
    {
        SetPlayerOffset();
    }
    //public void BecomeCustomPed()//OLD
    //{
    //    GameFiber.StartNew(delegate
    //    {
    //        try
    //        {
    //            ResetOffsetForCurrentModel();
    //            Player.IsCustomizingPed = true;
    //            MenuPool menuPool = new MenuPool();
    //            PedSwapCustomMenu = new CustomizePedMenu(menuPool, this, Names, Player, Entities, Settings);
    //            PedSwapCustomMenu.Setup();
    //            PedSwapCustomMenu.Show();
    //            GameFiber.Yield();
    //            while (menuPool.IsAnyMenuOpen())
    //            {
    //                PedSwapCustomMenu.Update();
    //                GameFiber.Yield();
    //            }
    //            PedSwapCustomMenu.Dispose();
    //            if (!PedSwapCustomMenu.ChoseNewModel && Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter && !Player.CharacterModelIsPrimaryCharacter)
    //            {
    //                AddOffset();
    //            }
    //            Player.IsCustomizingPed = false;
    //        }
    //        catch (Exception ex)
    //        {
    //            EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
    //            EntryPoint.ModController.CrashUnload();
    //        }
    //    }, "Custom Ped Loop");
    //}
    public void BecomeCreatorPed()
    {
        try
        {

            GameFiber.StartNew(delegate
            {
                try
                {
                    ResetOffsetForCurrentModel();
                    Player.IsCustomizingPed = true;
                    MenuPool menuPool = new MenuPool();
                    PedCustomizer PedCustomizer = new PedCustomizer(menuPool, this, Names, Player, Entities, Settings, DispatchablePeople, Heads, ClothesNames, Gangs, Agencies, TattooNames, GameSaves, SavedOutfits, Interactionable);
                    PedCustomizer.Setup();
                    PedCustomizer.Start();
                    GameFiber.Yield();

                    while (true)
                    {
                        PedCustomizer.Update();
                        if(PedCustomizer.ChoseToClose)
                        {
                            break;
                        }
                        GameFiber.Yield();
                    }
                    if(!PedCustomizer.ChoseToClose)
                    {
                        PedCustomizer.Dispose(true);
                    }
                    if (!PedCustomizer.SetupAsNewPlayer && Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter && !Player.CharacterModelIsPrimaryCharacter)
                    {
                        AddOffset();
                    }
                    Player.IsCustomizingPed = false;
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole("PEDSWAP: BecomeCustomPed2; " + ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "Custom Ped Loop 2");
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole("PEDSWAP: BecomeCustomPed2; " + ex.Message + " " + ex.StackTrace, 0);
            EntryPoint.ModController.CrashUnload();
        }
    }




    public void BecomeKnownPed(PedExt toBecome, bool deleteOld, bool clearNearPolice)
    {
        try
        {
            if(toBecome == null || !toBecome.Pedestrian.Exists())
            {
                return;
            }
            Game.FadeScreenOut(500, true);
            ResetOffsetForCurrentModel();
            Ped TargetPed = toBecome.Pedestrian;// FindPedToSwapWith(radius, nearest);
            if (!TargetPed.Exists())
            {
                Game.FadeScreenIn(0);
                return;
            }
            StoreTargetPedData(TargetPed);
            NativeFunction.Natives.CHANGE_PLAYER_PED<uint>(Game.LocalPlayer, TargetPed, true, true);
            toBecome.MatchPlayerPedType(Player);
            HandlePreviousPed(deleteOld, TargetPed);
            PostTakeover(CurrentModelPlayerIs.Name, false, toBecome.Name, toBecome.Money, RandomItems.GetRandomNumberInt(Settings.SettingsManager.PlayerOtherSettings.PlayerSpeechSkill_Min, Settings.SettingsManager.PlayerOtherSettings.PlayerSpeechSkill_Max), "");
            GameFiber.Sleep(500);
            Game.FadeScreenIn(500, true);
            GiveHistory(false);
        }
        catch (Exception e3)
        {
            EntryPoint.WriteToConsole("PEDSWAP: TakeoverPed Error; " + e3.Message + " " + e3.StackTrace, 0);
        }
    }

    public void BecomeExistingPed(float radius, bool nearest, bool deleteOld, bool clearNearPolice, bool createRandomPedIfNoneReturned)
    {
        try
        {
            Game.FadeScreenOut(500, true);
            ResetOffsetForCurrentModel();
            Ped TargetPed = FindPedToSwapWith(radius, nearest);
            if (!TargetPed.Exists())
            {
                if (createRandomPedIfNoneReturned)
                {
                    BecomeRandomPed();
                }
                else
                {
                    Game.FadeScreenIn(0);
                }
                return;
            }
            StoreTargetPedData(TargetPed);
            NativeFunction.Natives.CHANGE_PLAYER_PED<uint>(Game.LocalPlayer, TargetPed, true, true);
            Player.RemoveAgencyStatus();
            HandlePreviousPed(deleteOld, TargetPed);
            PostTakeover(CurrentModelPlayerIs.Name, true, "", 0, 0,"");
            GameFiber.Sleep(500);
            Game.FadeScreenIn(500, true);
            GiveHistory(false);
        }
        catch (Exception e3)
        {
            EntryPoint.WriteToConsole("PEDSWAP: TakeoverPed Error; " + e3.Message + " " + e3.StackTrace, 0);
        }
    }
    public void BecomeExistingPed(Ped TargetPed, string modelName, string fullName, int money, PedVariation variation, int speechSkill, string voiceName)
    {
        try
        {
            Game.FadeScreenOut(500, true);
            ResetOffsetForCurrentModel();
            if (!TargetPed.Exists())
            {
                Game.FadeScreenIn(0);
                return;
            }
            Time.PauseTime();
            CurrentPed = Game.LocalPlayer.Character;
            CurrentModelPlayerIs = TargetPed.Model;
            NativeFunction.Natives.CHANGE_PLAYER_PED<uint>(Game.LocalPlayer, TargetPed, true, true);
            Player.RemoveAgencyStatus();
            HandlePreviousPed(true, TargetPed);
            PostLoad(modelName, false, fullName, money, variation, speechSkill, voiceName);

            GameFiber.Sleep(500);
            Game.FadeScreenIn(500, true);

            GiveHistory(false);
            Player.DisplayPlayerNotification();
        }
        catch (Exception e3)
        {
            EntryPoint.WriteToConsole("PEDSWAP: TakeoverPed Error; " + e3.Message + " " + e3.StackTrace, 0);
        }
    }
    public void BecomeRandomCop()
    {
        ResetOffsetForCurrentModel();
        Cop toSwapWith = FindCopToSwapWith(2000f, true);
        if (toSwapWith == null || !toSwapWith.Pedestrian.Exists())
        {
            return;
        }
        Ped TargetPed = toSwapWith.Pedestrian;

        //EntryPoint.WriteToConsole($"BecomeRandomCop: CurrentModelPlayerIs ModelName: {CurrentModelPlayerIs.Name} PlayerModelName: {Game.LocalPlayer.Character.Model.Name}", 2);
        //EntryPoint.WriteToConsole($"BecomeRandomCop: TargetPed ModelName: {TargetPed.Model.Name}", 2);
        StoreTargetPedData(TargetPed);
        //EntryPoint.WriteToConsole($"BecomeRandomCop2: CurrentModelPlayerIs ModelName: {CurrentModelPlayerIs.Name} PlayerModelName: {Game.LocalPlayer.Character.Model.Name}", 2);
        //EntryPoint.WriteToConsole($"BecomeRandomCop2: TargetPed ModelName: {TargetPed.Model.Name}", 2);
        NativeFunction.Natives.CHANGE_PLAYER_PED<uint>(Game.LocalPlayer, TargetPed, false, false);
        NativeFunction.Natives.SET_PED_AS_COP(Player.Character, true);//causes old ped to be deleted!
        //EntryPoint.WriteToConsole($"BecomeRandomCop3: CurrentModelPlayerIs ModelName: {CurrentModelPlayerIs.Name} PlayerModelName: {Game.LocalPlayer.Character.Model.Name}", 2);
        //EntryPoint.WriteToConsole($"BecomeRandomCop3: TargetPed ModelName: {TargetPed.Model.Name}", 2);
        Player.SetAgencyStatus(toSwapWith.AssignedAgency);
        //EntryPoint.WriteToConsole($"BecomeRandomCop4: CurrentModelPlayerIs ModelName: {CurrentModelPlayerIs.Name} PlayerModelName: {Game.LocalPlayer.Character.Model.Name}", 2);
        //EntryPoint.WriteToConsole($"BecomeRandomCop4: TargetPed ModelName: {TargetPed.Model.Name}", 2);
        HandlePreviousPed(false, TargetPed);
        PostTakeover(CurrentModelPlayerIs.Name, true, "", 0, 0, "");
        //EntryPoint.WriteToConsole($"BecomeRandomCop5: CurrentModelPlayerIs ModelName: {CurrentModelPlayerIs.Name} PlayerModelName: {Game.LocalPlayer.Character.Model.Name}", 2);
        IssueWeapons(null, toSwapWith.WeaponInventory.Sidearm, toSwapWith.WeaponInventory.LongGun);
        //Player.AliasedCop = new Cop(Game.LocalPlayer.Character, Settings, Player.Character.Health, toSwapWith.AssignedAgency, true, Crimes, Weapons, "Jack Bauer", CurrentModelPlayerIs.Name);
        //Entities.Pedestrians.AddEntity(Player.AliasedCop);
        //Player.AliasedCop.WeaponInventory.IssueWeapons(Weapons, true, true, true);
    }
    public void BecomeRandomPed()
    {
        try
        {
            Game.FadeScreenOut(500, true);
            ResetOffsetForCurrentModel();

            Vector3 FinalSpawnLocation = Player.Character.Position;
            SpawnLocation sl = new SpawnLocation(Player.Character.Position.Around2D(15f));
            sl.GetClosestSidewalk();
            if (sl.HasSidewalk)
            {
                FinalSpawnLocation = sl.SidewalkPosition;
            }
            else
            {
                sl.GetClosestStreet(true);
                sl.GetClosestSideOfRoad();
            }
            if(sl.HasSideOfRoadPosition)
            {
                FinalSpawnLocation = sl.StreetPosition;
            }


            float finalSpawnGroundZ;
            bool foundGround = NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(FinalSpawnLocation.X, FinalSpawnLocation.Y, FinalSpawnLocation.Z, out finalSpawnGroundZ, true, false);
            if (foundGround)
            {
                FinalSpawnLocation = new Vector3(FinalSpawnLocation.X, FinalSpawnLocation.Y, finalSpawnGroundZ);
            }
            if(FinalSpawnLocation == Vector3.Zero)
            {
                FinalSpawnLocation = Player.Character.GetOffsetPositionFront(2f);
            }
           

            Ped TargetPed = new Ped(FinalSpawnLocation, Game.LocalPlayer.Character.Heading);
            EntryPoint.SpawnedEntities.Add(TargetPed);
            GameFiber.Yield();
            if (!TargetPed.Exists())
            {
                Game.FadeScreenIn(0);
                return;
            }
            TargetPed.RandomizeVariation();
            StoreTargetPedData(TargetPed);
            NativeFunction.Natives.CHANGE_PLAYER_PED<uint>(Game.LocalPlayer, TargetPed, true, true);
            Player.RemoveAgencyStatus();
            HandlePreviousPed(false, TargetPed);
            PostTakeover(CurrentModelPlayerIs.Name, true, "", 0, 0, "");
            GameFiber.Sleep(500);
            Game.FadeScreenIn(500, true);

            GiveHistory(false);
        }
        catch (Exception e3)
        {
            EntryPoint.WriteToConsole("PEDSWAP: TakeoverPed Error; " + e3.Message + " " + e3.StackTrace, 0);
        }
    }



    public void BecomeSecurity(Agency agency)
    {
        try
        {
            if (agency == null)
            {
                return;
            }
            DispatchablePerson toBecome = agency.Personnel.PickRandom();//?.Copy();
            if (toBecome == null)
            {
                return;
            }
            Game.FadeScreenOut(500, true);
            ResetOffsetForCurrentModel();
            Ped TargetPed = new Ped(toBecome.ModelName, Player.Character.Position.Around2D(15f), Game.LocalPlayer.Character.Heading);
            EntryPoint.SpawnedEntities.Add(TargetPed);
            GameFiber.Yield();
            NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(Game.GetHashKey(toBecome.ModelName));
            if (!TargetPed.Exists())
            {
                Game.FadeScreenIn(0);
                return;
            }
            TargetPed.RandomizeVariation();
            StoreTargetPedData(TargetPed);
            NativeFunction.Natives.CHANGE_PLAYER_PED<uint>(Game.LocalPlayer, TargetPed, true, true);
            Player.RemoveAgencyStatus();
            HandlePreviousPed(false, TargetPed);
            PostTakeover(CurrentModelPlayerIs.Name, true, "", 0, 0, "");
            if (toBecome != null)
            {
                Player.CurrentModelVariation = toBecome.SetPedVariation(Game.LocalPlayer.Character, agency.PossibleHeads, false);
            }
            Player.SetAgencyStatus(agency);
            IssueWeapons(agency.GetRandomMeleeWeapon(Weapons), agency.GetRandomWeapon(true, Weapons), agency.GetRandomWeapon(false, Weapons));
            if (RandomItems.RandomPercent(100f))
            {
                SpawnLocation vehicleSpawn = new SpawnLocation(Player.Position);
                vehicleSpawn.GetClosestStreet(false);
                if (vehicleSpawn.HasSpawns)
                {
                    SpawnTask carSpawn = new SecurityGuardSpawnTask(agency, vehicleSpawn, agency.GetRandomVehicle(0, false, false, true, "", Settings)?.Copy(), toBecome, false, Settings, Weapons, Names, true, World, Crimes, ModItems);
                    carSpawn.AllowAnySpawn = true;
                    carSpawn.WillAddDriver = false;
                    carSpawn.AttemptSpawn();
                    carSpawn.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));//World.Vehicles.AddEntity(x, ResponseType.Security));
                    VehicleExt createdVehicle = carSpawn.CreatedVehicles.FirstOrDefault();
                    if (createdVehicle != null && createdVehicle.Vehicle.Exists())
                    {
                        Player.Character.WarpIntoVehicle(createdVehicle.Vehicle, -1);
                        Player.VehicleOwnership.TakeOwnershipOfVehicle(createdVehicle, false);
                    }
                }
            }
            GameFiber.Sleep(500);
            Game.FadeScreenIn(500, true);
            GiveHistory(true);
        }
        catch (Exception e3)
        {
            EntryPoint.WriteToConsole("PEDSWAP: TakeoverPed Error; " + e3.Message + " " + e3.StackTrace, 0);
        }
    }
    public void BecomeCop(Agency agency)
    {
        try
        {
            if(agency == null)
            {
                return;
            }
            DispatchablePerson toBecome = agency.Personnel.PickRandom();//?.Copy();
            if(toBecome == null)
            {
                return;
            }

            Game.FadeScreenOut(500, true);
            ResetOffsetForCurrentModel();
            Ped TargetPed = new Ped(toBecome.ModelName, Player.Character.Position.Around2D(15f), Game.LocalPlayer.Character.Heading);
            EntryPoint.SpawnedEntities.Add(TargetPed);
            GameFiber.Yield();
            NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(Game.GetHashKey(toBecome.ModelName));
            if (!TargetPed.Exists())
            {
                Game.FadeScreenIn(0);
                return;
            }
            TargetPed.RandomizeVariation();
            StoreTargetPedData(TargetPed);
            NativeFunction.Natives.CHANGE_PLAYER_PED<uint>(Game.LocalPlayer, TargetPed, true, true);
            Player.RemoveAgencyStatus();
            HandlePreviousPed(false, TargetPed);
            PostTakeover(CurrentModelPlayerIs.Name, true, "", 0, 0, "");

            if (toBecome != null)
            {
                Player.CurrentModelVariation = toBecome.SetPedVariation(Game.LocalPlayer.Character, agency.PossibleHeads, false);
            }
            Player.SetAgencyStatus(agency);
            IssueWeapons(agency.GetRandomMeleeWeapon(Weapons), agency.GetRandomWeapon(true, Weapons),agency.GetRandomWeapon(false, Weapons));
            if (RandomItems.RandomPercent(100f))
            {
                SpawnLocation vehicleSpawn = new SpawnLocation(Player.Position);
                vehicleSpawn.GetClosestStreet(false);
                if (vehicleSpawn.HasSpawns)
                {
                    SpawnTask carSpawn = new LESpawnTask(agency, vehicleSpawn, agency.GetRandomVehicle(0, false, false, true, "", Settings)?.Copy(), toBecome, false, Settings, Weapons, Names, true, World, ModItems, false);
                    carSpawn.AllowAnySpawn = true;
                    carSpawn.WillAddDriver = false;
                    carSpawn.AttemptSpawn();
                    carSpawn.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));//World.Vehicles.AddEntity(x, ResponseType.LawEnforcement));
                    VehicleExt createdVehicle = carSpawn.CreatedVehicles.FirstOrDefault();
                    if (createdVehicle != null && createdVehicle.Vehicle.Exists())
                    {
                        Player.Character.WarpIntoVehicle(createdVehicle.Vehicle, -1);
                        Player.VehicleOwnership.TakeOwnershipOfVehicle(createdVehicle, false);
                    }
                }
            }
            GameFiber.Sleep(500);
            Game.FadeScreenIn(500, true);
            GiveHistory(true);    
        }
        catch (Exception e3)
        {
            EntryPoint.WriteToConsole("PEDSWAP: TakeoverPed Error; " + e3.Message + " " + e3.StackTrace, 0);
        }
    }
    public void BecomeGangMember(Gang gang)
    {
        try
        {
            if (gang == null)
            {
                return;
            }
            DispatchablePerson toBecome = gang.Personnel.PickRandom();//?.Copy();
            if(toBecome == null)
            {
                return;
            }
            Game.FadeScreenOut(500, true);
            ResetOffsetForCurrentModel();
            Ped TargetPed = new Ped(toBecome.ModelName, Player.Character.Position.Around2D(15f), Game.LocalPlayer.Character.Heading);
            EntryPoint.SpawnedEntities.Add(TargetPed);
            GameFiber.Yield();
            NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(Game.GetHashKey(toBecome.ModelName));
            if (!TargetPed.Exists())
            {
                Game.FadeScreenIn(0);
                return;
            }
            TargetPed.RandomizeVariation();
            StoreTargetPedData(TargetPed);
            NativeFunction.Natives.CHANGE_PLAYER_PED<uint>(Game.LocalPlayer, TargetPed, true, true);
            Player.RemoveAgencyStatus();
            HandlePreviousPed(false, TargetPed);
            PostTakeover(CurrentModelPlayerIs.Name, true, "", 0, 0, "");

            if (toBecome != null)
            {
                Player.CurrentModelVariation = toBecome.SetPedVariation(Game.LocalPlayer.Character, gang.PossibleHeads, false);
            }
            Player.RelationshipManager.GangRelationships.SetGang(gang, false);
            IssueWeapons(RandomItems.RandomPercent(gang.PercentageWithMelee) ? gang.GetRandomMeleeWeapon(Weapons) : null, RandomItems.RandomPercent(gang.PercentageWithSidearms) ? gang.GetRandomWeapon(true, Weapons) : null, RandomItems.RandomPercent(gang.PercentageWithLongGuns) ? gang.GetRandomWeapon(false, Weapons) : null);
            if (RandomItems.RandomPercent(gang.VehicleSpawnPercentage))
            {
                SpawnLocation vehicleSpawn = new SpawnLocation(Player.Position);
                vehicleSpawn.GetClosestStreet(false);
                if (vehicleSpawn.HasSpawns)
                {
                    SpawnTask carSpawn = new GangSpawnTask(gang, vehicleSpawn, gang.GetRandomVehicle(0, false, false, true,"", Settings), toBecome, false, Settings, Weapons, Names, true, Crimes, PedGroups, ShopMenus, World, ModItems, false, false, false);
                    carSpawn.AllowAnySpawn = true;
                    carSpawn.WillAddDriver = false;
                    carSpawn.AttemptSpawn();
                    carSpawn.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));//World.Vehicles.AddEntity(x, ResponseType.None));
                    VehicleExt createdVehicle = carSpawn.CreatedVehicles.FirstOrDefault();
                    if (createdVehicle != null && createdVehicle.Vehicle.Exists())
                    {
                        Player.Character.WarpIntoVehicle(createdVehicle.Vehicle, -1);
                        Player.VehicleOwnership.TakeOwnershipOfVehicle(createdVehicle, false);
                    }
                }
            }
            GameFiber.Sleep(500);
            Game.FadeScreenIn(500, true);
            GiveHistory(false);  
        }
        catch (Exception e3)
        {
            EntryPoint.WriteToConsole("PEDSWAP: TakeoverPed Error; " + e3.Message + " " + e3.StackTrace, 0);
        }
    }
    public void BecomeEMT(Agency agency)
    {
        try
        {
            if (agency == null)
            {
                return;
            }
            DispatchablePerson toBecome = agency.Personnel.PickRandom();//?.Copy();
            if (toBecome == null)
            {
                return;
            }
            Game.FadeScreenOut(500, true);
            ResetOffsetForCurrentModel();
            Ped TargetPed = new Ped(toBecome.ModelName, Player.Character.Position.Around2D(15f), Game.LocalPlayer.Character.Heading);
            EntryPoint.SpawnedEntities.Add(TargetPed);
            GameFiber.Yield();
            NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(Game.GetHashKey(toBecome.ModelName));
            if (!TargetPed.Exists())
            {
                Game.FadeScreenIn(0);
                return;
            }
            TargetPed.RandomizeVariation();
            StoreTargetPedData(TargetPed);
            NativeFunction.Natives.CHANGE_PLAYER_PED<uint>(Game.LocalPlayer, TargetPed, true, true);
            Player.RemoveAgencyStatus();
            HandlePreviousPed(false, TargetPed);
            PostTakeover(CurrentModelPlayerIs.Name, true, "", 0, 0, "");

            Player.SetAgencyStatus(agency);

            if (toBecome != null)
            {
                Player.CurrentModelVariation = toBecome.SetPedVariation(Game.LocalPlayer.Character, agency.PossibleHeads, false);
            }
            SpawnLocation vehicleSpawn = new SpawnLocation(Player.Position);
            vehicleSpawn.GetClosestStreet(false);
            if (vehicleSpawn.HasSpawns)
            {
                SpawnTask carSpawn = new EMTSpawnTask(agency, vehicleSpawn, agency.GetRandomVehicle(0, false, false, true, "", Settings), toBecome, false, Settings, Weapons, Names, true, World, ModItems);
                carSpawn.AllowAnySpawn = true;
                carSpawn.WillAddDriver = false;
                carSpawn.AttemptSpawn();
                carSpawn.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));//World.Vehicles.AddEntity(x, ResponseType.EMS));
                VehicleExt createdVehicle = carSpawn.CreatedVehicles.FirstOrDefault();
                if (createdVehicle != null && createdVehicle.Vehicle.Exists())
                {
                    Player.Character.WarpIntoVehicle(createdVehicle.Vehicle, -1);
                    Player.VehicleOwnership.TakeOwnershipOfVehicle(createdVehicle, false);
                }
            }     
            GameFiber.Sleep(500);
            Game.FadeScreenIn(500, true);
            GiveHistory(false);
        }
        catch (Exception e3)
        {
            EntryPoint.WriteToConsole("PEDSWAP: TakeoverPed Error; " + e3.Message + " " + e3.StackTrace, 0);
        }
    }
    public void BecomeFireFighter(Agency agency)
    {
        try
        {
            if (agency == null)
            {
                return;
            }
            DispatchablePerson toBecome = agency.Personnel.PickRandom();//?.Copy();
            if (toBecome == null)
            {
                return;
            }
            Game.FadeScreenOut(500, true);
            ResetOffsetForCurrentModel();
            Ped TargetPed = new Ped(toBecome.ModelName, Player.Character.Position.Around2D(15f), Game.LocalPlayer.Character.Heading);
            EntryPoint.SpawnedEntities.Add(TargetPed);
            GameFiber.Yield();
            NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(Game.GetHashKey(toBecome.ModelName));
            if (!TargetPed.Exists())
            {
                Game.FadeScreenIn(0);
                return;
            }
            TargetPed.RandomizeVariation();
            StoreTargetPedData(TargetPed);
            NativeFunction.Natives.CHANGE_PLAYER_PED<uint>(Game.LocalPlayer, TargetPed, true, true);
            //Player.SetAgencyStatus(agency);
            Player.RemoveAgencyStatus();
            HandlePreviousPed(false, TargetPed);
            PostTakeover(CurrentModelPlayerIs.Name, true, "", 0, 0, "");
            Player.SetAgencyStatus(agency);
            if (toBecome != null)
            {
                Player.CurrentModelVariation = toBecome.SetPedVariation(Game.LocalPlayer.Character, agency.PossibleHeads, false);
            }
            IssueWeapons(agency.GetRandomMeleeWeapon(Weapons),null,null);
            SpawnLocation vehicleSpawn = new SpawnLocation(Player.Position);
            vehicleSpawn.GetClosestStreet(false);
            if (vehicleSpawn.HasSpawns)
            {
                SpawnTask carSpawn = new FireFighterSpawnTask(agency, vehicleSpawn, agency.GetRandomVehicle(0, false, false, true, "", Settings), toBecome, false, Settings, Weapons, Names, true, World, ModItems, ShopMenus);
                carSpawn.AllowAnySpawn = true;
                carSpawn.WillAddDriver = false;
                carSpawn.AttemptSpawn();
                carSpawn.CreatedVehicles.ForEach(x => x.AddVehicleToList(World));//World.Vehicles.AddEntity(x, ResponseType.Fire));
                VehicleExt createdVehicle = carSpawn.CreatedVehicles.FirstOrDefault();
                if (createdVehicle != null && createdVehicle.Vehicle.Exists())
                {
                    Player.Character.WarpIntoVehicle(createdVehicle.Vehicle, -1);
                    Player.VehicleOwnership.TakeOwnershipOfVehicle(createdVehicle, false);
                }
            }
            GameFiber.Sleep(500);
            Game.FadeScreenIn(500, true);
            GiveHistory(false);
        }
        catch (Exception e3)
        {
            EntryPoint.WriteToConsole("PEDSWAP: TakeoverPed Error; " + e3.Message + " " + e3.StackTrace, 0);
        }
    }


    public void BecomeSamePed(string modelName, string fullName, int money, PedVariation variation, string voiceName)
    {
        try
        {
            //Player.RemoveAgencyStatus();
            Player.ModelName = modelName;
            Player.CurrentModelVariation = variation.Copy();
            Player.PlayerName = fullName;
            Player.BankAccounts.SetCash(money);
            if (Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter && !Player.CharacterModelIsPrimaryCharacter)
            {
                Player.WeaponEquipment.StoreWeapons();
                SetPlayerOffset();
                NativeHelper.ChangeModel(AliasModelName(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias));
                NativeHelper.ChangeModel(modelName);
                Game.LocalPlayer.Character.ResetVariation();
                Player.WeaponEquipment.GiveBackStoredWeapons();
                GameFiber.Sleep(100);
                Player.WeaponEquipment.SetUnarmed();
            }
            if (variation != null)
            {
                variation.ApplyToPed(Game.LocalPlayer.Character);
            }
            Player.SetVoice(voiceName);
            Player.DisplayPlayerNotification();
        }
        catch (Exception e3)
        {
            EntryPoint.WriteToConsole("PEDSWAP: TakeoverPed Error; " + e3.Message + " " + e3.StackTrace, 0);
        }
    }
    public void BecomeSavedPed(string playerName, string modelName, int money, PedVariation variation, int speechSkill, string voiceName)
    {
        try
        {
            ResetOffsetForCurrentModel();
            Ped TargetPed = new Ped(modelName, Game.LocalPlayer.Character.GetOffsetPositionFront(15f), Game.LocalPlayer.Character.Heading);
            EntryPoint.SpawnedEntities.Add(TargetPed);
            GameFiber.Yield();
            NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(Game.GetHashKey(modelName));
            if (!TargetPed.Exists())
            {
                return;
            }
            Time.PauseTime();
            CurrentPed = Game.LocalPlayer.Character;
            CurrentModelPlayerIs = TargetPed.Model;
            Vector3 MyPos = Game.LocalPlayer.Character.Position;
            float MyHeading = Game.LocalPlayer.Character.Heading;
            NativeFunction.Natives.CHANGE_PLAYER_PED<uint>(Game.LocalPlayer, TargetPed, false, false);
            Game.LocalPlayer.Character.Position = MyPos;
            Game.LocalPlayer.Character.Heading = MyHeading;
            Player.RemoveAgencyStatus();
            HandlePreviousPed(true, TargetPed);
            PostLoad(modelName, false, playerName, money, variation, speechSkill, voiceName);
        }
        catch (Exception e3)
        {
            EntryPoint.WriteToConsole("PEDSWAP: TakeoverPed Error; " + e3.Message + " " + e3.StackTrace, 0);
        }
    }
    public void Dispose()
    {
        if(IsDisposed)
        {
            return;
        }
        IsDisposed = true;
        Vehicle Car = Game.LocalPlayer.Character.CurrentVehicle;
        bool WasInCar = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        int SeatIndex = 0;
        if (WasInCar)
        {
            SeatIndex = Game.LocalPlayer.Character.SeatIndex;
        }

        ResetOffsetForCurrentModel();

        // ResetExistingModelHash();

        NativeHelper.ChangeModel(InitialPlayerModel.Name);

        if (InitialPlayerVariation != null)
        {
            InitialPlayerVariation.ApplyToPed(Game.LocalPlayer.Character);
        }
        //if (Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter && !Player.CharacterModelIsPrimaryCharacter)
        //{
        //    SetPlayerOffset(InitialPlayerModel.Hash);
        //}
        if (Car.Exists() && WasInCar)
        {
            Game.LocalPlayer.Character.WarpIntoVehicle(Car, SeatIndex);
        }
        if (Settings.SettingsManager.PedSwapSettings.SetRandomMoney && CurrentPedMoney > 0)
        {
            Player.BankAccounts.SetCash(CurrentPedMoney);
        }
    }
    public void RemoveOffset()
    {
        ResetOffsetForCurrentModel();
    }
    public void Setup()
    {
        InitialPlayerModel = Game.LocalPlayer.Character.Model;
        InitialPlayerVariation = NativeHelper.GetPedVariation(Game.LocalPlayer.Character);
        CurrentModelPlayerIs = InitialPlayerModel;
        IsDisposed = false;
    }
    public void TreatAsCivilian()
    {
        Player.RemoveAgencyStatus();
    }
    public void TreatAsCop()
    {
        Player.SetAgencyStatus(Agencies.GetAgency("LSPD"));
    }
    private void ActivatePreviousScenarios()
    {
        if (TargetPedUsingScenario)
        {
            NativeFunction.Natives.TASK_USE_NEAREST_SCENARIO_TO_COORD_WARP<bool>(Game.LocalPlayer.Character, TargetPedPosition.X, TargetPedPosition.Y, TargetPedPosition.Z, 5f, 0);
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                try
                {
                    while (!Player.IsMoveControlPressed)
                    {
                        GameFiber.Yield();
                    }
                    NativeFunction.Natives.CLEAR_PED_TASKS(Game.LocalPlayer.Character);
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "ScenarioWatcher");
        }
    }
    private string AliasModelName(string MainCharacterToAlias)
    {
        if (MainCharacterToAlias == "Michael")
            return "player_zero";
        else if (MainCharacterToAlias == "Franklin")
            return "player_one";
        else if (MainCharacterToAlias == "Trevor")
            return "player_two";
        else
            return "player_zero";
    }
    private bool CanTakeoverPed(Ped myPed)
    {
        if (myPed.Exists() && myPed.Handle != Game.LocalPlayer.Character.Handle && myPed.IsAlive && myPed.IsHuman && myPed.IsNormalPerson() && !InSameCar(myPed, Game.LocalPlayer.Character) && !IsBelowWorld(myPed) && (!myPed.IsInAnyVehicle(false) || myPed.CurrentVehicle?.Driver?.Handle == myPed.Handle))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private Cop FindCopToSwapWith(float Radius, bool Nearest)
    {
        Cop PedToReturn = null;
        if (Nearest)
        {
            PedToReturn = Entities.Pedestrians.PoliceList.Where(x => x.WasModSpawned && (!x.IsInVehicle || x.IsDriver)).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();//closestPed.Where(s => CanTakeoverPed(s)).OrderBy(s => Vector3.Distance(Game.LocalPlayer.Character.Position, s.Position)).FirstOrDefault();
        }
        else
        {
            PedToReturn = Entities.Pedestrians.PoliceList.Where(x => x.DistanceToPlayer <= Radius && x.WasModSpawned && (!x.IsInVehicle || x.IsDriver) && x.Pedestrian.Exists() && x.Pedestrian.DistanceTo(Game.LocalPlayer.Character) <= Radius).PickRandom();//closestPed.Where(s => CanTakeoverPed(s)).OrderBy(s => RandomItems.MyRand.Next()).FirstOrDefault();
        }
        return PedToReturn;
    }
    private Ped FindPedToSwapWith(float Radius, bool Nearest)
    {
        Ped PedToReturn = null;
        if (Nearest)
        {
            PedToReturn = Entities.Pedestrians.CivilianList.Where(x => CanTakeoverPed(x.Pedestrian)).OrderBy(x => x.DistanceToPlayer).FirstOrDefault()?.Pedestrian;//closestPed.Where(s => CanTakeoverPed(s)).OrderBy(s => Vector3.Distance(Game.LocalPlayer.Character.Position, s.Position)).FirstOrDefault();
        }
        else
        {
            PedToReturn = Entities.Pedestrians.CivilianList.Where(x => CanTakeoverPed(x.Pedestrian) && x.DistanceToPlayer <= Radius && x.Pedestrian.Exists() && x.Pedestrian.DistanceTo(Game.LocalPlayer.Character) <= Radius).PickRandom()?.Pedestrian;//closestPed.Where(s => CanTakeoverPed(s)).OrderBy(s => RandomItems.MyRand.Next()).FirstOrDefault();
        }
        if (PedToReturn == null && !PedToReturn.Exists())
        {
            return null;
        }
        //else if (PedToReturn.IsInAnyVehicle(false))
        //{
        //    if (PedToReturn.CurrentVehicle.Driver.Exists())
        //    {
        //        //PedToReturn.CurrentVehicle.Driver.MakePersistent();
        //        return PedToReturn.CurrentVehicle.Driver;
        //    }
        //    else
        //    {
        //        // PedToReturn.MakePersistent();
        //        return PedToReturn;
        //    }
        //}
        else
        {
            //PedToReturn.MakePersistent();
            return PedToReturn;
        }
    }
    private void GiveHistory(bool legalOnly)
    {
        if (!legalOnly && RandomItems.RandomPercent(Settings.SettingsManager.PedSwapSettings.PercentageToGetRandomWeapon))
        {
            WeaponInformation myGun = Weapons.GetRandomRegularWeapon();
            if (myGun != null)
            {
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.ModelName, myGun.AmmoAmount, false);
            }
        }
        if (!legalOnly && RandomItems.RandomPercent(Settings.SettingsManager.PedSwapSettings.PercentageToGetCriminalHistory))
        {
            Player.CriminalHistory.AddCrime(Crimes.CrimeList.PickRandom());
        }
        if(RandomItems.RandomPercent(Settings.SettingsManager.PedSwapSettings.PercentageToGetRandomItems))
        {
            if (Settings.SettingsManager.PedSwapSettings.MaxRandomItemsToGet >= 1 && Settings.SettingsManager.PedSwapSettings.MaxRandomItemsAmount >= 1)
            {
                int ItemsToGet = RandomItems.GetRandomNumberInt(1, Settings.SettingsManager.PedSwapSettings.MaxRandomItemsToGet);
                for (int i = 0; i < ItemsToGet; i++)
                {
                    Player.Inventory.Add(ModItems.GetRandomItem(true, false), RandomItems.GetRandomNumberInt(1, Settings.SettingsManager.PedSwapSettings.MaxRandomItemsAmount));
                }
            }
        }
        if (legalOnly || RandomItems.RandomPercent(Settings.SettingsManager.PedSwapSettings.PercentageToGetDriversLicense))
        {
            string stateID = Player.CurrentLocation?.CurrentZone?.StateID;
            if(string.IsNullOrEmpty(stateID))
            {
                stateID = StaticStrings.SanAndreasStateID;
            }
            Player.Licenses.DriversLicense = new DriversLicense();
            Player.Licenses.DriversLicense.IssueLicense(Time, 12, stateID);
        }
        if (legalOnly || RandomItems.RandomPercent(Settings.SettingsManager.PedSwapSettings.PercentageToGetCCWLicense))
        {
            string stateID = Player.CurrentLocation?.CurrentZone?.StateID;
            if (string.IsNullOrEmpty(stateID))
            {
                stateID = StaticStrings.SanAndreasStateID;
            }
            Player.Licenses.CCWLicense = new CCWLicense();
            Player.Licenses.CCWLicense.IssueLicense(Time, 12, stateID);
        }
        if (Settings.SettingsManager.NeedsSettings.ApplyNeeds)
        {
            if (Settings.SettingsManager.PedSwapSettings.SetRandomNeeds)
            {
                Player.HumanState.SetRandom();
            }
            else
            {
                Player.HumanState.Reset();
            }
        }
        if(RandomItems.RandomPercent(Settings.SettingsManager.PedSwapSettings.PercentageToGetRandomPhone))
        {
            Player.CellPhone.RandomizeSettings();
        }
        if (RandomItems.RandomPercent(Settings.SettingsManager.PedSwapSettings.PercentageToGetRandomBankAccount))
        {
            Player.BankAccounts.CreateRandomAccount(RandomItems.GetRandomNumberInt(Settings.SettingsManager.PedSwapSettings.RandomBankAccountMoneyMin, Settings.SettingsManager.PedSwapSettings.RandomBankAccountMoneyMax));
        }
    }
    private void HandlePreviousPed(bool deleteOld, Ped TargetPed)
    {
        Ped previousPed = null;
        if(CurrentPed.Exists() && CurrentPed.Handle != Game.LocalPlayer.Character.Handle)
        {
            previousPed = CurrentPed;
            //EntryPoint.WriteToConsoleTestLong("HandlePreviousPed Using 'CurrentPed' as it is not equal to the player character.");
        }
        else if (TargetPed.Exists() && TargetPed.Handle != Game.LocalPlayer.Character.Handle)
        {
            previousPed = TargetPed;
            //EntryPoint.WriteToConsoleTestLong("HandlePreviousPed Using 'TargetPed' as it is not equal to the player character.");
        }


        //if (!CurrentPed.Exists() || CurrentPed.Handle == Game.LocalPlayer.Character.Handle)
        //{
        //    return;
        //}
        
        if (CurrentPedIsDead && previousPed.Exists() && previousPed.IsAlive)
        {
            previousPed.Kill();
            previousPed.Health = 0;
        }

        if (deleteOld)
        {
            previousPed.Delete();
        }
        else
        {
            if (!CurrentPedIsDead)
            {
                previousPed.IsPersistent = true;
            }
            PedExt toCreate = Entities.Pedestrians.GetPedExt(previousPed.Handle);
            if (toCreate == null)
            {
                toCreate = new PedExt(previousPed, Settings, Crimes, Weapons, CurrentPedName, "Person", World);
            }
            int WantedToSet = Player.WantedLevel;
            if (Player.WantedLevel == 3)
            {
                WantedToSet++;//just make it deadly chase if its 3, get it over with, most likely i should add crimes here or there might be unexpected issues
            }
            toCreate.SetWantedLevel(WantedToSet);
            if(CurrentPedIsBusted)
            {
                toCreate.SetBusted();
            }

            previousPed.RelationshipGroup = new RelationshipGroup("FORMERPLAYER");

            Entities.Pedestrians.AddEntity(toCreate);
            //EntryPoint.WriteToConsole($"HandlePreviousPed WantedToSet {WantedToSet} WantedLevel {toCreate.WantedLevel} IsBusted {toCreate.IsBusted}", 5);
            TaskFormerPed(previousPed, toCreate.IsWanted, toCreate.IsBusted);
        }
    }
    private bool InSameCar(Ped myPed, Ped PedToCompare)
    {
        bool ImInVehicle = myPed.IsInAnyVehicle(false);
        bool YourInVehicle = PedToCompare.IsInAnyVehicle(false);
        if (ImInVehicle && YourInVehicle)
        {
            if (myPed.CurrentVehicle == PedToCompare.CurrentVehicle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    private bool IsBelowWorld(Ped myPed)
    {
        if (myPed.Position.Z <= -50)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void IssueWeapons(IssuableWeapon melee,IssuableWeapon sidearm, IssuableWeapon longGun)
    {
        if (melee != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Player.Character, (uint)melee.GetHash(), false))
        {
            NativeFunction.Natives.GIVE_WEAPON_TO_PED(Player.Character, (uint)melee.GetHash(), 200, false, false);
            melee.ApplyVariation(Player.Character);
            EntryPoint.WriteToConsole($"ADDED WEAPON {melee.ModelName}");
        }
        if (sidearm != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Player.Character, (uint)sidearm.GetHash(), false))
        {
            NativeFunction.Natives.GIVE_WEAPON_TO_PED(Player.Character, (uint)sidearm.GetHash(), 200, false, false);
            sidearm.ApplyVariation(Player.Character);
            EntryPoint.WriteToConsole($"ADDED WEAPON {sidearm.ModelName}");
        }
        if (longGun != null && !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Player.Character, (uint)longGun.GetHash(), false))
        {
            NativeFunction.Natives.GIVE_WEAPON_TO_PED(Player.Character, (uint)longGun.GetHash(), 200, false, false);
            longGun.ApplyVariation(Player.Character);
            EntryPoint.WriteToConsole($"ADDED WEAPON {longGun.ModelName}");
        }
    }
    private void MakeAllies(Ped[] PedList)
    {
        Player.GroupID = NativeFunction.Natives.CREATE_GROUP<int>(0);
        NativeFunction.Natives.SET_PED_AS_GROUP_LEADER(Player.Character, Player.GroupID);
        NativeFunction.Natives.SET_PED_AS_GROUP_MEMBER(Player.Character, Player.GroupID);
        //Game.LocalPlayer.Character.RelationshipGroup.SetRelationshipWith(TargetPedRelationshipGroup, Relationship.Like);
        foreach (Ped PedToAlly in PedList)
        {
            if (PedToAlly.Exists())
            {
                NativeFunction.Natives.SET_PED_AS_GROUP_MEMBER(PedToAlly, Player.GroupID);
                PedToAlly.StaysInVehiclesWhenJacked = true;
            }
        }
    }
    private void PostLoad(string ModelToChange, bool setRandomDemographics, string nameToAssign, int moneyToAssign, PedVariation variation, int speechSkill, string voiceName)
    {
        NativeFunction.Natives.x2206BF9A37B7F724("MinigameTransitionOut", 5000, false);
        bool isMale = Game.LocalPlayer.Character.IsMale;
        //if (Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter && !Player.CharacterModelIsPrimaryCharacter) //if (!TargetPedAlreadyTakenOver && Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter)
        //{
        //    SetPlayerOffset();
        //    NativeHelper.ChangeModel(AliasModelName(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias));
        //    NativeHelper.ChangeModel(ModelToChange);
        //}
        //variation.ApplyToPed(Game.LocalPlayer.Character);


        Player.ModelName = ModelToChange;
        Player.CurrentModelVariation = variation.Copy();


        if (Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter && !Player.CharacterModelIsPrimaryCharacter) //if (!TargetPedAlreadyTakenOver && Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter)
        {
            SetPlayerOffset();
            NativeHelper.ChangeModel(AliasModelName(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias));
            NativeHelper.ChangeModel(ModelToChange);
            Game.LocalPlayer.Character.ResetVariation();
        }
        variation.ApplyToPed(Game.LocalPlayer.Character);


        if (setRandomDemographics)
        {
            NewPlayer(ModelToChange, isMale);
        }
        else
        {
            NewPlayer(ModelToChange, isMale, nameToAssign, moneyToAssign, speechSkill, voiceName);
        }
        
        






        NativeFunction.Natives.CLEAR_TIMECYCLE_MODIFIER<int>();
        NativeFunction.Natives.x80C8B1846639BB19(0);
        NativeFunction.Natives.STOP_GAMEPLAY_CAM_SHAKING<int>(true);
        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        Game.LocalPlayer.Character.Inventory.GiveNewWeapon(2725352035, 0, true);
        if (Settings.SettingsManager.PlayerOtherSettings.SetSlowMoOnDeath)
        {
            Game.TimeScale = 1f;
        }
        NativeFunction.Natives.xB4EDDC19532BFB85();
        Game.HandleRespawn();
        NativeFunction.Natives.NETWORK_REQUEST_CONTROL_OF_ENTITY<bool>(Game.LocalPlayer.Character);
        NativeFunction.Natives.xC0AA53F866B3134D();
        NativeFunction.Natives.SET_PED_CONFIG_FLAG(Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
        Player.WeaponEquipment.SetUnarmed();
        Time.UnPauseTime();
    }
    private void PostTakeover(string ModelToChange, bool setRandomDemographics, string nameToAssign, int moneyToAssign, int speechSkill, string voiceName)
    {
        NativeFunction.Natives.x2206BF9A37B7F724("MinigameTransitionOut", 5000, false);
        //if (Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter && !Player.CharacterModelIsPrimaryCharacter) //if (!TargetPedAlreadyTakenOver && Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter)
        //{
        //    SetPlayerOffset();
        //    NativeHelper.ChangeModel(AliasModelName(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias));
        //    NativeHelper.ChangeModel(ModelToChange);
        //}
        //if (!Game.LocalPlayer.Character.IsConsideredMainCharacter() && TargetPedVariation != null)
        //{
        //    TargetPedVariation.ApplyToPed(Game.LocalPlayer.Character);
        //}


        Player.ModelName = TargetPedModel.Name;
        Player.CurrentModelVariation = TargetPedVariation;

        if (Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter && !Player.CharacterModelIsPrimaryCharacter) //if (!TargetPedAlreadyTakenOver && Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter)
        {
            SetPlayerOffset();
            NativeHelper.ChangeModel(AliasModelName(Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias));
            NativeHelper.ChangeModel(ModelToChange);
        }
        if (!Game.LocalPlayer.Character.IsConsideredMainCharacter() && TargetPedVariation != null)
        {
            TargetPedVariation.ApplyToPed(Game.LocalPlayer.Character);
        }

        VehicleExt NewVehicle = null;
        if (TargetPedInVehicle)
        {
            if (TargetPedVehicle.Exists())
            {
                Game.LocalPlayer.Character.WarpIntoVehicle(TargetPedVehicle, -1);
                NativeFunction.Natives.SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER<bool>(TargetPedVehicle, true);
            }
            NewVehicle = Entities.Vehicles.GetVehicleExt(TargetPedVehicle);
            if (NewVehicle != null)
            {
                NewVehicle.IsStolen = false;
                if (NewVehicle.Vehicle.Exists())
                {
                    Player.VehicleOwnership.TakeOwnershipOfVehicle(NewVehicle, false);
                    NewVehicle.Vehicle.IsStolen = false;
                }
            }
        }
        else
        {
            Player.VehicleOwnership.ClearVehicleOwnership();
            Game.LocalPlayer.Character.IsCollisionEnabled = true;
        }
        if (setRandomDemographics)
        {
           NewPlayer(TargetPedModelName, TargetPedIsMale);
        }
        else
        {
            NewPlayer(TargetPedModelName, TargetPedIsMale, nameToAssign, moneyToAssign, speechSkill, voiceName);
        }


        if (NewVehicle != null)
        {
            NewVehicle.IsStolen = false;
            if (NewVehicle.Vehicle.Exists())
            {
                Player.VehicleOwnership.TakeOwnershipOfVehicle(NewVehicle, false);
                NewVehicle.Vehicle.IsStolen = false;
            }
        }


        NativeFunction.Natives.CLEAR_TIMECYCLE_MODIFIER<int>();
        NativeFunction.Natives.x80C8B1846639BB19(0);
        NativeFunction.Natives.STOP_GAMEPLAY_CAM_SHAKING<int>(true);
        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        Game.LocalPlayer.Character.Inventory.GiveNewWeapon(2725352035, 0, true);
        if (Settings.SettingsManager.PlayerOtherSettings.SetSlowMoOnDeath)
        {
            Game.TimeScale = 1f;
        }
        NativeFunction.Natives.xB4EDDC19532BFB85();
        Game.HandleRespawn();
        NativeFunction.Natives.NETWORK_REQUEST_CONTROL_OF_ENTITY<bool>(Game.LocalPlayer.Character);
        NativeFunction.Natives.xC0AA53F866B3134D();
        NativeFunction.Natives.SET_PED_CONFIG_FLAG(Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
        // NativeFunction.Natives.SET_PED_CONFIG_FLAG(Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, true);
        ActivatePreviousScenarios();
        Player.WeaponEquipment.SetUnarmed();
        Time.UnPauseTime();
        GameFiber.Wait(50);
        Player.DisplayPlayerNotification();
    }
    public void ResetOffsetForCurrentModel()
    {
        EntryPoint.WriteToConsole($"PEDSWAP ResetOffsetForCurrentModel START CurrentModelPlayerIs {CurrentModelPlayerIs.Name} {CurrentModelPlayerIs.Hash} CharacterModelIsPrimaryCharacter {Player.CharacterModelIsPrimaryCharacter} ModelName{Player.ModelName}");
        if ((Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter || HasSetOffset) && CurrentModelPlayerIs != 0)
        {
            EntryPoint.WriteToConsole($"PEDSWAP ResetOffsetForCurrentModel RAN CurrentModelPlayerIs {CurrentModelPlayerIs.Name} {CurrentModelPlayerIs.Hash} CharacterModelIsPrimaryCharacter {Player.CharacterModelIsPrimaryCharacter} ModelName{Player.ModelName}");
            unsafe
            {
                var PedPtr = (ulong)Game.LocalPlayer.Character.MemoryAddress;
                ulong SkinPtr = *((ulong*)(PedPtr + 0x20));
                *((ulong*)(SkinPtr + 0x18)) = CurrentModelPlayerIs.Hash;
            }
            HasSetOffset = false;
        }
    }
    private void SetPlayerOffset(ulong ModelHash)
    {
        if (!Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter)
        {
            return;
        }
        //EntryPoint.WriteToConsoleTestLong($"PEDSWAP SetPlayerOffset RAN CurrentModelPlayerIs{CurrentModelPlayerIs.Name} {CurrentModelPlayerIs.Hash} CharacterModelIsPrimaryCharacter {Player.CharacterModelIsPrimaryCharacter} ModelName{Player.ModelName} ModelHash{ModelHash}");
        //bigbruh in discord, supplied the below, seems to work just fine
        unsafe
        {
            var PedPtr = (ulong)Game.LocalPlayer.Character.MemoryAddress;
            ulong SkinPtr = *((ulong*)(PedPtr + 0x20));
            *((ulong*)(SkinPtr + 0x18)) = ModelHash;
        }
    }
    public void SetPlayerOffset()
    {
        if(!Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter)
        {
            return;
        }
        ulong ModelHash = 0;
        if (Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias == "Michael")
        {
            ModelHash = 225514697;
        }
        else if (Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias == "Franklin")
        {
            ModelHash = 2602752943;
        }
        else if (Settings.SettingsManager.PedSwapSettings.MainCharacterToAlias == "Trevor")
        {
            ModelHash = 2608926626;
        }
        //EntryPoint.WriteToConsoleTestLong($"PEDSWAP SetPlayerOffset 2 RAN CurrentModelPlayerIs{CurrentModelPlayerIs.Name} {CurrentModelPlayerIs.Hash} CharacterModelIsPrimaryCharacter {Player.CharacterModelIsPrimaryCharacter} ModelName{Player.ModelName}");

        if (ModelHash != 0)
        {
            //bigbruh in discord, supplied the below, seems to work just fine
            unsafe
            {
                var PedPtr = (ulong)Game.LocalPlayer.Character.MemoryAddress;
                ulong SkinPtr = *((ulong*)(PedPtr + 0x20));
                *((ulong*)(SkinPtr + 0x18)) = ModelHash;
            }
            HasSetOffset = true;
        }

        //unsafe
        //{
        //    var PedPtr = (ulong)Game.LocalPlayer.Character.MemoryAddress;
        //    ulong SkinPtr = *((ulong*)(PedPtr + 0x20));
        //    *((ulong*)(SkinPtr + 0x18)) = (ulong)225514697;
        //}
    }
    private void StoreTargetPedData(Ped TargetPed)
    {
        CurrentModelPlayerIs = TargetPed.Model;
        CurrentPedMoney = Player.BankAccounts.GetMoney(false);
        CurrentPedPosition = Player.Position;
        CurrentPedIsDead = Player.Character.IsDead;
        CurrentPedIsBusted = Player.IsBusted;
        CurrentPedName = Player.PlayerName;
        if (Player.Character.IsInAnyVehicle(false) && Player.Character.CurrentVehicle.Exists())
        {
            CurrentPedVehicle = Player.Character.CurrentVehicle;
            CurrentPedVehicleSeat = Game.LocalPlayer.Character.SeatIndex;
        }
        TargetPedModel = TargetPed.Model;
        TargetPedModelName = TargetPed.Model.Name;
        TargetPedIsMale = TargetPed.IsMale;
        TargetPedVariation = NativeHelper.GetPedVariation(TargetPed);
        TargetPedPosition = TargetPed.Position;
        TargetPedRelationshipGroup = TargetPed.RelationshipGroup;
        Time.PauseTime();
        if (Game.LocalPlayer.Character.IsDead)
        {
            NativeFunction.Natives.xB69317BF5E782347(Game.LocalPlayer.Character);//NETWORK_REQUEST_CONTROL_OF_ENTITY
            NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
            Game.HandleRespawn();
        }
        TargetPedInVehicle = TargetPed.IsInAnyVehicle(false);
        if (TargetPedInVehicle)
        {
            TargetPedVehicle = TargetPed.CurrentVehicle;
        }
        TargetPedUsingScenario = NativeFunction.Natives.IS_PED_USING_ANY_SCENARIO<bool>(TargetPed);//bool Scenario = false;
        CurrentPed = Game.LocalPlayer.Character;
        if (TargetPed.IsInAnyVehicle(false))
        {
            Game.LocalPlayer.Character.WarpIntoVehicle(TargetPedVehicle, -1);
            MakeAllies(TargetPedVehicle.Passengers);
        }
        else
        {
            MakeAllies(Array.ConvertAll(Rage.World.GetEntities(Game.LocalPlayer.Character.Position, 5f, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).Where(x => x is Ped).ToArray(), (x => (Ped)x)));
        }
    }
    private void TaskFormerPed(Ped FormerPlayer, bool isWanted, bool isBusted)
    {
        if (!FormerPlayer.Exists() || isBusted || FormerPlayer.IsDead || FormerPlayer.Handle == Game.LocalPlayer.Character.Handle)
        {
            return;
        }
        if (CurrentPedVehicle != null && CurrentPedVehicle.Exists())
        {
            FormerPlayer.WarpIntoVehicle(CurrentPedVehicle, CurrentPedVehicleSeat);
        }
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(FormerPlayer, (int)eCombatAttributes.BF_AlwaysFight, true);
        NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(FormerPlayer, (int)eCombatAttributes.BF_CanFightArmedPedsWhenNotArmed, true);
        FormerPlayer.BlockPermanentEvents = true;
        FormerPlayer.KeepTasks = true;

        if (isWanted)
        {
            FormerPlayer.RelationshipGroup = new RelationshipGroup("FORMERPLAYER");
            Game.SetRelationshipBetweenRelationshipGroups("FORMERPLAYER", "COP", Relationship.Hate);
            Game.SetRelationshipBetweenRelationshipGroups("COP", "FORMERPLAYER", Relationship.Hate);
        }

        if (FormerPlayer.IsInAnyVehicle(false) && FormerPlayer.CurrentVehicle.Exists())
        {
            if (isWanted)
            {
                NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", FormerPlayer, FormerPlayer.Position.X, FormerPlayer.Position.Y, FormerPlayer.Position.Z, 500f, -1, false, false);
                NativeFunction.Natives.SET_DRIVE_TASK_DRIVING_STYLE(FormerPlayer, (int)eCustomDrivingStyles.Code3);
               // EntryPoint.WriteToConsole($"PEDSWAP: HandlePreviousPed Tasking {FormerPlayer.Handle} Vehicle Escape");
            }
            else
            {
                unsafe
                {
                    int lol = 0;
                    NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                    NativeFunction.CallByName<bool>("TASK_PAUSE", 0, RandomItems.MyRand.Next(4000, 8000));
                    NativeFunction.CallByName<bool>("TASK_VEHICLE_DRIVE_WANDER", 0, FormerPlayer.CurrentVehicle, 30f, (int)VehicleDrivingFlags.FollowTraffic, 10f);
                    NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                    NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                    NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", FormerPlayer, lol);
                    NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                }
                //EntryPoint.WriteToConsole($"PEDSWAP: HandlePreviousPed Tasking {FormerPlayer.Handle} Vehicle Wander");
            }
        }
        else if (NativeFunction.Natives.IS_PED_USING_ANY_SCENARIO<bool>(FormerPlayer))
        {
            return;
        }
        else
        {
            if (isWanted)
            {
                Cop toAttack = Entities.Pedestrians.PoliceList.Where(x => x.Pedestrian.Exists()).OrderBy(x => x.Pedestrian.DistanceTo2D(FormerPlayer)).FirstOrDefault();
                if (toAttack != null)
                {
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_COMBAT_PED", 0, toAttack.Pedestrian, 0, 16);
                        NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", 0, toAttack.Pedestrian.Position.X, toAttack.Pedestrian.Position.Y, toAttack.Pedestrian.Position.Z, 500f, -1, false, false);
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", FormerPlayer, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                    //EntryPoint.WriteToConsole($"PEDSWAP: HandlePreviousPed Tasking {FormerPlayer.Handle} Wanted Attack");
                }
                else
                {
                    NativeFunction.CallByName<bool>("TASK_SMART_FLEE_COORD", FormerPlayer, FormerPlayer.Position.X, FormerPlayer.Position.Y, FormerPlayer.Position.Z, 500f, -1, false, false);
                    //EntryPoint.WriteToConsole($"PEDSWAP: HandlePreviousPed Tasking {FormerPlayer.Handle} Wanted Flee");
                }
                NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(FormerPlayer, 100f, 0);
            }
            else
            {
                NativeFunction.Natives.TASK_WANDER_STANDARD(FormerPlayer, 0, 0);
                //EntryPoint.WriteToConsole($"PEDSWAP: HandlePreviousPed Tasking {FormerPlayer.Handle} Normal Wander");
            }
        }
        FormerPlayer.IsPersistent = false;
    }
    public void NewPlayer(string modelName, bool isMale)//gotta go
    {
        NewPlayer(
            modelName,
            isMale,
            GetName(modelName, Names.GetRandomName(isMale)),
            RandomItems.MyRand.Next(Settings.SettingsManager.PedSwapSettings.RandomMoneyMin, Settings.SettingsManager.PedSwapSettings.RandomMoneyMax),
            RandomItems.GetRandomNumberInt(Settings.SettingsManager.PlayerOtherSettings.PlayerSpeechSkill_Min, Settings.SettingsManager.PlayerOtherSettings.PlayerSpeechSkill_Max),
            ""
            );
    }
    public void NewPlayer(string modelName, bool isMale, string playerName, int moneyToSpawnWith, int speechSkill, string voiceName)//gotta go
    {
        Player.Reset(true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true);
        Player.SetDemographics(
            modelName, 
            isMale, 
            playerName, 
            moneyToSpawnWith, 
            speechSkill,
            voiceName
            );
    }
    private string GetName(string modelBeforeSpoof, string defaultName)//gotta get outta here
    {
        if (modelBeforeSpoof.ToLower() == "player_zero")
        {
            return "Michael De Santa";
        }
        else if (modelBeforeSpoof.ToLower() == "player_one")
        {
            return "Franklin Clinton";
        }
        else if (modelBeforeSpoof.ToLower() == "player_two")
        {
            return "Trevor Philips";
        }
        else
        {
            return defaultName;
        }
    }
    private class TakenOverPed
    {
        public TakenOverPed(Ped _Pedestrian, PoolHandle _OriginalHandle)
        {
            Pedestrian = _Pedestrian;
            OriginalHandle = _OriginalHandle;
        }
        public TakenOverPed(Ped _Pedestrian, PoolHandle _OriginalHandle, PedVariation _Variation, Model _OriginalModel, uint _GameTimeTakenover)
        {
            Pedestrian = _Pedestrian;
            OriginalHandle = _OriginalHandle;
            Variation = _Variation;
            OriginalModel = _OriginalModel;
            GameTimeTakenover = _GameTimeTakenover;
        }
        public uint GameTimeTakenover { get; set; }
        public PoolHandle OriginalHandle { get; set; }
        public Model OriginalModel { get; set; }
        public Ped Pedestrian { get; set; }
        public PedVariation Variation { get; set; }
    }
}