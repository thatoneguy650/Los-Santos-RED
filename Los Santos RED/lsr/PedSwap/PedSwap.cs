using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public class PedSwap : IPedSwap
{
    private ITimeControllable World;
    private IEntityProvideable Entities;
    private IPedSwappable Player;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private ICrimes Crimes;
    public PedSwap(ITimeControllable world, IPedSwappable player, ISettingsProvideable settings, IEntityProvideable entities, IWeapons  weapons, ICrimes crimes)
    {
        World = world;
        Player = player;
        Settings = settings;
        Entities = entities;
        Weapons = weapons;
        Crimes = crimes;
    }
    private Ped CurrentPed;
    private Vector3 CurrentPedPosition;
    private PedVariation TargetPedVariation;
    private bool TargetPedIsMale;
    private string LastModelHash;
    private string TargetPedModelName;
    private Model TargetPedModel;
    private RelationshipGroup TargetPedRelationshipGroup;
    private bool TargetPedAlreadyTakenOver;
    private uint TargetPedHash;
    private bool TargetPedInVehicle;
    private Vector3 TargetPedPosition;
    private bool TargetPedUsingScenario;
    private Vehicle TargetPedVehicle;
    private PedVariation InitialVariation;
    private Model InitialModel;
    public int CurrentPedMoney { get; private set; }
    public void Setup()
    {
        InitialModel = Game.LocalPlayer.Character.Model;
        InitialVariation = NativeHelper.GetPedVariation(Game.LocalPlayer.Character);
    }
    public void TakeoverPed(float Radius, bool Nearest, bool DeleteOld, bool ClearNearPolice)
    {
        try
        {
            Ped TargetPed = FindPedToSwapWith(Radius, Nearest);
            if (TargetPed == null)
            {
                return;
            }
            StoreTargetPedData(TargetPed);
            NativeFunction.Natives.CHANGE_PLAYER_PED<uint>(Game.LocalPlayer, TargetPed, false, false);
            CurrentPed.IsPersistent = false;
            if (DeleteOld)
            {
                CurrentPed.Delete();
            }
            else
            {
                TaskFormerPed(CurrentPed);
            }
            PostTakeover(LastModelHash);
            
        }
        catch (Exception e3)
        {
            EntryPoint.WriteToConsole("TakeoverPed! TakeoverPed Error; " + e3.Message + " " + e3.StackTrace,0);
        }
    }

    private void GiveHistory()
    {
        if (RandomItems.RandomPercent(Settings.SettingsManager.GeneralSettings.PedSwap_PercentageToGetRandomWeapon))
        {
            WeaponInformation myGun = Weapons.GetRandomRegularWeapon();
            if (myGun != null)
            {
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.ModelName, myGun.AmmoAmount, false);
            }
        }
        if(RandomItems.RandomPercent(Settings.SettingsManager.GeneralSettings.PedSwap_PercentageToGetCriminalHistory))
        {
            Player.AddCrimeToHistory(Crimes.CrimeList.PickRandom());
        }
    }

    public void BecomeRandomPed(bool DeleteOld)
    {

        try
        {
            Ped TargetPed = new Ped();
            if (TargetPed == null)
            {
                return;
            }
            TargetPedModel = TargetPed.Model;
            TargetPedModelName = TargetPed.Model.Name;
            TargetPedIsMale = TargetPed.IsMale;
            TargetPed.RandomizeVariation();
            Vector3 MyPos = Game.LocalPlayer.Character.Position;
            float MyHeading = Game.LocalPlayer.Character.Heading;
            StoreTargetPedData(TargetPed);
            NativeFunction.Natives.CHANGE_PLAYER_PED<uint>(Game.LocalPlayer, TargetPed, false, false);
            Game.LocalPlayer.Character.Position = MyPos;
            Game.LocalPlayer.Character.Heading = MyHeading;
            PostTakeover(LastModelHash);
            if (CurrentPed.Exists())//if (TargetPed.Exists())
            {
                CurrentPed.Delete();
                EntryPoint.WriteToConsole("Become Ped Deleted CurrentPed", 3);
            }
        }
        catch (Exception e3)
        {
            EntryPoint.WriteToConsole("TakeoverPed! TakeoverPed Error; " + e3.Message + " " + e3.StackTrace, 0);
        }
    }
    public void BecomeSavedPed(string playerName, bool isMale, int money, string modelName, PedVariation variation)
    {
        try
        {
            Ped PedToBecome = new Ped(modelName,Game.LocalPlayer.Character.GetOffsetPositionFront(5f),Game.LocalPlayer.Character.Heading);
            Ped ExistingPed = Game.LocalPlayer.Character;

            if (PedToBecome == null)
            {
                return;
            }

            Vector3 MyPos = Game.LocalPlayer.Character.Position;
            float MyHeading = Game.LocalPlayer.Character.Heading;
            NativeFunction.Natives.CHANGE_PLAYER_PED<uint>(Game.LocalPlayer, PedToBecome, false, false);
            Game.LocalPlayer.Character.Position = MyPos;
            Game.LocalPlayer.Character.Heading = MyHeading;

            SetPlayerOffset();
            NativeHelper.ChangeModel(Settings.SettingsManager.GeneralSettings.MainCharacterToAliasModelName);
            NativeHelper.ChangeModel(modelName);
            Ped PedBecame = Game.LocalPlayer.Character;
            if (variation != null)
            {
                variation.ReplacePedComponentVariation(PedBecame);
            }
            else
            {
                PedBecame.RandomizeVariation();
            }


            if (ExistingPed.Exists())
            {
                ExistingPed.Delete();
                EntryPoint.WriteToConsole("Become Saved Ped Deleted CurrentPed", 3);
            }

            EntryPoint.ModController.NewPlayer(modelName, isMale, playerName, money);
            NativeFunction.Natives.CLEAR_TIMECYCLE_MODIFIER<int>();
            NativeFunction.Natives.x80C8B1846639BB19(0);
            NativeFunction.Natives.STOP_GAMEPLAY_CAM_SHAKING<int>(true);
            Game.TimeScale = 1f;
            NativeFunction.Natives.xB4EDDC19532BFB85();
            Game.HandleRespawn();
            NativeFunction.Natives.NETWORK_REQUEST_CONTROL_OF_ENTITY<bool>(Game.LocalPlayer.Character);
            NativeFunction.Natives.xC0AA53F866B3134D();
            NativeFunction.Natives.SET_PED_CONFIG_FLAG(Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
            Player.DisplayPlayerNotification();





            ////this temp bullshit
            //GameFiber.StartNew(delegate
            //{
            //    uint GameTimeToStop = Game.GameTime + 5000;
            //    while (Game.GameTime <= GameTimeToStop)
            //    {
            //        if(PedToBecome.Exists())
            //        {
            //            EntryPoint.WriteToConsole($"PedToBecome {PedToBecome.Handle} {PedToBecome.Position}", 3);
            //        }
            //        if (ExistingPed.Exists())
            //        {
            //            EntryPoint.WriteToConsole($"ExistingPed {ExistingPed.Handle} {ExistingPed.Position}", 3);
            //        }
            //        if (PedBecame.Exists())
            //        {
            //            EntryPoint.WriteToConsole($"PedBecame {PedBecame.Handle} {PedBecame.Position}", 3);
            //        }


                    
            //        GameFiber.Sleep(1000);
            //    }

            //}, "IsShootingChecker");
        }
        catch (Exception e3)
        {
            EntryPoint.WriteToConsole("TakeoverPed! TakeoverPed Error; " + e3.Message + " " + e3.StackTrace, 0);
        }
    }
    public void Dispose()
    {
        Vehicle Car = Game.LocalPlayer.Character.CurrentVehicle;
        bool WasInCar = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        int SeatIndex = 0;
        if (WasInCar)
        {
            SeatIndex = Game.LocalPlayer.Character.SeatIndex;
        }
        NativeHelper.ChangeModel(InitialModel.Name);
        InitialVariation.ReplacePedComponentVariation(Game.LocalPlayer.Character);
        SetPlayerOffset(InitialModel.Hash);
        if (Car.Exists() && WasInCar)
        {
            Game.LocalPlayer.Character.WarpIntoVehicle(Car, SeatIndex);
        }
        if (Settings.SettingsManager.GeneralSettings.PedSwap_SetRandomMoney && CurrentPedMoney > 0)
        {
            Player.SetMoney(CurrentPedMoney);
        }
    }
    private Ped FindPedToSwapWith(float Radius, bool Nearest)
    {
        Ped PedToReturn = null;
        if (Nearest)
        {
            PedToReturn = Entities.CivilianList.Where(x => CanTakeoverPed(x.Pedestrian)).OrderBy(x => x.DistanceToPlayer).FirstOrDefault()?.Pedestrian;//closestPed.Where(s => CanTakeoverPed(s)).OrderBy(s => Vector3.Distance(Game.LocalPlayer.Character.Position, s.Position)).FirstOrDefault();
        }
        else
        {
            PedToReturn = Entities.CivilianList.Where(x => CanTakeoverPed(x.Pedestrian) && x.DistanceToPlayer <= Radius).PickRandom()?.Pedestrian;//closestPed.Where(s => CanTakeoverPed(s)).OrderBy(s => RandomItems.MyRand.Next()).FirstOrDefault();
        }
        if (PedToReturn == null)
        {
            //EntryPoint.WriteToConsole("Ped Takeover! No Peds Found");
            return null;
        }
        else if (PedToReturn.IsInAnyVehicle(false))
        {
            if (PedToReturn.CurrentVehicle.Driver.Exists())
            {
                PedToReturn.CurrentVehicle.Driver.MakePersistent();
                return PedToReturn.CurrentVehicle.Driver;
            }
            else
            {
                PedToReturn.MakePersistent();
                return PedToReturn;
            }
        }
        else
        {
            PedToReturn.MakePersistent();
            return PedToReturn;
        }
    }
    private void StoreTargetPedData(Ped TargetPed)
    {
        CurrentPedMoney = Player.Money;
        CurrentPedPosition = Player.Position;
        TargetPedModel = TargetPed.Model;
        TargetPedModelName = TargetPed.Model.Name;
        TargetPedIsMale = TargetPed.IsMale;
        TargetPedVariation = NativeHelper.GetPedVariation(TargetPed);
        TargetPedPosition = TargetPed.Position;
        TargetPedRelationshipGroup = TargetPed.RelationshipGroup;
        TargetPedAlreadyTakenOver = false;
        TargetPedHash = TargetPed.Model.Hash;

        World.PauseTime();
        if (Game.LocalPlayer.Character.IsDead)
        {
            NativeFunction.Natives.xB69317BF5E782347(Game.LocalPlayer.Character);//"NETWORK_REQUEST_CONTROL_OF_ENTITY" 
            NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
            Game.HandleRespawn();
        }
        if (TargetPedHash == 225514697 || TargetPedHash == 2602752943 || TargetPedHash == 2608926626)
        {
            TargetPedAlreadyTakenOver = true;
            NativeHelper.ChangeModel(TargetPedModel.Name);
            if (!Game.LocalPlayer.Character.IsConsideredMainCharacter())
            {
                TargetPedVariation.ReplacePedComponentVariation(Game.LocalPlayer.Character);
            }
        }
        if (!TargetPedAlreadyTakenOver)
        {
            LastModelHash = TargetPed.Model.Name;
        }
        TargetPedInVehicle = TargetPed.IsInAnyVehicle(false);//bool wasInVehicle = TargetPed.IsInAnyVehicle(false);
        if (TargetPedInVehicle)
        {
            TargetPedVehicle = TargetPed.CurrentVehicle;
        }
        TargetPedUsingScenario = NativeFunction.Natives.IS_PED_USING_ANY_SCENARIO<bool>(TargetPed);//bool Scenario = false;
        if (Game.LocalPlayer.Character.LastVehicle.Exists())
        {
            Game.LocalPlayer.Character.LastVehicle.Delete();
        }
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
    private void TaskFormerPed(Ped FormerPlayer)
    {
        if (FormerPlayer.IsDead)
        {
            return;
        }
        if (FormerPlayer.IsInAnyVehicle(false))
        {
            FormerPlayer.Tasks.CruiseWithVehicle(FormerPlayer.CurrentVehicle, 30f, VehicleDrivingFlags.Normal); //normal driving style
        }
        if (NativeFunction.Natives.IS_PED_USING_ANY_SCENARIO<bool>(FormerPlayer))
        {
            return;
        }
        else
        {
            FormerPlayer.Tasks.ClearImmediately();
            FormerPlayer.Tasks.Wander();
        }
    }
    private void PostTakeover(string ModelToChange)
    {
        NativeFunction.Natives.x2206BF9A37B7F724("MinigameTransitionOut", 5000, false);
        if (!TargetPedAlreadyTakenOver && Settings.SettingsManager.GeneralSettings.PedSwap_AliasPedAsMainCharacter)
        {
            SetPlayerOffset();
            NativeHelper.ChangeModel(Settings.SettingsManager.GeneralSettings.MainCharacterToAliasModelName);
            NativeHelper.ChangeModel(ModelToChange);
        }

        if (!Game.LocalPlayer.Character.IsConsideredMainCharacter())
        {
            TargetPedVariation.ReplacePedComponentVariation(Game.LocalPlayer.Character);
        }

        if (TargetPedInVehicle)
        {
            Game.LocalPlayer.Character.WarpIntoVehicle(TargetPedVehicle, -1);
            NativeFunction.Natives.SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER<bool>(Game.LocalPlayer.Character.CurrentVehicle, true);
            Player.UpdateCurrentVehicle();
            if(Player.CurrentVehicle != null)
            {
                Player.CurrentVehicle.IsStolen = false;
            }
        }
        else
        {
            Game.LocalPlayer.Character.IsCollisionEnabled = true;
        }

        EntryPoint.ModController.NewPlayer(TargetPedModelName, TargetPedIsMale);
        Player.CurrentModelName = TargetPedModel.Name;
        Player.CurrentModelVariation = TargetPedVariation;
        NativeFunction.Natives.CLEAR_TIMECYCLE_MODIFIER<int>();
        NativeFunction.Natives.x80C8B1846639BB19(0);
        NativeFunction.Natives.STOP_GAMEPLAY_CAM_SHAKING<int>(true);
        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        Game.LocalPlayer.Character.Inventory.GiveNewWeapon(2725352035, 0, true);
        Game.TimeScale = 1f;
        NativeFunction.Natives.xB4EDDC19532BFB85();
        Game.HandleRespawn();
        NativeFunction.Natives.NETWORK_REQUEST_CONTROL_OF_ENTITY<bool>(Game.LocalPlayer.Character);
        NativeFunction.Natives.xC0AA53F866B3134D();
        NativeFunction.Natives.SET_PED_CONFIG_FLAG(Game.LocalPlayer.Character, (int)PedConfigFlags.PED_FLAG_DRUNK, false);
        // NativeFunction.Natives.SET_PED_CONFIG_FLAG(Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, true);
        ActivatePreviousScenarios();
        GiveHistory();
        Player.SetUnarmed();
        World.UnPauseTime();
        GameFiber.Wait(50);
        Player.DisplayPlayerNotification();
    }
    private void ActivatePreviousScenarios()
    {
        if (TargetPedUsingScenario)
        {
            NativeFunction.Natives.TASK_USE_NEAREST_SCENARIO_TO_COORD_WARP<bool>(Game.LocalPlayer.Character, TargetPedPosition.X, TargetPedPosition.Y, TargetPedPosition.Z, 5f, 0);
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                while (!Player.IsMoveControlPressed)
                {
                    GameFiber.Yield();
                }
                Game.LocalPlayer.Character.Tasks.Clear();
            }, "ScenarioWatcher");
        }
    }
    private void MakeAllies(Ped[] PedList)
    {
        Game.LocalPlayer.Character.RelationshipGroup.SetRelationshipWith(TargetPedRelationshipGroup, Relationship.Like);
        foreach (Ped PedToAlly in PedList)
        {
            NativeFunction.CallByName<bool>("SET_PED_AS_GROUP_MEMBER", PedToAlly, Game.LocalPlayer.Character.Group);
            PedToAlly.StaysInVehiclesWhenJacked = true;
        }
    }
    private bool CanTakeoverPed(Ped myPed)
    {
        if (myPed.Exists() && myPed.Handle != Game.LocalPlayer.Character.Handle && myPed.IsAlive && myPed.IsHuman && myPed.IsNormalPerson() && !InSameCar(myPed, Game.LocalPlayer.Character) && !IsBelowWorld(myPed))
        {
            return true;
        }
        else
        {
            return false;
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
    private void SetPlayerOffset(ulong ModelHash)
    {
        //bigbruh in discord, supplied the below, seems to work just fine
        unsafe
        {
            var PedPtr = (ulong)Game.LocalPlayer.Character.MemoryAddress;
            ulong SkinPtr = *((ulong*)(PedPtr + 0x20));
            *((ulong*)(SkinPtr + 0x18)) = ModelHash;
        }
    }
    private void SetPlayerOffset()
    {
        ////i have no idea how this works
        const int WORLD_OFFSET = 8;
        const int SECOND_OFFSET = 0x20;
        const int THIRD_OFFSET = 0x18;

        Memory GTA = new Memory("GTA5");
        UInt64 WorldFlirtPointer = GTA.PointerScan("48 8B 05 ? ? ? ? 45 ? ? ? ? 48 8B 48 08 48 85 C9 74 07");
        UInt64 World = GTA.ReadRelativeAddress(WorldFlirtPointer);
        UInt64 Player = GTA.Read<UInt64>(World, new int[] { WORLD_OFFSET });
        UInt64 Second = GTA.Read<UInt64>(Player + SECOND_OFFSET);
        UInt64 Third = GTA.Read<UInt64>(Second + THIRD_OFFSET);

        if (Settings.SettingsManager.GeneralSettings.PedSwap_MainCharacterToAlias == "Michael")
        {
            GTA.Write<uint>(Player + SECOND_OFFSET, 225514697, new int[] { THIRD_OFFSET });
        }
        else if (Settings.SettingsManager.GeneralSettings.PedSwap_MainCharacterToAlias == "Franklin")
        {
            GTA.Write<uint>(Player + SECOND_OFFSET, 2602752943, new int[] { THIRD_OFFSET });
        }
        else if (Settings.SettingsManager.GeneralSettings.PedSwap_MainCharacterToAlias == "Trevor")
        {
            GTA.Write<uint>(Player + SECOND_OFFSET, 2608926626, new int[] { THIRD_OFFSET });
        }

        //bigbruh in discord, supplied the below, seems to work just fine
        //unsafe
        //{
        //    var PedPtr = (ulong)Game.LocalPlayer.Character.MemoryAddress;
        //    ulong SkinPtr = *((ulong*)(PedPtr + 0x20));
        //    *((ulong*)(SkinPtr + 0x18)) = (ulong)225514697;
        //}
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