using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class TransactableLocation : InteractableLocation
{
    [XmlIgnore]
    public StorePurchaseMenu PurchaseMenu { get; private set; }
    [XmlIgnore]
    public StoreSellMenu SellMenu { get; private set; }

    [XmlIgnore]
    public ShopMenu Menu { get; set; }
    public string MenuID { get; set; }
    public bool HasCustomItemPostion => ItemPreviewPosition != Vector3.Zero;
    public Vector3 ItemPreviewPosition { get; set; } = Vector3.Zero;
    public float ItemPreviewHeading { get; set; } = 0f;
    public Vector3 ItemDeliveryPosition { get; set; } = Vector3.Zero;
    public float ItemDeliveryHeading { get; set; } = 0f;
    public TransactableLocation(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
        ButtonPromptText = $"Transact with {_Name}";
    }
    public TransactableLocation() : base()
    {
    }
    public override void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        if (CanInteract)
        {
            CanInteract = false;

            CreateInteractionMenu();
            StartTransaction(player, modItems, world, settings, weapons, time);

            ProcessTransactionMenu();

            DisposeTransactionMenu();
            DisposeInteractionMenu();

            CanInteract = true;
        }
    }
    public void CreateTransactionMenu(IActivityPerformable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        if (Menu.Items.Any(x => x.Purchaseable))
        {
            PurchaseMenu = new StorePurchaseMenu(MenuPool, InteractionMenu, this, modItems, player, world, settings, weapons, time);
            PurchaseMenu.Setup();
        }
        if (Menu.Items.Any(x => x.Sellable))
        {
            SellMenu = new StoreSellMenu(MenuPool, InteractionMenu, this,modItems,player,world,settings,weapons,time);//was IsUsingCustomCam before
            SellMenu.Setup();
        }
    }
    private void StartTransaction(IActivityPerformable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {      
        //CreateInteractionMenu();
        bool hasPurchaseMenu = false;
        bool hasSellMenu = false;
        if (Menu.Items.Any(x => x.Purchaseable))
        {
            PurchaseMenu = new StorePurchaseMenu(MenuPool, InteractionMenu, this, modItems, player, world, settings, weapons, time);
            PurchaseMenu.Setup();
            hasPurchaseMenu = true;
        }
        if (Menu.Items.Any(x => x.Sellable))
        {
            SellMenu = new StoreSellMenu(MenuPool, InteractionMenu, this, modItems, player, world, settings, weapons, time);//was IsUsingCustomCam before
            SellMenu.Setup();
            hasSellMenu = true;
        }
        if (hasSellMenu && hasPurchaseMenu)
        {
            ClearPreviews();
            InteractionMenu.Visible = true;

        }
        else if (hasSellMenu)
        {
            SellMenu.Show();
        }
        else
        {
            PurchaseMenu.Show();
        }
        InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
        EntryPoint.WriteToConsole("InteractableLocation OnInteract");
    }
    public void ProcessTransactionMenu()
    {
        while (IsAnyMenuVisible)
        {
            MenuPool.ProcessMenus();
            PurchaseMenu?.Update();
            SellMenu?.Update();
            GameFiber.Yield();
        }    
    }
    public void DisposeTransactionMenu()
    {
        PurchaseMenu?.Dispose();
        SellMenu?.Dispose();
        //DisposeInteractionMenu();
        //GameFiber.Sleep(1000);
    }
    public void ClearPreviews()
    {
        PurchaseMenu?.ClearPreviews();
        SellMenu?.ClearPreviews();
    }
    public void OnAmountChanged(ModItem modItem)
    {
        PurchaseMenu?.OnAmountChanged(modItem);
        SellMenu?.OnAmountChanged(modItem);
    }
    private void InteractionMenu_OnItemSelect(RAGENativeUI.UIMenu sender, RAGENativeUI.Elements.UIMenuItem selectedItem, int index)
    {
        if (selectedItem.Text == "Buy")
        {
            SellMenu?.Dispose();
            PurchaseMenu?.Show();
        }
        if (selectedItem.Text == "Sell")
        {
            PurchaseMenu?.Dispose();
            SellMenu?.Show();
        }
    }
}
