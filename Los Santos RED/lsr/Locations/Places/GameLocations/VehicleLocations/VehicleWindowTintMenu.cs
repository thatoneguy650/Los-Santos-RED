using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

public class VehicleWindowTintMenu
{
    private ILocationInteractable Player;
    private UIMenu InteractionMenu;
    private MenuPool MenuPool;
    private ModShopMenu ModShopMenu;
    private VehicleExt ModdingVehicle;
    private VehicleVariation CurrentVariation;
    private GameLocation GameLocation;
    private List<TintMenuItem> TintMenuItems = new List<TintMenuItem>();
    private UIMenu tintFullMenu;
    private UIMenu tintGroupMenu;
    private List<VehicleTintLookup> VehicleTints;

    public VehicleWindowTintMenu(MenuPool menuPool, UIMenu interactionMenu, ILocationInteractable player, VehicleExt moddingVehicle, ModShopMenu modShopMenu, VehicleVariation currentVariation, GameLocation gameLocation)
    {
        MenuPool = menuPool;
        InteractionMenu = interactionMenu;
        Player = player;
        ModdingVehicle = moddingVehicle;
        ModShopMenu = modShopMenu;
        CurrentVariation = currentVariation;
        GameLocation = gameLocation;
    }

    public void Setup()
    {
        if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
        {
            return;
        }

        VehicleTints = new List<VehicleTintLookup>()
        {
            new VehicleTintLookup(0, "None", "Standard", 500),
            new VehicleTintLookup(4, "Stock", "Standard", 600),
            new VehicleTintLookup(3, "Light Smoke", "Standard", 900),
            new VehicleTintLookup(2, "Dark Smoke", "Standard", 1200),
            new VehicleTintLookup(1, "Pure Black", "Standard", 1500),
            new VehicleTintLookup(5, "Limo", "Special", 1000),
            new VehicleTintLookup(6, "Green", "Special", 1100)
        };

        CreateTintMenu();
    }

    private void CreateTintMenu()
    {
        tintFullMenu = MenuPool.AddSubMenu(InteractionMenu, "Window Tint");
        tintFullMenu.SubtitleText = "WINDOW TINTS";
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count - 1].Description = "Pick Window Tints";

        foreach (string tintGroup in VehicleTints.GroupBy(x => x.TintGroup).Select(x => x.Key).Distinct().OrderBy(x => x))
        {
            SetupTintGroup(tintGroup);
        }
    }

    private void SetupTintGroup(string tintGroup)
    {
        tintGroupMenu = MenuPool.AddSubMenu(tintFullMenu, tintGroup);
        tintGroupMenu.SubtitleText = "WINDOW TINTS";
        tintFullMenu.MenuItems[tintFullMenu.MenuItems.Count - 1].Description = "Choose a tint group";

        tintGroupMenu.OnMenuOpen += (sender) =>
        {
            ResetTints();
            if (sender.CurrentSelection == -1)
            {
                return;
            }
            UIMenuItem selectedItem = sender.MenuItems[sender.CurrentSelection];
            if (selectedItem == null)
            {
                return;
            }
            TintMenuItem lookupResult = TintMenuItems.Where(x => x.UIMenuItem == selectedItem).FirstOrDefault();
            if (lookupResult == null)
            {
                return;
            }
            SetWindowTint(lookupResult.TintID, false);
        };

        tintGroupMenu.OnMenuClose += (sender) =>
        {
            ResetTints();
        };

        tintGroupMenu.OnIndexChange += (sender, newIndex) =>
        {
            if (newIndex == -1)
            {
                return;
            }
            UIMenuItem selectedItem = sender.MenuItems[newIndex];
            if (selectedItem == null)
            {
                return;
            }
            TintMenuItem lookupResult = TintMenuItems.Where(x => x.UIMenuItem == selectedItem).FirstOrDefault();
            if (lookupResult == null)
            {
                return;
            }
            SetWindowTint(lookupResult.TintID, false);
        };

        int counter = 0;
        foreach (VehicleTintLookup tint in VehicleTints.Where(x => x.TintGroup == tintGroup))
        {
            UIMenuItem tintItem = new UIMenuItem(tint.TintName, $"Select {tint.TintName}");
            TintMenuItems.Add(new TintMenuItem(tintItem, tint.TintID, counter));
            bool isSelected = CurrentVariation.WindowTint == tint.TintID;
            if (isSelected)
            {
                tintItem.RightLabel = "";
                tintItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
            }
            else
            {
                tintItem.RightLabel = $"~r~${tint.Price}~s~";
                tintItem.RightBadge = UIMenuItem.BadgeStyle.None;
            }

            tintItem.Activated += (sender, selectedItem) =>
            {
                TintMenuItem lookupResult = TintMenuItems.Where(x => x.UIMenuItem == selectedItem).FirstOrDefault();
                if (lookupResult == null)
                {
                    return;
                }
                if (CurrentVariation.WindowTint == lookupResult.TintID)
                {
                    DisplayMessage("Already Set as Window Tint");
                    return;
                }
                if (!ChargeClient(tint.Price))
                {
                    return;
                }
                SetWindowTint(lookupResult.TintID, true);
                SyncTints(lookupResult.TintID);
            };

            tintGroupMenu.AddItem(tintItem);
            counter++;
        }
    }

    private void SetWindowTint(int tintID, bool setVariation)
    {
        if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
        {
            return;
        }
        if (setVariation)
        {
            CurrentVariation.WindowTint = tintID;
            CurrentVariation.Apply(ModdingVehicle);
        }
        NativeFunction.Natives.SET_VEHICLE_MOD_KIT(ModdingVehicle.Vehicle, 0);
        NativeFunction.Natives.SET_VEHICLE_WINDOW_TINT(ModdingVehicle.Vehicle, tintID);
    }

    private void ResetTints()
    {
        if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists() || CurrentVariation == null)
        {
            return;
        }
        NativeFunction.Natives.SET_VEHICLE_MOD_KIT(ModdingVehicle.Vehicle, 0);
        NativeFunction.Natives.SET_VEHICLE_WINDOW_TINT(ModdingVehicle.Vehicle, CurrentVariation.WindowTint);
    }

    private void SyncTints(int tintID)
    {
        foreach (TintMenuItem tintMenuItem in TintMenuItems)
        {
            VehicleTintLookup tint = VehicleTints.FirstOrDefault(x => x.TintID == tintMenuItem.TintID);
            if (tint == null)
            {
                continue;
            }
            if (tintMenuItem.TintID == tintID)
            {
                tintMenuItem.UIMenuItem.RightLabel = "";
                tintMenuItem.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
            }
            else
            {
                tintMenuItem.UIMenuItem.RightLabel = $"~r~${tint.Price}~s~";
                tintMenuItem.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.None;
            }
        }
    }

    private void DisplayMessage(string message)
    {
        if (GameLocation == null)
        {
            return;
        }
        GameLocation.PlaySuccessSound();
        GameLocation.DisplayMessage("Information", message);
    }

    private bool ChargeClient(int price)
    {
        if (Player.BankAccounts.GetMoney(true) < price)
        {
            DisplayNotEnoughFunds(price);
            return false;
        }
        DisplayPurchased(price);
        Player.BankAccounts.GiveMoney(-1 * price, true);
        return true;
    }

    private void DisplayNotEnoughFunds(int price)
    {
        if (GameLocation == null)
        {
            return;
        }
        GameLocation.PlayErrorSound();
        GameLocation.DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transaction, as you do not have the required funds");
    }

    private void DisplayPurchased(int price)
    {
        if (GameLocation == null)
        {
            return;
        }
        GameLocation.PlaySuccessSound();
        GameLocation.DisplayMessage("~g~Purchased", "Thank you for your purchase");
    }

    private class TintMenuItem
    {
        public UIMenuItem UIMenuItem { get; set; }
        public int TintID { get; set; }
        public int Index { get; set; }

        public TintMenuItem(UIMenuItem uiMenuItem, int tintID, int index)
        {
            UIMenuItem = uiMenuItem;
            TintID = tintID;
            Index = index;
        }
    }

    private class VehicleTintLookup
    {
        public VehicleTintLookup(int tintID, string tintName, string tintGroup, int price)
        {
            TintID = tintID;
            TintName = tintName;
            TintGroup = tintGroup;
            Price = price;
        }

        public int TintID { get; set; }
        public string TintName { get; set; }
        public string TintGroup { get; set; }
        public int Price { get; set; }
    }
}