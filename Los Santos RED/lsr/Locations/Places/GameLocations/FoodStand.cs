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

public class FoodStand : GameLocation
{
    public FoodStand() : base()
    {

    }
    public override string TypeName { get; set; } = "Food Stand";
    public override int MapIcon { get; set; } = 480;
    public override float MapIconScale { get; set; } = 0.25f;
    public override string ButtonPromptText { get; set; }
    public FoodStand(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
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
        possibleLocations.FoodStands.Add(this);
        base.AddLocation(possibleLocations);
    }
}

