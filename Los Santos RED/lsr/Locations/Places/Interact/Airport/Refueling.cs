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
    private float PercentFuelNeeded;
    private int VehicleToFillFuelTankCapacity;
    private int UnitsOfFuelNeeded;
    private float PercentFilledPerUnit;
    private float AmountToFill;
    private VehicleExt VehicleExt;
    private bool IsCancelled = false;
    private string Name;
    public Refueling(ILocationInteractable player, string name,int pricePerUnit, ISettingsProvideable settings)
    {
        Player = player;
        PricePerUnit = pricePerUnit;
        Name = name;
        Settings = settings;
    }
    public int UnitsAdded = 0;
    public void Setup()
    {
        if (VehicleExt.Vehicle.Exists())
        {
            PercentFuelNeeded = (100f - VehicleExt.Vehicle.FuelLevel) / 100f;
            VehicleToFillFuelTankCapacity = VehicleExt.FuelTankCapacity;
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
        else
        {
            IsCancelled = true;
        }
    }
    public bool Refuel(int UnitsToAdd)
    {
        if (UnitsToAdd * PricePerUnit > Player.BankAccounts.Money)
        {
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "ERROR", "HUD_LIQUOR_STORE_SOUNDSET", 0);
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Purchase Failed", "We are sorry, we are unable to complete this transation. Please make sure you have the funds.");
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
                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET", 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~g~Purchased", $"Thank you for purchasing {UnitsAdded} gallons of fuel for a total price of ~r~${UnitsAdded * PricePerUnit}~s~ at {Name}");
                }
                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                NativeFunction.Natives.ENABLE_ALL_CONTROL_ACTIONS(0);
                Player.ButtonPrompts.RemovePrompts("Fueling");
            }, "FastForwardWatcher");
            return true;
        }
    }
}

