using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;
using System.Linq;

public class VehicleNeonMenu
{
    private readonly ILocationInteractable Player;
    private readonly UIMenu InteractionMenu;
    private readonly MenuPool MenuPool;
    private readonly ModShopMenu ModShopMenu;
    private readonly VehicleExt ModdingVehicle;
    private readonly VehicleVariation CurrentVariation;
    private readonly GameLocation GameLocation;
    private readonly List<NeonMenuItem> NeonMenuItems = new List<NeonMenuItem>();
    private readonly bool[] originalNeonStates = new bool[4];
    private UIMenu neonMenu;
    private readonly List<VehicleNeonLookup> VehicleNeons = new List<VehicleNeonLookup>
    {
        //new VehicleNeonLookup(0, "Left Neon", 1000),
        //new VehicleNeonLookup(1, "Right Neon", 1000),
        new VehicleNeonLookup(2, "Front", 1000),
        new VehicleNeonLookup(3, "Back", 1000),
        new VehicleNeonLookup(11, "Sides", 2000, new List<int> { 0, 1 }),
        new VehicleNeonLookup(12, "All Around", 4000, new List<int> { 0, 1, 2, 3 })
    };
    private const int RemoveSinglePrice = 500;
    private const int RemoveSidesPrice = 1000;
    private const int RemoveAllPrice = 2000;

    public VehicleNeonMenu(MenuPool menuPool, UIMenu interactionMenu, ILocationInteractable player, VehicleExt moddingVehicle, ModShopMenu modShopMenu, VehicleVariation currentVariation, GameLocation gameLocation)
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
        if (ModdingVehicle?.Vehicle.Exists() != true || MenuPool == null || InteractionMenu == null) return;
        CreateNeonMenu();
    }

    private void CreateNeonMenu()
    {
        neonMenu = MenuPool.AddSubMenu(InteractionMenu, "Neon Lights");
        neonMenu.SetBannerType(EntryPoint.LSRedColor);
        neonMenu.SubtitleText = "Vehicle Neon Lights";
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count - 1].Description = "Toggle, buy or remove neon lights";

        NeonMenuItems.Clear();
        int index = 0;
        foreach (VehicleNeonLookup neon in VehicleNeons)
        {
            UIMenuItem item = new UIMenuItem(neon.Name, $"Enable, purchase or remove {neon.Name}") { RightLabel = "~r~$" + neon.Price + "~s~" };
            neonMenu.AddItem(item);
            NeonMenuItems.Add(new NeonMenuItem(item, neon.ID, index++));
        }

        neonMenu.OnMenuOpen += OnMenuOpen;
        neonMenu.OnIndexChange += OnIndexChange;
        neonMenu.OnItemSelect += OnItemSelect;
        UpdateNeonMenuItems();
    }

    private void OnMenuOpen(UIMenu sender)
    {
        if (ModdingVehicle?.Vehicle == null || !ModdingVehicle.Vehicle.Exists()) return;

        UpdateNeonMenuItems();

        bool hasNeons = CurrentVariation.VehicleNeons.Any(n => n.IsEnabled);
        if (!hasNeons) return;

        bool anyOn = false;
        for (int i = 0; i < 4; i++)
        {
            try
            {
                bool enabled = NativeFunction.Natives.IS_VEHICLE_NEON_LIGHT_ENABLED<bool>(ModdingVehicle.Vehicle, i);
                originalNeonStates[i] = enabled;
                if (enabled) anyOn = true;
            }
            catch
            {
                originalNeonStates[i] = false;
            }
        }

        if (!anyOn)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    NativeFunction.Natives.SET_VEHICLE_NEON_LIGHT_ENABLED(ModdingVehicle.Vehicle, i, true);
                }
                catch { }
            }
        }
    }

    private void OnIndexChange(UIMenu sender, int newIndex) { }

    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        NeonMenuItem item = NeonMenuItems.Find(n => n.UIMenuItem == selectedItem);
        if (item == null) return;
        VehicleNeonLookup neonLookup = VehicleNeons.FirstOrDefault(n => n.ID == item.NeonID);
        if (neonLookup == null) return;

        List<int> idsToToggle = neonLookup.GroupIDs ?? new List<int> { neonLookup.ID };

        bool allEnabled = true;
        foreach (int id in idsToToggle)
        {
            VehicleNeon neon = CurrentVariation.VehicleNeons.Find(n => n.ID == id);
            if (neon == null || !neon.IsEnabled)
            {
                allEnabled = false;
                break;
            }
        }

        bool wasFirstInstall = !CurrentVariation.VehicleNeons.Any(n => n.IsEnabled) && !allEnabled;

        if (allEnabled)
        {
            int priceToRemove = (idsToToggle.Count == 4) ? RemoveAllPrice : RemoveSinglePrice;
            if (!ChargeClient(priceToRemove)) return;

            foreach (int id in idsToToggle)
            {
                VehicleNeon neon = CurrentVariation.VehicleNeons.Find(n => n.ID == id);
                if (neon != null) neon.IsEnabled = false;
            }
        }
        else
        {
            if (!ChargeClient(neonLookup.Price)) return;

            foreach (int id in idsToToggle)
            {
                VehicleNeon neon = CurrentVariation.VehicleNeons.Find(n => n.ID == id);
                if (neon == null)
                {
                    neon = new VehicleNeon { ID = id, IsEnabled = true };
                    CurrentVariation.VehicleNeons.Add(neon);
                }
                else
                {
                    neon.IsEnabled = true;
                }
            }

            // First neon install → set default white color
            if (wasFirstInstall)
            {
                CurrentVariation.NeonColorR = 255;
                CurrentVariation.NeonColorG = 255;
                CurrentVariation.NeonColorB = 255;
            }
        }

        CurrentVariation.Apply(ModdingVehicle);
        UpdateNeonMenuItems();
        // Switching neon states affected Xenon lights, So re-applying Xenon state
        // Preserve Xenon toggle state
        bool hasXenons = CurrentVariation.VehicleMods.Any(m => m.ID == 22 && m.Output == 1);
        try { NativeFunction.Natives.TOGGLE_VEHICLE_MOD(ModdingVehicle.Vehicle, 22, hasXenons); }
        catch { }

        // Preserve Xenon color if installed
        if (hasXenons && CurrentVariation.XenonLightColor >= 0)
        {
            try { NativeFunction.Natives.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX(ModdingVehicle.Vehicle, CurrentVariation.XenonLightColor); }
            catch { }
        }
    }

    private void UpdateNeonMenuItems()
    {
        foreach (NeonMenuItem item in NeonMenuItems)
        {
            VehicleNeonLookup lookup = VehicleNeons[item.Index];
            List<int> ids = lookup.GroupIDs ?? new List<int> { lookup.ID };

            bool allEnabled = true;
            foreach (int id in ids)
            {
                VehicleNeon neon = CurrentVariation.VehicleNeons.Find(n => n.ID == id);
                if (neon == null || !neon.IsEnabled)
                {
                    allEnabled = false;
                    break;
                }
            }

            item.UIMenuItem.RightBadge = allEnabled ? UIMenuItem.BadgeStyle.Tick : UIMenuItem.BadgeStyle.None;

            int removePrice;
            if (ids.Count == 4)
                removePrice = RemoveAllPrice;
            else if (ids.Count == 2)
                removePrice = RemoveSidesPrice;
            else
                removePrice = RemoveSinglePrice;

            if (allEnabled)
            {
                item.UIMenuItem.RightLabel = "~r~$" + removePrice + " Remove~s~";
            }
            else
            {
                // Calculate discount for "All Around" based on already installed neons
                int price = lookup.Price;
                if (lookup.ID == 12) // All Around
                {
                    int installedCount = CurrentVariation.VehicleNeons.Count(n => n.IsEnabled);
                    price = 4000 - (installedCount * 1000);
                    if (price < 1000) price = 1000; // optional minimum
                }
                item.UIMenuItem.RightLabel = "~r~$" + price + "~s~";
            }
        }
        neonMenu?.RefreshIndex();
    }

    private bool ChargeClient(int price)
    {
        if (Player?.BankAccounts == null || Player.BankAccounts.GetMoney(true) < price)
        {
            ModShopMenu.DisplayInsufficientFundsMessage(price);
            return false;
        }
        Player.BankAccounts.GiveMoney(-price, true);
        ModShopMenu.DisplayPurchasedMessage(price);
        return true;
    }

    private class NeonMenuItem
    {
        public UIMenuItem UIMenuItem { get; set; }
        public int NeonID { get; set; }
        public int Index { get; set; }
        public NeonMenuItem(UIMenuItem uiMenuItem, int neonID, int index)
        {
            UIMenuItem = uiMenuItem;
            NeonID = neonID;
            Index = index;
        }
    }

    private class VehicleNeonLookup
    {
        public int ID { get; }
        public string Name { get; }
        public int Price { get; }
        public List<int> GroupIDs { get; }
        public VehicleNeonLookup(int id, string name, int price, List<int> groupIDs = null)
        {
            ID = id;
            Name = name;
            Price = price;
            GroupIDs = groupIDs;
        }
    }
}