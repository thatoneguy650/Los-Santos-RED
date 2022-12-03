using LosSantosRED.lsr.Helper;
using LSR.Vehicles;
using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using System.Xml.Linq;

public class Refueling
{
    private ILocationInteractable Player;
    private ISettingsProvideable Settings;
    private int PricePerUnit;

    private VehicleExt VehicleExt;
    private bool IsCancelled = false;
    private string Name;
    public Refueling(ILocationInteractable player, string name, int pricePerUnit, VehicleExt vehicleExt, ISettingsProvideable settings)
    {
        Player = player;
        PricePerUnit = pricePerUnit;
        Name = name;
        VehicleExt = vehicleExt;
        Settings = settings;
    }
    public int UnitsAdded = 0;
    public int VehicleToFillFuelTankCapacity { get; private set; }
    public float PercentFuelNeeded { get; private set; }
    public int UnitsOfFuelNeeded { get; private set; }
    public float PercentFilledPerUnit { get; private set; }
    public float AmountToFill { get; private set; }
    public bool CanRefuel => VehicleExt != null && VehicleExt.Vehicle.Exists() && !VehicleExt.Vehicle.IsEngineOn && VehicleExt.Vehicle.FuelLevel < 100f && VehicleExt.RequiresFuel;
    public void Setup()
    {
        if(VehicleExt != null && VehicleExt.Vehicle.Exists())
        {
            VehicleToFillFuelTankCapacity = VehicleExt.FuelTankCapacity;
            GetFuelStatus();
        }
        else
        {
            IsCancelled = true;
        }
    }
    public void GetFuelStatus()
    {
        PercentFuelNeeded = (100f - VehicleExt.Vehicle.FuelLevel) / 100f;
        UnitsOfFuelNeeded = (int)Math.Ceiling(PercentFuelNeeded * VehicleToFillFuelTankCapacity);
        if (VehicleToFillFuelTankCapacity == 0)
        {
            PercentFilledPerUnit = 0;
        }
        else
        {
            PercentFilledPerUnit = 100f / VehicleToFillFuelTankCapacity;
        }
        AmountToFill = UnitsOfFuelNeeded * PricePerUnit;
    }
    public bool RefuelSlow(int UnitsToAdd, GasPump gasPump)
    {
        if (UnitsToAdd * PricePerUnit > Player.BankAccounts.Money)
        {
            PurchaseFailed();
            return false;
        }
        else
        {
            Player.ButtonPrompts.AddPrompt("Fueling", "Cancel Fueling", "CancelFueling", Settings.SettingsManager.KeySettings.InteractCancel, 99);
            int UnitsAdded = 0;
            GameFiber FastForwardWatcher = GameFiber.StartNew(delegate
            {
                uint GameTimeBetweenUnits = 1500;
                uint GameTimeAddedUnit = Game.GameTime;
                int dotsAdded = 0;
                if (VehicleExt.Vehicle.Exists())
                {
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, VehicleExt.Vehicle, 2000);
                        NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, VehicleExt.Vehicle, -1, 0, 2);
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }
                }
                while (UnitsAdded < UnitsToAdd && VehicleExt.Vehicle.Exists() && !VehicleExt.Vehicle.IsEngineOn)
                {
                    string tabs = new string('.', dotsAdded);
                    Game.DisplayHelp($"Fueling Progress {UnitsAdded}/{UnitsToAdd}");
                    NativeHelper.DisablePlayerControl();
                    if (Game.GameTime - GameTimeAddedUnit >= GameTimeBetweenUnits)
                    {
                        UnitsAdded++;
                        GameTimeAddedUnit = Game.GameTime;
                        if (VehicleExt.Vehicle.FuelLevel + PercentFilledPerUnit > 100f)
                        {
                            VehicleExt.Vehicle.FuelLevel = 100f;
                        }
                        else
                        {
                            VehicleExt.Vehicle.FuelLevel += PercentFilledPerUnit;
                        }
                        Player.BankAccounts.GiveMoney(-1 * PricePerUnit);
                        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET", 0);

                        EntryPoint.WriteToConsole($"Gas pump added unit of gas Percent Added {PercentFilledPerUnit} Money Subtracted {-1 * PricePerUnit}");
                    }
                    if (Player.ButtonPrompts.IsPressed("CancelFueling"))
                    {
                        break;
                    }
                    GameFiber.Yield();
                }
                if (UnitsAdded > 0)
                {
                    PurchaseSucceeded(UnitsToAdd);
                }
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                NativeFunction.Natives.ENABLE_ALL_CONTROL_ACTIONS(0);
                Player.ButtonPrompts.RemovePrompts("Fueling");
                if (gasPump != null)
                {
                    gasPump.IsFueling = false;
                }
            }, "FastForwardWatcher");
            return true;
        }
    }
    public bool RefuelQuick(int UnitsToAdd)
    {
        if (UnitsToAdd * PricePerUnit > Player.BankAccounts.Money)
        {
            PurchaseFailed();
            return false;
        }
        else
        {
            if (VehicleExt != null && VehicleExt.Vehicle.Exists())
            {
                VehicleExt.Vehicle.FuelLevel += PercentFilledPerUnit * UnitsToAdd;
                if (VehicleExt.Vehicle.FuelLevel >= 99.9f)
                {
                    VehicleExt.Vehicle.FuelLevel = 100.0f;
                }
                Player.BankAccounts.GiveMoney(-1 * PricePerUnit * UnitsToAdd);
                if (UnitsToAdd > 0)
                {
                    PurchaseSucceeded(UnitsToAdd);
                }
            }
        }
        return true;
    }
    private void PurchaseFailed()
    {
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "ERROR", "HUD_LIQUOR_STORE_SOUNDSET", 0);
        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Purchase Failed", "We are sorry, we are unable to complete this transation. Please make sure you have the funds.");
    }
    private void PurchaseSucceeded(int UnitsToAdd)
    {
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET", 0);
        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~g~Purchased", $"Thank you for purchasing {UnitsToAdd} gallons of fuel for a total price of ~r~${UnitsToAdd * PricePerUnit}~s~ at {Name}");
    }
    public void DisplayFuelingFailedReason()
    {
        if (VehicleExt == null || (VehicleExt != null && !VehicleExt.Vehicle.Exists()))
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Fueling Failed", $"No vehicle found to fuel");
        }
        else if (VehicleExt != null && VehicleExt.Vehicle.Exists() && !VehicleExt.RequiresFuel)
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Fueling Failed", $"Incompatible Fueling");
        }
        else if (VehicleExt != null && VehicleExt.Vehicle.Exists() && VehicleExt.Vehicle.IsEngineOn)
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Fueling Failed", $"Vehicle engine is still on");
        }
        else if (VehicleExt != null && VehicleExt.Vehicle.Exists() && !VehicleExt.Vehicle.IsEngineOn && VehicleExt.Vehicle.FuelLevel >= 100f)
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Fueling Failed", $"Vehicle fuel tank is already full");
        }
    }
}

