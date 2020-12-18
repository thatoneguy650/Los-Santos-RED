using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

//needs full rewrite/refactor and to be moved
public class PedSwap
{
    private Vector3 CurrentPedPosition;
    private Vector3 TargetPedPosition;
    private bool TargetPedUsingScenario;
    private Ped CurrentPed;
    private bool TargetPedInVehicle;
    private Vehicle TargetPedVehicle;
    private bool TargetPedAlreadyTakenOver;
    private List<TakenOverPed> TakenOverPeds = new List<TakenOverPed>();
    private Model OriginalModel;
    private string LastModelHash;
    private PedVariation CurrentPedVariation;
    private uint GameTimeLastTakenOver;
    private bool CurrentPlayerIsMale = false;
    private string CurrentPlayerModel;
    private List<string> ShopPeds = new List<string>() { "s_m_y_ammucity_01", "s_m_m_ammucountry", "u_m_y_tattoo_01", "s_f_y_shop_low", "s_f_y_shop_mid", "s_f_m_shop_high", "s_m_m_autoshop_01", "s_m_m_autoshop_02" };
    public PedSwap()
    {
        GameTimeLastTakenOver = Game.GameTime;
        CurrentPlayerModel = Game.LocalPlayer.Character.Model.Name;
        CurrentPedVariation = GetPedVariation(Game.LocalPlayer.Character);
        CurrentPlayerIsMale = Game.LocalPlayer.Character.IsMale;
        GiveName();
    }
    public Vehicle OwnedCar { get; set; }
    public string SuspectName { get; set; }
    public bool RecentlyTakenOver
    {
        get
        {
            if (Game.GameTime - GameTimeLastTakenOver <= 5000)//Right when you takeover a ped they might become wanted for some weird reason, this stops that
                return true;
            else
                return false;
        }
    }
    public void TakeoverPed(float Radius, bool Nearest, bool DeleteOld, bool ClearNearPolice)
    {
        try
        {
            Ped TargetPed = FindPedToSwapWith(Radius, Nearest);

            if (TargetPed == null)
                return;

            if (ClearNearPolice)
            {
                Mod.World.Pedestrians.ClearPolice();
                Mod.World.Vehicles.ClearPolice();
            }

            StoreTargetPedData(TargetPed);

            NativeFunction.CallByName<uint>("CHANGE_PLAYER_PED", Game.LocalPlayer, TargetPed, false, false);
            CurrentPed.IsPersistent = false;

            if (DeleteOld)
                CurrentPed.Delete();
            else
                TaskFormerPed(CurrentPed);

            PostTakeover(LastModelHash);
        }
        catch (Exception e3)
        {
            Mod.Debug.WriteToLog("TakeoverPed", "TakeoverPed Error; " + e3.Message + " " + e3.StackTrace);
        }
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
            Mod.Debug.WriteToLog("Ped Takeover", "No Peds Found");
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
            Mod.Debug.WriteToLog("CopyPedComponentVariation", "CopyPedComponentVariation Error; " + e.Message);
            return null;
        }
    }
    private void GiveName()
    {
        if (CurrentPlayerModel.ToLower() == "player_zero")
            SuspectName = "Michael De Santa";
        else if (CurrentPlayerModel.ToLower() == "player_one")
            SuspectName = "Franklin Clinton";
        else if (CurrentPlayerModel.ToLower() == "player_two")
            SuspectName = "Trevor Philips";
        else
            SuspectName = Mod.DataMart.Names.GetRandomName(CurrentPlayerIsMale);
    }
    private void StoreTargetPedData(Ped TargetPed)
    {
        CurrentPedVariation = GetPedVariation(TargetPed);
        CurrentPlayerModel = TargetPed.Model.Name;
        CurrentPlayerIsMale = TargetPed.IsMale;
        Mod.World.Time.PauseTime();

        CurrentPedPosition = Game.LocalPlayer.Character.Position;
        TargetPedPosition = TargetPed.Position;

        if (Game.LocalPlayer.Character.IsDead)
        {
            Mod.Player.Respawning.RespawnInPlace(false);
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
                if (!IsConsideredMainCharacter(Game.LocalPlayer.Character))
                    CurrentPedVariation.ReplacePedComponentVariation(Game.LocalPlayer.Character);
            }
        }

        OriginalModel = TargetPed.Model;

        AddPedToTakenOverPeds(new TakenOverPed(TargetPed, TargetPed.Handle, GetPedVariation(TargetPed), TargetPed.Model, Game.GameTime));

        if (!TargetPedAlreadyTakenOver)
            LastModelHash = TargetPed.Model.Name;

        TargetPedInVehicle = TargetPed.IsInAnyVehicle(false);//bool wasInVehicle = TargetPed.IsInAnyVehicle(false);
        if (TargetPedInVehicle)
            TargetPedVehicle = TargetPed.CurrentVehicle;

        TargetPedUsingScenario = NativeFunction.CallByName<bool>("IS_PED_USING_ANY_SCENARIO", TargetPed);//bool Scenario = false;

        if (Game.LocalPlayer.Character.LastVehicle.Exists())
            Game.LocalPlayer.Character.LastVehicle.Delete();

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
    private void PostTakeover(string ModelToChange)
    {
        NativeFunction.Natives.x2206BF9A37B7F724("MinigameTransitionOut", 5000, false);

        if (!TargetPedAlreadyTakenOver)
        {
            SetPlayerOffset();
            ChangeModel(Mod.DataMart.Settings.SettingsManager.General.MainCharacterToAliasModelName);
            ChangeModel(ModelToChange);
        }

        if (!IsConsideredMainCharacter(Game.LocalPlayer.Character))
            CurrentPedVariation.ReplacePedComponentVariation(Game.LocalPlayer.Character);

        if (TargetPedInVehicle)
        {
            Game.LocalPlayer.Character.WarpIntoVehicle(TargetPedVehicle, -1);
            NativeFunction.CallByName<bool>("SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER", Game.LocalPlayer.Character.CurrentVehicle, true);
            if (OwnedCar != null && OwnedCar.Exists())
                OwnedCar.IsPersistent = false;
            OwnedCar = TargetPedVehicle;
            OwnedCar.IsPersistent = true;
        }
        else
        {
            Game.LocalPlayer.Character.IsCollisionEnabled = true;
        }

        if (Mod.DataMart.Settings.SettingsManager.General.PedTakeoverSetRandomMoney)
            Mod.Player.SetMoney(RandomItems.MyRand.Next(Mod.DataMart.Settings.SettingsManager.General.PedTakeoverRandomMoneyMin, Mod.DataMart.Settings.SettingsManager.General.PedTakeoverRandomMoneyMax));

        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        Game.LocalPlayer.Character.Inventory.GiveNewWeapon(2725352035, 0, true);


        Mod.Player.ResetState(true);

        Game.TimeScale = 1f;
        Mod.Player.CurrentPoliceResponse.SetWantedLevel(0, "Reset After Takeover as a precaution",false);

        NativeFunction.Natives.xB4EDDC19532BFB85();
        Game.HandleRespawn();
        NativeFunction.CallByName<bool>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
        NativeFunction.Natives.xC0AA53F866B3134D();

        Mod.Player.CurrentPoliceResponse.Reset();
        Mod.Player.ArrestWarrant.Reset();
        GameTimeLastTakenOver = Game.GameTime;
        Mod.Menu.SelectedTakeoverRadius = -1f;//reset this on the menu
        if(CurrentPed.Exists())
            CurrentPed.IsPersistent = false;
        ActivatePreviousScenarios();

        Mod.Player.SetUnarmed();
        GiveName();
        Mod.World.Time.UnpauseTime();

        Mod.Player.WeaponDropping.Reset();

        //PlayerHealth.Health = Game.LocalPlayer.Character.Health;
        //PlayerHealth.Armor = Game.LocalPlayer.Character.Armor;

        GameFiber.Wait(50);
        Mod.Player.DisplayPlayerNotification();

    }
    private void AllyClosePedsToPlayer(Ped[] PedList)
    {
        foreach (Ped PedToAlly in PedList)
        {
            NativeFunction.CallByName<bool>("SET_PED_AS_GROUP_MEMBER", PedToAlly, Game.LocalPlayer.Character.Group);
            PedToAlly.StaysInVehiclesWhenJacked = true;
        }
    }
    private void ActivatePreviousScenarios()
    {
        if (TargetPedUsingScenario)
        {
            NativeFunction.CallByName<bool>("TASK_USE_NEAREST_SCENARIO_TO_COORD_WARP", Game.LocalPlayer.Character, TargetPedPosition.X, TargetPedPosition.Y, TargetPedPosition.Z, 5f, 0);
            GameFiber ScenarioWatcher = GameFiber.StartNew(delegate
            {
                while (!Mod.Input.IsMoveControlPressed)
                    GameFiber.Yield();
                Game.LocalPlayer.Character.Tasks.Clear();
        }, "ScenarioWatcher");
            Mod.Debug.GameFibers.Add(ScenarioWatcher);
        }
    }
    private void AddPedToTakenOverPeds(TakenOverPed MyPed)
    {
        if (!TakenOverPeds.Any(x => x.OriginalHandle == MyPed.Pedestrian.Handle))
        {
            TakenOverPeds.Add(MyPed);
        }
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

        if (Mod.DataMart.Settings.SettingsManager.General.MainCharacterToAlias == "Michael")
            GTA.Write<uint>(Player + SECOND_OFFSET, 225514697, new int[] { THIRD_OFFSET });
        else if (Mod.DataMart.Settings.SettingsManager.General.MainCharacterToAlias == "Franklin")
            GTA.Write<uint>(Player + SECOND_OFFSET, 2602752943, new int[] { THIRD_OFFSET });
        else if (Mod.DataMart.Settings.SettingsManager.General.MainCharacterToAlias == "Trevor")
            GTA.Write<uint>(Player + SECOND_OFFSET, 2608926626, new int[] { THIRD_OFFSET });

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
        if(NativeFunction.CallByName<bool>("IS_PED_USING_ANY_SCENARIO", FormerPlayer))
        {
            return;
        }
        else
        {
            FormerPlayer.Tasks.ClearImmediately();
            FormerPlayer.Tasks.Wander();
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
    private bool CanTakeoverPed(Ped myPed)
    {
        if (myPed.Exists() && myPed != Game.LocalPlayer.Character && myPed.IsAlive && myPed.IsHuman && !myPed.IsPoliceArmy() && !InSameCar(myPed,Game.LocalPlayer.Character) && IsNormalPed(myPed) && !IsBelowWorld(myPed))
            return true;
        else
            return false;
    }
    private bool IsNormalPed(Ped myPed)
    {
        if (myPed.Model.Hash == 225514697 || myPed.Model.Hash == 2602752943 || myPed.Model.Hash == 2608926626)
        {
            return false;
        }
        int PedType = NativeFunction.CallByName<int>("GET_PED_TYPE", myPed);
        if (PedType == 4 || PedType == 5 || PedType == 26)
        {
            string ModelName = myPed.Model.Name.ToLower();
            if (ShopPeds.Contains(ModelName))
                return false;
            else
                return true;
        }
        else
        {
            return false;
        }
    }
    private bool IsBelowWorld(Ped myPed)
    {
        if (myPed.Position.Z <= -50)
            return true;
        else
            return false;
    }
    private bool InSameCar(Ped myPed, Ped PedToCompare)
    {
        bool ImInVehicle = myPed.IsInAnyVehicle(false);
        bool YourInVehicle = PedToCompare.IsInAnyVehicle(false);
        if (ImInVehicle && YourInVehicle)
        {
            if (myPed.CurrentVehicle == PedToCompare.CurrentVehicle)
                return true;
            else
                return false;
        }
        else
            return false;
    }
    private bool IsConsideredMainCharacter(Ped myPed)
    {
        int PedType = NativeFunction.CallByName<int>("GET_PED_TYPE", myPed);//Function.Call<int>(Hash.GET_PED_TYPE, myPed);
        if (PedType == 0 || PedType == 1 || PedType == 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private class TakenOverPed
    {
        public Ped Pedestrian { get; set; }
        public PoolHandle OriginalHandle { get; set; }
        public PedVariation Variation { get; set; }
        public Model OriginalModel { get; set; }
        public uint GameTimeTakenover { get; set; }
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
    }
}

