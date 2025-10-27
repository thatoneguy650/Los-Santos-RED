using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class Restaurant : GameLocation
{
    public Restaurant() : base()
    {

    }
    public FoodType FoodType { get; set; } = FoodType.Generic;
    public override string TypeName { get; set; } = "Restaurant";
    public override int MapIcon { get; set; } = 621;
    public override string ButtonPromptText { get; set; }
    public override int RegisterCashMin { get; set; } = 200;
    public override int RegisterCashMax { get; set; } = 1450;
    public override int PurchasePrice { get; set; } = 600000;
    public override int SalesPrice { get; set; } = 200000;
    public override int PayoutMin { get; set; } = 5000;
    public override int PayoutMax { get; set; } = 16000;
    public override bool IsOwnable { get; set; } = true;
    public Restaurant(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID,FoodType foodType) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
        FoodType = foodType;
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Dine at {Name}";
        return true;
    }
    public override List<Tuple<string, string>> DirectoryInfo(int currentHour, float distanceTo)
    {
        List<Tuple<string, string>> BaseList = base.DirectoryInfo(currentHour, distanceTo).ToList();
        BaseList.Add(Tuple.Create("Type: ", FoodType.ToString()));
        return BaseList;
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.Restaurants.Add(this);
        base.AddLocation(possibleLocations);
    }
}

