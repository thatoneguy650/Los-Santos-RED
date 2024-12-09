using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleOwnership
{
    private IVehicleOwnable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    public List<VehicleExt> OwnedVehicles { get; set; } = new List<VehicleExt>();

    public VehicleOwnership(IVehicleOwnable player, IEntityProvideable world, ISettingsProvideable settings)
    {
        Player = player;
        World = world;
        Settings = settings;
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
            car.ResetItems();
            car.RemoveOwnership();
            car.RemoveBlip();       
        }
        OwnedVehicles.Clear();
        //EntryPoint.WriteToConsole($"PLAYER EVENT: OWNED VEHICLEs CLEARED");
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
        }
        else
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info", string.Format("~y~{0}", Player.PlayerName), "No Vehicle Found");
        }
    }
    public void RemoveOwnershipOfVehicle(VehicleExt toOwn)
    {
        if(toOwn == null)
        {
            return;
        }
        //EntryPoint.WriteToConsole($"PLAYER EVENT: OWNED VEHICLE REMOVED {toOwn.Vehicle.Handle}");
        if (OwnedVehicles.Any(x => x.Handle == toOwn.Handle))
        {
            OwnedVehicles.Remove(toOwn);
        }
        toOwn.RemoveOwnership();
        toOwn.RemoveBlip();
        UpdateOwnedBlips();     
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
            if(Player.IsCop)
            {
                allowPolice = true;
            }
            toTakeOwnershipOf = World.Vehicles.GetClosestVehicleExt(Player.Character.Position, allowPolice, 10f);
        }
        if (toTakeOwnershipOf == null || !toTakeOwnershipOf.Vehicle.Exists())
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~b~Personal Info", string.Format("~y~{0}", Player.PlayerName), "No Vehicle Found");
            return;
        }
        TakeOwnershipOfVehicle(toTakeOwnershipOf, true); 
    }
    public void TakeOwnershipOfVehicle(VehicleExt toOwn, bool showNotification)
    {
        if(toOwn == null || !toOwn.Vehicle.Exists() || OwnedVehicles.Any(x => x.Handle == toOwn.Handle))
        { 
            return; 
        }
        toOwn.SetNotWanted();
        toOwn.AddOwnership();
        Player.VehicleManager.OnTookOwnership(toOwn);
        OwnedVehicles.Add(toOwn);
        UpdateOwnedBlips();
        if (showNotification)
        {
            DisplayPlayerVehicleNotification(toOwn);
        }
        EntryPoint.WriteToConsole($"PLAYER EVENT: OWNED VEHICLE ADDED {toOwn.Vehicle.Handle}");    
    }
    private void UpdateOwnedBlips()
    {
        foreach (VehicleExt car in OwnedVehicles)
        {
            if(!car.Vehicle.Exists())
            {
                continue;
            }

            if (Player.CurrentVehicle?.Handle == car.Handle || !Settings.SettingsManager.VehicleSettings.AttachOwnedVehicleBlips)
            {
                car.RemoveBlip();
            }
            else if (Settings.SettingsManager.VehicleSettings.AttachOwnedVehicleBlips)
            {
                car.AddOwnershipBlip();
            }
        }
    }
    public void DisplayPlayerVehicleNotification(VehicleExt toDescribe)
    {
        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~g~Vehicle Info", $"~y~{Player.PlayerName}", GetVehicleNotificationText(toDescribe));
    }
    private string GetVehicleNotificationText(VehicleExt toDescribe)
    {
        string NotifcationText = "~s~Vehicle: None";
        if(toDescribe == null)
        {
            return NotifcationText;
        }
        return toDescribe.GetRegularDescription(true); 
    }
}

