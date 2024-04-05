using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BeautyShop : GameLocation
{
    public BeautyShop(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public BeautyShop() : base()
    {

    }
    public override string TypeName { get; set; } = "Beauty Shop";
    public override int MapIcon { get; set; } = (int)BlipSprite.Barber;
    public int StandardHairColoringPrice { get; set; } = 45;
    public int StandardMakeupPrice { get; set; } = 55;
    public int PremiumColoringExtra { get; set; } = 25;
    public int PremiumMakeupExtra { get; set; } = 25;
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Shop At {Name}";
        return true;
    }

}

