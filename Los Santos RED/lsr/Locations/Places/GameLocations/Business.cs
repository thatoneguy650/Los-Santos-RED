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

public class Business : GameLocation
{
    public Business() : base()
    {

    }
    public override string TypeName { get; set; } = "Business";
    public override int MapIcon { get; set; } = 408;
    public override string ButtonPromptText { get; set; }
    public override int RegisterCashMin { get; set; } = 200;
    public override int RegisterCashMax { get; set; } = 1450;
    public Business(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Inquire about {Name}";
        return true;
    }
    public override List<Tuple<string, string>> DirectoryInfo(int currentHour, float distanceTo)
    {
        List<Tuple<string, string>> BaseList = base.DirectoryInfo(currentHour, distanceTo).ToList();
        return BaseList;
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.Businesses.Add(this);
        base.AddLocation(possibleLocations);
    }
}