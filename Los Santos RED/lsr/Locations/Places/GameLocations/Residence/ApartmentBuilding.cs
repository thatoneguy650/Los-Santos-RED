using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class ApartmentBuilding : GameLocation
{
    private bool HasTransitionedToResidence = false;
    protected IPlacesOfInterest PlacesOfInterest;
    public ApartmentBuilding()
    {

    }
    public ApartmentBuilding(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        ButtonPromptText = $"Interact with {Name}";
        OpenTime = 0;
        CloseTime = 24;
    }
    public List<int> ResidenceIDs { get; set; } = new List<int>();
    [XmlIgnore]
    public List<Residence> Residences { get; set; } = new List<Residence>();
    public override string TypeName => "Apartment Building";
    public override void OnInteract()//ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        //Player = player;
        //ModItems = modItems;
        //World = world;
        //Settings = settings;
        //Weapons = weapons;
        //Time = time;
        //PlacesOfInterest = placesOfInterest;
        if (IsLocationClosed())
        {
            return;
        }
        if (!CanInteract)
        {
            return;
        }
        if (Interior != null && Interior.IsTeleportEntry)
        {
            Interior.Teleport(Player, this, null);
        }
        else
        {
            StandardInteract(null, false);
        }
    }
    public override void StandardInteract(LocationCamera locationCamera, bool isInside)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        HasTransitionedToResidence = false;
        GameFiber.StartNew(delegate
        {
            try
            {
                SetupLocationCamera(locationCamera, isInside, false);
                CreateInteractionMenu();
                InteractionMenu.Visible = true;
                if (!HasBannerImage)
                {
                    InteractionMenu.SetBannerType(EntryPoint.LSRedColor);
                }
                GenerateResidenceMenu();
                while (IsAnyMenuVisible || Time.IsFastForwarding)// || KeepInteractionGoing)
                {
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }
                if (!HasTransitionedToResidence)
                {
                    DisposeInteractionMenu();

                    DisposeCamera(isInside);
                    DisposeInterior();

                    //StoreCamera.Dispose();
                    Player.ActivityManager.IsInteractingWithLocation = false;
                    //CanInteract = true;
                    Player.IsTransacting = false;
                }
                Player.ActivityManager.IsInteractingWithLocation = false;

                Player.IsTransacting = false;
                CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "ApartmentInteract");
    }

    private void GenerateResidenceMenu()
    {
        foreach(int residenceID in ResidenceIDs)
        {
            Residence foundResidence = World.ModDataFileManager.PlacesOfInterest.PossibleLocations.Residences.FirstOrDefault(x => x.ResidenceID == residenceID);
            if(foundResidence == null)
            {
                continue;
            }
            UIMenuItem PurchaseResidenceMenuItem = new UIMenuItem($"{foundResidence.Name}{(foundResidence.IsOwned ? " - Owned" : foundResidence.IsRented ? " - Rented" : "") }", (foundResidence.IsOwnedOrRented ? "Select to enter this residence" : "Select to inquire about this residence"));
            PurchaseResidenceMenuItem.Activated += (sender, e) =>
            {
                HasTransitionedToResidence = true;
                sender.Visible = false;
                foundResidence.OnInteractFromApartment(Player,ModItems,World,Settings,Weapons,Time, PlacesOfInterest, StoreCamera);
            };
            InteractionMenu.AddItem(PurchaseResidenceMenuItem);       
        }
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.ApartmentBuildings.Add(this);
        base.AddLocation(possibleLocations);
    }
}

