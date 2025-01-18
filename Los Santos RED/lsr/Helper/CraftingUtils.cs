using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Helper.Crafting
{
    static public class CraftingUtils
    {
        public static Dictionary<string, Ingredient> GetIngredientLookup(List<Ingredient> ingredient)
        {
            Dictionary<string, Ingredient> ingredientLookup = new Dictionary<string, Ingredient>();
            for(int i=0;i<ingredient.Count;i++)
            {
                ingredientLookup.Add(ingredient[i].IngredientName, ingredient[i]);
            }
            return ingredientLookup;
        }
    }
    public class CraftableItemLookupModel
    {
        public string RecipeName { get; set; }
        public CraftableItem CraftableItem { get; set; }
        public Dictionary<string, Ingredient> IngredientLookup { get; set; }
    }
}
