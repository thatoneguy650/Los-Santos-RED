using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Linq;
using LosSantosRED.lsr.Helper.Crafting;
using System.Collections.Generic;
using Rage.Native;

namespace Mod
{
    public class Crafting
    {
        private Player Player;
        private IModItems ModItems;
        private ISettingsProvideable Settings;
        private IWeapons Weapons;
        private bool IsCrafting = false;
        public CraftingMenu CraftingMenu { get; set; }
        public ICraftableItems CraftableItems;


        public Crafting(Player player, ICraftableItems craftableItems, IModItems modItems, ISettingsProvideable settings, IWeapons weapons)
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
            Player.Crafting = this;
        }
        private void SetupCraftableLookup()
        {
            CraftableItems.CraftablesLookup = new System.Collections.Generic.Dictionary<string, CraftableItemLookupModel>();
            CraftableItems.IngredientCraftableLookup = new Dictionary<string, List<string>>();

            //Just holding reference to the craftable item in case any of the other details are required anywhere else.
            foreach (var craftableItem in CraftableItems.Items)
            {
                foreach (var ingredient in craftableItem.Ingredients)
                {

                    if (CraftableItems.IngredientCraftableLookup.ContainsKey(ingredient.IngredientName))
                    {
                        CraftableItems.IngredientCraftableLookup[ingredient.IngredientName].Add(craftableItem.Name);
                    }
                    else
                    {
                        CraftableItems.IngredientCraftableLookup[ingredient.IngredientName] = new List<string>() { craftableItem.Name };
                    }
                }
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
            foreach (var _ingredient in ingredient)
            {
                ingredientLookup.Add(_ingredient.IngredientName, _ingredient);
            }
            return ingredientLookup;
        }
        private void DeductIngredientsFromInventory(Dictionary<string,ModItem> itemsToRemove, CraftableItemLookupModel craftItem, int quantity)
        {
            foreach(Ingredient ingredient in craftItem.IngredientLookup.Values)
            {
                if(ingredient.IsConsumed)
                {
                    Player.Inventory.Remove(itemsToRemove[ingredient.IngredientName], craftItem.IngredientLookup[ingredient.IngredientName].Quantity * quantity);
                }
            }
        }
        public int GetQuantityOfCraftable(CraftableItemLookupModel craftItem, Dictionary<string, ModItem> itemsToRemove, string craftingFlag = null)
        {
            if ((!string.IsNullOrEmpty(craftItem.CraftableItem.CraftingFlag)) && craftingFlag != craftItem.CraftableItem.CraftingFlag)
            {
                return 0;
            }
            int quantity = 0;
            int ingredientsSatisfied = 0;
            int ingredientsToSatisfy = craftItem.CraftableItem.Ingredients.Count;
            foreach (var item in Player.Inventory.ItemsList)
            {
                if (ingredientsSatisfied == ingredientsToSatisfy)
                {
                    break;
                }
                if (craftItem.IngredientLookup.ContainsKey(item.ModItem.Name))
                {
                    var instancesCraftable = item.Amount / craftItem.IngredientLookup[item.ModItem.Name].Quantity;
                    if (instancesCraftable == 0)
                    {
                        quantity = 0;
                        break;
                    }
                    else
                    {
                        quantity = quantity == 0 ? instancesCraftable : Math.Min(quantity, instancesCraftable);
                        if (!itemsToRemove.ContainsKey(item.ModItem.Name))
                        {
                            itemsToRemove.Add(item.ModItem.Name, item.ModItem);
                        }
                        ingredientsSatisfied++;
                    }
                }
            }
            if(ingredientsSatisfied != ingredientsToSatisfy)
            {
                return 0;
            }
            return quantity;
        }
        private void PerformAnimation(CraftableItemLookupModel craftItem)
        {
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, craftItem.CraftableItem.AnimationDictionary, craftItem.CraftableItem.AnimationName, 4.0f, -4.0f, -1, 0, 0, false, false, false);
        }
        public void CraftItem(string productName, Dictionary<string,ModItem> itemsToRemove, int quantity = 1, string craftingFlag = null)
        {
            if (IsCrafting)
            {
                Game.DisplayNotification("~r~Cooldown active. ~w~Cannot craft.");
                return;
            }
            Player.ActivityManager.StopDynamicActivity();
            CraftableItemLookupModel craftItem = CraftableItems.CraftablesLookup[productName];
            CraftingMenu.Hide();
            DeductIngredientsFromInventory(itemsToRemove, craftItem, quantity);
            Player.IsSetDisabledControls = true;
            IsCrafting = true;
            if((!string.IsNullOrEmpty(craftItem.CraftableItem.AnimationDictionary)) && (!string.IsNullOrEmpty(craftItem.CraftableItem.AnimationName)))
            {
                PerformAnimation(craftItem);
            }
            if (!string.IsNullOrEmpty(CraftableItems.CraftablesLookup[productName].CraftableItem.CrimeId))
            {
                Player.Violations.SetContinuouslyViolating(CraftableItems.CraftablesLookup[productName].CraftableItem.CrimeId);
            }
            GameFiber.Wait(CraftableItems.CraftablesLookup[productName].CraftableItem.Cooldown);
            ModItem itemToGive = ModItems.Get(CraftableItems.CraftablesLookup[productName].CraftableItem.Resultant);
            itemToGive.AddToPlayerInventory(Player, quantity * (CraftableItems.CraftablesLookup[productName].CraftableItem.SingleUnit ? 1 : CraftableItems.CraftablesLookup[productName].CraftableItem.ResultantAmount));
            IsCrafting = false;
            if (!string.IsNullOrEmpty(CraftableItems.CraftablesLookup[productName].CraftableItem.CrimeId))
            {
                Player.Violations.StopContinuouslyViolating(CraftableItems.CraftablesLookup[productName].CraftableItem.CrimeId);
            }
            Player.IsSetDisabledControls = false;
            Game.DisplayNotification($"~g~{productName} ~w~crafted");
            CraftingMenu.Show(craftingFlag);
        }
    }
}