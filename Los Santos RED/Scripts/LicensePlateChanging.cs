using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionsMethods;


public static class LicensePlateChanging
{
    private static readonly Random rnd;
    private static Rage.Object Screwdriver;
    private static Rage.Object LicensePlate;
    public static bool PlayerChangingPlate { get; set; }

    static LicensePlateChanging()
    {
        rnd = new Random();
    }
    public static void Initialize()
    {
        Screwdriver = null;
        LicensePlate = null;
        PlayerChangingPlate = false;
        SpareLicensePlates.Add(new GTALicensePlate(General.RandomString(8), 1, 1, false));
    }
    public static void Dispose()
    {

    }
    public static List<GTALicensePlate> SpareLicensePlates = new List<GTALicensePlate>();
    public static void RemoveNearestLicensePlate()
    {
        Vehicle ClosestVehicle = (Vehicle)World.GetClosestEntity(Game.LocalPlayer.Character.Position, 10f, GetEntitiesFlags.ConsiderCars);
        if (ClosestVehicle.LicensePlate == "        ")
            return;

        if (ClosestVehicle != null)
        {
            GTAVehicle VehicleToChange = General.TrackedVehicles.Where(x => x.VehicleEnt.Handle == ClosestVehicle.Handle).FirstOrDefault();
            if (VehicleToChange == null)
            {
                VehicleToChange = new GTAVehicle(ClosestVehicle,0, false, false,null, true, new GTALicensePlate(ClosestVehicle.LicensePlate, (uint)ClosestVehicle.Handle, NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", ClosestVehicle),false));
                General.TrackedVehicles.Add(VehicleToChange);
            }

            if (ClosestVehicle.HasDriver)
            {
                //PedReactToThreatening(ClosestVehicle.Driver);
            }
            ChangeLicensePlateAnimation(VehicleToChange, false);
        }
    }
    public static void ChangeNearestLicensePlate()
    {
        if (!SpareLicensePlates.Any())
            return;

        Vehicle ClosestVehicle = (Vehicle)World.GetClosestEntity(Game.LocalPlayer.Character.Position, 10f, GetEntitiesFlags.ConsiderCars);

        if (ClosestVehicle != null)
        {
            GTAVehicle VehicleToChange = General.TrackedVehicles.Where(x => x.VehicleEnt.Handle == ClosestVehicle.Handle).FirstOrDefault();
            if (VehicleToChange == null)
            {
                VehicleToChange = new GTAVehicle(ClosestVehicle,0, false, false,null,true, new GTALicensePlate(ClosestVehicle.LicensePlate, (uint)ClosestVehicle.Handle, NativeFunction.CallByName<int>("GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", ClosestVehicle), false));
                General.TrackedVehicles.Add(VehicleToChange);
            }

            ChangeLicensePlateAnimation(VehicleToChange, true);
        }
    }
    public static void ChangeLicensePlateAnimation(GTAVehicle VehicleToChange, bool ChangePlates)
    {
        if (!ChangePlates && VehicleToChange.VehicleEnt.LicensePlate == "        ")// Plate already removed
            return;

        GameFiber ChangeLicensePlateAnimation = GameFiber.StartNew(delegate
        {
            try
            {
                Vector3 CarPosition = VehicleToChange.VehicleEnt.Position;
                Vector3 ChangeSpot = GetLicensePlateChangePosition(VehicleToChange.VehicleEnt);
                if (ChangeSpot == Vector3.Zero)
                    return;

                General.SetPedUnarmed(Game.LocalPlayer.Character, false);
                if (!MovePedToCarPosition(VehicleToChange.VehicleEnt, Game.LocalPlayer.Character, VehicleToChange.VehicleEnt.Heading, ChangeSpot, true))
                    return;

                PlayerChangingPlate = true;
                Screwdriver = General.AttachScrewdriverToPed(Game.LocalPlayer.Character);
                if (ChangePlates)
                    LicensePlate = AttachLicensePlateToPed(Game.LocalPlayer.Character);

                General.RequestAnimationDictionay("mp_car_bomb");
                uint GameTimeStartedAnimation = Game.GameTime;
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "mp_car_bomb", "car_bomb_mechanic", 2.0f, -2.0f, 5000, 0, 0, false, false, false);
                bool Continue = true;
                while (Game.LocalPlayer.Character.IsAlive)//CarPosition.DistanceTo2D(VehicleToChange.VehicleEnt.Position) <= 0.5f && Game.LocalPlayer.Character.DistanceTo2D(ChangeSpot) <= 0.5f && Game.LocalPlayer.Character.IsAlive)
                {
                    if (Extensions.IsMoveControlPressed())
                    {
                        Continue = false;
                        break;
                    }
                    if (Game.GameTime - GameTimeStartedAnimation >= 2000)
                        break;
                    GameFiber.Yield();
                }

                if (Continue && CarPosition.DistanceTo2D(VehicleToChange.VehicleEnt.Position) <= 1f && Game.LocalPlayer.Character.DistanceTo2D(ChangeSpot) <= 2f && !Game.LocalPlayer.Character.IsDead)
                {
                    if (ChangePlates)
                        ChangePlate(VehicleToChange);
                    else
                        RemovePlate(VehicleToChange);
                }
                else
                {
                    Game.LocalPlayer.Character.Tasks.Clear();
                }
                if (LicensePlate != null && LicensePlate.Exists())
                    LicensePlate.Delete();
                GameFiber.Sleep(750);
                if(Screwdriver != null && Screwdriver.Exists())
                    Screwdriver.Delete();

                LicensePlate = null;
                Screwdriver = null;
                PlayerChangingPlate = false;
            }
            catch (Exception e)
            {
                if (LicensePlate != null && LicensePlate.Exists())
                    LicensePlate.Delete();
                if (Screwdriver != null && Screwdriver.Exists())
                    Screwdriver.Delete();

                LicensePlate = null;
                Screwdriver = null;
                PlayerChangingPlate = false;
                Debugging.WriteToLog("ChangeLicensePlate", e.StackTrace);
            }
        }, "PlayDispatchQueue");
        Debugging.GameFibers.Add(ChangeLicensePlateAnimation);
    }
    public static bool RemovePlate(GTAVehicle VehicleToChange)
    {
        if (VehicleToChange.VehicleEnt.Exists())
        {
            SpareLicensePlates.Add(VehicleToChange.CarPlate);
            //Menus.UpdateLists();
            VehicleToChange.CarPlate = null;
            VehicleToChange.VehicleEnt.LicensePlate = "        ";
            VehicleToChange.CarPlate = null;
            return true;
        }
        return false;
    }
    public static bool ChangePlate(GTAVehicle VehicleToChange)
    {
        if (VehicleToChange.VehicleEnt.Exists())
        {
            GTALicensePlate PlateToAdd = SpareLicensePlates[Menus.ChangePlateIndex];
            GTALicensePlate PlateToRemove = VehicleToChange.CarPlate;
            SpareLicensePlates.RemoveAt(Menus.ChangePlateIndex);
            if (PlateToRemove != null)
            {
                SpareLicensePlates.Add(PlateToRemove);
            }

            VehicleToChange.CarPlate = PlateToAdd;
            VehicleToChange.VehicleEnt.LicensePlate = PlateToAdd.PlateNumber;
            NativeFunction.CallByName<int>("SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX", VehicleToChange.VehicleEnt, PlateToAdd.PlateType);
            //Menus.UpdateLists();
            return true;
        }
        return false;
    }
    public static bool MovePedToCarPosition(Vehicle TargetVehicle, Ped PedToMove, float DesiredHeading, Vector3 PositionToMoveTo, bool StopDriver)
    {
        bool Continue = true;
        bool isPlayer = false;
        if (PedToMove == Game.LocalPlayer.Character)
            isPlayer = true;
        Ped Driver = TargetVehicle.Driver;
        //Vector3 CarPosition = TargetVehicle.Position;
        NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", PedToMove, PositionToMoveTo.X, PositionToMoveTo.Y, PositionToMoveTo.Z, DesiredHeading, -1);

        while (!(PedToMove.DistanceTo2D(PositionToMoveTo) <= 0.15f && PedToMove.Heading.IsWithin(DesiredHeading - 5f, DesiredHeading + 5f)))
        {
            GameFiber.Yield();
            if (isPlayer && Extensions.IsMoveControlPressed())
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
    //public static bool MovePedToCarPosition(Vehicle TargetVehicle, Ped PedToMove, float DesiredHeading, Vector3 PositionToMoveTo, bool StopDriver)
    //{
    //    bool Continue = true;
    //    bool isPlayer = false;
    //    if (PedToMove == Game.LocalPlayer.Character)
    //        isPlayer = true;
    //    Ped Driver = TargetVehicle.Driver;
    //    Vector3 CarPosition = TargetVehicle.Position;
    //    NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", PedToMove, PositionToMoveTo.X, PositionToMoveTo.Y, PositionToMoveTo.Z, DesiredHeading, -1);

    //    while (!(PedToMove.DistanceTo2D(PositionToMoveTo) <= 0.15f && PedToMove.Heading.IsWithin(DesiredHeading - 5f, DesiredHeading + 5f)))
    //    {
    //        GameFiber.Yield();
    //        if (isPlayer && Extensions.IsMoveControlPressed())
    //        {
    //            Continue = false;
    //            break;
    //        }
    //        if (StopDriver && TargetVehicle.Driver != null)
    //            NativeFunction.CallByName<uint>("TASK_VEHICLE_TEMP_ACTION", Driver, TargetVehicle, 27, -1);
    //    }
    //    if (!Continue)
    //    {
    //        PedToMove.Tasks.Clear();
    //        return false;
    //    }
    //    return true;
    //}
    public static Vector3 GetLicensePlateChangePosition(Vehicle VehicleToChange)
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
    public static Rage.Object AttachLicensePlateToPed(Ped Pedestrian)
    {
        Rage.Object LicensePlate = new Rage.Object("p_num_plate_01", Pedestrian.GetOffsetPositionUp(55f));
        LicensePlate.IsVisible = true;
        int BoneIndexLeftHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 18905);
        LicensePlate.AttachTo(Game.LocalPlayer.Character, BoneIndexLeftHand, new Vector3(0.19f, 0.08f, 0.0f), new Rotator(-57.2f, 90f, -173f));

        return LicensePlate;
    }
    //public static void PedReactToThreatening(Ped Attacker)
    //{
    //    int RandomNum = rnd.Next(1, 20);
    //    if (RandomNum <= 4 && LosSantosRED.PlayerIsNotWanted) //Murder
    //    {
    //        GTAWeapon GunToGive = GTAWeapons.GetRandomRegularWeaponByCategory(GTAWeapon.WeaponCategory.Pistol);
    //        Attacker.Inventory.GiveNewWeapon(GunToGive.Name, GunToGive.AmmoAmount, true);
    //        Attacker.Tasks.FightAgainst(Game.LocalPlayer.Character);
    //        Attacker.BlockPermanentEvents = true;
    //        Attacker.KeepTasks = true;
    //    }
    //    //else if (RandomNum == 2) //Run Away
    //    //{
    //    //    Attacker.Tasks.Cower(30000);
    //    //    Attacker.BlockPermanentEvents = true;
    //    //    Attacker.KeepTasks = true;
    //    //}
    //    else //Flee
    //    {
    //        Attacker.Tasks.Flee(Game.LocalPlayer.Character, 100f, 30000);
    //        Attacker.BlockPermanentEvents = true;
    //        Attacker.KeepTasks = true;
    //    }
    //}
}

