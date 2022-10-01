using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;

public class PlateTheft : DynamicActivity
{
    private Vector3 CarPosition;
    private Vector3 ChangeSpot;
    private float DistanceToCheckCars = 10f;
    private Rage.Object LicensePlateModel;
    private LicensePlate PlateToAdd;
    private IPlateChangeable Player;
    private Rage.Object ScrewdriverModel;
    private ISettingsProvideable Settings;
    private VehicleExt TargetVehicle;
    private IEntityProvideable World;
    public PlateTheft(IPlateChangeable player, ISettingsProvideable settings, IEntityProvideable world)
    {
        Player = player;
        Settings = settings;
        World = world;
    }
    public PlateTheft(IPlateChangeable player, LicensePlate plateToChange, ISettingsProvideable settings, IEntityProvideable world) : this(player, settings, world)
    {
        PlateToAdd = plateToChange;
    }
    public override string DebugString => "";
    public override ModItem ModItem { get; set; }
    public override bool CanPause { get; set; } = false;
    public override bool CanCancel { get; set; } = false;
    public override string PausePrompt { get; set; } = "Pause Activity";
    public override string CancelPrompt { get; set; } = "Stop Activity";
    public override string ContinuePrompt { get; set; } = "Continue Activity";
    private bool IsChangingPlate => PlateToAdd != null;
    private bool TargetVehicleHasPlate
    {
        get
        {
            if (TargetVehicle.Vehicle.LicensePlate == "        ")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public override void Cancel()
    {
        Player.IsPerformingActivity = false;
    }
    public override void Continue()
    {
        //no no
    }
    public override void Pause()
    {
    }
    public override bool IsPaused() => false;
    public override void Start()
    {
        EntryPoint.WriteToConsole($"PLAYER EVENT: STARTED PLATE THEFT - IsChangingPlate: {IsChangingPlate}", 3);
        Setup();
        if (ChangeSpot != Vector3.Zero)
        {
            GameFiber ChangeLicensePlateAnimation = GameFiber.StartNew(delegate
            {
                Enter();
                Player.IsPerformingActivity = false;
            }, "PlayDispatchQueue");
        }
    }
    private Rage.Object AttachLicensePlateToPed(Ped Pedestrian)
    {
        Rage.Object LicensePlate = null;
        try
        {
            LicensePlate = new Rage.Object("p_num_plate_01", Pedestrian.GetOffsetPositionUp(55f));
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"Error Spawning Model {ex.Message} {ex.StackTrace}");
        }
        LicensePlate.IsVisible = true;
        int BoneIndexLeftHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, 18905);
        LicensePlate.AttachTo(Player.Character, BoneIndexLeftHand, new Vector3(0.19f, 0.08f, 0.0f), new Rotator(-57.2f, 90f, -173f));

        return LicensePlate;
    }
    private Rage.Object AttachScrewdriverToPed(Ped Pedestrian)
    {
        Rage.Object Screwdriver = null;
        try
        {
           Screwdriver = new Rage.Object("prop_tool_screwdvr01", Pedestrian.GetOffsetPositionUp(50f));
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"Error Spawning Model {ex.Message} {ex.StackTrace}");
        }
        if (!Screwdriver.Exists())
            return null;
        int BoneIndexRightHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Pedestrian, 57005);
        Screwdriver.AttachTo(Pedestrian, BoneIndexRightHand, new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f));
        return Screwdriver;
    }
    private void Enter()
    {
        try
        {
            Player.WeaponEquipment.SetUnarmed();
            if (!MovePedToCarPosition(TargetVehicle.Vehicle, Player.Character, TargetVehicle.Vehicle.Heading, ChangeSpot, true))
            {
                Player.IsPerformingActivity = false;
                return;
            }
            Player.IsChangingLicensePlates = true;
            ScrewdriverModel = AttachScrewdriverToPed(Player.Character);
            if (IsChangingPlate)
            {
                LicensePlateModel = AttachLicensePlateToPed(Player.Character);
            }
            AnimationDictionary.RequestAnimationDictionay("mp_car_bomb");
            uint GameTimeStartedAnimation = Game.GameTime;
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "mp_car_bomb", "car_bomb_mechanic", 2.0f, -2.0f, 5000, 0, 0, false, false, false);
            while (Player.Character.IsAlive && Game.GameTime - GameTimeStartedAnimation < 2000 && !Player.IsMoveControlPressed)
            {
                GameFiber.Yield();
            }

            if (!Player.IsMoveControlPressed && CarPosition.DistanceTo2D(TargetVehicle.Vehicle.Position) <= 1f && Player.Character.DistanceTo2D(ChangeSpot) <= 2f && Player.Character.IsAlive)
            {
                UpdateModelPlates(IsChangingPlate);
            }
            else
            {
                //Player.Character.Tasks.Clear();
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            }

            if (LicensePlateModel != null && LicensePlateModel.Exists())
            {
                LicensePlateModel.Delete();
            }
            GameFiber.Sleep(750);
            if (ScrewdriverModel != null && ScrewdriverModel.Exists())
            {
                ScrewdriverModel.Delete();
            }

            LicensePlateModel = null;
            ScrewdriverModel = null;
            Player.IsChangingLicensePlates = false;
            Player.IsPerformingActivity = false;
        }
        catch (Exception e)
        {
            if (LicensePlateModel != null && LicensePlateModel.Exists())
                LicensePlateModel.Delete();
            if (ScrewdriverModel != null && ScrewdriverModel.Exists())
                ScrewdriverModel.Delete();

            LicensePlateModel = null;
            ScrewdriverModel = null;
            Player.IsChangingLicensePlates = false;
            Player.IsPerformingActivity = false;
            //EntryPoint.WriteToConsole("ChangeLicensePlate" + e.Message + e.StackTrace);
        }
    }
    private bool FloatIsWithin(float value, float minimum, float maximum)
    {
        return value >= minimum && value <= maximum;
    }
    private Vector3 GetLicensePlateChangePosition(Vehicle VehicleToChange)
    {
        Vector3 Position;
        Vector3 Right;
        Vector3 Forward;
        Vector3 Up;

        if (VehicleToChange.HasBone("numberplate"))
        {
            Position = VehicleToChange.GetBonePosition("numberplate");
            VehicleToChange.GetBoneAxes("numberplate", out Right, out Forward, out Up);
            return Vector3.Add(Forward * -1.0f, Position);
        }
        else if (VehicleToChange.HasBone("boot"))
        {
            Position = VehicleToChange.GetBonePosition("boot");
            VehicleToChange.GetBoneAxes("boot", out Right, out Forward, out Up);
            return Vector3.Add(Forward * -1.75f, Position);
        }
        else if (VehicleToChange.IsBike)
        {
            return VehicleToChange.GetOffsetPositionFront(-1.5f);
        }
        else if (VehicleToChange.HasBone("bumper_r"))
        {
            Position = VehicleToChange.GetBonePosition("bumper_r");
            VehicleToChange.GetBoneAxes("bumper_r", out Right, out Forward, out Up);
            Position = Vector3.Add(Forward * -1.0f, Position);
            return Vector3.Add(Right * 0.25f, Position);
        }
        else
            return Vector3.Zero;
    }
    private bool MovePedToCarPosition(Vehicle TargetVehicle, Ped PedToMove, float DesiredHeading, Vector3 PositionToMoveTo, bool StopDriver)
    {
        bool Continue = true;
        bool isPlayer = false;
        if (PedToMove == Player.Character)
            isPlayer = true;
        Ped Driver = TargetVehicle.Driver;
        NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", PedToMove, PositionToMoveTo.X, PositionToMoveTo.Y, PositionToMoveTo.Z, DesiredHeading, -1);

        while (!(PedToMove.DistanceTo2D(PositionToMoveTo) <= 0.15f && FloatIsWithin(PedToMove.Heading, DesiredHeading - 5f, DesiredHeading + 5f)))
        {
            GameFiber.Yield();
            if (isPlayer && Player.IsMoveControlPressed)
            {
                Continue = false;
                break;
            }
            if (StopDriver && TargetVehicle.Driver != null)
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Driver, TargetVehicle, 27, -1);
        }
        if (!Continue)
        {
            //PedToMove.Tasks.Clear();
            NativeFunction.Natives.CLEAR_PED_TASKS(PedToMove);
            return false;
        }
        return true;
    }
    private void Setup()
    {
        TargetVehicle = World.Vehicles.GetClosestVehicleExt(Player.Character.Position, false, DistanceToCheckCars);//GetTargetVehicle();
        if (TargetVehicle != null && TargetVehicle.Vehicle.Exists())//make sure we found a vehicle to change the plates of
        {
            CarPosition = TargetVehicle.Vehicle.Position;
            ChangeSpot = GetLicensePlateChangePosition(TargetVehicle.Vehicle);
        }
    }
    private void UpdateModelPlates(bool IsChanging)
    {
        LicensePlate PlateToRemove = TargetVehicle.CarPlate;
        if (IsChanging)
        {
            EntryPoint.WriteToConsole($"PLAYER EVENT: STARTED PLATE THEFT - IsChanging: {IsChanging} PlateToAdd {PlateToAdd.PlateNumber}", 3);
            Player.SpareLicensePlates.Remove(PlateToAdd);// Menu.Instance.SelectedPlateIndex);//need to pass this in somehow???
            if (PlateToRemove != null)
            {
                EntryPoint.WriteToConsole($"PLAYER EVENT: STARTED PLATE THEFT - IsChanging: {IsChanging} PlateToRemove {PlateToRemove.PlateNumber}", 3);
                Player.SpareLicensePlates.Add(PlateToRemove);
            }
            TargetVehicle.CarPlate = PlateToAdd;
            TargetVehicle.Vehicle.LicensePlate = PlateToAdd.PlateNumber;
            NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", TargetVehicle.Vehicle, PlateToAdd.PlateType);
        }
        else
        {
            EntryPoint.WriteToConsole($"PLAYER EVENT: STARTED PLATE THEFT - IsChanging: {IsChanging} PlateToRemove {TargetVehicle.CarPlate}", 3);
            Player.SpareLicensePlates.Add(TargetVehicle.CarPlate);
            TargetVehicle.CarPlate = null;
            TargetVehicle.Vehicle.LicensePlate = "        ";
            TargetVehicle.CarPlate = null;
        }
    }
}