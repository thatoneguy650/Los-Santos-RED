using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class Bar : GameLocation
{
    public Bar() : base()
    {

    }
    public override string TypeName { get; set; } = "Bar";
    public override int MapIcon { get; set; } = (int)BlipSprite.Bar;
    public override string ButtonPromptText { get; set; }
    public Bar(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
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
        possibleLocations.Bars.Add(this);
        base.AddLocation(possibleLocations);
    }
}

