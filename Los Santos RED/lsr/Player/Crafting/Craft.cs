using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Linq;
using LosSantosRED.lsr.Helper.Crafting;
using System.Collections.Generic;
using Rage.Native;
using LosSantosRED.lsr.Helper;

namespace Mod
{
    public class Crafting
    {
        private Player Player;
        private IModItems ModItems;
        private ISettingsProvideable Settings;
        private IWeapons Weapons;
        public bool IsCrafting { get; private set; } = false;
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
            foreach (CraftableItem craftableItem in CraftableItems.Items)
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
        private void PerformAnimation(CraftableItem craftItem)
        {
            if(Player.ActivityManager.IsInteractingWithLocation && !Player.InteriorManager.IsInsideTeleportInterior)
            {
                return;
            }
            string dictionary = "missmechanic";
            string animation = "work2_base";

            //anim@scripted@ulp_missions@paperwork@male@
            //action
            if (craftItem != null && craftItem.HasCustomAnimations)
            {
                dictionary = craftItem.AnimationDictionary;
                animation = craftItem.AnimationName;
            }
            if(!AnimationDictionary.RequestAnimationDictionayResult(dictionary))
            {
                return;
            }

            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, dictionary, animation, 4.0f, -4.0f, -1, 1, 0, false, false, false);
        }
        public void CraftItem(string productName, int quantity = 1, string craftingFlag = null)
        {
            if (IsCrafting)
            {
                Game.DisplayHelp("Cannot start crafting, ~r~Cooldown active.");
                return;
            }
            Player.ActivityManager.StopDynamicActivity();
            CraftableItemLookupModel craftItem = CraftableItems.CraftablesLookup[productName];


            if (craftItem == null)
            {
                Game.DisplayHelp("Cannot start crafting.");
                return;
            }

            CraftableItem finalCraftableItem = craftItem.CraftableItem;

            if (finalCraftableItem == null)
            {
                Game.DisplayHelp("Cannot start crafting.");
                return;
            }

            ModItem itemToGive = ModItems.Get(finalCraftableItem.Resultant);

            if(itemToGive == null)
            {
                Game.DisplayHelp("Cannot start crafting.");
                return;
            }

            int finalQuantity = quantity * (finalCraftableItem.SingleUnit ? 1 : finalCraftableItem.ResultantAmount);

            CraftingMenu.Hide();
            DeductIngredientsFromInventory(craftItem, quantity);
            Player.IsSetDisabledControlsWithCamera = true;
            IsCrafting = true;

            PerformAnimation(finalCraftableItem);

            if (!string.IsNullOrEmpty(finalCraftableItem.CrimeId))
            {
                Player.Violations.SetContinuouslyViolating(finalCraftableItem.CrimeId);
            }


            uint GameTimeStartedCrafting = Game.GameTime;
            Player.ButtonPrompts.AttemptAddPrompt("craftingStop", "Stop Crafting", "stopcraftingprompt1", Settings.SettingsManager.KeySettings.InteractCancel, 999);
            int craftedQuantity = 0;
            EntryPoint.WriteToConsole($"craftedQuantity{craftedQuantity} finalQuantity{finalQuantity}");
            while (craftedQuantity < finalQuantity)//Game.GameTime - GameTimeStartedCrafting <= (finalCraftableItem.Cooldown * finalQuantity))
            {
                if (!Player.IsAliveAndFree || Player.IsUnconscious || Player.ButtonPrompts.IsPressed("stopcraftingprompt1"))
                {
                    IsCrafting = false;
                    if (!string.IsNullOrEmpty(finalCraftableItem.CrimeId))
                    {
                        Player.Violations.StopContinuouslyViolating(finalCraftableItem.CrimeId);
                    }
                    Player.IsSetDisabledControlsWithCamera = false;

                    if(Player.ButtonPrompts.IsPressed("stopcraftingprompt1"))
                    {
                        Player.ButtonPrompts.RemovePrompts("craftingStop");
                        Game.DisplayHelp("Crafting cancelled.");
                    }
                    else
                    {
                        Game.DisplayHelp("Crafting failed.");
                    }
                    
                    NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                    return;
                }
                if(Game.GameTime - GameTimeStartedCrafting >= finalCraftableItem.Cooldown)
                {
                    GameTimeStartedCrafting = Game.GameTime;
                    itemToGive.AddToPlayerInventory(Player, 1);
                    NativeHelper.PlaySuccessSound();
                    craftedQuantity++;
                    if (craftedQuantity < finalQuantity)
                    {
                        Game.DisplaySubtitle($"Crafted {productName} {craftedQuantity}/{finalQuantity} {itemToGive.MeasurementName}(s)", finalCraftableItem.Cooldown);
                    }
                    EntryPoint.WriteToConsole($"CRAFTED ONE craftedQuantity{craftedQuantity} finalQuantity{finalQuantity}");
                }
                GameFiber.Yield();
            }
            Player.ButtonPrompts.RemovePrompts("craftingStop");   
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            IsCrafting = false; 
            if (!string.IsNullOrEmpty(finalCraftableItem.CrimeId))
            {
                Player.Violations.StopContinuouslyViolating(finalCraftableItem.CrimeId);
            }
            Player.IsSetDisabledControlsWithCamera = false;
            Game.DisplaySubtitle($"Crafted {productName} - {finalQuantity} {itemToGive.MeasurementName}(s)");
        }
    }
}