using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public class PedClothingShopMenuItem
{
    private PedVariation WorkingVariation;
    private UIMenu SubMenu;
    private UIMenuItem SubMenuItem;
    //private List<OptionalClothingChoice> OptionalClothingChoices;
    private List<UIMenuListScrollerItem<OptionalClothingChoice>> OptionalVariationScrollers;
    private UIMenuListScrollerItem<AppliedOverlay> AppliedOverlaysScroller;
    private UIMenuCheckboxItem isDefaultNotApplied;

    public string Name { get; set; }
    public string Description { get; set; } 
    public int PurchasePrice { get; set; }


    public bool IsAccessory { get; set; }
    public List<string> ModelNames { get; set; } = new List<string>();
    public List<PedClothingComponent> ForceSetComponenets { get; set; }



    public List<AppliedOverlay> ForceSetOverlays { get; set; }


    public string Category { get; set; }
    public string SubCategory { get; set; }
    public ePedFocusZone PedFocusZone { get; set; } = ePedFocusZone.None;
    public PedClothingShopMenuItem()
    {

    }

    public PedClothingShopMenuItem(string name, string description, int purchasePrice, List<string> modelNames, List<PedClothingComponent> forceSetComponenets)
    {
        Name = name;
        Description = description;
        PurchasePrice = purchasePrice;
        ModelNames = modelNames;
        ForceSetComponenets = forceSetComponenets;
    }

    public void AddToMenu(ILocationInteractable player, MenuPool menuPool, UIMenu interactionMenu, ClothingShop clothingShop, OrbitCamera orbitCamera, bool IsPurchase)
    {
        if (PurchasePrice <= -1)
        {
            return;         
        }
        WorkingVariation = player.CurrentModelVariation.Copy();
        SubMenu = menuPool.AddSubMenu(interactionMenu, Name);
        SubMenuItem = interactionMenu.MenuItems[interactionMenu.MenuItems.Count() - 1];
        SubMenuItem.Description = Description;
        SubMenu.OnMenuOpen += (sender) =>
        {
            ApplyItems(player, WorkingVariation, false);
            orbitCamera?.SetHint(PedFocusZone);
        };
        SubMenu.OnMenuClose += (sender) =>
        {
            player.CurrentModelVariation.ApplyToPed(player.Character, false);
            orbitCamera?.Reset();
        };
        //Give Optional Variations if available
        OptionalVariationScrollers = new List<UIMenuListScrollerItem<OptionalClothingChoice>>();
        int part = 1;
        foreach (PedClothingComponent pedClothingComponent in ForceSetComponenets)
        {
            List<OptionalClothingChoice> OptionalClothingChoices = new List<OptionalClothingChoice>();
            foreach (int possibleTextureIDs in pedClothingComponent.PossibleTextures)
            {
                OptionalClothingChoices.Add(new OptionalClothingChoice($"Style {possibleTextureIDs}", possibleTextureIDs) { ComponentID = pedClothingComponent.ComponentID, IsProp = pedClothingComponent.IsProp });
            }
            UIMenuListScrollerItem<OptionalClothingChoice> variationsScrollerMenu = new UIMenuListScrollerItem<OptionalClothingChoice>($"Part {part}", "Pick optional variations to be set", OptionalClothingChoices);
            variationsScrollerMenu.IndexChanged += (sender, oldIndex, newIndex) =>
            {
                ApplyItems(player, WorkingVariation, false);
            };
            if (pedClothingComponent.PossibleTextures.Count > 1)
            {
                SubMenu.AddItem(variationsScrollerMenu);
            }
            OptionalVariationScrollers.Add(variationsScrollerMenu);
            part++;
        }


        if (ForceSetOverlays != null)
        {
            UIMenuListScrollerItem<AppliedOverlay> OverlaysScrollerMenu = new UIMenuListScrollerItem<AppliedOverlay>($"Overlay", "Pick optional overlays to be set", ForceSetOverlays);
            OverlaysScrollerMenu.IndexChanged += (sender, oldIndex, newIndex) =>
            {
                ApplyItems(player, WorkingVariation, false);
            };
            if (ForceSetOverlays != null && ForceSetOverlays.Count() > 1)
            {
                SubMenu.AddItem(OverlaysScrollerMenu);
            }
            AppliedOverlaysScroller = OverlaysScrollerMenu;
        }


        isDefaultNotApplied = new UIMenuCheckboxItem("Default Not Applied",false,"Check to enable being default not applied");
        if (IsAccessory)
        {
            SubMenu.AddItem(isDefaultNotApplied);
        }


        if (IsPurchase)
        {

            UIMenuItem purchaseMenuItem = new UIMenuItem("Purchase", "Select to purchase this item") { RightLabel = $"${PurchasePrice}" };
            purchaseMenuItem.Activated += (sender, args) =>
            {
                if (player.BankAccounts.GetMoney(true) < PurchasePrice)
                {
                    clothingShop?.PlayErrorSound();
                    clothingShop?.DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
                    return;
                }
                clothingShop?.PlaySuccessSound();
                clothingShop?.DisplayMessage("~g~Purchased", $"Thank you for your purchase");
                player.BankAccounts.GiveMoney(-1 * PurchasePrice, true);
                player.OutfitManager.PurchasePedClothingItem(this);
                ApplyItems(player, player.CurrentModelVariation, true);
            };
            SubMenu.AddItem(purchaseMenuItem);
        }
        else
        {
            UIMenuItem applyMenuItem = new UIMenuItem("Apply", "Select to apply this item");
            applyMenuItem.Activated += (sender, args) =>
            {
                ApplyItems(player, player.CurrentModelVariation, true);
                NativeHelper.PlayAcceptSound();
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "Applied", "Purchased item has been applied");
            };
            SubMenu.AddItem(applyMenuItem);
        }
    }



    private void ApplyItems(ILocationInteractable player, PedVariation pedVariation, bool setDefaultNotApply)
    {
        ApplyComponentsAndProps(player, pedVariation, setDefaultNotApply);
        ApplyOverlays(player, pedVariation);
        pedVariation.ApplyToPed(player.Character, false);
    }

    private void ApplyComponentsAndProps(ILocationInteractable player, PedVariation pedVariation, bool setDefaultNotApply)
    {
        if (ForceSetComponenets != null)
        {
            foreach (PedClothingComponent pedClothingComponent in ForceSetComponenets.Where(x => !x.IsProp))
            {
                pedVariation.Components.RemoveAll(x => x.ComponentID == pedClothingComponent.ComponentID);
            }
            foreach (PedClothingComponent pedClothingComponent in ForceSetComponenets.Where(x => x.IsProp))
            {
                pedVariation.Props.RemoveAll(x => x.PropID == pedClothingComponent.ComponentID);
            }

            foreach (PedClothingComponent pedClothingComponent in ForceSetComponenets)
            {
                int textureSelected = pedClothingComponent.PossibleTextures.FirstOrDefault();
                if (OptionalVariationScrollers != null && OptionalVariationScrollers.Any())
                {
                    UIMenuListScrollerItem<OptionalClothingChoice> occ = OptionalVariationScrollers.Where(x => x.SelectedItem.ComponentID == pedClothingComponent.ComponentID && x.SelectedItem.IsProp == pedClothingComponent.IsProp).FirstOrDefault();
                    if (occ != null)
                    {
                        textureSelected = occ.SelectedItem.VariationID;
                    }
                }
                if (!pedClothingComponent.IsProp)
                {
                    PedComponent toAdd = new PedComponent(pedClothingComponent.ComponentID, pedClothingComponent.DrawableID, textureSelected);
                    if(IsAccessory && setDefaultNotApply)
                    {
                        toAdd.IsDefaultNotApplied = isDefaultNotApplied.Checked;
                    }
                    pedVariation.Components.Add(toAdd);

                }
                else
                {
                    PedPropComponent toAdd = new PedPropComponent(pedClothingComponent.ComponentID, pedClothingComponent.DrawableID, textureSelected);
                    if (IsAccessory && setDefaultNotApply)
                    {
                        toAdd.IsDefaultNotApplied = isDefaultNotApplied.Checked;
                    }
                    pedVariation.Props.Add(toAdd);
                }

            }
        }
    }
    private void ApplyOverlays(ILocationInteractable player, PedVariation pedVariation)
    {
        if(ForceSetOverlays == null || !ForceSetOverlays.Any())
        {
            return;
        }
        if (pedVariation == null)
        {
            return;
        }
        AppliedOverlay overlaySelected = ForceSetOverlays.FirstOrDefault();
        UIMenuListScrollerItem<AppliedOverlay> occ = AppliedOverlaysScroller;
        if (occ != null)
        {
            overlaySelected = occ.SelectedItem;
        }
        pedVariation.AppliedOverlays.RemoveAll(x=> x.ZoneName == overlaySelected.ZoneName);
        pedVariation.AppliedOverlays.Add(overlaySelected);
        //pedVariation.AppliedOverlays.RemoveAll(x=> x.ZoneName == );
    }
    private class OptionalClothingChoice
    {
        public OptionalClothingChoice(string name, int variationID)
        {
            Name = name;
            VariationID = variationID;
        }

        public string Name { get; set; }
        public int VariationID { get; set; }
        public int ComponentID { get; set; }
        public bool IsProp { get; set; }
        public override string ToString()
        {
            return Name.ToString();
        }
    }
}

