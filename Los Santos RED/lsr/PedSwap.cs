using ExtensionsMethods;
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public class PedSwap
{
    private static readonly Lazy<PedSwap> lazy =
    new Lazy<PedSwap>(() => new PedSwap());
    public static PedSwap Instance { get { return lazy.Value; } }
    private PedSwap()
    {
    }
    private Ped CurrentPed;
    private Vector3 CurrentPedPosition;
    private PedVariation CurrentPedVariation;
    private bool IsMaleBeforeTakeover;
    private string LastModelHash;
    private string ModelBeforeTakeOver;
    private Model OriginalModel;
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
        ChangeModel(DataMart.Instance.Settings.SettingsManager.General.MainCharacterToAliasModelName);
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
            Ped TargetPed = FindPedToSwapWith(Radius, Nearest);
            if (TargetPed == null)
            {
                return;
            }
            if (ClearNearPolice)
            {
                Mod.World.Instance.ClearPolice();
            }
            StoreTargetPedData(TargetPed);
            NativeFunction.CallByName<uint>("CHANGE_PLAYER_PED", Game.LocalPlayer, TargetPed, false, false);
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
            Debug.Instance.WriteToLog("TakeoverPed", "TakeoverPed Error; " + e3.Message + " " + e3.StackTrace);
        }
    }
    private void ActivatePreviousScenarios()
    {
        if (TargetPedUsingScenario)
        {
            NativeFunction.CallByName<bool>("TASK_USE_NEAREST_SCENARIO_TO_COORD_WARP", Game.LocalPlayer.Character, TargetPedPosition.X, TargetPedPosition.Y, TargetPedPosition.Z, 5f, 0);
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                while (!Input.Instance.IsMoveControlPressed)
                {
                    GameFiber.Yield();
                }
                Game.LocalPlayer.Character.Tasks.Clear();
            }, "ScenarioWatcher");
            Debug.Instance.GameFibers.Add(ScenarioWatcher);
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
        foreach (Ped PedToAlly in PedList)
        {
            NativeFunction.CallByName<bool>("SET_PED_AS_GROUP_MEMBER", PedToAlly, Game.LocalPlayer.Character.Group);
            PedToAlly.StaysInVehiclesWhenJacked = true;
        }
    }
    private bool CanTakeoverPed(Ped myPed)
    {
        if (myPed.Exists() && myPed.Handle != Game.LocalPlayer.Character.Handle && myPed.IsAlive && myPed.IsHuman && myPed.IsNormalPerson() && !myPed.IsPoliceArmy() && !InSameCar(myPed, Game.LocalPlayer.Character) && !IsBelowWorld(myPed))
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
        Ped[] closestPed = Array.ConvertAll(Rage.World.GetEntities(Game.LocalPlayer.Character.Position, Radius, GetEntitiesFlags.ConsiderHumanPeds | GetEntitiesFlags.ExcludePlayerPed | GetEntitiesFlags.ConsiderAllPeds).Where(x => x is Ped).ToArray(), (x => (Ped)x));
        if (Nearest)
            PedToReturn = closestPed.Where(s => CanTakeoverPed(s)).OrderBy(s => Vector3.Distance(Game.LocalPlayer.Character.Position, s.Position)).FirstOrDefault();
        else
            PedToReturn = closestPed.Where(s => CanTakeoverPed(s)).OrderBy(s => RandomItems.MyRand.Next()).FirstOrDefault();
        if (PedToReturn == null)
        {
            Debug.Instance.WriteToLog("Ped Takeover", "No Peds Found");
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
                myPedVariation.MyPedComponents.Add(new PedComponent(ComponentNumber, NativeFunction.CallByName<int>("GET_PED_DRAWABLE_VARIATION", myPed, ComponentNumber), NativeFunction.CallByName<int>("GET_PED_TEXTURE_VARIATION", myPed, ComponentNumber), NativeFunction.CallByName<int>("GET_PED_PALETTE_VARIATION", myPed, ComponentNumber)));
            }
            for (int PropNumber = 0; PropNumber < 8; PropNumber++)
            {
                myPedVariation.MyPedProps.Add(new PedPropComponent(PropNumber, NativeFunction.CallByName<int>("GET_PED_PROP_INDEX", myPed, PropNumber), NativeFunction.CallByName<int>("GET_PED_PROP_TEXTURE_INDEX", myPed, PropNumber)));
            }
            return myPedVariation;
        }
        catch (Exception e)
        {
            Debug.Instance.WriteToLog("CopyPedComponentVariation", "CopyPedComponentVariation Error; " + e.Message);
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
            ChangeModel(DataMart.Instance.Settings.SettingsManager.General.MainCharacterToAliasModelName);
            ChangeModel(ModelToChange);
        }

        if (!Game.LocalPlayer.Character.IsConsideredMainCharacter())
        {
            CurrentPedVariation.ReplacePedComponentVariation(Game.LocalPlayer.Character);
        }

        if (TargetPedInVehicle)
        {
            Game.LocalPlayer.Character.WarpIntoVehicle(TargetPedVehicle, -1);
            NativeFunction.CallByName<bool>("SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER", Game.LocalPlayer.Character.CurrentVehicle, true);
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

        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        Game.LocalPlayer.Character.Inventory.GiveNewWeapon(2725352035, 0, true);
        Game.TimeScale = 1f;
        NativeFunction.Natives.xB4EDDC19532BFB85();
        Game.HandleRespawn();
        NativeFunction.CallByName<bool>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
        NativeFunction.Natives.xC0AA53F866B3134D();


        NativeFunction.CallByName<bool>("SET_PED_CONFIG_FLAG", Game.LocalPlayer.Character, (int)PedConfigFlags._PED_FLAG_DISABLE_STARTING_VEH_ENGINE, true);
        ActivatePreviousScenarios();
        Mod.Player.Instance.SetUnarmed();
        Mod.World.Instance.UnPauseTime();
        GameFiber.Wait(50);
        Mod.Player.Instance.DisplayPlayerNotification();
    }
    private void SetPlayerOffset()
    {
        //i have no idea how this works
        const int WORLD_OFFSET = 8;
        const int SECOND_OFFSET = 0x20;
        const int THIRD_OFFSET = 0x18;

        Memory GTA = new Memory("GTA5");
        UInt64 WorldFlirtPointer = GTA.PointerScan("48 8B 05 ? ? ? ? 45 ? ? ? ? 48 8B 48 08 48 85 C9 74 07");
        UInt64 World = GTA.ReadRelativeAddress(WorldFlirtPointer);
        UInt64 Player = GTA.Read<UInt64>(World, new int[] { WORLD_OFFSET });
        UInt64 Second = GTA.Read<UInt64>(Player + SECOND_OFFSET);
        UInt64 Third = GTA.Read<UInt64>(Second + THIRD_OFFSET);

        if (DataMart.Instance.Settings.SettingsManager.General.MainCharacterToAlias == "Michael")
        {
            GTA.Write<uint>(Player + SECOND_OFFSET, 225514697, new int[] { THIRD_OFFSET });
        }
        else if (DataMart.Instance.Settings.SettingsManager.General.MainCharacterToAlias == "Franklin")
        {
            GTA.Write<uint>(Player + SECOND_OFFSET, 2602752943, new int[] { THIRD_OFFSET });
        }
        else if (DataMart.Instance.Settings.SettingsManager.General.MainCharacterToAlias == "Trevor")
        {
            GTA.Write<uint>(Player + SECOND_OFFSET, 2608926626, new int[] { THIRD_OFFSET });
        }
    }
    private void StoreTargetPedData(Ped TargetPed)
    {
        OriginalMoney = Mod.Player.Instance.Money;
        ModelBeforeTakeOver = TargetPed.Model.Name;
        IsMaleBeforeTakeover = TargetPed.IsMale;
        CurrentPedVariation = GetPedVariation(TargetPed);
        TargetPedPosition = TargetPed.Position;
        CurrentPedPosition = Game.LocalPlayer.Character.Position;
        Mod.World.Instance.PauseTime();
        if (Game.LocalPlayer.Character.IsDead)
        {
            Mod.Player.Instance.RespawnHere(false,true);
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
        TargetPedUsingScenario = NativeFunction.CallByName<bool>("IS_PED_USING_ANY_SCENARIO", TargetPed);//bool Scenario = false;
        if (Game.LocalPlayer.Character.LastVehicle.Exists())
        {
            Game.LocalPlayer.Character.LastVehicle.Delete();
        }
        CurrentPed = Game.LocalPlayer.Character;
        if (TargetPed.IsInAnyVehicle(false))
        {
            Game.LocalPlayer.Character.WarpIntoVehicle(TargetPedVehicle, -1);
            AllyClosePedsToPlayer(TargetPed.CurrentVehicle.Passengers);
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
        if (NativeFunction.CallByName<bool>("IS_PED_USING_ANY_SCENARIO", FormerPlayer))
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