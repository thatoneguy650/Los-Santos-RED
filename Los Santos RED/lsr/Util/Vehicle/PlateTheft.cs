using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionsMethods;
using LSR.Vehicles;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;

public class PlateTheft
{
    private IPlayer Player;
    private Rage.Object ScrewdriverModel;
    private Rage.Object LicensePlateModel;
    private float DistanceToCheckCars = 10f;
    private Vehicle VehicleWithPlate;

    private VehicleExt VehicleToChange;
    private LicensePlate PlateToAdd;
    private bool HasLicensePlate
    {
        get
        {
            if(VehicleToChange.Vehicle.LicensePlate == "        ")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public PlateTheft(IPlayer player)
    {
        Player = player;
    }
    public PlateTheft(IPlayer player, LicensePlate plateToChange) : this(player)
    {
        PlateToAdd = plateToChange;
    }
    public void RemovePlate()
    {
        GetClosestTrackedVehicle();
        if(HasLicensePlate)
        {
            PlateAnimation(false);
        }
    }
    public void ChangePlate(LicensePlate plateToAdd)
    {
        PlateToAdd = plateToAdd;
        GetClosestTrackedVehicle();
        PlateAnimation(true);
    }
    private void GetClosestTrackedVehicle()
    {
        VehicleWithPlate = (Vehicle)Rage.World.GetClosestEntity(Game.LocalPlayer.Character.Position, DistanceToCheckCars, GetEntitiesFlags.ConsiderCars);
        if (VehicleWithPlate != null && VehicleWithPlate.LicensePlate != "        ")
        {
            VehicleToChange = Player.TrackedVehicles.Where(x => x.Vehicle.Handle == VehicleWithPlate.Handle).FirstOrDefault();
            if (VehicleToChange == null)
            {
                VehicleToChange = new VehicleExt(VehicleWithPlate);
                Player.TrackedVehicles.Add(VehicleToChange);
            }
        }
    }
    private void PlateAnimation(bool IsChanging)
    {
        GameFiber ChangeLicensePlateAnimation = GameFiber.StartNew(delegate
        {
            try
            {
                Vector3 CarPosition = VehicleToChange.Vehicle.Position;
                Vector3 ChangeSpot = GetLicensePlateChangePosition(VehicleToChange.Vehicle);
                if (ChangeSpot == Vector3.Zero)
                    return;

                Player.SetUnarmed();
                if (!MovePedToCarPosition(VehicleToChange.Vehicle, Game.LocalPlayer.Character, VehicleToChange.Vehicle.Heading, ChangeSpot, true))
                    return;

                Player.IsChangingLicensePlates = true;
                ScrewdriverModel = AttachScrewdriverToPed(Game.LocalPlayer.Character);
                if (IsChanging)
                {
                    LicensePlateModel = AttachLicensePlateToPed(Game.LocalPlayer.Character);
                }

                AnimationDictionary AnimDictionary = new AnimationDictionary("mp_car_bomb");
                uint GameTimeStartedAnimation = Game.GameTime;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "mp_car_bomb", "car_bomb_mechanic", 2.0f, -2.0f, 5000, 0, 0, false, false, false);
                bool Continue = true;
                while (Game.LocalPlayer.Character.IsAlive)//CarPosition.DistanceTo2D(VehicleToChange.VehicleEnt.Position) <= 0.5f && Game.LocalPlayer.Character.DistanceTo2D(ChangeSpot) <= 0.5f && Game.LocalPlayer.Character.IsAlive)
                {
                    if (EntryPoint.IsMoveControlPressed)
                    {
                        Continue = false;
                        break;
                    }
                    if (Game.GameTime - GameTimeStartedAnimation >= 2000)
                        break;
                    GameFiber.Yield();
                }

                if (Continue && CarPosition.DistanceTo2D(VehicleToChange.Vehicle.Position) <= 1f && Game.LocalPlayer.Character.DistanceTo2D(ChangeSpot) <= 2f && !Game.LocalPlayer.Character.IsDead)
                {
                    UpdateModelPlates(IsChanging);
                }
                else
                {
                    Game.LocalPlayer.Character.Tasks.Clear();
                }
                if (LicensePlateModel != null && LicensePlateModel.Exists())
                    LicensePlateModel.Delete();
                GameFiber.Sleep(750);
                if(ScrewdriverModel != null && ScrewdriverModel.Exists())
                    ScrewdriverModel.Delete();

                LicensePlateModel = null;
                ScrewdriverModel = null;
                Player.IsChangingLicensePlates = false;
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
                Debug.Instance.WriteToLog("ChangeLicensePlate", e.StackTrace);
            }
        }, "PlayDispatchQueue");
        Debug.Instance.GameFibers.Add(ChangeLicensePlateAnimation);
    }
    private void UpdateModelPlates(bool IsChanging)
    {
        if (IsChanging)
        {
            LicensePlate PlateToRemove = VehicleToChange.CarPlate;
            Player.SpareLicensePlates.Remove(PlateToAdd);// Menu.Instance.SelectedPlateIndex);//need to pass this in somehow???
            if (PlateToRemove != null)
            {
                Player.SpareLicensePlates.Add(PlateToRemove);
            }
            VehicleToChange.CarPlate = PlateToAdd;
            VehicleToChange.Vehicle.LicensePlate = PlateToAdd.PlateNumber;
            NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", VehicleToChange.Vehicle, PlateToAdd.PlateType);
        }
        else
        {
            Player.SpareLicensePlates.Add(VehicleToChange.CarPlate);
            VehicleToChange.CarPlate = null;
            VehicleToChange.Vehicle.LicensePlate = "        ";
            VehicleToChange.CarPlate = null;
        }
    }
    private Rage.Object AttachScrewdriverToPed(Ped Pedestrian)
    {
        Rage.Object Screwdriver = new Rage.Object("prop_tool_screwdvr01", Pedestrian.GetOffsetPositionUp(50f));
        if (!Screwdriver.Exists())
            return null;
        int BoneIndexRightHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Pedestrian, 57005);
        Screwdriver.AttachTo(Pedestrian, BoneIndexRightHand, new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f));
        return Screwdriver;
    }
    private bool MovePedToCarPosition(Vehicle TargetVehicle, Ped PedToMove, float DesiredHeading, Vector3 PositionToMoveTo, bool StopDriver)
    {
        bool Continue = true;
        bool isPlayer = false;
        if (PedToMove == Game.LocalPlayer.Character)
            isPlayer = true;
        Ped Driver = TargetVehicle.Driver;
        NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", PedToMove, PositionToMoveTo.X, PositionToMoveTo.Y, PositionToMoveTo.Z, DesiredHeading, -1);

        while (!(PedToMove.DistanceTo2D(PositionToMoveTo) <= 0.15f && FloatIsWithin(PedToMove.Heading,DesiredHeading - 5f, DesiredHeading + 5f)))
        {
            GameFiber.Yield();
            if (isPlayer && EntryPoint.IsMoveControlPressed)
            {
                Continue = false;
                break;
            }
            if (StopDriver && TargetVehicle.Driver != null)
                NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Driver, TargetVehicle, 27, -1);
        }
        if (!Continue)
        {
            PedToMove.Tasks.Clear();
            return false;
        }
        return true;
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
    private Rage.Object AttachLicensePlateToPed(Ped Pedestrian)
    {
        Rage.Object LicensePlate = new Rage.Object("p_num_plate_01", Pedestrian.GetOffsetPositionUp(55f));
        LicensePlate.IsVisible = true;
        int BoneIndexLeftHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 18905);
        LicensePlate.AttachTo(Game.LocalPlayer.Character, BoneIndexLeftHand, new Vector3(0.19f, 0.08f, 0.0f), new Rotator(-57.2f, 90f, -173f));

        return LicensePlate;
    }
    private bool FloatIsWithin(float value, float minimum, float maximum)
    {
        return value >= minimum && value <= maximum;
    }
}

