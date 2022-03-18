using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class Transaction
{
    private MenuPool MenuPool;
    private UIMenu ParentMenu;
    private InteractableLocation Store;
    public PurchaseMenu PurchaseMenu { get; private set; }
    public SellMenu SellMenu { get; private set; }
    public ShopMenu ShopMenu { get; set; }
    public bool HasCustomItemPostion => ItemPreviewPosition != Vector3.Zero;
    public Vector3 ItemPreviewPosition { get; set; } = Vector3.Zero;
    public float ItemPreviewHeading { get; set; } = 0f;
    public Vector3 ItemDeliveryPosition { get; set; } = Vector3.Zero;
    public float ItemDeliveryHeading { get; set; } = 0f;

    public bool PreviewItems { get; set; } = true;


    public PersonTransaction PersonTransaction;

    public Transaction(MenuPool menuPool, UIMenu parentMenu, ShopMenu menu, InteractableLocation store)
    {
        MenuPool = menuPool;
        ParentMenu = parentMenu;
        ShopMenu = menu;
        Store = store;
    }
    public Transaction() : base()
    {
    }
    public void CreateTransactionMenu(IActivityPerformable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        if (ShopMenu != null && ShopMenu.Items.Any(x => x.Purchaseable))
        {

            if (Store != null)
            {
                PurchaseMenu = new PurchaseMenu(MenuPool, ParentMenu, ShopMenu, this, modItems, player, world, settings, weapons, time, Store.BannerImage, Store.HasBannerImage, Store.RemoveBanner, Store.Name);
            }
            else
            {
                PurchaseMenu = new PurchaseMenu(MenuPool, ParentMenu, ShopMenu, this, modItems, player, world, settings, weapons, time, null, false, true, "");
            }
            PurchaseMenu.Setup();
        }
        if (ShopMenu != null && ShopMenu.Items.Any(x => x.Sellable))
        {
            if (Store != null)
            {
                SellMenu = new SellMenu(MenuPool, ParentMenu, ShopMenu, this, modItems, player, world, settings, weapons, time, Store.BannerImage, Store.HasBannerImage, Store.RemoveBanner, Store.Name);//was IsUsingCustomCam before
            }
            else
            {
                SellMenu = new SellMenu(MenuPool, ParentMenu, ShopMenu, this, modItems, player, world, settings, weapons, time, null,false, true, "");//was IsUsingCustomCam before

            }
            SellMenu.Setup();
        }
    }
    public void ProcessTransactionMenu()
    {
        while (MenuPool.IsAnyMenuOpen())
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
    public void OnItemPurchased(ModItem modItem, MenuItem menuItem, int TotalItems)
    {
        if(Store != null)
        {
            Store.OnItemPurchased(modItem);
        }
        if(PersonTransaction != null)
        {
            PersonTransaction.OnItemPurchased(modItem, menuItem, TotalItems);
        }
    }
    public void OnItemSold(ModItem modItem, MenuItem menuItem, int TotalItems)
    {
        if (Store != null)
        {
            Store.OnItemSold(modItem);
        }
        if (PersonTransaction != null)
        {
            PersonTransaction.OnItemSold(modItem, menuItem, TotalItems);
        }
    }
}
