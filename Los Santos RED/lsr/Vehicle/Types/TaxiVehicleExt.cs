using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSR.Vehicles
{
    public class TaxiVehicleExt : VehicleExt
    {
        public override bool CanRandomlyHaveIllegalItems { get; set; } = true;
        public override bool CanUpdatePlate => false;
        public TaxiFirm TaxiFirm { get; set; }
        public override bool IsTaxi => true;
        public TaxiVehicleExt(Vehicle vehicle, ISettingsProvideable settings) : base(vehicle, settings)
        {
            VehicleInteractionMenu = new TaxiInteractionMenu(this, this);
        }
        public override void AddVehicleToList(IEntityProvideable world)
        {
            world.Vehicles.AddTaxi(this);
        }
        public override string GetDebugString()
        {
            string toReturn = base.GetDebugString();
            if (TaxiFirm == null)
            {
                return toReturn;
            }
            toReturn += $" TaxiFirm: {TaxiFirm.FullName}";
            if(Vehicle.Exists())
            {
                toReturn += $" Position: {Vehicle.Position} ";
            }
            return toReturn;
        }

        public override void UpdateInteractPrompts(IButtonPromptable player)
        {
            if (!Vehicle.Exists() || VehicleInteractionMenu.IsShowingMenu)// || Vehicle.Speed >= 0.5f || !Vehicle.Driver.Exists() || Vehicle.Driver.IsFleeing)
            {
                player.ButtonPrompts.RemovePrompts("VehicleInteract");
                return;
            }
            Ped driver = Vehicle.Driver;
            bool hasDriver = driver.Exists() && driver.Handle != player.Character.Handle && !driver.IsFleeing;
            //EntryPoint.WriteToConsole($"UpdateInteractPrompts TAXI {hasDriver}");
            bool hasPassengers = Vehicle.PassengerCount > 0;
            bool showRegularMenu = false;
            bool showGetInMenu = false;
            bool showTaxiMenu = false;

            string taxiPromptGetInString = "Get In Taxi";
            string taxiPromptInsideString = "Taxi Menu";
            if (TaxiFirm != null && TaxiFirm.IsRideShare)
            {
                taxiPromptGetInString = "Get In Rideshare";
                taxiPromptInsideString = "Rideshare Menu";
            }


            if(hasDriver && player.IsNotWanted)
            {
                if (player.IsInVehicle)
                {
                    showTaxiMenu = true;
                }
                else if (!hasPassengers && Vehicle.Speed <= 1.0f)
                {
                    showGetInMenu = true;
                }
            }
            else
            {
                showRegularMenu = true;
            }

            if(!showGetInMenu)
            {
                player.ButtonPrompts.RemovePrompt($"VehicleInteractTaxiGetIn");
            }
            if(!showTaxiMenu)
            {
                player.ButtonPrompts.RemovePrompt("VehicleInteractTaxiMenu");
            }

            if (showGetInMenu)
            {
                if (!player.ButtonPrompts.HasPrompt($"VehicleInteractTaxiGetIn"))
                {
                    player.ButtonPrompts.RemovePrompts("VehicleInteract");
                    //EntryPoint.WriteToConsole($"UpdateInteractPrompts TAXI {hasDriver} GET IN AS PASS MENU");
                    Action action = () => { player.ActivityManager.EnterVehicleAsPassenger(false, true, true); };
                    player.ButtonPrompts.AttemptAddPrompt("VehicleInteract", taxiPromptGetInString, $"VehicleInteractTaxiGetIn", Settings.SettingsManager.KeySettings.VehicleInteractModifier, Settings.SettingsManager.KeySettings.VehicleInteract, 999, action);
                }
            }
            else if (showTaxiMenu)
            {
                if (!player.ButtonPrompts.HasPrompt($"VehicleInteractTaxiMenu"))
                {
                    player.ButtonPrompts.RemovePrompts("VehicleInteract");
                   // EntryPoint.WriteToConsole($"UpdateInteractPrompts TAXI {hasDriver} OPEN TAXI MENU");
                    Action action = () => { player.ShowVehicleInteractMenu(false); };
                    player.ButtonPrompts.AttemptAddPrompt("VehicleInteract", taxiPromptInsideString, $"VehicleInteractTaxiMenu", Settings.SettingsManager.KeySettings.VehicleInteractModifier, Settings.SettingsManager.KeySettings.VehicleInteract, 999, action);
                }
            }
            else if (showRegularMenu)
            {
                //player.ButtonPrompts.RemovePrompts("Vehicle Interact");
                base.UpdateInteractPrompts(player);
            }
            else
            {
                player.ButtonPrompts.RemovePrompts("VehicleInteract");
            }
        }





    }
}
