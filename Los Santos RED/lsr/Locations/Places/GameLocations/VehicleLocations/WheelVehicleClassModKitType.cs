using LosSantosRED.lsr.Interface;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WheelVehicleClassModKitType : VehicleModKitType
{

    private UIMenu WheelClassSubMenu;

    public WheelVehicleClassModKitType(string wheelTypeName, int wheelTypeID,int modkitTypeID, ModShopMenu modShopMenu, UIMenu interactionMenu, MenuPool menuPool, ILocationInteractable player) : base(wheelTypeName, modkitTypeID, modShopMenu, interactionMenu, menuPool, player)
    {
        WheelTypeName = wheelTypeName;
        WheelTypeID = wheelTypeID;
    }

    public string WheelTypeName { get; set; }
    public int WheelTypeID { get; set; }

    public void AddWheelTypeMenus()
    {    
        AddToMenu();
    }
    protected override void ResetModType()
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
        NativeFunction.Natives.SET_VEHICLE_WHEEL_TYPE(ModdingVehicle.Vehicle, CurrentVariation.WheelType);
        SetVehicleMod(currentMod.Output, false);

    }
    protected override void SetVehicleMod(int modKitValueID, bool updateVariation)
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
            CurrentVariation.WheelType = WheelTypeID;
            SyncMenuItems();
        }
        NativeFunction.Natives.SET_VEHICLE_WHEEL_TYPE(ModdingVehicle.Vehicle, WheelTypeID);
        NativeFunction.Natives.SET_VEHICLE_MOD(ModdingVehicle.Vehicle, TypeID, modKitValueID, false);
        EntryPoint.WriteToConsole($"SetVehicleMod RAN {TypeID} {modKitValueID}");
    }
    public override void AddToMenu()
    {
        WheelClassSubMenu = MenuPool.AddSubMenu(InteractionMenu, WheelTypeName);
        if (ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
        {
            return;
        }


        NativeFunction.Natives.SET_VEHICLE_WHEEL_TYPE(ModdingVehicle.Vehicle, WheelTypeID);

        int TotalMods = NativeFunction.Natives.GET_NUM_VEHICLE_MODS<int>(ModdingVehicle.Vehicle, TypeID);
        if (TotalMods == 0)
        {
            return;
        }
        WheelClassSubMenu.OnMenuOpen += (sender) =>
        {
            ModKitMenuItem modKitMenuItem = MenuItems.Where(x => x.Index == WheelClassSubMenu.CurrentSelection).FirstOrDefault();
            if (modKitMenuItem == null)
            {
                return;
            }
            ResetModType();
            SetVehicleMod(modKitMenuItem.ID, false);
        };
        WheelClassSubMenu.OnMenuClose += (sender) =>
        {
            ResetModType();
        };
        WheelClassSubMenu.OnIndexChange += (sender, newIndex) =>
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
            int modKitPrice = ModShopMenu.GetModKitPrice(TypeID, myId);
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
            WheelClassSubMenu.AddItem(modkitItem);
        }
    }
    


}