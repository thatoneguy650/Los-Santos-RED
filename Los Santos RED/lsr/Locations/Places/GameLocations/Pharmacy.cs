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

public class Pharmacy : GameLocation
{
    public Pharmacy() : base()
    {

    }
    public override string TypeName { get; set; } = "Pharmacy";
    public override int MapIcon { get; set; } = (int)BlipSprite.CriminalDrugs;
    public override string ButtonPromptText { get; set; }
    public Pharmacy(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
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
        possibleLocations.Pharmacies.Add(this);
        base.AddLocation(possibleLocations);
    }
}

