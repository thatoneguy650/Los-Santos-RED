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
            foreach(var craftableItem in CraftableItems.Items)
            {
                CraftableItems.CraftablesLookup.Add(
                    craftableItem.Name,
                    new CraftableItemLookupModel()
                    {
                        RecipeName = craftableItem.Name,
                        IngredientLookup = GetIngredientLookup(craftableItem.Ingredients),
                        CraftableItem = craftableItem,
                    });
            }
        }
        private Dictionary<string, Ingredient> GetIngredientLookup(List<Ingredient> ingredient)
        {
            Dictionary<string, Ingredient> ingredientLookup = new Dictionary<string, Ingredient>();
            foreach(var _ingredient in ingredient)
            {
                ingredientLookup.Add(_ingredient.IngredientName, _ingredient);
            }
            return ingredientLookup;
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
            foreach(var item in Player.Inventory.ItemsList)
            {
                if(ingredientsSatisfied.Count == ingredientsToSatisfy)
                {
                    break;
                }
                if (craftItem.IngredientLookup.ContainsKey(item.ModItem.Name))
                {
                    if ((craftItem.IngredientLookup[item.ModItem.Name].Quantity * quantity) <= item.Amount)
                    {
                        ingredientsSatisfied.Add(item);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return ingredientsSatisfied.Count == ingredientsToSatisfy;
        }
        public void CraftItem(string productName, int quantity = 1)
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
                ModItem itemToGive = ModItems.Get(CraftableItems.CraftablesLookup[productName].CraftableItem.Resultant);
                itemToGive.AddToPlayerInventory(Player, quantity * (CraftableItems.CraftablesLookup[productName].CraftableItem.SingleUnit?1:CraftableItems.CraftablesLookup[productName].CraftableItem.ResultantAmount));
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