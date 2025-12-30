using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PedClothingShopMenuItem
{
    public string Name { get; set; }
    public string Description { get; set; } 
    public int PurchasePrice { get; set; }
    public string ModelName { get; set; }
    public PedClothingComponent ClothingItem { get; set; }
    public List<PedClothingComponent> ForceSetComponenets { get; set; }
    public PedClothingShopMenuItem()
    {

    }

    public PedClothingShopMenuItem(string name, string description, int purchasePrice, string modelName, PedClothingComponent clothingItem, List<PedClothingComponent> forceSetComponenets)
    {
        Name = name;
        Description = description;
        PurchasePrice = purchasePrice;
        ModelName = modelName;
        ClothingItem = clothingItem;
        ForceSetComponenets = forceSetComponenets;
    }

    public void AddToMenu(ILocationInteractable player, MenuPool menuPool, UIMenu interactionMenu, ClothingShop clothingShop)
    {
        if (PurchasePrice <= -1)
        {
            return;
            
        }
        UIMenuItem ClothingItem = new UIMenuItem(Name, Description) { RightLabel = $"${PurchasePrice}" };
        ClothingItem.Activated += (sender, args) =>
        {
            if(player.BankAccounts.GetMoney(true) < PurchasePrice)
            {
                
                clothingShop?.PlayErrorSound();
                clothingShop?.DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
                return;
            }
            player.BankAccounts.GiveMoney(-1 * PurchasePrice, true);
            ApplyComponenets(player);
        };
        interactionMenu.AddItem(ClothingItem);
    }
    private void ApplyComponenets(ILocationInteractable player)
    {
        player.CurrentModelVariation.Components.RemoveAll(x => x.ComponentID == ClothingItem.ComponentID);
        if(ForceSetComponenets != null)
        {
            foreach(PedClothingComponent pedClothingComponent in ForceSetComponenets)
            {
                player.CurrentModelVariation.Components.RemoveAll(x => x.ComponentID == pedClothingComponent.ComponentID);
            }
        }
        player.CurrentModelVariation.Components.Add(new PedComponent(ClothingItem.ComponentID, ClothingItem.DrawableID, ClothingItem.PossibleTextures.FirstOrDefault()));
        if (ForceSetComponenets != null)
        {
            foreach (PedClothingComponent pedClothingComponent in ForceSetComponenets)
            {
                player.CurrentModelVariation.Components.Add(new PedComponent(pedClothingComponent.ComponentID, pedClothingComponent.DrawableID, pedClothingComponent.PossibleTextures.FirstOrDefault()));
            }
        }
        player.CurrentModelVariation.ApplyToPed(player.Character, false);
    }
}

