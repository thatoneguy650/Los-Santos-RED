using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using RAGENativeUI.Elements;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PedClothingShopMenuItem
{
    private PedVariation WorkingVariation;
    private UIMenu SubMenu;
    private UIMenuItem SubMenuItem;
    //private List<OptionalClothingChoice> OptionalClothingChoices;
    private List<UIMenuListScrollerItem<OptionalClothingChoice>> OptionalVariationScrollers;

    public string Name { get; set; }
    public string Description { get; set; } 
    public int PurchasePrice { get; set; }
    public List<string> ModelNames { get; set; } = new List<string>();
    public List<PedClothingComponent> ForceSetComponenets { get; set; }
    public string Category { get; set; }
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

    public void AddToMenu(ILocationInteractable player, MenuPool menuPool, UIMenu interactionMenu, ClothingShop clothingShop)
    {
        if (PurchasePrice <= -1)
        {
            return;         
        }
 
        UIMenuItem categoryMenu = interactionMenu.MenuItems.Where(x => x.Text == Category).FirstOrDefault();


        WorkingVariation = player.CurrentModelVariation.Copy();
        SubMenu = menuPool.AddSubMenu(interactionMenu, Name);
        SubMenuItem = interactionMenu.MenuItems[interactionMenu.MenuItems.Count() - 1];
        SubMenuItem.Description = Description;

        SubMenu.OnMenuOpen += (sender) =>
        {
            ApplyComponents(player, WorkingVariation);
        };
        SubMenu.OnMenuClose += (sender) =>
        {
            player.CurrentModelVariation.ApplyToPed(player.Character, false);
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
                ApplyComponents(player, WorkingVariation);
            };
            if (pedClothingComponent.PossibleTextures.Count > 1)
            {
                SubMenu.AddItem(variationsScrollerMenu);
            }
            OptionalVariationScrollers.Add(variationsScrollerMenu);
            part++;
        }


        UIMenuItem purchaseMenuItem = new UIMenuItem("Purchase", "Select to purchase this item") { RightLabel = $"${PurchasePrice}" };
        purchaseMenuItem.Activated += (sender, args) =>
        {
            if(player.BankAccounts.GetMoney(true) < PurchasePrice)
            {  
                clothingShop?.PlayErrorSound();
                clothingShop?.DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
                return;
            }
            clothingShop?.PlaySuccessSound();
            clothingShop?.DisplayMessage("~g~Purchased", $"Thank you for your purchase");
            player.BankAccounts.GiveMoney(-1 * PurchasePrice, true);
            ApplyComponents(player, player.CurrentModelVariation);
            //disable submehnu purchase thingo?
        };
        SubMenu.AddItem(purchaseMenuItem);
    }



    private void ApplyComponents(ILocationInteractable player, PedVariation pedVariation)
    {
        if (ForceSetComponenets != null)
        {



            foreach (PedClothingComponent pedClothingComponent in ForceSetComponenets.Where(x=> !x.IsProp))
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
                    pedVariation.Components.Add(toAdd);

                }
                else
                {
                    PedPropComponent toAdd = new PedPropComponent(pedClothingComponent.ComponentID, pedClothingComponent.DrawableID, textureSelected);
                    pedVariation.Props.Add(toAdd);
                }

            }
        }
        pedVariation.ApplyToPed(player.Character, false);
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

