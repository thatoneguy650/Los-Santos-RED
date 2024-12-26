using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlasticSurgeryClinic : GameLocation
{
    public PlasticSurgeryClinic(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public PlasticSurgeryClinic() : base()
    {

    }
    public override string TypeName { get; set; } = "Plastic Surgery Clinic";
    public override int MapIcon { get; set; } = (int)BlipSprite.Barber;
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
        if (IsLocationClosed())
        {
            return;
        }
        if (CanInteract)
        {
            Game.DisplayHelp("Closed for Renovations. Check back Later.~r~WIP~s~");
        }
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.PlasticSurgeryClinics.Add(this);
        base.AddLocation(possibleLocations);
    }
}

