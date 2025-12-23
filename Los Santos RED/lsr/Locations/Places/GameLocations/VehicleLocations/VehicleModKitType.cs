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

        if (TotalMods <= 0)
        {
            return;
        }

        UIMenu modKitTypeSubMenu = MenuPool.AddSubMenu(InteractionMenu, TypeName);
        modKitTypeSubMenu.SetBannerType(EntryPoint.LSRedColor);

        UIMenuItem modKitTypeSubMenuItem = InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count - 1];
        modKitTypeSubMenuItem.Description = !string.IsNullOrEmpty(Description) ? Description : TypeName;

        // Compute installed mod once
        VehicleMod installedMod = CurrentVariation?.VehicleMods?.FirstOrDefault(x => x.ID == TypeID);

        int counter = 0;
        MenuItems.Clear(); // safety

        for (int i = -1; i < TotalMods; i++)
        {
            int myId = i;
            string modItemName = GetModItemName(myId);

            if (myId == -1) modItemName = "Stock";

            UIMenuItem modkitItem = new UIMenuItem(modItemName, "Select to install this modification");

            bool isInstalled = installedMod != null && installedMod.Output == myId;
            int price = isInstalled ? 0 : GetPrice(TypeID, myId);

            modkitItem.RightLabel = isInstalled ? "" : $"~r~${price}~s~";
            modkitItem.RightBadge = isInstalled ? UIMenuItem.BadgeStyle.Tick : UIMenuItem.BadgeStyle.None;

            modkitItem.Activated += (sender, e) =>
            {
                if (isInstalled)
                {
                    ModShopMenu.DisplayMessage("You already have this mod installed");
                    return;
                }

                if (Player.BankAccounts.GetMoney(true) < price)
                {
                    ModShopMenu.DisplayInsufficientFundsMessage(price);
                    return;
                }

                ModShopMenu.DisplayPurchasedMessage(price);
                Player.BankAccounts.GiveMoney(-price, true);

                SetVehicleMod(myId, true);
                SyncMenuItems(); // update all badges/prices instantly
            };

            MenuItems.Add(new ModKitMenuItem(modkitItem, myId, counter));
            modKitTypeSubMenu.AddItem(modkitItem);
            counter++;
        }

        // Preview handlers
        modKitTypeSubMenu.OnMenuOpen += (sender) =>
        {
            var item = MenuItems.FirstOrDefault(x => x.Index == modKitTypeSubMenu.CurrentSelection);
            if (item != null) SetVehicleMod(item.ID, false);
        };

        modKitTypeSubMenu.OnMenuClose += (sender) => ResetModType();

        modKitTypeSubMenu.OnIndexChange += (sender, newIndex) =>
        {
            var item = MenuItems.FirstOrDefault(x => x.Index == newIndex);
            if (item != null) SetVehicleMod(item.ID, false);
        };
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




