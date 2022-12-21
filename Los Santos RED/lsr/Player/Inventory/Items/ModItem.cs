using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage.Native;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Mod;
using System.Runtime;

[Serializable()]
[XmlInclude(typeof(BinocularsItem))]
[XmlInclude(typeof(BongItem))]
[XmlInclude(typeof(ConsumableItem))]

[XmlInclude(typeof(DrillItem))]
[XmlInclude(typeof(DrinkItem))]
[XmlInclude(typeof(FlashlightItem))]
[XmlInclude(typeof(FoodItem))]
[XmlInclude(typeof(HammerItem))]
[XmlInclude(typeof(HotelStayItem))]
[XmlInclude(typeof(IngestItem))]
[XmlInclude(typeof(InhaleItem))]
[XmlInclude(typeof(InjectItem))]
[XmlInclude(typeof(LicensePlateItem))]
[XmlInclude(typeof(PipeSmokeItem))]
[XmlInclude(typeof(PliersItem))]
[XmlInclude(typeof(ScrewdriverItem))]
[XmlInclude(typeof(ShovelItem))]
[XmlInclude(typeof(SmokeItem))]


[XmlInclude(typeof(TapeItem))]
[XmlInclude(typeof(UmbrellaItem))]
[XmlInclude(typeof(VehicleItem))]
[XmlInclude(typeof(WeaponItem))]


public class ModItem
{
    private UIMenuNumericScrollerItem<int> sellScroller;
    private UIMenuNumericScrollerItem<int> purchaseScroller;

    public ModItem()
    {

    }
    public ModItem(string name, ItemType itemType)
    {
        Name = name;
        ItemType = itemType;
    }
    public ModItem(string name, string description, ItemType itemType)
    {
        Name = name;
        Description = description;
        ItemType = itemType;
    }
    [XmlIgnore]
    public string MenuCategory { get; set; }
    [XmlIgnore]
    public PhysicalItem ModelItem { get; set; }
    [XmlIgnore]
    public PhysicalItem PackageItem { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = "";
    public string MeasurementName { get; set; } = "Item";
    public bool CleanupItemImmediately { get; set; } = false;//should be at the prop level?
    public bool IsPossessionIllicit { get; set; } = false;
    public bool ConsumeOnPurchase { get; set; } = false;
    public int AmountPerPackage { get; set; } = 1;
    public string ModelItemID { get; set; }
    public string PackageItemID { get; set; }
    public virtual bool CanConsume { get; set; } = false;//no no
    public ItemType ItemType { get; set; } = ItemType.None;
    public ItemSubType ItemSubType { get; set; } = ItemSubType.None;
    public float PercentLostOnUse { get; set; } = 0.0f;

    public virtual void Setup(PhysicalItems physicalItems, IWeapons weapons)
    {
        if (ModelItemID != "")
        {
            ModelItem = physicalItems.Get(ModelItemID);
        }
        if (PackageItemID != "")
        {
            PackageItem = physicalItems.Get(PackageItemID);
        }
        MenuCategory = ItemType.ToString();
    }

    public virtual string FullDescription(ISettingsProvideable Settings)
    {
        return $"{Description}~n~" 
            + GetTypeDescription(Settings)
            + GetExtendedDescription(Settings)
            + (MeasurementName != "Item" ? " " + MeasurementName + "(s)" : "");
        
    }
    public virtual bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants)
    {
        return false;
    }
    public virtual bool ConsumeItem(IActionable actionable, bool applyNeeds)
    {
        return false;
    }
    public virtual string GetTypeDescription(ISettingsProvideable settings)
    {
        return $"~n~Type: ~p~{ItemType}~s~" + (ItemSubType != ItemSubType.None ? $" - ~p~{ItemSubType}~s~" : "");
    }
    public virtual string GetExtendedDescription(ISettingsProvideable settings)
    {
        return "";
    }
    public virtual string PurchaseMenuDescription(ISettingsProvideable settings)
    {
        return "";
    }
    public virtual string SellMenuDescription(ISettingsProvideable settings)
    {
        return "";
    }
    public virtual void AddNewItem(IModItems modItems)
    {

    }


    public virtual void CreateSellMenuItem(Transaction Transaction, MenuItem menuItem, UIMenu sellMenu, ISettingsProvideable settings, ILocationInteractable player, bool isStealing, IEntityProvideable world)
    {

        sellScroller = new UIMenuNumericScrollerItem<int>(menuItem.ModItemName, "", 1, 1, 1) { Formatter = v => $"{(v == 1 && MeasurementName == "Item" ? "" : v.ToString() + " ")}{(MeasurementName != "Item" || v > 1 ? MeasurementName : "")}{(v > 1 ? "(s)" : "")}{(MeasurementName != "Item" || v > 1 ? " - " : "")}${(v * menuItem.SalesPrice)}", Value = 1 };
        UpdateSellMenuItem(menuItem, settings, player, isStealing);
        sellScroller.Activated += (sender, selectedItem) =>
        {
            SellItem(Transaction, player, menuItem, sellScroller.Value, isStealing);
            UpdateSellMenuItem(menuItem, settings, player, isStealing);
        };
        UIMenu CategoryMenu = sellMenu.Children.Where(x => x.Value.SubtitleText == MenuCategory).FirstOrDefault().Value;
        if (CategoryMenu != null)
        {
            CategoryMenu.AddItem(sellScroller);

        }
        else
        {
            sellMenu.AddItem(sellScroller);
        }
    }
    private void UpdateSellMenuItem(MenuItem menuItem, ISettingsProvideable settings, ILocationInteractable player, bool isStealing)
    {
        bool isEnabled = true;
        InventoryItem PlayerInventoryItem = player.Inventory.ItemsList.Where(x => x.ModItem.Name == menuItem.ModItemName).FirstOrDefault();
        int MaxSell = 1;
        int PlayerItems = 0;
        if (PlayerInventoryItem != null)
        {
            PlayerItems = PlayerInventoryItem.Amount;
            MaxSell = PlayerInventoryItem.Amount;
        }
        int RemainingToSell = MaxSell;
        if (menuItem.NumberOfItemsToPurchaseFromPlayer != -1)
        {

            RemainingToSell = menuItem.NumberOfItemsToPurchaseFromPlayer - menuItem.ItemsBoughtFromPlayer;

            if (RemainingToSell >= 1 && PlayerItems >= 1)
            {
                MaxSell = Math.Min(MaxSell, RemainingToSell);
            }
            else
            {
                MaxSell = 1;
                isEnabled = false;
            }
        }
        else
        {
            if (PlayerItems <= 0)
            {
                MaxSell = 1;
                isEnabled = false;
            }
        }
        string formattedPurchasePrice = menuItem.SalesPrice.ToString("C0");
        string description = Description;
        description += "~n~~s~";
        description += $"~n~Price: ~g~{formattedPurchasePrice}~s~";
        description += $"~n~Type: ~p~{ItemType}~s~" + (ItemSubType != ItemSubType.None ? $" - ~p~{ItemSubType}~s~" : "");
        description += $"~n~~b~{AmountPerPackage}~s~ Item(s) per Package";
        if (AmountPerPackage > 1)
        {
            description += $"~n~~b~{((float)menuItem.SalesPrice / (float)AmountPerPackage).ToString("C2")} ~s~per Item";
        }

        description += SellMenuDescription(settings);


        description += $"~n~{RemainingToSell} {MeasurementName}(s) For Purchase~s~";
        description += $"~n~Player Inventory: {PlayerItems}~s~ {MeasurementName}(s)";
        sellScroller.Maximum = MaxSell;
        sellScroller.Enabled = isEnabled;
        sellScroller.Description = description;
        EntryPoint.WriteToConsole($"Item: {Name} formattedPurchasePrice {formattedPurchasePrice}");
    }
    private bool SellItem(Transaction Transaction, ILocationInteractable player, MenuItem menuItem, int TotalItems, bool isStealing)
    {
        int TotalPrice = menuItem.SalesPrice * TotalItems;
        if (player.Inventory.Remove(this, TotalItems))
        {
            player.BankAccounts.GiveMoney(TotalPrice);
            Transaction.MoneySpent += TotalPrice;



            menuItem.ItemsBoughtFromPlayer += TotalItems;



            Transaction.OnAmountChanged(this);
            Transaction.OnItemSold(this, menuItem, TotalItems);
            while (player.ActivityManager.IsPerformingActivity)
            {
                GameFiber.Sleep(500);
            }
            return true;
        }
        return false;
    }


    public virtual void CreatePurchaseMenuItem(Transaction Transaction, MenuItem menuItem, UIMenu purchaseMenu, ISettingsProvideable settings, ILocationInteractable player, bool isStealing, IEntityProvideable world)
    {
        purchaseScroller = new UIMenuNumericScrollerItem<int>(menuItem.ModItemName, "", 1, 99, 1)
        {
            Formatter = v => $"{(v == 1 && MeasurementName == "Item" ? "" : v.ToString() + " ")}" +
            $"{(MeasurementName != "Item" || v > 1 ? MeasurementName : "")}" +
            $"{(v > 1 ? "(s)" : "")}" +
            $"{(MeasurementName != "Item" || v > 1 ? " - " : "")}" +
            $"{(menuItem.PurchasePrice == 0 ? "FREE" : $"${(v * menuItem.PurchasePrice)}")}",
            Value = 1
        };
        UpdatePurchaseMenuItem(menuItem, settings, player, isStealing);
        purchaseScroller.Activated += (sender,selectedItem) =>
        {
            PurchaseItem(Transaction, player, menuItem, purchaseScroller.Value, isStealing);
            UpdatePurchaseMenuItem(menuItem, settings, player, isStealing);
        };
        UIMenu CategoryMenu = purchaseMenu.Children.Where(x => x.Value.SubtitleText == MenuCategory).FirstOrDefault().Value;
        if (CategoryMenu != null)
        {
            CategoryMenu.AddItem(purchaseScroller);

        }
        else
        {
            purchaseMenu.AddItem(purchaseScroller);
        }
    }
    public void UpdatePurchaseMenuItem(MenuItem menuItem, ISettingsProvideable settings, ILocationInteractable player, bool isStealing)
    {
        if (menuItem != null && purchaseScroller != null)
        {
            InventoryItem PlayerInventoryItem = player.Inventory.ItemsList.Where(x => x.ModItem.Name == menuItem.ModItemName).FirstOrDefault();
            int PlayerItems = 0;
            if (PlayerInventoryItem != null)
            {
                PlayerItems = PlayerInventoryItem.Amount;
            }

            string formattedPurchasePrice = menuItem.PurchasePrice.ToString("C0");

            if (isStealing)
            {
                formattedPurchasePrice = "FREE";
            }
            if (menuItem.PurchasePrice == 0)
            {
                formattedPurchasePrice = "FREE";
            }

            string description = Description;
            description += "~n~~s~";
            description += $"~n~Price: ~r~{formattedPurchasePrice}~s~";
            description += $"~n~Type: ~p~{ItemType}~s~" + (ItemSubType != ItemSubType.None ? $" - ~p~{ItemSubType}~s~" : "");
            description += $"~n~~b~{AmountPerPackage}~s~ Item(s) per Package";
            if (AmountPerPackage > 1)
            {
                description += $"~n~~b~{((float)menuItem.PurchasePrice / (float)AmountPerPackage).ToString("C2")} ~s~per Item";
            }

            description += PurchaseMenuDescription(settings);

            bool enabled = true;
            int RemainingToBuy = 99;
            int MaxBuy = 99;
            if (menuItem.NumberOfItemsToSellToPlayer != -1)
            {


                RemainingToBuy = menuItem.NumberOfItemsToSellToPlayer - menuItem.ItemsSoldToPlayer;




                if (RemainingToBuy <= 0)
                {
                    MaxBuy = 0;
                    RemainingToBuy = 1;
                    enabled = false;
                }
                else
                {
                    MaxBuy = RemainingToBuy;
                }
                description += $"~n~{MaxBuy} {MeasurementName}(s) For Purchase~s~";
            }
            description += $"~n~Player Inventory: {PlayerItems}~s~ {MeasurementName}(s)";
            purchaseScroller.Maximum = RemainingToBuy;
            purchaseScroller.Enabled = enabled;
            purchaseScroller.Description = description;
        }
    }
    private bool PurchaseItem(Transaction Transaction, ILocationInteractable player, MenuItem menuItem, int TotalItems, bool isStealing)
    {
        int TotalPrice = menuItem.PurchasePrice * TotalItems;
        if (player.BankAccounts.Money >= TotalPrice || isStealing)
        {
            Transaction.OnItemPurchased(this, menuItem, TotalItems);






            menuItem.ItemsSoldToPlayer += TotalItems;






            if (ConsumeOnPurchase)
            {
                player.ActivityManager.UseInventoryItem(this, false);
            }
            else
            {
                player.Inventory.Add(this, TotalItems * AmountPerPackage);
            }
            if (!isStealing)
            {
                player.BankAccounts.GiveMoney(-1 * TotalPrice);
                Transaction.MoneySpent += TotalPrice;
            }
            return true;
        }
        Transaction.DisplayInsufficientFundsMessage();
        return false;
    }

    public virtual void CreatePreview(Transaction Transaction, Camera StoreCam)
    {
        try
        {
            string ModelToSpawn = "";
            bool useClose = true;
            if (PackageItem != null)
            {
                ModelToSpawn = PackageItem.ModelName;
                useClose = !PackageItem.IsLarge;
            }
            if (ModelToSpawn == "")
            {
                ModelToSpawn = ModelItem.ModelName;
                useClose = !ModelItem.IsLarge;
            }
            Vector3 Position = Vector3.Zero;
            if (StoreCam.Exists())
            {
                if (useClose)
                {
                    Position = StoreCam.Position + StoreCam.Direction;
                }
                else
                {
                    Position = StoreCam.Position + (StoreCam.Direction.ToNormalized() * 3f);
                }
            }
            else
            {
                Vector3 GPCamPos = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
                Vector3 GPCamDir = NativeHelper.GetGameplayCameraDirection();
                if (useClose)
                {
                    Position = GPCamPos + GPCamDir / 2;
                }
                else
                {
                    Position = GPCamPos + GPCamDir.ToNormalized() * 3f;
                }
            }
            if (ModelToSpawn != "" && NativeFunction.Natives.IS_MODEL_VALID<bool>(Game.GetHashKey(ModelToSpawn)))
            {
                try
                {
                   Transaction.SellingProp = new Rage.Object(ModelToSpawn, Position);
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole($"Error Spawning Model {ex.Message} {ex.StackTrace}");
                }
                if (Transaction.SellingProp.Exists())
                {
                    Transaction.SellingProp.SetRotationYaw(Transaction.SellingProp.Rotation.Yaw + 45f);
                    if (Transaction.SellingProp != null && Transaction.SellingProp.Exists())
                    {
                        NativeFunction.Natives.SET_ENTITY_HAS_GRAVITY(Transaction.SellingProp, false);
                    }
                }
                EntryPoint.WriteToConsole("SIMPLE TRANSACTION: PREVIEW ITEM RAN", 5);
            }
            else
            {
                if (Transaction.SellingProp.Exists())
                {
                    Transaction.SellingProp.Delete();
                }
            }
        }
        catch (Exception ex)
        {
            Game.DisplayNotification($"Error Displaying Model {ex.Message} {ex.StackTrace}");
        }
    }
}

