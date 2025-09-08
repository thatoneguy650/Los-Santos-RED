using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

public enum WindowTints
{
    WINDOWTINT_NONE = 0,
    WINDOWTINT_LIMO = 1,
    WINDOWTINT_DARKSMOKE = 2,
    WINDOWTINT_LIGHTSMOKE = 3,
    WINDOWTINT_GREEN = 6
}

public class VehicleWindowTintMenu
{
    private readonly ILocationInteractable Player;
    private readonly UIMenu InteractionMenu;
    private readonly MenuPool MenuPool;
    private readonly ModShopMenu ModShopMenu;
    private readonly VehicleExt ModdingVehicle;
    private readonly VehicleVariation CurrentVariation;
    private readonly GameLocation GameLocation;
    private readonly List<TintMenuItem> TintMenuItems = new List<TintMenuItem>();
    private UIMenu tintMenu;
    private readonly List<VehicleTintLookup> VehicleTints = new List<VehicleTintLookup>
    {
        new VehicleTintLookup((int)WindowTints.WINDOWTINT_NONE, "None", 500),
        new VehicleTintLookup((int)WindowTints.WINDOWTINT_LIGHTSMOKE, "Light Smoke", 900),
        new VehicleTintLookup((int)WindowTints.WINDOWTINT_DARKSMOKE, "Dark Smoke", 1200),
        new VehicleTintLookup((int)WindowTints.WINDOWTINT_LIMO, "Limo", 1500),
        new VehicleTintLookup((int)WindowTints.WINDOWTINT_GREEN, "Green", 2000)
    };
    private int originalTint;

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
        if (ModdingVehicle?.Vehicle.Exists() != true || MenuPool == null || InteractionMenu == null)
            return;

        int vehicleTint = NativeFunction.Natives.GET_VEHICLE_WINDOW_TINT<int>(ModdingVehicle.Vehicle);
        if (vehicleTint == -1)
            vehicleTint = (int)WindowTints.WINDOWTINT_NONE;
        CurrentVariation.WindowTint = Enum.IsDefined(typeof(WindowTints), vehicleTint) ? vehicleTint : (int)WindowTints.WINDOWTINT_NONE;
        originalTint = CurrentVariation.WindowTint;
        CreateTintMenu();
    }

    private void CreateTintMenu()
    {
        if (MenuPool == null || InteractionMenu == null)
            return;

        tintMenu = MenuPool.AddSubMenu(InteractionMenu, "Window Tint");
        if (tintMenu == null)
            return;

        tintMenu.SubtitleText = "WINDOW TINTS";
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count - 1].Description = "Select Window Tints";

        TintMenuItems.Clear();
        int counter = 0;
        foreach (VehicleTintLookup tint in VehicleTints.OrderBy(x => x.Price))
        {
            UIMenuItem tintItem = new UIMenuItem(tint.TintName, $"Select {tint.TintName}");
            tintMenu.AddItem(tintItem);
            TintMenuItems.Add(new TintMenuItem(tintItem, tint.TintID, counter++));
        }

        tintMenu.OnMenuOpen += OnMenuOpen;
        tintMenu.OnIndexChange += OnIndexChange;
        tintMenu.OnItemSelect += OnItemSelect;
        tintMenu.OnMenuClose += OnMenuClose;
        UpdateTintMenuItems();
    }

    private void OnMenuOpen(UIMenu sender)
    {
        if (TintMenuItems.Count == 0)
            CreateTintMenu();

        UpdateTintMenuItems();
        if (sender.MenuItems.Count == 0)
            return;

        int currentTintIndex = TintMenuItems.FirstOrDefault(x => x.TintID == CurrentVariation.WindowTint)?.Index ?? 0;
        sender.CurrentSelection = currentTintIndex;
        sender.Visible = true;
        SetWindowTint(TintMenuItems[currentTintIndex].TintID, false);
    }

    private void OnIndexChange(UIMenu sender, int newIndex)
    {
        if (newIndex < 0 || newIndex >= sender.MenuItems.Count)
            return;

        TintMenuItem lookupResult = TintMenuItems.FirstOrDefault(x => x.UIMenuItem == sender.MenuItems[newIndex]);
        if (lookupResult != null)
            SetWindowTint(lookupResult.TintID, false);
    }

    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        TintMenuItem lookupResult = TintMenuItems.FirstOrDefault(x => x.UIMenuItem == selectedItem);
        if (lookupResult == null || CurrentVariation.WindowTint == lookupResult.TintID)
        {
            if (CurrentVariation.WindowTint == lookupResult?.TintID)
                ModShopMenu.DisplayMessage("Already Set as Window Tint");
            return;
        }

        VehicleTintLookup tint = VehicleTints.FirstOrDefault(x => x.TintID == lookupResult.TintID);
        if (tint == null || !ChargeClient(tint.Price))
            return;

        SetWindowTint(lookupResult.TintID, true);
        UpdateTintMenuItems();
        EntryPoint.WriteToConsole($"VehicleWindowTintMenu.OnItemSelect: Applied TintID {lookupResult.TintID} for vehicle {ModdingVehicle.Vehicle.Model.Name}");
    }

    private void OnMenuClose(UIMenu sender)
    {
        ResetTints();
        if (tintMenu != null)
            tintMenu.Visible = false;
    }

    private void UpdateTintMenuItems()
    {
        if (TintMenuItems.Count == 0)
            return;

        foreach (TintMenuItem tintMenuItem in TintMenuItems)
        {
            VehicleTintLookup tint = VehicleTints.FirstOrDefault(x => x.TintID == tintMenuItem.TintID);
            if (tint == null)
                continue;

            tintMenuItem.UIMenuItem.RightLabel = tintMenuItem.TintID == CurrentVariation.WindowTint ? "" : $"~r~${tint.Price}~s~";
            tintMenuItem.UIMenuItem.RightBadge = tintMenuItem.TintID == CurrentVariation.WindowTint ? UIMenuItem.BadgeStyle.Tick : UIMenuItem.BadgeStyle.None;
        }

        if (tintMenu != null)
            tintMenu.RefreshIndex();
    }

    private void SetWindowTint(int tintID, bool setVariation)
    {
        if (ModdingVehicle?.Vehicle.Exists() != true || !Enum.IsDefined(typeof(WindowTints), tintID))
            return;

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
        if (ModdingVehicle?.Vehicle.Exists() != true || CurrentVariation == null)
            return;

        int resetTint = Enum.IsDefined(typeof(WindowTints), CurrentVariation.WindowTint) ? CurrentVariation.WindowTint : originalTint;
        NativeFunction.Natives.SET_VEHICLE_MOD_KIT(ModdingVehicle.Vehicle, 0);
        NativeFunction.Natives.SET_VEHICLE_WINDOW_TINT(ModdingVehicle.Vehicle, resetTint);
    }

    private bool ChargeClient(int price)
    {
        if (Player?.BankAccounts == null || Player.BankAccounts.GetMoney(true) < price)
        {
            ModShopMenu.DisplayInsufficientFundsMessage(price);
            return false;
        }

        ModShopMenu.DisplayPurchasedMessage(price);
        Player.BankAccounts.GiveMoney(-1 * price, true);
        return true;
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
        public int TintID { get; }
        public string TintName { get; }
        public int Price { get; }

        public VehicleTintLookup(int tintID, string tintName, int price)
        {
            TintID = tintID;
            TintName = tintName;
            Price = price;
        }
    }
}