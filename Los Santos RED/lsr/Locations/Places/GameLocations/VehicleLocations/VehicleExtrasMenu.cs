using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage.Native;
using Rage;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAGENativeUI.Elements;
using System.Drawing;


public class VehicleExtrasMenu
{
    private MenuPool MenuPool;
    private UIMenu VehicleHeaderMenu;
    private ILocationInteractable Player;
    private VehicleExt ModdingVehicle;
    private VehicleVariation CurrentVariation;
    private GameLocation GameLocation;
    private ModShopMenu ModShopMenu;
    private UIMenu ExtrasMenu;
    private List<ExtraMenuItem> MenuItems = new List<ExtraMenuItem>();

    public VehicleExtrasMenu(MenuPool menuPool, UIMenu vehicleHeaderMenu, ILocationInteractable player, VehicleExt moddingVehicle, ModShopMenu modShopMenu, VehicleVariation currentVariation, GameLocation gameLocation)
    {
        MenuPool = menuPool;
        VehicleHeaderMenu = vehicleHeaderMenu;
        Player = player;
        ModdingVehicle = moddingVehicle;
        CurrentVariation = currentVariation;
        GameLocation = gameLocation;
        ModShopMenu = modShopMenu;
    }
    public void Setup()
    {
        ExtrasMenu = MenuPool.AddSubMenu(VehicleHeaderMenu, "Extras");
        ExtrasMenu.SetBannerType(EntryPoint.LSRedColor);
        ExtrasMenu.SubtitleText = "EXTRAS";
        VehicleHeaderMenu.MenuItems[VehicleHeaderMenu.MenuItems.Count() - 1].Description = "Pick Extras";
        AddExtras();
    }
    private void AddExtras()
    {
        if(ModdingVehicle == null || !ModdingVehicle.Vehicle.Exists())
        {
            return;
        }


        ExtrasMenu.OnMenuOpen += (sender) =>
        {
            ExtraMenuItem extraMenuItem = MenuItems.Where(x => x.Index == ExtrasMenu.CurrentSelection).FirstOrDefault();
            if (extraMenuItem == null)
            {
                return;
            }
            ResetExtras();
            SetVehicleExtra(extraMenuItem.ExtraID, true, false);
        };
        ExtrasMenu.OnMenuClose += (sender) =>
        {
            ResetExtras();
        };
        ExtrasMenu.OnIndexChange += (sender, newIndex) =>
        {
            ExtraMenuItem extraMenuItem = MenuItems.Where(x => x.Index == newIndex).FirstOrDefault();
            if (extraMenuItem == null)
            {
                EntryPoint.WriteToConsole($"OnIndexChange NEWINDEX:{newIndex}");
                return;
            }
            ResetExtras();
            SetVehicleExtra(extraMenuItem.ExtraID, true, false);
        };



        MenuItems = new List<ExtraMenuItem>();
        int counter = 0;
        for (int i = 0; i < 12; i++)
        {
            int myId = i;
            if (NativeFunction.Natives.DOES_EXTRA_EXIST<bool>(ModdingVehicle.Vehicle, i))
            {
                UIMenuItem extraMenuItem = new UIMenuItem($"Extra {myId + 1}","Select to apply this extra");
                bool isOn = NativeFunction.Natives.IS_VEHICLE_EXTRA_TURNED_ON<bool>(ModdingVehicle.Vehicle, i);
                int price = GetPrice(myId);
                if (isOn)
                {
                    extraMenuItem.RightLabel = "";
                    extraMenuItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
                }
                else
                {
                    extraMenuItem.RightLabel = $"~r~${GetPrice(i)}~s~";
                    extraMenuItem.RightBadge = UIMenuItem.BadgeStyle.None;
                }
                MenuItems.Add(new ExtraMenuItem(extraMenuItem, myId, isOn, counter));
                extraMenuItem.Activated += (sender, selectedItem) =>
                {
                    bool isCurrentlyOn = false;
                    VehicleExtra currentMod = CurrentVariation.VehicleExtras.Where(x => x.ID == myId).FirstOrDefault();
                    if(currentMod != null)
                    {
                        isCurrentlyOn = currentMod.IsTurnedOn;
                    }
                    if(!isCurrentlyOn)
                    {
                        if (Player.BankAccounts.GetMoney(true) < price)
                        {
                            ModShopMenu.DisplayInsufficientFundsMessage(price);
                            return;
                        }
                        ModShopMenu.DisplayPurchasedMessage(price);
                        Player.BankAccounts.GiveMoney(-1 * price, true);
                    }


                    //bool isCurrentlyOn = NativeFunction.Natives.IS_VEHICLE_EXTRA_TURNED_ON<bool>(ModdingVehicle.Vehicle, myId);
                    SetVehicleExtra(myId, !isCurrentlyOn, true);
                };
                ExtrasMenu.AddItem(extraMenuItem);
                counter++;
            }
            
        }
    }
    protected void SetVehicleExtra(int extraID,bool isOn, bool updateVariation)
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
            VehicleExtra currentMod = CurrentVariation.VehicleExtras.Where(x => x.ID == extraID).FirstOrDefault();
            if (currentMod != null)
            {
                currentMod.IsTurnedOn = isOn;
            }
            else
            {
                CurrentVariation.VehicleExtras.Add(new VehicleExtra(extraID, isOn));
            }
            SyncMenuItems();
        }
        NativeFunction.Natives.SET_VEHICLE_EXTRA(ModdingVehicle.Vehicle, extraID, !isOn);
        EntryPoint.WriteToConsole($"SetVehicleExtra RAN {extraID} {isOn}");
    }
    protected virtual void ResetExtras()
    {
        for (int i = 0; i < 12; i++)
        {
           VehicleExtra selectedExtra = CurrentVariation.VehicleExtras.Where(x => x.ID == i).FirstOrDefault();
            if(selectedExtra == null || !selectedExtra.IsTurnedOn)
            {
                NativeFunction.Natives.SET_VEHICLE_EXTRA(ModdingVehicle.Vehicle, i, true);
                EntryPoint.WriteToConsole($"ResetExtras {i} IsTurnedOn{selectedExtra.IsTurnedOn}");
            }
            else
            {
                NativeFunction.Natives.SET_VEHICLE_EXTRA(ModdingVehicle.Vehicle, i, false);
            }
        }
    }

    private void SyncMenuItems()
    {
       // VehicleExtra currentMod = CurrentVariation.VehicleMods.Where(x => x.ID == TypeID).FirstOrDefault();
        foreach (ExtraMenuItem item in MenuItems)
        {
            VehicleExtra currentExtra = CurrentVariation.VehicleExtras.Where(x => x.ID == item.ExtraID).FirstOrDefault();
            if (currentExtra != null && currentExtra.IsTurnedOn)
            {
                EntryPoint.WriteToConsole($"SyncMenuItems ExtraID{item.ExtraID} IsOn{item.IsOn}");
                item.UIMenuItem.RightLabel = "";
                item.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
            }
            else
            {
                item.UIMenuItem.RightLabel = $"~r~${GetPrice(item.ExtraID)}~s~";
                item.UIMenuItem.RightBadge = UIMenuItem.BadgeStyle.None;
            }
        }
    }
    private int GetPrice(int extraID)
    {
        return 500 + (100 * extraID);
    }
    protected class ExtraMenuItem
    {
        public ExtraMenuItem(UIMenuItem uIMenuItem, int extraID, bool isOn, int index)
        {
            UIMenuItem = uIMenuItem;
            ExtraID = extraID;
            IsOn = isOn;
            Index = index;
        }

        public UIMenuItem UIMenuItem { get; set; }
        public int ExtraID { get; set; }
        public bool IsOn { get; set; }
        public int Index { get; set; }
    }
}

