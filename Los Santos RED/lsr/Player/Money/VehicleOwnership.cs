using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleOwnership
{
    private IVehicleOwnable Player;
    private IEntityProvideable World;
    public List<VehicleExt> OwnedVehicles { get; set; } = new List<VehicleExt>();

    public VehicleOwnership(IVehicleOwnable player, IEntityProvideable world)
    {
        Player = player;
        World = world;
    }

    public void Reset()
    {
        ClearVehicleOwnership();
    }

    public void Setup()
    {

    }
    public void Update()
    {
        UpdateOwnedBlips();
    }

    public void Dispose()
    {
        ClearVehicleOwnership();
    }

    public void ClearVehicleOwnership()
    {
        foreach (VehicleExt car in OwnedVehicles)
        {
            if (car.Vehicle.Exists())
            {
                Blip attachedBlip = car.Vehicle.GetAttachedBlip();
                if (attachedBlip.Exists())
                {
                    attachedBlip.Delete();
                }
                if (car.AttachedBlip.Exists())
                {
                    car.AttachedBlip.Delete();
                }
                car.Vehicle.IsPersistent = false;
            }
        }
        OwnedVehicles.Clear();
        EntryPoint.WriteToConsole($"PLAYER EVENT: OWNED VEHICLEs CLEARED", 5);
    }
    public void RemoveOwnershipOfNearestCar()
    {
        VehicleExt toTakeOwnershipOf;
        if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
        {
            toTakeOwnershipOf = Player.CurrentVehicle;
        }
        else
        {
            toTakeOwnershipOf = World.Vehicles.GetClosestVehicleExt(Player.Character.Position, false, 10f);
        }
        if (toTakeOwnershipOf != null && toTakeOwnershipOf.Vehicle.Exists())
        {
            RemoveOwnershipOfVehicle(toTakeOwnershipOf);
            //DisplayPlayerNotification();
        }
        else
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info", string.Format("~y~{0}", Player.PlayerName), "No Vehicle Found");
        }
    }
    public void RemoveOwnershipOfVehicle(VehicleExt toOwn)
    {
        if (toOwn != null && toOwn.Vehicle.Exists())
        {
            Blip attachedBlip = toOwn.Vehicle.GetAttachedBlip();
            if (attachedBlip.Exists())
            {
                attachedBlip.Delete();
            }

            if (toOwn.AttachedBlip.Exists())
            {
                toOwn.AttachedBlip.Delete();
            }

            toOwn.Vehicle.IsPersistent = false;
            toOwn.OwnedByPlayer = false;
            
        }
        if (OwnedVehicles.Any(x => x.Handle == toOwn.Handle))
        {
            OwnedVehicles.Remove(toOwn);
        }
        UpdateOwnedBlips();
        EntryPoint.WriteToConsole($"PLAYER EVENT: OWNED VEHICLE REMOVED {toOwn.Vehicle.Handle}", 5);
    }
    public void TakeOwnershipOfNearestCar()
    {
        VehicleExt toTakeOwnershipOf;
        if (Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
        {
            toTakeOwnershipOf = Player.CurrentVehicle;
        }
        else
        {

            bool allowPolice = false;
#if DEBUG
            allowPolice = true;
#endif

            toTakeOwnershipOf = World.Vehicles.GetClosestVehicleExt(Player.Character.Position, allowPolice, 10f);
        }
        if (toTakeOwnershipOf != null && toTakeOwnershipOf.Vehicle.Exists())
        {
            TakeOwnershipOfVehicle(toTakeOwnershipOf, true);
            //DisplayPlayerNotification();
        }
        else
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info", string.Format("~y~{0}", Player.PlayerName), "No Vehicle Found");
        }
    }
    public void TakeOwnershipOfVehicle(VehicleExt toOwn, bool showNotification)
    {
        if (toOwn != null && toOwn.Vehicle.Exists() && !OwnedVehicles.Any(x => x.Handle == toOwn.Handle))
        {
            toOwn.SetNotWanted();
            toOwn.Vehicle.IsStolen = false;
            toOwn.Vehicle.IsPersistent = true;
            toOwn.OwnedByPlayer = true;

            OwnedVehicles.Add(toOwn);
            UpdateOwnedBlips();
            if (showNotification)
            {
                DisplayPlayerVehicleNotification(toOwn);
            }
            EntryPoint.WriteToConsole($"PLAYER EVENT: OWNED VEHICLE ADDED {toOwn.Vehicle.Handle}", 5);
        }
    }

    private void UpdateOwnedBlips()
    {
        //EntryPoint.WriteToConsole($"PLAYER EVENT: UpdateOwnedBlips CurrentVehicle {CurrentVehicle != null}", 5);
        foreach (VehicleExt car in OwnedVehicles)
        {
            if (car.Vehicle.Exists())
            {
                if (Player.CurrentVehicle?.Handle == car.Handle)
                {
                    if (car.AttachedBlip.Exists())
                    {
                        car.AttachedBlip.Delete();
                    }
                }
                else
                {
                    if (!car.AttachedBlip.Exists())
                    {
                        car.AttachedBlip = car.Vehicle.AttachBlip();
                        car.AttachedBlip.Sprite = BlipSprite.GetawayCar;
                        car.AttachedBlip.Color = System.Drawing.Color.Red;
                    }
                }
            }
        }
        //}
    }


    public void DisplayPlayerVehicleNotification(VehicleExt toDescribe)
    {
        string NotifcationText = "";
        VehicleExt VehicleToDescribe = toDescribe;
        bool usingOwned = true;
        if (VehicleToDescribe != null)
        {
            //string Make = VehicleToDescribe.MakeName();
            //string Model = VehicleToDescribe.ModelName();
            //string VehicleName = "";
            //if (Make != "")
            //{
            //    VehicleName = Make;
            //}
            //if (Model != "")
            //{
            //    VehicleName += " " + Model;
            //}

            string VehicleName = VehicleToDescribe.FullName(false);


            //string VehicleNameColor = "~p~";
            //string VehicleString = "";
            if (usingOwned)
            {
                NotifcationText += $"Vehicle: ~p~{VehicleName}~n~~s~Status: ~p~Owned~s~";
            }
            else if (!VehicleToDescribe.IsStolen)
            {
                NotifcationText += $"Vehicle: ~p~{VehicleName}~n~~s~Status: ~p~Unknown~s~";
            }
            else
            {
                NotifcationText += $"Vehicle: ~r~{VehicleName}~n~~s~Status: ~r~Stolen~s~";
            }
            if (VehicleToDescribe.CarPlate != null && VehicleToDescribe.CarPlate.IsWanted)
            {
                NotifcationText += $"~n~Plate: ~r~{VehicleToDescribe.CarPlate.PlateNumber} ~r~(Wanted)~s~";
            }
            else
            {
                NotifcationText += $"~n~Plate: ~p~{VehicleToDescribe.CarPlate.PlateNumber} ~s~";
            }
        }

        if (NotifcationText != "")
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~g~Vehicle Info", $"~y~{Player.PlayerName}", NotifcationText);
        }
        else
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~g~Vehicle Info", $"~y~{Player.PlayerName}", "~s~Vehicle: None");
        }
    }


}

