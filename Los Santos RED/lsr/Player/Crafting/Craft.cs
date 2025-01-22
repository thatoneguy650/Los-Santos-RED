using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Linq;
using LosSantosRED.lsr.Helper.Crafting;
using System.Collections.Generic;

namespace Mod
{
    public class Crafting
    {
        private Player Player;
        private ICraftableItems CraftableItems;
        private IModItems ModItems;
        private ISettingsProvideable Settings;
        private IWeapons Weapons;
        private bool IsCrafting = false;


        public Crafting(Player player, ICraftableItems craftableItems, IModItems modItems, ISettingsProvideable settings,IWeapons weapons)
        {
            Player = player;
            CraftableItems = craftableItems;
            ModItems = modItems;
            Settings = settings;
            Weapons = weapons;
        }
        public void Setup()
        {
            //Pre-processing for optimization - when a player wants to craft multiple batches of an ingredient, we can cache the last crafted recipe or have a lookup table making it faster to read instead of using .Find() over and over. Re-evaluate later.
            SetupCraftableLookup();
        }
        private void SetupCraftableLookup()
        {
            CraftableItems.CraftablesLookup = new System.Collections.Generic.Dictionary<string, CraftableItemLookupModel>();
            //Just holding reference to the craftable item in case any of the other details are required anywhere else.
            for (int i = 0; i < CraftableItems.Items.Count; i++)
            {
                CraftableItems.CraftablesLookup.Add(
                    CraftableItems.Items[i].Name, 
                    new CraftableItemLookupModel() { 
                        RecipeName = CraftableItems.Items[i].Name, 
                        IngredientLookup = CraftingUtils.GetIngredientLookup(CraftableItems.Items[i].Ingredients),
                        CraftableItem = CraftableItems.Items[i],
                    });
            }
        }
        private void DeductIngredientsFromInventory(List<InventoryItem> ingredientsSatisfied, CraftableItemLookupModel craftItem, int quantity)
        {
            foreach(var ingredient in ingredientsSatisfied)
            {
                Player.Inventory.Remove(ModItems.Get(craftItem.IngredientLookup[ingredient.ModItem.Name].IngredientName), craftItem.IngredientLookup[ingredient.ModItem.Name].Quantity * quantity);
            }
        }
        private bool CheckIngredientsAvailable(CraftableItemLookupModel craftItem, int ingredientsToSatisfy, List<InventoryItem> ingredientsSatisfied, int quantity)
        {
            for (int i = 0; i < Player.Inventory.ItemsList.Count && ingredientsSatisfied.Count < ingredientsToSatisfy; i++)
            {
                if (craftItem.IngredientLookup.ContainsKey(Player.Inventory.ItemsList[i].ModItem.Name))
                {
                    if ((craftItem.IngredientLookup[Player.Inventory.ItemsList[i].ModItem.Name].Quantity * quantity) <= Player.Inventory.ItemsList[i].Amount)
                    {
                        ingredientsSatisfied.Add(Player.Inventory.ItemsList[i]);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return ingredientsSatisfied.Count == ingredientsToSatisfy;
        }
        public void CraftItem(string productName, int quantity)
        {
            if(IsCrafting)
            {
                Game.DisplayNotification("~r~Cooldown active. ~w~Cannot craft.");
                return;
            }
            List<InventoryItem> ingredientsSatisfied = new List<InventoryItem>();
            int ingredientsToSatisfy = CraftableItems.CraftablesLookup[productName].IngredientLookup.Count;
            CraftableItemLookupModel craftItem = CraftableItems.CraftablesLookup[productName];
            bool ingredientsAvailableForCrafting = CheckIngredientsAvailable(craftItem, ingredientsToSatisfy, ingredientsSatisfied, quantity);
            if(ingredientsAvailableForCrafting)
            {
                DeductIngredientsFromInventory(ingredientsSatisfied, craftItem, quantity);
                Player.IsSetDisabledControls = true;
                IsCrafting = true;
                if(!string.IsNullOrEmpty(CraftableItems.CraftablesLookup[productName].CraftableItem.CrimeId))
                {
                    Player.Violations.SetContinuouslyViolating(CraftableItems.CraftablesLookup[productName].CraftableItem.CrimeId);
                }
                GameFiber.Wait(CraftableItems.CraftablesLookup[productName].CraftableItem.Cooldown);
                switch (CraftableItems.CraftablesLookup[productName].CraftableItem.CraftType)
                {
                    case CraftableType.Weapon:
                        WeaponInformation myGun = Weapons.GetWeapon(CraftableItems.CraftablesLookup[productName].CraftableItem.Resultant);
                        if (myGun != null)
                        {
                            Game.LocalPlayer.Character.Inventory.GiveNewWeapon(myGun.ModelName, myGun.AmmoAmount, true);
                        }
                        break;
                    case CraftableType.ModItem:
                    default:
                        Player.Inventory.Add(ModItems.Get(CraftableItems.CraftablesLookup[productName].CraftableItem.Resultant), (quantity * CraftableItems.CraftablesLookup[productName].CraftableItem.ResultantAmount));
                        break;
                }
                IsCrafting = false;
                if(!string.IsNullOrEmpty(CraftableItems.CraftablesLookup[productName].CraftableItem.CrimeId))
                {
                    Player.Violations.StopContinuouslyViolating(CraftableItems.CraftablesLookup[productName].CraftableItem.CrimeId);
                }
                Player.IsSetDisabledControls = false;
                Game.DisplayNotification($"~g~{productName} ~w~crafted");
            }
            else
            {
                Game.DisplayNotification("You do not have the required materials to craft this item");
            }
        }
    }
}