using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class PoliceStation : InteractableLocation, ILocationDispatchable, ILocationRespawnable, ILocationAgencyAssignable
{
    public PoliceStation(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public PoliceStation() : base()
    {

    }
    public override string TypeName { get; set; } = "Police Station";
    public override int MapIcon { get; set; } = (int)BlipSprite.PoliceStation;
    public override Color MapIconColor { get; set; } = Color.White;
    public override float MapIconScale { get; set; } = 1.0f;
    //public List<ConditionalLocation> PossiblePedSpawns { get; set; }
    //public List<ConditionalLocation> PossibleVehicleSpawns { get; set; }
    //public string AssignedAgencyID { get; set; }
    //[XmlIgnore]
    //public Agency AssignedAgency { get; set; }
    //[XmlIgnore]
    //public bool IsDispatchFilled { get; set; } = false;


    public Vector3 RespawnLocation { get; set; }
    public float RespawnHeading { get; set; }

    public void StoreData(IAgencies agencies)
    {
        if (AssignedAgencyID != null)
        {
            AssignedAgency = agencies.GetAgency(AssignedAgencyID);
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
            Game.DisplayHelp("Police Personnel Only.~r~WIP~s~");
        }
    }

}

