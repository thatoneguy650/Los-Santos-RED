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

public class GasStation : GameLocation, IGasPumpable
{
    public GasStation() : base()
    {

    }
    public override string TypeName { get; set; } = "Gas Station";
    public override int MapIcon { get; set; } = 361;// (int)BlipSprite.JerryCan;
    public override string ButtonPromptText { get; set; }
    public int PricePerGallon { get; set; } = 3;
    public override int RegisterCashMin { get; set; } = 200;
    public override int RegisterCashMax { get; set; } = 1550;
    public GasStation(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
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
        possibleLocations.GasStations.Add(this);
        base.AddLocation(possibleLocations);
    }
}

