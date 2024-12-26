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

public class PawnShop : GameLocation
{
    public PawnShop() : base()
    {

    }


    public override string TypeName { get; set; } = "Pawn Shop";
    public override int MapIcon { get; set; } = (int)BlipSprite.PointOfInterest;
    public override string ButtonPromptText { get; set; }
    public override int MinPriceRefreshHours { get; set; } = 12;
    public override int MaxPriceRefreshHours { get; set; } = 24;
    public override int MinRestockHours { get; set; } = 12;
    public override int MaxRestockHours { get; set; } = 24;
    public PawnShop(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Shop At {Name}";
        return true;
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.PawnShops.Add(this);
        base.AddLocation(possibleLocations);
    }
}

