using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Serializable()]
public class LicensePlateItem : ModItem
{
    public LicensePlateItem()
    {
        Description = "License Plate";
        ItemSubType = ItemSubType.LicensePlates;
    }

    public LicensePlateItem(string name) : base(name, ItemType.LicensePlates)
    {
        Description = "License Plate";
        ItemSubType = ItemSubType.LicensePlates;
    }
    public LSR.Vehicles.LicensePlate LicensePlate { get; set; }
    public override string DisplayName => LicensePlate == null ? Name : LicensePlate.ToString();
    public override string DisplayDescription => LicensePlate == null ? Description : LicensePlate.GenerateDescription();
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        PlateTheft plateTheft = new PlateTheft(actionable, this, settings, world, actionable.ActivityManager.CurrentScrewdriver);
        if(plateTheft.CanPerform(actionable))
        {
            ModItem li = actionable.Inventory.Get(typeof(ScrewdriverItem))?.ModItem;
            if (li == null)
            {
                Game.DisplayHelp($"Need a ~r~Screwdriver~s~ to change plates.");
                return false;
            }
            actionable.ActivityManager.StartLowerBodyActivity(plateTheft);
            return true;
        }
        return false;
    }
    public override void AddNewItem(IModItems modItems)
    {
        LicensePlateItem existingItem = modItems.PossibleItems.LicensePlateItems.FirstOrDefault(x => x.Name == Name);
        if (existingItem != null)
        {
            existingItem.LicensePlate.IsWanted = LicensePlate.IsWanted;
        }
        else
        {
            modItems.PossibleItems.LicensePlateItems.Add(this);
        }
    }
    public override void CreateSimpleSellMenu(ILocationInteractable player, UIMenu sellPlateSubMenu, GameLocation gameLocation, int cleanPlateCost, int wantedPlateCost, bool useAccounts)
    {
        int price = LicensePlate == null ? cleanPlateCost : LicensePlate.IsWanted ? wantedPlateCost : cleanPlateCost;
        UIMenuItem MenuItem = new UIMenuItem(DisplayName, DisplayDescription) { RightLabel = price.ToString("C0") };
        MenuItem.Activated += (sender, e) =>
        {
            if (!player.Inventory.Remove(this, 1))
            {
                return;
            }
            player.BankAccounts.GiveMoney(price, useAccounts);
            MenuItem.Enabled = false;
            if(gameLocation == null)
            {
                return;
            }
            gameLocation.PlaySuccessSound();
            gameLocation.DisplayMessage("~g~Sale", $"You have sold your license plate.");
        };
        sellPlateSubMenu.AddItem(MenuItem);
    }

}

