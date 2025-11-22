using Rage.Native;
using RAGENativeUI.Elements;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using LSR.Vehicles;
using LosSantosRED.lsr.Interface;
using System.Windows.Forms;


public class VehicleModKitType
{
    protected VehicleVariation CurrentVariation => ModShopMenu.CurrentVariation;
    protected VehicleExt ModdingVehicle => ModShopMenu.ModdingVehicle;
    protected UIMenu InteractionMenu;
    protected MenuPool MenuPool;
    protected ModShopMenu ModShopMenu;
    protected ILocationInteractable Player;
    protected List<ModKitMenuItem> MenuItems = new List<ModKitMenuItem>();
    public VehicleModKitType(string name, int id, ModShopMenu modShopMenu, UIMenu interactionMenu, MenuPool menuPool, ILocationInteractable player)
    {
        TypeName = name;
        TypeID = id;
        ModShopMenu = modShopMenu;
        MenuPool = menuPool;
        InteractionMenu = interactionMenu;
        Player = player;
    }
    public VehicleModKitType(string name, int id, string description, ModShopMenu modShopMenu, UIMenu interactionMenu, MenuPool menuPool, ILocationInteractable player)
    {
        TypeName = name;
        TypeID = id;
        ModShopMenu = modShopMenu;
        MenuPool = menuPool;
        InteractionMenu = interactionMenu;
        Player = player;
        Description = description;
    }
    public string TypeName { get; }
    public int TypeID { get; }
    public string Description { get; }
    public virtual void AddToMenu()
    {
        if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
        {
            return;
        }
        int TotalMods = NativeFunction.Natives.GET_NUM_VEHICLE_MODS<int>(ModdingVehicle.Vehicle, TypeID);
        EntryPoint.WriteToConsole($"{TypeName}ID:{TypeID}TotalMods {TotalMods}");
        if (TotalMods > 0)
        {
            UIMenu modKitTypeSubMenu = MenuPool.AddSubMenu(InteractionMenu, TypeName);
            UIMenuItem modKitTypeSubMenuItem = InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1];

            if (!string.IsNullOrEmpty(Description))
            {
                modKitTypeSubMenuItem.Description = Description;
            }
            else
            {
                modKitTypeSubMenuItem.Description = TypeName;
            }


            modKitTypeSubMenu.OnMenuOpen += (sender) =>
            {
                ModKitMenuItem modKitMenuItem = MenuItems.Where(x => x.Index == modKitTypeSubMenu.CurrentSelection).FirstOrDefault();
                if (modKitMenuItem == null)
                {
                    return;
                }
                ResetModType();
                SetVehicleMod(modKitMenuItem.ID, false);
            };
            modKitTypeSubMenu.OnMenuClose += (sender) =>
            {
                ResetModType();
            };
            modKitTypeSubMenu.OnIndexChange += (sender, newIndex) =>
            {
                ModKitMenuItem modKitMenuItem = MenuItems.Where(x => x.Index == newIndex).FirstOrDefault();
                if(modKitMenuItem == null)
                {
                    EntryPoint.WriteToConsole($"OnIndexChange NEWINDEX:{newIndex}");
                    return;
                }
                SetVehicleMod(modKitMenuItem.ID, false);
            };
            int counter = 0;
            for (int i = -1; i < TotalMods; i++)
            {
                int myId = i;
                
                string modItemName = GetModItemName(myId);
                UIMenuItem modkitItem = new UIMenuItem($"{modItemName}", "Description");
                MenuItems.Add(new ModKitMenuItem(modkitItem, myId, counter));
                counter++;
                bool hasModInstalled = false;



                VehicleMod existingMod = CurrentVariation.VehicleMods?.Where(x => x.ID == TypeID).FirstOrDefault();
                if (existingMod != null && existingMod.Output == myId)
                {
                    hasModInstalled = true;
                    EntryPoint.WriteToConsole($"YOU HAVE INSTALLED typeID:{TypeID} valueID:{myId}");
                }




                int modKitPrice = GetPrice(TypeID, myId);
                modkitItem.RightLabel = $"~r~${modKitPrice}~s~";
                modkitItem.RightBadge = UIMenuItem.BadgeStyle.None;

                if (hasModInstalled)
                {
                    modkitItem.RightLabel = "";
                    modkitItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
                }
                
                modkitItem.Activated += (sender, e) =>
                {
                    VehicleMod existingMod2 = CurrentVariation.VehicleMods?.Where(x => x.ID == TypeID).FirstOrDefault();
                    if (existingMod2 != null && existingMod2.Output == myId)
                    {
                        ModShopMenu.DisplayMessage("You already have this mod installed");
                        return;
                    }
                    if (Player.BankAccounts.GetMoney(true) < modKitPrice)
                    {
                        ModShopMenu.DisplayInsufficientFundsMessage(modKitPrice);
                        return;
                    }
                    ModShopMenu.DisplayPurchasedMessage(modKitPrice);
                    Player.BankAccounts.GiveMoney(-1 * modKitPrice, true);

                    //apply the modkit permanently
                    if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
                    {
                        return;
                    }
                    SetVehicleMod(myId, true);
                    //modkitItem.RightLabel = "";
                    //modkitItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
                    EntryPoint.WriteToConsole($"APPLY MODKIT modKitDescription.ID:{TypeID} i:{myId}");
                    //NativeFunction.Natives.SET_VEHICLE_MOD(ModdingVehicle.Vehicle, modKitDescription.ID, myId, false);
                };
                modKitTypeSubMenu.AddItem(modkitItem);
            }
        }
    }
    protected virtual void ResetModType()
    {
        //SyncMenuItems();
        if (CurrentVariation.VehicleMods == null || !CurrentVariation.VehicleMods.Any(x => x.ID == TypeID))
        {
            SetVehicleMod(-1, false);
            return;
        }
        VehicleMod currentMod = CurrentVariation.VehicleMods.FirstOrDefault(x => x.ID == TypeID);
        if (currentMod == null)
        {
            SetVehicleMod(-1, false);
            return;
        }
        SetVehicleMod(currentMod.Output, false);

    }
    protected virtual void SetVehicleMod(int modKitValueID, bool updateVariation)
    {
        if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
        {
            return;
        }
        if (updateVariation)
        {
            if (CurrentVariation == null)
            {
                return;
            }
            VehicleMod currentMod = CurrentVariation.VehicleMods.Where(x => x.ID == TypeID).FirstOrDefault();
            if (currentMod != null)
            {
                currentMod.Output = modKitValueID;
            }
            else
            {
                CurrentVariation.VehicleMods.Add(new VehicleMod(TypeID, modKitValueID));
            }
            SyncMenuItems();
        }
        NativeFunction.Natives.SET_VEHICLE_MOD(ModdingVehicle.Vehicle, TypeID, modKitValueID, false);
        EntryPoint.WriteToConsole($"SetVehicleMod RAN {TypeID} {modKitValueID}");
    }
    protected void SyncMenuItems()
    {
        VehicleMod currentMod = CurrentVariation.VehicleMods.Where(x => x.ID == TypeID).FirstOrDefault();
        foreach (ModKitMenuItem item in MenuItems)
        {
            EntryPoint.WriteToConsole($"ID:{item.ID} TEXT:{item.UIMenuItem.Text} {item.Index}");
            if (currentMod != null && currentMod.Output == item.ID)
            {
                EntryPoint.WriteToConsole($"SyncMenuItems TypeID{TypeID} modkitItemID{item.ID}");
                item.UIMenuItem.RightLabel = "";
                item.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
            }
            else
            {
                item.UIMenuItem.RightLabel = $"~r~${GetPrice(TypeID, item.ID)}~s~";
                item.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.None;
            }
        }
    }
    protected virtual string GetModItemName(int modKitValueID)
    {
        if (modKitValueID == -1)
        {
            EntryPoint.WriteToConsole($"GetModItemName: {modKitValueID} IS NONE");
            return "None";
        }

        try
        {
            // Safely get the mod text label from Rockstar
            string label = NativeFunction.Natives.GET_MOD_TEXT_LABEL<string>(
                ModdingVehicle.Vehicle,
                TypeID,
                modKitValueID
            );

            if (!string.IsNullOrEmpty(label))
            {
                // Check if the text label exists
                bool exists = NativeFunction.Natives.DOES_TEXT_LABEL_EXIST<bool>(label);
                if (exists)
                {
                    // Rockstar stores many mod names in the audio lookup dictionary
                    string display = NativeFunction.Natives.GET_FILENAME_FOR_AUDIO_CONVERSATION<string>(label);

                    if (!string.IsNullOrEmpty(display))
                    {
                        return display;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"Safe GetModItemName error: {ex.Message}");
        }

        // Fallback name
        return $"Item {modKitValueID}";
    }


    protected virtual int GetPrice(int ModKitTypeID, int ModKitTypeValueID)
    {
        if(ModShopMenu == null)
        {
            return 500;
        }
        if(ModShopMenu.VehicleVariationShopMenu == null)
        {
            return ModShopMenu.DefaultPrice + (ModShopMenu.DefaultPriceScalar * ModKitTypeValueID);
        }
        VehicleModKitShopMenuItem vmksmi = ModShopMenu.VehicleVariationShopMenu.VehicleModKitShopMenuItems.Where(x=> x.ModTypeID == ModKitTypeID).FirstOrDefault();
        if(vmksmi == null)
        {
            return ModShopMenu.DefaultPrice + (ModShopMenu.DefaultPriceScalar * ModKitTypeValueID);
        }
        return vmksmi.BasePrice + (vmksmi.PriceScale * ModKitTypeValueID);
    }
    protected class ModKitMenuItem
    {
        public ModKitMenuItem(UIMenuItem uIMenuItem, int iD, int index)
        {
            UIMenuItem = uIMenuItem;
            ID = iD;
            Index = index;
        }

        public UIMenuItem UIMenuItem { get; set; }
        public int ID { get; set; }
        public int Index { get; set; }
    }

}




