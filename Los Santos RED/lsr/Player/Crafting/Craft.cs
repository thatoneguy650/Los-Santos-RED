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
            SetupCraftableLookup();
            Player.Crafting = this;
        }
        private void SetupCraftableLookup()
        {
            CraftableItems.CraftablesLookup = new System.Collections.Generic.Dictionary<string, CraftableItemLookupModel>();

            //Just holding reference to the craftable item in case any of the other details are required anywhere else.
            foreach (var craftableItem in CraftableItems.Items)
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
            foreach (var _ingredient in ingredient)
            {
                ingredientLookup.Add(_ingredient.IngredientName, _ingredient);
            }
            return ingredientLookup;
        }
        private void DeductIngredientsFromInventory(CraftableItemLookupModel craftItem, int quantity)
        {
            foreach (Ingredient ingredient in craftItem.CraftableItem.Ingredients)
            {
                if (ingredient.IsConsumed)
                {
                    Player.Inventory.Remove(Player.Inventory.ItemsList.Find(x=>x.ModItem.Name == ingredient.IngredientName).ModItem, ingredient.Quantity * quantity);
                }
            }
        }
        private void PerformAnimation(CraftableItemLookupModel craftItem)
        {
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, craftItem.CraftableItem.AnimationDictionary, craftItem.CraftableItem.AnimationName, 4.0f, -4.0f, -1, 0, 0, false, false, false);
        }
        public void CraftItem(string productName, int quantity = 1, string craftingFlag = null)
        {
            if (IsCrafting)
            {
                Game.DisplayNotification("~r~Cooldown active. ~w~Cannot craft.");
                return;
            }
            Player.ActivityManager.StopDynamicActivity();
            CraftableItemLookupModel craftItem = CraftableItems.CraftablesLookup[productName];
            CraftingMenu.Hide();
            DeductIngredientsFromInventory(craftItem, quantity);
            Player.IsSetDisabledControls = true;
            IsCrafting = true;
            if ((!string.IsNullOrEmpty(craftItem.CraftableItem.AnimationDictionary)) && (!string.IsNullOrEmpty(craftItem.CraftableItem.AnimationName)))
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
            CraftingMenu.RedrawCraftingMenu(craftingFlag);
        }
    }
}