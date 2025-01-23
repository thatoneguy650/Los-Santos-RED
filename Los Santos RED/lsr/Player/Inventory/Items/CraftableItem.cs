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
    public CraftableItem(string name, string resultant, List<Ingredient> ingredients)
    {
        Name = name;
        Resultant = resultant;
        Ingredients = ingredients;
    }
    public string Name { get; set; }
    public string Resultant { get; set; }
    public int ResultantAmount { get; set; }
    public bool SingleUnit { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public int Cooldown { get; set;}
    public string CrimeId { get; set; }
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
