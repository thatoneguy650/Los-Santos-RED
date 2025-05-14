using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
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

    public ModShopMenu(ILocationInteractable player, MenuPool menuPool, UIMenu interactionMenu, VehicleModShop vehicleModShop, int maxRepairCost, int repairHours, int washCost, int washHours)
    {
        Player = player;
        MenuPool = menuPool;
        InteractionMenu = interactionMenu;
        GameLocation = vehicleModShop;
        MaxRepairCost = maxRepairCost;
        RepairHours = repairHours;
        WashCost = washCost;
        WashHours = washHours;
    }



    public void CreateMenu()
    {
        if(Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        if(Player.CurrentVehicle.Vehicle.Health < Player.CurrentVehicle.Vehicle.MaxHealth)
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

        NativeFunction.Natives.SET_VEHICLE_MOD_KIT(Player.CurrentVehicle.Vehicle, 0);

        foreach (ModKitDescription modKitDescription in ModKitDescriptions.OrderBy(x=> x.Name))
        {
            int TotalMods = NativeFunction.Natives.GET_NUM_VEHICLE_MODS<int>(Player.CurrentVehicle.Vehicle, modKitDescription.ID);
            EntryPoint.WriteToConsole($"{modKitDescription.Name}ID:{modKitDescription.ID}TotalMods {TotalMods}");
            if (TotalMods > 0)
            {
                UIMenu subMenu = MenuPool.AddSubMenu(InteractionMenu, modKitDescription.Name);
                for (int i = 0; i < TotalMods; i++)
                {
                    string modItemName;
                    unsafe
                    {
                        IntPtr ptr2 = NativeFunction.CallByHash<IntPtr>(0x8935624F8C5592CC, Player.CurrentVehicle.Vehicle,modKitDescription.ID,i);
                        modItemName = Marshal.PtrToStringAnsi(ptr2);
                    }
                    if (NativeFunction.CallByHash<bool>(0xAC09CA973C564252, modItemName)) // DOES_TEXT_LABEL_EXIST
                    {
                        // Retrieve the filename for the audio conversation
                        IntPtr filenamePtr = NativeFunction.CallByHash<IntPtr>(0x7B5280EBA9840C72, modItemName); // GET_FILENAME_FOR_AUDIO_CONVERSATION
                        // Convert the filenamePtr to a string and check if it's valid
                        string filename = filenamePtr != IntPtr.Zero ? Marshal.PtrToStringAnsi(filenamePtr) : string.Empty;
                        modItemName = filename;
                    }
                    if (string.IsNullOrEmpty(modItemName))
                    {
                        modItemName = $"Item {i}";
                    }
                    UIMenuItem modkitItem = new UIMenuItem($"{modItemName}", "Description");
                    int myId = i;
                    modkitItem.Activated += (sender, e) =>
                    {
                        
                        if(Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
                        {
                            return;
                        }
                        EntryPoint.WriteToConsole($"APPLY MODKIT modKitDescription.ID:{modKitDescription.ID} i:{myId}");
                        NativeFunction.Natives.SET_VEHICLE_MOD(Player.CurrentVehicle.Vehicle, modKitDescription.ID, myId, false);
                    };

                    subMenu.AddItem(modkitItem);
                }
            }
        }
    }

    private void AddRepairItem()
    {
        FinalRepairCost = MaxRepairCost;
        int CurrentHealth = Player.CurrentVehicle.Vehicle.Health;
        int MaxHealth = Player.CurrentVehicle.Vehicle.MaxHealth;
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
        if (!Player.IsInVehicle || Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        if (Player.BankAccounts.GetMoney(false) <= totalRepairCost)
        {
            NativeHelper.PlayErrorSound();
            GameLocation?.DisplayMessage("~r~Cash Only", "You do not have enough cash on hand.");
            return;
        }
        Player.CurrentVehicle.Engine.SetState(false);
        RepairVehicle();
        Player.BankAccounts.GiveMoney(-1 * totalRepairCost, false);
        NativeHelper.PlaySuccessSound();
        GameLocation?.DisplayMessage("~g~Repaired", $"Thank you for fixing your vehicle at ~y~~s~");
    }
    private void RepairVehicle()
    {
        if (!Player.IsInVehicle || Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        Player.CurrentVehicle.Vehicle.Repair();
        Player.CurrentVehicle.Vehicle.Wash();
    }
}

