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

public class LiquorStore : GameLocation
{
    public LiquorStore() : base()
    {

    }
    public override string TypeName { get; set; } = "Liquor Store";
    public override int MapIcon { get; set; } = (int)BlipSprite.Bar;
    public override string ButtonPromptText { get; set; }
    public override int RegisterCashMin { get; set; } = 500;
    public override int RegisterCashMax { get; set; } = 1750;
    public override int PurchasePrice { get; set; } = 500000;
    public override int SalesPrice { get; set; } = 150000;
    public override int PayoutMin { get; set; } = 4000;
    public override int PayoutMax { get; set; } = 10000;
    public override bool IsOwnable { get; set; } = true;
    public LiquorStore(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
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
        possibleLocations.LiquorStores.Add(this);
        base.AddLocation(possibleLocations);
    }
}

