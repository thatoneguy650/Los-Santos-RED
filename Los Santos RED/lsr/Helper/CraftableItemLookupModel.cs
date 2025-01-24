using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Helper.Crafting
{
    public class CraftableItemLookupModel
    {
        public string RecipeName { get; set; }
        public CraftableItem CraftableItem { get; set; }
        public Dictionary<string, Ingredient> IngredientLookup { get; set; }
    }
}
