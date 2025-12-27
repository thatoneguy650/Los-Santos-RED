using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Linq;

public class VehicleModToggle : VehicleModKitType
{
    public VehicleModToggle(
        string name,
        int typeID,
        ModShopMenu modShopMenu,
        UIMenu interactionMenu,
        MenuPool menuPool,
        ILocationInteractable player)
        : base(name, typeID, modShopMenu, interactionMenu, menuPool, player)
    {
    }

    public override void AddToMenu()
    {
        if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
            return;

        NativeFunction.Natives.SET_VEHICLE_MOD_KIT(ModdingVehicle.Vehicle, 0);

        bool isOn = NativeFunction.Natives.IS_TOGGLE_MOD_ON<bool>(ModdingVehicle.Vehicle, TypeID);

        UIMenu menu = MenuPool.AddSubMenu(InteractionMenu, TypeName);
        menu.SetBannerType(EntryPoint.LSRedColor);

        InteractionMenu.MenuItems.Last().Description = $"Toggle {TypeName}";

        UIMenuItem offItem = new UIMenuItem("None");
        UIMenuItem onItem = new UIMenuItem("Installed");

        menu.AddItem(offItem);
        menu.AddItem(onItem);

        UpdateUI(isOn);

        menu.OnItemSelect += (sender, selectedItem, index) =>
        {
            bool wantOn = selectedItem == onItem;
            if (wantOn == isOn) return;

            int price = GetPrice(TypeID, wantOn ? 1 : 0);
            if (Player.BankAccounts.GetMoney(true) < price)
            {
                ModShopMenu.DisplayInsufficientFundsMessage(price);
                return;
            }

            ModShopMenu.DisplayPurchasedMessage(price);
            Player.BankAccounts.GiveMoney(-price, true);

            NativeFunction.Natives.TOGGLE_VEHICLE_MOD(ModdingVehicle.Vehicle, TypeID, wantOn);

            VehicleMod mod = CurrentVariation.VehicleMods.FirstOrDefault(x => x.ID == TypeID);
            if (mod != null)
                mod.Output = wantOn ? 1 : -1;
            else
                CurrentVariation.VehicleMods.Add(new VehicleMod(TypeID, wantOn ? 1 : -1));

            isOn = wantOn;
            UpdateUI(isOn);

            if (TypeID == 22) // Xenon Lights
            {
                if (wantOn)
                {
                    if (CurrentVariation.XenonLightColor < 0)
                    {
                        CurrentVariation.XenonLightColor = 0; // Default white
                        try { NativeFunction.Natives.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX(ModdingVehicle.Vehicle, 0); }
                        catch { }
                    }
                }
                else
                {
                    CurrentVariation.XenonLightColor = -1; // Reset on uninstall
                }
            }
        };

        menu.OnMenuClose += (sender) =>
        {
            NativeFunction.Natives.TOGGLE_VEHICLE_MOD(ModdingVehicle.Vehicle, TypeID, isOn);
        };

        void UpdateUI(bool currentState)
        {
            int price = GetPrice(TypeID, currentState ? 0 : 1);

            if (currentState)
            {
                onItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
                offItem.RightBadge = UIMenuItem.BadgeStyle.None;
                offItem.RightLabel = $"~r~${price}~s~";
                onItem.RightLabel = "";
            }
            else
            {
                offItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
                onItem.RightBadge = UIMenuItem.BadgeStyle.None;
                onItem.RightLabel = $"~r~${price}~s~";
                offItem.RightLabel = "";
            }
        }
    }

    protected override int GetPrice(int modType, int value)
    {
        if (TypeName == "Turbo")
            return value == 1 ? 30000 : 12000;
        if (TypeName == "Xenon Lights")
            return value == 1 ? 7500 : 3000;

        // Default for other toggles
        return value == 1 ? 5000 : 1000;
    }
}