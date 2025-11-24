using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


public class ModShopMenu
{
    private ILocationInteractable Player;
    private UIMenu InteractionMenu;
    private GameLocation GameLocation;
    private MenuPool MenuPool;
    private IPlateTypes PlateTypes;
    private int FinalRepairCost;
    private int MaxRepairCost;
    private int RepairHours;
    private int WashCost;
    private int WashHours;
    private List<int> RestrictedModTypes;
    public int DefaultPrice { get; private set; }
    public int DefaultPriceScalar { get; private set; }
    public VehicleVariation CurrentVariation { get; private set; }
    public VehicleExt ModdingVehicle { get; private set; }
    public VehicleVariationShopMenu VehicleVariationShopMenu { get; private set; }

    public ModShopMenu(ILocationInteractable player, MenuPool menuPool, UIMenu interactionMenu, VehicleModShop vehicleModShop, VehicleVariationShopMenu vehicleVariationShopMenu, 
        int maxRepairCost, int repairHours, int washCost, int washHours, List<int> restrictedModTypes, int defaultPrice, int defaultPriceScalar, IPlateTypes plateTypes)
    {
        Player = player;
        MenuPool = menuPool;
        InteractionMenu = interactionMenu;
        GameLocation = vehicleModShop;
        MaxRepairCost = maxRepairCost;
        RepairHours = repairHours;
        WashCost = washCost;
        WashHours = washHours;
        RestrictedModTypes = restrictedModTypes;
        VehicleVariationShopMenu = vehicleVariationShopMenu;
        DefaultPrice = defaultPrice;
        DefaultPriceScalar = defaultPriceScalar;
        PlateTypes = plateTypes;
    }



    public void CreateMenu()
    {
        if(Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        ModdingVehicle = Player.CurrentVehicle;
        CurrentVariation = NativeHelper.GetVehicleVariation(ModdingVehicle.Vehicle);

        if(ModdingVehicle.Vehicle.Health < ModdingVehicle.Vehicle.MaxHealth)
        {
            AddRepairItem();
        }
        else
        {
            AddModItems();
        }
        InteractionMenu.Visible = true;


        EntryPoint.WriteToConsole($"InteractionMenu.Visible {InteractionMenu.Visible}");
    }

    private void AddModItems()
    {
        AddColorCategories();
        AddExtraCategories();
        AddLicensePlateCategories();
        AddModCategories();
        ThisIsANewName();
    }

    private void AddLicensePlateCategories()
    {

        EntryPoint.WriteToConsole("AddLicensePlateCategories");
        VehicleLicensePlateMenu vehicleLicensePlateMenu = new VehicleLicensePlateMenu(MenuPool, InteractionMenu, Player, ModdingVehicle, this, CurrentVariation, GameLocation, PlateTypes);
        vehicleLicensePlateMenu.Setup();
    }

    private void AddColorCategories()
    {
        EntryPoint.WriteToConsole("AddColorCategories");
        VehicleColorMenu vehicleColorMenu = new VehicleColorMenu(MenuPool, InteractionMenu, Player, ModdingVehicle, this, CurrentVariation, GameLocation);
        vehicleColorMenu.Setup();
    }

    private void AddExtraCategories()
    {
        EntryPoint.WriteToConsole("AddExtraCategories");
        VehicleExtrasMenu vehicleExtrasMenu = new VehicleExtrasMenu(MenuPool, InteractionMenu, Player, ModdingVehicle, this, CurrentVariation, GameLocation);
        vehicleExtrasMenu.Setup();
    }

    private void AddModCategories()
    {
        EntryPoint.WriteToConsole("AddModCategories");
        NativeFunction.Natives.SET_VEHICLE_MOD_KIT(ModdingVehicle.Vehicle, 0);
        List<VehicleModKitType> VehicleModKitTypes = new List<VehicleModKitType>()
        {
            new PerformanceVehicleModKitType("Engine",11, this, InteractionMenu,MenuPool,Player),
            new PerformanceVehicleModKitType("Brakes",12, this, InteractionMenu,MenuPool,Player),
            new PerformanceVehicleModKitType("Transmission",13, this, InteractionMenu,MenuPool,Player),
            new PerformanceVehicleModKitType("Suspension",15, this, InteractionMenu,MenuPool,Player),


            new VehicleModKitType("Horns",14,"", this, InteractionMenu,MenuPool,Player),   
            new VehicleModKitType("Armor",16, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Nitrous",17, this, InteractionMenu,MenuPool,Player),
            new VehicleModToggle("Turbo",18, this, InteractionMenu,MenuPool,Player),


            new VehicleModKitType("Spoilers",0,"Add a spoiler", this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Front Bumper",1,"Front Bumper", this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Rear Bumper",2, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Side Skirt",3, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Exhaust",4, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Frame",5, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Grille",6, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Hood",7, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Fender",8, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Right Fender",9, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Roof",10, this, InteractionMenu,MenuPool,Player),

            new VehicleModKitType("Subwoofer",19, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Tire Smoke",20, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Hydraulics",21, this, InteractionMenu,MenuPool,Player),
            new VehicleModToggle("Xenon Lights",22, this, InteractionMenu,MenuPool,Player),
            new WheelVehicleModKitType("Front Wheels",23, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Back Wheels",24, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Plate holders", 25, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Vanity Plate", 26, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Trim Design", 27, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Ornaments", 28, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Interior (3)", 29, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Dial Design", 30, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Interior5", 31, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Seats", 32, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Steering Wheel", 33, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Shift Lever", 34, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Plaques", 35, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Ice", 36, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Trunk", 37, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Hydro", 38, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Engine (1)", 39, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Boost", 40, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Engine (3)", 41, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Pushbar", 42, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Aerials", 43, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Chassis (4)", 44, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Chassis (5)", 45, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Door-L", 46, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Door-R", 47, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Livery", 48, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Lightbar", 49, this, InteractionMenu,MenuPool,Player),
        };
        foreach(VehicleModKitType vmkt in VehicleModKitTypes)
        {
            vmkt.AddToMenu();
        }
    }
    private void ThisIsANewName()
    {
        EntryPoint.WriteToConsole("AddWindowTintCategories");
        VehicleWindowTintMenu windowTintMenu = new VehicleWindowTintMenu(MenuPool, InteractionMenu, Player, ModdingVehicle, this, CurrentVariation, GameLocation);
        windowTintMenu.Setup();
    }
    public void DisplayInsufficientFundsMessage(int price)
    {
        if (GameLocation == null)
        {
            return;
        }
        GameLocation.PlayErrorSound();
        GameLocation.DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
    }
    public void DisplayPurchasedMessage(int price)
    {
        if (GameLocation == null)
        {
            return;
        }
        GameLocation.PlaySuccessSound();
        GameLocation.DisplayMessage("~g~Purchased", $"Thank you for your purchase");
    }
    //public int GetModKitPrice(int modKitTypeID, int modKitValueID)
    //{
    //    if(modKitValueID == -1)
    //    {
    //        return 0;
    //    }
    //    if(VehicleVariationShopMenu == null)
    //    {
    //        return DefaultPrice + (DefaultPriceScalar * modKitValueID);
    //    }
    //    VehicleVariationShopMenu.

    //    return 500;
    //}
    private void AddRepairItem()
    {
        FinalRepairCost = MaxRepairCost;
        int CurrentHealth = ModdingVehicle.Vehicle.Health;
        int MaxHealth = ModdingVehicle.Vehicle.MaxHealth;
        float healthPercentage = (float)CurrentHealth / (float)MaxHealth;
        bool isFullHealth = CurrentHealth == MaxHealth;
        FinalRepairCost = (int)Math.Ceiling((1.0f - healthPercentage) * MaxRepairCost);
        FinalRepairCost.Round(100);
        UIMenuItem repairVehicle = new UIMenuItem("Repair Vehicle", $"Repair the current vehicle.~n~~g~Vehicle Health: {CurrentHealth}/{MaxHealth}") { RightLabel = "~r~" + FinalRepairCost.ToString("C0") + "~s~" };
        repairVehicle.Activated += (sender, e) =>
        {
            DoRepairItems();
            InteractionMenu.Clear();
            CreateMenu();
        };
        InteractionMenu.AddItem(repairVehicle);
    }
    private void DoRepairItems()
    {
        int totalRepairCost = FinalRepairCost;
        if (!Player.IsInVehicle || ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
        {
            return;
        }
        if (Player.BankAccounts.GetMoney(false) <= totalRepairCost)
        {
            NativeHelper.PlayErrorSound();
            GameLocation?.DisplayMessage("~r~Cash Only", "You do not have enough cash on hand.");
            return;
        }
        ModdingVehicle.Engine.SetState(false);
        RepairVehicle();
        Player.BankAccounts.GiveMoney(-1 * totalRepairCost, false);
        NativeHelper.PlaySuccessSound();
        GameLocation?.DisplayMessage("~g~Repaired", $"Thank you for fixing your vehicle at ~y~~s~");
    }
    private void RepairVehicle()
    {
        if (!Player.IsInVehicle || ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
        {
            return;
        }
        ModdingVehicle.Vehicle.Repair();
        ModdingVehicle.Vehicle.Wash();
    }

    public void DisplayMessage(string messageBody)
    {
        if (GameLocation == null)
        {
            return;
        }
        GameLocation.PlayErrorSound();
        GameLocation.DisplayMessage("~y~Information", messageBody);
    }


}

