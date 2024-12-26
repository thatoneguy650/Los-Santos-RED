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

public class ClothingShop : GameLocation
{
    public ClothingShop() : base()
    {

    }
    public override string TypeName { get; set; } = "Clothing Store";
    public override int MapIcon { get; set; } = (int)BlipSprite.ClothesStore;
    public override string ButtonPromptText { get; set; }
    public Vector3 ChangingRoomLocation { get; set; }
    public override int RegisterCashMin { get; set; } = 300;
    public override int RegisterCashMax { get; set; } = 1550;
    public ClothingShop(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID, Vector3 changingRoomLocation) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
        ChangingRoomLocation = changingRoomLocation;
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Shop At {Name}";
        return true;
    }
    public override void OnInteract()//ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        //Player = player;
        //ModItems = modItems;
        //World = world;
        //Settings = settings;
        //Weapons = weapons;
        //Time = time;

        if (CanInteract)
        {
            Game.DisplayHelp("LSR Location Closed. Use the Ped Update/Create under the PedSwap menu to change clothing items on other peds.");
        }
        if (IsLocationClosed())
        {
            return;
        }

    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.ClothingShops.Add(this);
        base.AddLocation(possibleLocations);
    }
}

