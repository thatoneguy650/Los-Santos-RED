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

        var menu = MenuPool.AddSubMenu(InteractionMenu, TypeName);
        InteractionMenu.MenuItems.Last().Description = $"Toggle {TypeName}";

        var offItem = new UIMenuItem("None");
        var onItem = new UIMenuItem("Installed");

        menu.AddItem(offItem);
        menu.AddItem(onItem);

        RefreshDisplay();

        menu.OnItemSelect += (sender, item, index) =>
        {
            bool wantOn = item == onItem;
            if (wantOn == isOn)
                return;

            int actionPrice = GetPrice(TypeID, wantOn ? 1 : 0);

            if (Player.BankAccounts.GetMoney(true) < actionPrice)
            {
                ModShopMenu.DisplayInsufficientFundsMessage(actionPrice);
                return;
            }

            Player.BankAccounts.GiveMoney(-actionPrice, true);
            ModShopMenu.DisplayPurchasedMessage(actionPrice);

            NativeFunction.Natives.TOGGLE_VEHICLE_MOD(ModdingVehicle.Vehicle, TypeID, wantOn);

            var mod = CurrentVariation.VehicleMods.FirstOrDefault(x => x.ID == TypeID);
            if (mod != null)
                mod.Output = wantOn ? 1 : -1;
            else
                CurrentVariation.VehicleMods.Add(new VehicleMod(TypeID, wantOn ? 1 : -1));

            isOn = wantOn;
            RefreshDisplay();
        };

        void RefreshDisplay()
        {
            int displayPrice = GetPrice(TypeID, isOn ? 0 : 1);
            if (isOn)
            {
                onItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
                offItem.RightBadge = UIMenuItem.BadgeStyle.None;
                offItem.RightLabel = $"~r~${displayPrice}~s~";
                onItem.RightLabel = "";
            }
            else
            {
                offItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
                onItem.RightBadge = UIMenuItem.BadgeStyle.None;
                onItem.RightLabel = $"~r~${displayPrice}~s~";
                offItem.RightLabel = "";
            }

            menu.RefreshIndex();
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