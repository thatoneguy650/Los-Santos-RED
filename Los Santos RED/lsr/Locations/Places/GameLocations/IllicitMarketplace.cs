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

public class IllicitMarketplace : GameLocation
{
    public IllicitMarketplace() : base()
    {

    }
    public override bool ShowsOnDirectory { get; set; } = false;
    public override string TypeName { get; set; } = "Illicit Marketplace";
    public override int MapIcon { get; set; } = 514;//441;
    public override string ButtonPromptText { get; set; }
    public override int MinPriceRefreshHours { get; set; } = 12;
    public override int MaxPriceRefreshHours { get; set; } = 24;
    public override int MinRestockHours { get; set; } = 12;
    public override int MaxRestockHours { get; set; } = 24;
    public IllicitMarketplace(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Discretly shop At {Name}";
        return true;
    }
}

