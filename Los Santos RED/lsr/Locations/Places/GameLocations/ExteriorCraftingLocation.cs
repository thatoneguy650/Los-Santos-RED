using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Linq;

public class ExteriorCraftingLocation : GameLocation
{
    private UIMenu OfferSubMenu;
    private string CanPurchaseRightLabel => $"{PurchasePrice:C0}";
    private UIMenuItem PurchaseCraftingLocationMenuItem;
    private UIMenuItem SellCraftingLocationItem;
    public ExteriorCraftingLocation(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        OpenTime = 0;
        CloseTime = 24;
    }
    public ExteriorCraftingLocation() : base()
    {

    }
    public override string TypeName { get; set; } = "Crafting Location";
    public override int MapIcon { get; set; } = 537;//873;//162;
    public string CraftingFlag { get; set; }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = GetButtonPromptText();
        return true;
    }
    public override void StandardInteract(LocationCamera locationCamera, bool isInside)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                CreateInteractionMenu();
                InteractionMenu.Visible = true;
                if (!HasBannerImage)
                {
                    InteractionMenu.SetBannerType(EntryPoint.LSRedColor);
                }
                GenerateMenu(isInside);
                while (IsAnyMenuVisible || Time.IsFastForwarding)
                {
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }
                DisposeInteractionMenu();
                DisposeInterior();
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
                Player.IsTransacting = false;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "CraftingExteriorInteract");
    }
    private void GenerateMenu(bool isInside)
    {
        AddInquireItems();
        AddInteractionItems(isInside);
    }
    private void AddInquireItems()
    {
        if ((!IsOwned && IsOwnable))
        {
            OfferSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Make an Offer");
            string offerDescription = "buy";
            InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = $"Select to {offerDescription.Trim()}";
            InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Lock;
            if (HasBannerImage)
            {
                BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
                OfferSubMenu.SetBannerType(BannerImage);
            }
            if (!HasBannerImage)
            {
                OfferSubMenu.SetBannerType(EntryPoint.LSRedColor);
            }
            PurchaseCraftingLocationMenuItem = new UIMenuItem("Purchase", "Select to purchase this business.") { RightLabel = CanPurchaseRightLabel };
            if (IsOwnable)
            {
                PurchaseCraftingLocationMenuItem.Activated += (sender, e) =>
                {
                    if (Purchase())
                    {
                        MenuPool.CloseAllMenus();
                        GenerateMenu(false);
                    }
                };
                OfferSubMenu.AddItem(PurchaseCraftingLocationMenuItem);
            }
            PurchaseCraftingLocationMenuItem.Description += "~n~~n~" + GetInquireDescription();
        }
    }
    private void AddInteractionItems(bool isInside)
    {
        if (!IsOwned)
        {
            return;
        }
        CreateOwnershipInteractionMenu();
        bool withAnimations = Interior?.IsTeleportEntry == true;
    }
    private string GetButtonPromptText()
    {
        if (IsOwned)
        {
            return $"Use {Name}";
        }
        else
        {
            return $"Inquire About {Name}";
        }
    }
    private void CreateOwnershipInteractionMenu()
    {
        if (IsOwned)
        {
            SellCraftingLocationItem = new UIMenuItem("Sell crafting location", "Sell the current business.") { RightLabel = CurrentSalesPrice.ToString("C0") };
            SellCraftingLocationItem.Activated += (sender, e) =>
            {
                OnSold();
                MenuPool.CloseAllMenus();
                Interior?.ForceExitPlayer(Player, this);
            };
            InteractionMenu.AddItem(SellCraftingLocationItem);
            UIMenuItem craftingMenuItem = new UIMenuItem("Craft Items", "");
            craftingMenuItem.Activated += (sender, selectedItem) =>
            {
                DisposeInteractionMenu();
                Player.Crafting.CraftingMenu.Show(CraftingFlag);
                NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
            };
            InteractionMenu.AddItem(craftingMenuItem);
        }
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.ExteriorCraftingLocations.Add(this);
        base.AddLocation(possibleLocations);
    }
    public override string GetInquireDescription()
    {
        return $"Provides ability to craft items requiring the: ~g~{CraftingFlag}";
    }
}

