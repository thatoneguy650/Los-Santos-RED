using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Drawing;
using System.Linq;

public class PlateTheft : DynamicActivity
{
    private Vector3 CarPosition;
    private Vector3 ChangeSpot;
    private float DistanceToCheckCars = 10f;
    private Rage.Object LicensePlateModel;
    private LicensePlateItem PlateToAdd;
    private IActionable Player;
    private Rage.Object ScrewdriverModel;
    private ISettingsProvideable Settings;
    private VehicleExt TargetVehicle;
    private IEntityProvideable World;
    private ScrewdriverItem ScrewdriverItem;
    public PlateTheft(IActionable player, ISettingsProvideable settings, IEntityProvideable world, ScrewdriverItem screwdriverItem)
    {
        Player = player;
        Settings = settings;
        World = world;
        ScrewdriverItem = screwdriverItem;
    }
    public PlateTheft(IActionable player, LicensePlateItem plateToChange, ISettingsProvideable settings, IEntityProvideable world, ScrewdriverItem screwdriverItem) : this(player, settings, world, screwdriverItem)
    {
        PlateToAdd = plateToChange;
    }
    public override string DebugString => "";
    public override ModItem ModItem { get; set; }
    public override bool CanPause { get; set; } = false;
    public override bool CanCancel { get; set; } = false;
    public override bool IsUpperBodyOnly { get; set; } = false;
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
        Player.ActivityManager.IsPerformingActivity = false;
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
        //EntryPoint.WriteToConsole($"PLAYER EVENT: STARTED PLATE THEFT - IsChangingPlate: {IsChangingPlate}");
        Setup();
        if(TargetVehicle == null || !TargetVehicle.Vehicle.Exists())
        {
            Game.DisplayHelp("No vehicle found");
            Player.IsChangingLicensePlates = false;
            Player.ActivityManager.IsPerformingActivity = false;
            return;
        }
        if(ChangeSpot == Vector3.Zero)
        {
            Game.DisplayHelp("Cannot remove plate");
            Player.IsChangingLicensePlates = false;
            Player.ActivityManager.IsPerformingActivity = false;
            return;
        }
        GameFiber ChangeLicensePlateAnimation = GameFiber.StartNew(delegate
        {
            try
            {
                Enter();
                Player.ActivityManager.IsPerformingActivity = false;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "PlayDispatchQueue");
    }
    public override bool CanPerform(IActionable player)
    {
        if (player.IsOnFoot && player.ActivityManager.CanPerformActivitiesExtended && !player.ActivityManager.IsResting)
        {
            return true;
        }
        Game.DisplayHelp($"Cannot Change Plate");
        return false;
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
            if (LicensePlate != null && LicensePlate.Exists())
            {
                LicensePlate.Delete();
            }
        }
        LicensePlate.IsVisible = true;
        int BoneIndexLeftHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, 18905);
        LicensePlate.AttachTo(Player.Character, BoneIndexLeftHand, new Vector3(0.19f, 0.08f, 0.0f), new Rotator(-57.2f, 90f, -173f));

        return LicensePlate;
    }
    private void Enter()
    {
        try
        {
            Player.WeaponEquipment.SetUnarmed();
            if (!MovePedToCarPosition(TargetVehicle.Vehicle, Player.Character, TargetVehicle.Vehicle.Heading, ChangeSpot, true))
            {
                Player.ActivityManager.IsPerformingActivity = false;
                return;
            }
            Player.IsChangingLicensePlates = true;
            ScrewdriverModel = Player.ActivityManager.AttachScrewdriverToPed(ScrewdriverItem, true);
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
                UpdateModelPlates();
            }
            else
            {
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
            Player.ActivityManager.IsPerformingActivity = false;
        }
        catch (Exception e)
        {
            if (LicensePlateModel != null && LicensePlateModel.Exists())
            {
                LicensePlateModel.Delete();
            }
            if (ScrewdriverModel != null && ScrewdriverModel.Exists())
            {
                ScrewdriverModel.Delete();
            }
            LicensePlateModel = null;
            ScrewdriverModel = null;
            Player.IsChangingLicensePlates = false;
            Player.ActivityManager.IsPerformingActivity = false;
            //EntryPoint.WriteToConsole("ChangeLicensePlate" + e.Message + e.StackTrace);
        }
    }
    private bool FloatIsWithin(float value, float minimum, float maximum)
    {
        return value >= minimum && value <= maximum;
    }




    private Vector3 GetLicensePlateChangePosition(Vehicle VehicleToChange)
    {
        if (!VehicleToChange.Exists())
        {
            return Vector3.Zero;
        }
        float halfLength = VehicleToChange.Model.Dimensions.Y / 2.0f;
        halfLength += 1.0f;
        return VehicleToChange.GetOffsetPositionFront(-1.0f * halfLength);
    }


    private Vector3 GetLicensePlateChangePosition_Old(Vehicle VehicleToChange)
    {
        Vector3 Position;
        Vector3 Right;
        Vector3 Forward;
        Vector3 Up;
        bool isFrontPlate = false;
        
        if (VehicleToChange.HasBone("numberplate"))
        {
            float HeadingRotation = -90f;
            EntryPoint.WriteToConsole("PLATE THEFT BONE: numberplate");
            Position = VehicleToChange.GetBonePosition("numberplate");
            if(Position.DistanceTo2D(VehicleToChange.GetOffsetPositionFront(2f)) < Position.DistanceTo2D(VehicleToChange.GetOffsetPositionFront(-2f)))
            {
                EntryPoint.WriteToConsole("PLATE THEFT BONE: numberplate IS FRONT PLATE");
                HeadingRotation = 90f;
            }
            Vector3 SpawnPosition = NativeHelper.GetOffsetPosition(Position, VehicleToChange.Heading + HeadingRotation, 1.5f);
            return SpawnPosition;

            VehicleToChange.GetBoneAxes("numberplate", out Right, out Forward, out Up);//GetBoneAxes no longer works as of 2022-12-23
            return Vector3.Add(Forward * -1.0f, Position);
        }
        //else if (VehicleToChange.HasBone("boot"))
        //{
        //    EntryPoint.WriteToConsole("PLATE THEFT BONE: boot");
        //    Position = VehicleToChange.GetBonePosition("boot");
        //    Vector3 SpawnPosition = NativeHelper.GetOffsetPosition(Position, VehicleToChange.Heading - 90, 1.75f);
        //    return SpawnPosition;
        //    VehicleToChange.GetBoneAxes("boot", out Right, out Forward, out Up);//GetBoneAxes no longer works as of 2022-12-23
        //    return Vector3.Add(Forward * -1.75f, Position);//return Vector3.Add(Forward * -1.75f, Position);
        //}
        else if (VehicleToChange.IsBike)
        {
            EntryPoint.WriteToConsole("PLATE THEFT BONE: IS BIKE");
            return VehicleToChange.GetOffsetPositionFront(-1.5f);
        }
        else if (VehicleToChange.HasBone("bumper_r"))
        {
            EntryPoint.WriteToConsole("PLATE THEFT BONE: bumper_r");
            Position = VehicleToChange.GetBonePosition("bumper_r");
            Vector3 SpawnPosition = NativeHelper.GetOffsetPosition(Position, VehicleToChange.Heading - 90, 1.5f);

            
            SpawnPosition = NativeHelper.GetOffsetPosition(SpawnPosition, VehicleToChange.Heading, VehicleToChange.Model.Dimensions.X / 2);

            return SpawnPosition;


            VehicleToChange.GetBoneAxes("bumper_r", out Right, out Forward, out Up);//GetBoneAxes no longer works as of 2022-12-23
            Position = Vector3.Add(Forward * -1.0f, Position);
            return Vector3.Add(Right * 0.25f, Position);
        }
        else
        {
            return Vector3.Zero;
        }
    }
    private bool MovePedToCarPosition(Vehicle TargetVehicle, Ped PedToMove, float DesiredHeading, Vector3 PositionToMoveTo, bool StopDriver)
    {
        bool Continue = true;
        bool isPlayer = false;
        if (PedToMove == Player.Character)
            isPlayer = true;
        Ped Driver = TargetVehicle.Driver;
        NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", PedToMove, PositionToMoveTo.X, PositionToMoveTo.Y, PositionToMoveTo.Z, DesiredHeading, -1);

        while (!(PedToMove.DistanceTo2D(PositionToMoveTo) <= 0.2f && FloatIsWithin(PedToMove.Heading, DesiredHeading - 5f, DesiredHeading + 5f)))//while (!(PedToMove.DistanceTo2D(PositionToMoveTo) <= 0.15f && FloatIsWithin(PedToMove.Heading, DesiredHeading - 5f, DesiredHeading + 5f)))
        {
            GameFiber.Yield();
            if (isPlayer && Player.IsMoveControlPressed)
            {
                Continue = false;
                break;
            }
            if (StopDriver && TargetVehicle.Driver != null)
            {
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Driver, TargetVehicle, 27, -1);
            }
//#if DEBUG
//            Rage.Debug.DrawArrowDebug(PositionToMoveTo + new Vector3(0f, 0f, 0f), Vector3.Zero, Rotator.Zero, 1f, Color.White);
//#endif
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
        if (TargetVehicle != null && TargetVehicle.Vehicle.Exists() && (IsChangingPlate || TargetVehicleHasPlate))//make sure we found a vehicle to change the plates of
        {
            CarPosition = TargetVehicle.Vehicle.Position;
            ChangeSpot = GetLicensePlateChangePosition(TargetVehicle.Vehicle);
        }
    }
    private void UpdateModelPlates()
    {
        LicensePlate PlateToRemove = TargetVehicle.CarPlate;
        if (IsChangingPlate)
        {
            //EntryPoint.WriteToConsole($"PLAYER EVENT: STARTED PLATE THEFT - IsChanging: {IsChangingPlate} PlateToAdd {PlateToAdd.LicensePlate.PlateNumber}");
            Player.Inventory.Remove(PlateToAdd);
            if (PlateToRemove != null && PlateToRemove.PlateNumber != "        ")
            {
                //EntryPoint.WriteToConsole($"PLAYER EVENT: STARTED PLATE THEFT - IsChanging: {IsChangingPlate} PlateToRemove {PlateToRemove.PlateNumber}");
                LicensePlateItem toAdd = new LicensePlateItem($"{TargetVehicle.CarPlate.PlateNumber}") { Description = TargetVehicle.CarPlate.GenerateDescription(), LicensePlate = PlateToRemove };
                Player.Inventory.Add(toAdd, 1.0f);
            }
            TargetVehicle.CarPlate = PlateToAdd.LicensePlate;
            TargetVehicle.Vehicle.LicensePlate = PlateToAdd.LicensePlate.PlateNumber;
            NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", TargetVehicle.Vehicle, PlateToAdd.LicensePlate.PlateType);
        }
        else
        {
            //EntryPoint.WriteToConsole($"PLAYER EVENT: STARTED PLATE THEFT - IsChanging: {IsChangingPlate} PlateToRemove {TargetVehicle.CarPlate}");
            if (TargetVehicle.CarPlate != null && TargetVehicle.CarPlate.PlateNumber != "        ")
            {
                LicensePlateItem toAdd = new LicensePlateItem($"{TargetVehicle.CarPlate.PlateNumber}") { Description = TargetVehicle.CarPlate.GenerateDescription(), LicensePlate = TargetVehicle.CarPlate };
                Player.Inventory.Add(toAdd, 1.0f);
            }
            TargetVehicle.CarPlate = null;
            TargetVehicle.Vehicle.LicensePlate = "        ";
            TargetVehicle.CarPlate = null;
        }
    }
}