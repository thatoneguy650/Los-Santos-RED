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
            return TargetVehicle != null
                && TargetVehicle.Vehicle != null
                && TargetVehicle.Vehicle.Exists()
                && !string.IsNullOrWhiteSpace(TargetVehicle.Vehicle.LicensePlate)
                && TargetVehicle.Vehicle.LicensePlate != "        ";
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
                EntryPoint.WriteToConsole("Plate theft activity failed: " + ex.Message + " " + ex.StackTrace, 0);
                CleanupActivity();
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
        if (Pedestrian == null || !Pedestrian.Exists())
        {
            return null;
        }

        Rage.Object LicensePlate = null;
        try
        {
            LicensePlate = new Rage.Object("p_num_plate_01", Pedestrian.GetOffsetPositionUp(55f));
            if (LicensePlate == null || !LicensePlate.Exists())
            {
                return null;
            }

            LicensePlate.IsVisible = true;
            int BoneIndexLeftHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Pedestrian, 18905);
            LicensePlate.AttachTo(Pedestrian, BoneIndexLeftHand, new Vector3(0.19f, 0.08f, 0.0f), new Rotator(-57.2f, 90f, -173f));
            return LicensePlate;
        }
        catch (Exception ex)
        {
            if (LicensePlate != null && LicensePlate.Exists())
            {
                LicensePlate.Delete();
            }
            EntryPoint.WriteToConsole("Unable to create the license plate prop: " + ex.Message, 1);
            return null;
        }
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
            EntryPoint.WriteToConsole("Plate change failed: " + e.Message + " " + e.StackTrace, 0);
            CleanupActivity();
        }
    }

    private void CleanupActivity()
    {
        try
        {
            if (LicensePlateModel != null && LicensePlateModel.Exists())
            {
                LicensePlateModel.Delete();
            }
        }
        catch { }

        try
        {
            if (ScrewdriverModel != null && ScrewdriverModel.Exists())
            {
                ScrewdriverModel.Delete();
            }
        }
        catch { }

        LicensePlateModel = null;
        ScrewdriverModel = null;
        Player.IsChangingLicensePlates = false;
        Player.ActivityManager.IsPerformingActivity = false;
    }

    private bool IsHeadingWithinTolerance(float currentHeading, float desiredHeading, float tolerance)
    {
        float difference = (currentHeading - desiredHeading) % 360f;
        if (difference > 180f)
        {
            difference -= 360f;
        }
        else if (difference < -180f)
        {
            difference += 360f;
        }
        return Math.Abs(difference) <= tolerance;
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
        const uint timeoutMilliseconds = 15000;

        if (TargetVehicle == null || !TargetVehicle.Exists() || PedToMove == null || !PedToMove.Exists())
        {
            return false;
        }

        bool isPlayer = PedToMove == Player.Character;
        Ped Driver = StopDriver ? TargetVehicle.Driver : null;
        uint startedAt = Game.GameTime;

        NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", PedToMove, PositionToMoveTo.X, PositionToMoveTo.Y, PositionToMoveTo.Z, DesiredHeading, -1);

        while (TargetVehicle.Exists() && PedToMove.Exists())
        {
            if (PedToMove.DistanceTo2D(PositionToMoveTo) <= 0.2f
                && IsHeadingWithinTolerance(PedToMove.Heading, DesiredHeading, 5f))
            {
                return true;
            }

            if ((isPlayer && Player.IsMoveControlPressed)
                || Game.GameTime - startedAt >= timeoutMilliseconds)
            {
                break;
            }

            if (StopDriver && Driver != null && Driver.Exists())
            {
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Driver, TargetVehicle, 27, -1);
            }

            GameFiber.Yield();
        }

        if (PedToMove.Exists())
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(PedToMove);
        }
        return false;
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
        if (TargetVehicle == null || TargetVehicle.Vehicle == null || !TargetVehicle.Vehicle.Exists())
        {
            return;
        }

        LicensePlate previousPlate = TargetVehicle.CarPlate;
        string previousVehiclePlateText = TargetVehicle.Vehicle.LicensePlate;
        LicensePlateItem removedPlateItem = null;
        if (previousPlate != null && !string.IsNullOrWhiteSpace(previousPlate.PlateNumber) && previousPlate.PlateNumber != "        ")
        {
            removedPlateItem = new LicensePlateItem(previousPlate.PlateNumber)
            {
                Description = previousPlate.GenerateDescription(),
                LicensePlate = previousPlate
            };
        }

        if (IsChangingPlate && (PlateToAdd == null || PlateToAdd.LicensePlate == null
            || string.IsNullOrWhiteSpace(PlateToAdd.LicensePlate.PlateNumber)))
        {
            Game.DisplayHelp("The selected license plate is invalid.");
            return;
        }

        InventoryItem replacementPlateInventoryEntry = IsChangingPlate ? Player.Inventory.Get(PlateToAdd) : null;
        float replacementPlateAmount = replacementPlateInventoryEntry == null ? 0f : replacementPlateInventoryEntry.RemainingPercent;
        InventoryItem previousPlateInventoryEntry = removedPlateItem == null ? null : Player.Inventory.Get(removedPlateItem);
        float previousPlateInventoryAmount = previousPlateInventoryEntry == null ? 0f : previousPlateInventoryEntry.RemainingPercent;
        bool replacementPlateConsumed = false;
        bool removedPlateAddedToInventory = false;
        try
        {
            if (IsChangingPlate)
            {
                if (!Player.Inventory.Remove(PlateToAdd))
                {
                    Game.DisplayHelp("You no longer have that license plate.");
                    return;
                }
                replacementPlateConsumed = true;
            }

            if (removedPlateItem != null)
            {
                Player.Inventory.Add(removedPlateItem, 1.0f);
                removedPlateAddedToInventory = true;
            }

            if (IsChangingPlate)
            {
                TargetVehicle.CarPlate = PlateToAdd.LicensePlate;
                TargetVehicle.Vehicle.LicensePlate = PlateToAdd.LicensePlate.PlateNumber;
                NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", TargetVehicle.Vehicle, PlateToAdd.LicensePlate.PlateType);
            }
            else
            {
                TargetVehicle.CarPlate = null;
                TargetVehicle.Vehicle.LicensePlate = "        ";
            }
        }
        catch
        {
            TargetVehicle.CarPlate = previousPlate;
            if (TargetVehicle.Vehicle != null && TargetVehicle.Vehicle.Exists())
            {
                TargetVehicle.Vehicle.LicensePlate = previousVehiclePlateText;
                if (previousPlate != null)
                {
                    NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", TargetVehicle.Vehicle, previousPlate.PlateType);
                }
            }

            if (removedPlateAddedToInventory && removedPlateItem != null)
            {
                if (previousPlateInventoryEntry != null)
                {
                    previousPlateInventoryEntry.RemainingPercent = previousPlateInventoryAmount;
                }
                else
                {
                    Player.Inventory.Remove(removedPlateItem);
                }
            }
            if (replacementPlateConsumed && PlateToAdd != null)
            {
                Player.Inventory.Add(PlateToAdd, replacementPlateAmount > 0f ? replacementPlateAmount : 1.0f);
            }
            throw;
        }
    }
}