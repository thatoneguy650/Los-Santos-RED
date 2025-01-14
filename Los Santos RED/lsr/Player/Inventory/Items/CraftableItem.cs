using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

[Serializable]
public class CraftableItem
{

    public CraftableItem()
    {
    }
    public CraftableItem(string name, List<Ingredient> ingredients, bool isIllegal)
    {
        Name = name;
        Ingredients = ingredients;
        IsIllegal = isIllegal;
    }
    public string Name { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public bool? IsIllegal { get; set; }
    [XmlIgnore]
    private string _ingredientList;
    [XmlIgnore]
    public string IngredientList
    {
        get
        {
            if (_ingredientList == null)
            {
                _ingredientList = GetIngredients();
            }
            return _ingredientList;
        }
    }
    private string GetIngredients()
    {
        StringBuilder ingredientStringBuilder = new StringBuilder();
        foreach (var ingredient in Ingredients)
        {
            ingredientStringBuilder.Append($"X{ingredient.Quantity} {ingredient.IngredientName}\n");
        }
        return ingredientStringBuilder.ToString().Trim();
    }

}

public class Ingredient
{
    public string IngredientName { get; set; }
    public int Quantity { get; set; }
}
