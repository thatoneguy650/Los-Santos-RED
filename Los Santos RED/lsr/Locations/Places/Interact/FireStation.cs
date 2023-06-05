using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class FireStation : GameLocation
{
    public FireStation(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public FireStation() : base()
    {

    }
    public override string TypeName { get; set; } = "Fire Station";
    public override int MapIcon { get; set; } = 436;

    public void StoreData(IAgencies agencies)
    {
        if (AssignedAssociationID != null)
        {
            AssignedAgency = agencies.GetAgency(AssignedAssociationID);
        }
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Enter {Name}";
        return true;
    }
    public override void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;
        if (IsLocationClosed())
        {
            return;
        }
        if (CanInteract)
        {
            Game.DisplayHelp("Closed for Renovations. Check back Later.~r~WIP~s~");
        }
    }
}

