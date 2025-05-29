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
    private int FinalRepairCost;
    private int MaxRepairCost;
    private int RepairHours;
    private int WashCost;
    private int WashHours;
    private List<int> RestrictedModTypes;
    public VehicleVariation CurrentVariation { get; private set; }
    public VehicleExt ModdingVehicle { get; private set; }

    public ModShopMenu(ILocationInteractable player, MenuPool menuPool, UIMenu interactionMenu, VehicleModShop vehicleModShop, int maxRepairCost, int repairHours, int washCost, int washHours, List<int> restrictedModTypes)
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
    }

    private void AddModItems()
    {
        AddModCategories();
        AddExtraCategories();
        AddColorCategories();
    }

    private void AddColorCategories()
    {
        VehicleColorMenu vehicleColorMenu = new VehicleColorMenu(MenuPool, InteractionMenu, Player, ModdingVehicle, CurrentVariation, GameLocation);
        vehicleColorMenu.Setup();
    }

    private void AddExtraCategories()
    {
        
    }

    private void AddModCategories()
    {
        List<ModKitDescription> ModKitDescriptions = new List<ModKitDescription>()
        {
            new ModKitDescription("Spoilers",0),
            new ModKitDescription("Front Bumper",1),
            new ModKitDescription("Rear Bumper",2),
            new ModKitDescription("Side Skirt",3),
            new ModKitDescription("Exhaust",4),
            new ModKitDescription("Frame",5),
            new ModKitDescription("Grille",6),
            new ModKitDescription("Hood",7),
            new ModKitDescription("Fender",8),
            new ModKitDescription("Right Fender",9),
            new ModKitDescription("Roof",10),
            new ModKitDescription("Engine",11),
            new ModKitDescription("Brakes",12),
            new ModKitDescription("Transmission",13),
            new ModKitDescription("Horns",14),
            new ModKitDescription("Suspension",15),
            new ModKitDescription("Armor",16),
            new ModKitDescription("Nitrous",17),
            new ModKitDescription("Turbo",18),
            new ModKitDescription("Subwoofer",19),
            new ModKitDescription("Tire Smoke",20),
            new ModKitDescription("Hydraulics",21),
            new ModKitDescription("Xenon",22),
            new ModKitDescription("Front Wheels",23),
            new ModKitDescription("Back Wheels (Motorcycle)",24),
            new ModKitDescription("Plate holders", 25),
            new ModKitDescription("Vanity Plate", 26),
            new ModKitDescription("Trim Design", 27),
            new ModKitDescription("Ornaments", 28),
            new ModKitDescription("Interior3", 29),
            new ModKitDescription("Dial Design", 30),
            new ModKitDescription("Interior5", 31),
            new ModKitDescription("Seats", 32),
            new ModKitDescription("Steering Wheel", 33),
            new ModKitDescription("Shift Lever", 34),
            new ModKitDescription("Plaques", 35),
            new ModKitDescription("Ice", 36),
            new ModKitDescription("Trunk", 37),
            new ModKitDescription("Hydro", 38),
            new ModKitDescription("Engine1", 39),
            new ModKitDescription("Boost", 40),
            new ModKitDescription("Engine3", 41),
            new ModKitDescription("Pushbar", 42),
            new ModKitDescription("Aerials", 43),
            new ModKitDescription("Chassis4", 44),
            new ModKitDescription("Chassis5", 45),
            new ModKitDescription("Door-L", 46),
            new ModKitDescription("Door-R", 47),
            new ModKitDescription("Livery", 48),
            new ModKitDescription("Lightbar", 49),
        };

        NativeFunction.Natives.SET_VEHICLE_MOD_KIT(ModdingVehicle.Vehicle, 0);

        List<VehicleModKitType> VehicleModKitTypes = new List<VehicleModKitType>()
        { 
            new VehicleModKitType("Spoilers",0, this, InteractionMenu,MenuPool,Player),
            new VehicleModKitType("Front Bumper",1, this, InteractionMenu,MenuPool,Player),
        };

        foreach(VehicleModKitType vmkt in VehicleModKitTypes)
        {
            vmkt.AddToMenu();
        }



        //if(RestrictedModTypes != null && RestrictedModTypes.Any())
        //{
        //    ModKitDescriptions.RemoveAll(x => RestrictedModTypes.Contains(x.ID));
        //    EntryPoint.WriteToConsole("Mod Kit Menu Removing Some Mod Kit Items");
        //}


        //foreach (ModKitDescription modKitDescription in ModKitDescriptions.OrderBy(x=> x.Name))
        //{
        //    int TotalMods = NativeFunction.Natives.GET_NUM_VEHICLE_MODS<int>(ModdingVehicle.Vehicle, modKitDescription.ID);
        //    EntryPoint.WriteToConsole($"{modKitDescription.Name}ID:{modKitDescription.ID}TotalMods {TotalMods}");
        //    if (TotalMods > 0)
        //    {
        //        UIMenu modKitTypeSubMenu = MenuPool.AddSubMenu(InteractionMenu, modKitDescription.Name);



        //        modKitTypeSubMenu.OnMenuOpen += (sender) =>
        //        {
        //            ResetModType(modKitDescription.ID);
        //            SetVehicleMod(modKitDescription.ID, modKitTypeSubMenu.CurrentSelection, false);
        //        };
        //        modKitTypeSubMenu.OnMenuClose += (sender) =>
        //        {
        //            ResetModType(modKitDescription.ID);
        //        };

        //        modKitTypeSubMenu.OnIndexChange += (sender, newIndex) =>
        //        {
        //            SetVehicleMod(modKitDescription.ID, newIndex, false);
        //        };

        //        for (int i = 0; i < TotalMods; i++)
        //        {
        //            string modItemName = GetModItemName(modKitDescription.ID, i);
                    
        //            UIMenuItem modkitItem = new UIMenuItem($"{modItemName}", "Description");

        //            bool hasModInstalled = false;
        //            VehicleMod existingMod = CurrentVariation.VehicleMods?.Where(x => x.ID == modKitDescription.ID).FirstOrDefault();
        //            if(existingMod != null && existingMod.Output == i)
        //            {
        //                hasModInstalled = true;
        //            }

        //            int modKitPrice = GetModKitPrice(modKitDescription.ID, i);
        //            modkitItem.RightLabel = $"~r~${modKitPrice}~s~";

        //            if(hasModInstalled)//need to update all these kajangos
        //            {
        //                modkitItem.RightLabel = "";
        //                modkitItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
        //            }


        //            int myId = i;
        //            modkitItem.Activated += (sender, e) =>
        //            {
                       
        //                if(Player.BankAccounts.GetMoney(true) < modKitPrice)
        //                {
        //                    DisplayInsufficientFundsMessage(modKitPrice);
        //                    return;
        //                }
        //                DisplayPurchasedMessage(modKitPrice);
        //                Player.BankAccounts.GiveMoney(-1 * modKitPrice, true);

        //                //apply the modkit permanently
        //                if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
        //                {
        //                    return;
        //                }
        //                SetVehicleMod(modKitDescription.ID, myId, true);
        //                modkitItem.RightLabel = "";
        //                modkitItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
        //                EntryPoint.WriteToConsole($"APPLY MODKIT modKitDescription.ID:{modKitDescription.ID} i:{myId}");
        //                //NativeFunction.Natives.SET_VEHICLE_MOD(ModdingVehicle.Vehicle, modKitDescription.ID, myId, false);
        //            };

        //            modKitTypeSubMenu.AddItem(modkitItem);
        //        }
        //    }
        //}
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


    public int GetModKitPrice(int modKitTypeID, int modKitValueID)
    {
        if(modKitValueID == -1)
        {
            return 0;
        }
        return 500;
    }

    //private void ResetModType(int modKitTypeID)
    //{
    //    if(CurrentVariation.VehicleMods == null || !CurrentVariation.VehicleMods.Any(x=> x.ID == modKitTypeID))
    //    {
    //        SetVehicleMod(modKitTypeID, -1, false);
    //        return;
    //    }
    //    VehicleMod currentMod = CurrentVariation.VehicleMods.FirstOrDefault(x => x.ID == modKitTypeID);
    //    if(currentMod == null)
    //    {
    //        SetVehicleMod(modKitTypeID, -1, false);
    //        return;
    //    }
    //    SetVehicleMod(modKitTypeID, currentMod.Output, false);
    //}



    //private string GetModItemName(int modKitTypeID, int modKitValueID)
    //{
    //    string modItemName;
    //    unsafe
    //    {
    //        IntPtr ptr2 = NativeFunction.CallByHash<IntPtr>(0x8935624F8C5592CC, ModdingVehicle.Vehicle, modKitTypeID, modKitValueID);
    //        modItemName = Marshal.PtrToStringAnsi(ptr2);
    //    }
    //    if (NativeFunction.CallByHash<bool>(0xAC09CA973C564252, modItemName)) // DOES_TEXT_LABEL_EXIST
    //    {
    //        // Retrieve the filename for the audio conversation
    //        IntPtr filenamePtr = NativeFunction.CallByHash<IntPtr>(0x7B5280EBA9840C72, modItemName); // GET_FILENAME_FOR_AUDIO_CONVERSATION
    //                                                                                                 // Convert the filenamePtr to a string and check if it's valid
    //        string filename = filenamePtr != IntPtr.Zero ? Marshal.PtrToStringAnsi(filenamePtr) : string.Empty;
    //        modItemName = filename;
    //    }
    //    if (string.IsNullOrEmpty(modItemName))
    //    {
    //        modItemName = $"Item {modKitValueID}";
    //    }
    //    return modItemName;
    //}
    //private void SetVehicleMod(int modKitTypeID, int modKitValueID, bool updateVariation)
    //{
    //    if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
    //    {
    //        return;
    //    }   
    //    if (updateVariation)
    //    {
    //        if (CurrentVariation == null)
    //        {
    //            return;
    //        }
    //        VehicleMod currentMod = CurrentVariation.VehicleMods.Where(x => x.ID == modKitTypeID).FirstOrDefault();
    //        if (currentMod != null)
    //        {
    //            currentMod.Output = modKitValueID;
    //        }
    //        else
    //        {
    //            CurrentVariation.VehicleMods.Add(new VehicleMod(modKitTypeID, modKitValueID));
    //        }
    //    }
    //    NativeFunction.Natives.SET_VEHICLE_MOD(ModdingVehicle.Vehicle, modKitTypeID, modKitValueID, false);
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
}

