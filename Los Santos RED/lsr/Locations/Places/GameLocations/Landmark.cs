using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class Landmark : GameLocation
{
    public Landmark(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        OpenTime = 0;
        CloseTime = 24;
    }
    public Landmark() : base()
    {

    }
    public override string TypeName { get; set; } = "Landmark";
    public override int MapIcon { get; set; } = 837;//873;//162;
    public override float MapIconScale { get; set; } = 0.5f;
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Interact With {Name}";
        return true;
    }
    public override void OnInteract()//ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        if (IsLocationClosed())
        {
            return;
        }
        if (CanInteract)
        {
            Game.DisplayHelp("~r~No Interaction~s~");
        }
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.Landmarks.Add(this);
        base.AddLocation(possibleLocations);
    }
}

