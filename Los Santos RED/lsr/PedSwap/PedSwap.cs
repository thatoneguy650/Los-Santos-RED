using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public class PedSwap : IPedswappable
{
    private ITimeControllable World;
    private IEntityProvideable Entities;
    private IPedSwappable Player;
    private ISettingsProvideable Settings;
    public PedSwap(ITimeControllable world, IPedSwappable player, ISettingsProvideable settings, IEntityProvideable entities)
    {
        World = world;
        Player = player;
        Settings = settings;
        Entities = entities;
    }
    private Ped CurrentPed;
    private Vector3 CurrentPedPosition;
    private PedVariation CurrentPedVariation;
    private bool IsMaleBeforeTakeover;
    private string LastModelHash;
    private string ModelBeforeTakeOver;
    private Model OriginalModel;
    private RelationshipGroup OriginalRelationshipGroup;
    private List<TakenOverPed> TakenOverPeds = new List<TakenOverPed>();
    private bool TargetPedAlreadyTakenOver;
    private bool TargetPedInVehicle;
    private Vector3 TargetPedPosition;
    private bool TargetPedUsingScenario;
    private Vehicle TargetPedVehicle;
    private PedVariation VanillaVariation;
    public int OriginalMoney { get; private set; }
    public void Dispose()
    {
        Vehicle Car = Game.LocalPlayer.Character.CurrentVehicle;
        bool WasInCar = Game.LocalPlayer.Character.IsInAnyVehicle(false);
        int SeatIndex = 0;
        if (WasInCar)
        {
            SeatIndex = Game.LocalPlayer.Character.SeatIndex;
        }
        ChangeModel(Settings.SettingsManager.General.MainCharacterToAliasModelName);
        VanillaVariation.ReplacePedComponentVariation(Game.LocalPlayer.Character);
        if(Car.Exists() && WasInCar)
        {
            Game.LocalPlayer.Character.WarpIntoVehicle(Car, SeatIndex);
        }
    }
    public void StoreInitialVariation()
    {
        VanillaVariation = GetPedVariation(Game.LocalPlayer.Character);
    }
    public void TakeoverPed(float Radius, bool Nearest, bool DeleteOld, bool ClearNearPolice)
    {
        try
        {
            if(Player.IsWanted)
            {
                Game.DisplayNotification("Lose your wanted level first");
                return;
            }
            Ped TargetPed = FindPedToSwapWith(Radius, Nearest);
            if (TargetPed == null)
            {
                return;
            }
            //if (ClearNearPolice)//might want this back
            //{
            //    World.ClearPolice();
            //}
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
            //Game.Console.Print("TakeoverPed! TakeoverPed Error; " + e3.Message + " " + e3.StackTrace);
        }
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
    private void AddPedToTakenOverPeds(TakenOverPed MyPed)
    {
        if (!TakenOverPeds.Any(x => x.OriginalHandle == MyPed.Pedestrian.Handle))
        {
            TakenOverPeds.Add(MyPed);
        }
    }
    private void AllyClosePedsToPlayer(Ped[] PedList)
    {
        Game.LocalPlayer.Character.RelationshipGroup.SetRelationshipWith(OriginalRelationshipGroup, Relationship.Like);
        //Game.LocalPlayer.Character.RelationshipGroup = OriginalRelationshipGroup;


        //turned off below and added above
        foreach (Ped PedToAlly in PedList)
        {
            NativeFunction.CallByName<bool>("SET_PED_AS_GROUP_MEMBER", PedToAlly, Game.LocalPlayer.Character.Group);
            PedToAlly.StaysInVehiclesWhenJacked = true;
        }
    }
    private bool CanTakeoverPed(Ped myPed)
    {
        if (myPed.Exists() && myPed.Handle != Game.LocalPlayer.Character.Handle && myPed.IsAlive && myPed.IsHuman && myPed.IsNormalPerson() && !InSameCar(myPed, Game.LocalPlayer.Character))// && !IsBelowWorld(myPed))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void ChangeModel(string ModelRequested)
    {
        Model characterModel = new Model(ModelRequested);
        characterModel.LoadAndWait();
        characterModel.LoadCollisionAndWait();
        Game.LocalPlayer.Model = characterModel;
        Game.LocalPlayer.Character.IsCollisionEnabled = true;
    }
    private Ped FindPedToSwapWith(float Radius, bool Nearest)
    {
        Ped PedToReturn = null;
        //Ped[] closestPed = Array.ConvertAll(Rage.World.GetEntities(Game.LocalPlayer.Character.Position, Radius, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed | GetEntitiesFlags.ConsiderAllPeds).Where(x => x is Ped).ToArray(), (x => (Ped)x));
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
            //Game.Console.Print("Ped Takeover! No Peds Found");
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
    private PedVariation GetPedVariation(Ped myPed)
    {
        try
        {
            PedVariation myPedVariation = new PedVariation
            {
                MyPedComponents = new List<PedComponent>(),
                MyPedProps = new List<PedPropComponent>()
            };
            for (int ComponentNumber = 0; ComponentNumber < 12; ComponentNumber++)
            {
                myPedVariation.MyPedComponents.Add(new PedComponent(ComponentNumber, NativeFunction.Natives.GET_PED_DRAWABLE_VARIATION<int>(myPed, ComponentNumber), NativeFunction.Natives.GET_PED_TEXTURE_VARIATION<int>(myPed, ComponentNumber), NativeFunction.Natives.GET_PED_PALETTE_VARIATION<int>(myPed, ComponentNumber)));
            }
            for (int PropNumber = 0; PropNumber < 8; PropNumber++)
            {
                myPedVariation.MyPedProps.Add(new PedPropComponent(PropNumber, NativeFunction.Natives.GET_PED_PROP_INDEX<int>(myPed, PropNumber), NativeFunction.Natives.GET_PED_PROP_TEXTURE_INDEX<int>(myPed, PropNumber)));
            }
            return myPedVariation;
        }
        catch (Exception e)
        {
            //Game.Console.Print("CopyPedComponentVariation! CopyPedComponentVariation Error; " + e.Message);
            return null;
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
    private void PostTakeover(string ModelToChange)
    {
        NativeFunction.Natives.x2206BF9A37B7F724("MinigameTransitionOut", 5000, false);
        if (!TargetPedAlreadyTakenOver)
        {
            SetPlayerOffset();
            ChangeModel(Settings.SettingsManager.General.MainCharacterToAliasModelName);
            ChangeModel(ModelToChange);
        }

        if (!Game.LocalPlayer.Character.IsConsideredMainCharacter())
        {
            CurrentPedVariation.ReplacePedComponentVariation(Game.LocalPlayer.Character);
        }

        if (TargetPedInVehicle)
        {
            Game.LocalPlayer.Character.WarpIntoVehicle(TargetPedVehicle, -1);
            NativeFunction.Natives.SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER<bool>(Game.LocalPlayer.Character.CurrentVehicle, true);
            //if (OwnedCar != null && OwnedCar.Exists())
            //    OwnedCar.IsPersistent = false;
            //OwnedCar = TargetPedVehicle;
            //OwnedCar.IsPersistent = true;
        }
        else
        {
            Game.LocalPlayer.Character.IsCollisionEnabled = true;
        }

        //Mod.NewPlayer(ModelBeforeTakeOver, IsMaleBeforeTakeover);
        EntryPoint.ModController.NewPlayer(ModelBeforeTakeOver, IsMaleBeforeTakeover);


        //Mod.Player.Instance.ResetState(true);
        //Mod.Player.Instance.CurrentPoliceResponse.SetWantedLevel(0, "Reset After Takeover as a precaution", false);
        //Mod.Player.Instance.CurrentPoliceResponse.Reset();
        // Mod.Player.Instance.ArrestWarrant.Reset();


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
        NativeFunction.Natives.SET_PED_CONFIG_FLAG(Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, true);
        ActivatePreviousScenarios();
        Player.SetUnarmed();
        World.UnPauseTime();
        GameFiber.Wait(50);
        Player.DisplayPlayerNotification();
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

        if (Settings.SettingsManager.General.MainCharacterToAlias == "Michael")
        {
            GTA.Write<uint>(Player + SECOND_OFFSET, 225514697, new int[] { THIRD_OFFSET });
        }
        else if (Settings.SettingsManager.General.MainCharacterToAlias == "Franklin")
        {
            GTA.Write<uint>(Player + SECOND_OFFSET, 2602752943, new int[] { THIRD_OFFSET });
        }
        else if (Settings.SettingsManager.General.MainCharacterToAlias == "Trevor")
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
    private void StoreTargetPedData(Ped TargetPed)
    {
        OriginalMoney = Player.Money;
        ModelBeforeTakeOver = TargetPed.Model.Name;
        IsMaleBeforeTakeover = TargetPed.IsMale;
        CurrentPedVariation = GetPedVariation(TargetPed);
        TargetPedPosition = TargetPed.Position;
        CurrentPedPosition = Game.LocalPlayer.Character.Position;
        OriginalRelationshipGroup = TargetPed.RelationshipGroup;
        World.PauseTime();
        if (Game.LocalPlayer.Character.IsDead)
        {
            //Player.RespawnHere(false,true);
            NativeFunction.Natives.xB69317BF5E782347(Game.LocalPlayer.Character);//"NETWORK_REQUEST_CONTROL_OF_ENTITY" 
            NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
            Game.HandleRespawn();
        }
        Vector3 PlayerOriginalPedPosition = Game.LocalPlayer.Character.Position;
        TargetPedAlreadyTakenOver = false;
        if (TakenOverPeds.Count > 0)
        {
            uint TargetModelHash = TargetPed.Model.Hash;
            if (TargetModelHash == 225514697 || TargetModelHash == 2602752943 || TargetModelHash == 2608926626)
            {
                TargetPedAlreadyTakenOver = true;
                ChangeModel(OriginalModel.Name);
                if (!Game.LocalPlayer.Character.IsConsideredMainCharacter())
                {
                    CurrentPedVariation.ReplacePedComponentVariation(Game.LocalPlayer.Character);
                }
            }
        }
        OriginalModel = TargetPed.Model;
        AddPedToTakenOverPeds(new TakenOverPed(TargetPed, TargetPed.Handle, GetPedVariation(TargetPed), TargetPed.Model, Game.GameTime));
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
            AllyClosePedsToPlayer(TargetPedVehicle.Passengers);
        }
        else
        {
            AllyClosePedsToPlayer(Array.ConvertAll(Rage.World.GetEntities(Game.LocalPlayer.Character.Position, 5f, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed).Where(x => x is Ped).ToArray(), (x => (Ped)x)));
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