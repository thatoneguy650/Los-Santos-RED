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
    public List<string> ResidenceIDs { get; set; } = new List<string>();
    [XmlIgnore]
    public List<Residence> Residences { get; set; } = new List<Residence>();

    public override string TypeName => "Apartment Building";

    public override void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;
        PlacesOfInterest = placesOfInterest;
        if (IsLocationClosed())
        {
            return;
        }
        if (!CanInteract)
        {
            return;
        }
        DoInteract();
    }
    private void DoInteract()
    {
        if (Interior != null && Interior.IsTeleportEntry)
        {
            LocationTeleporter locationTeleporter = new LocationTeleporter(Player, this, Settings);
            locationTeleporter.Teleport(null);
        }
        else
        {
            StandardInteract(false, null);
        }
    }
    public override void StandardInteract(bool isInside, LocationCamera locationCamera)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        HasTransitionedToResidence = false;
        GameFiber.StartNew(delegate
        {
            try
            {
                StoreCamera = new LocationCamera(this, Player, Settings, NoEntryCam || isInside);
                StoreCamera.SayGreeting = false;
                if (isInside && Interior != null)
                {
                    StoreCamera.IsInterior = true;
                    StoreCamera.Interior = Interior;
                }
                StoreCamera.Setup();
                CreateInteractionMenu();
                InteractionMenu.Visible = true;
                //InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
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
                //EntryPoint.WriteToConsole($"PLAYER EVENT: RESIDENCE LOOP CLOSING IsAnyMenuVisible {IsAnyMenuVisible} Time.IsFastForwarding {Time.IsFastForwarding}");
                if (!HasTransitionedToResidence)
                {
                    DisposeInteractionMenu();
                    StoreCamera.Dispose();
                }
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
                Player.IsTransacting = false;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "ResidenceInteract");
    }

    private void GenerateResidenceMenu()
    {
        foreach(string residenceID in ResidenceIDs)
        {
            Residence foundResidence = World.ModDataFileManager.PlacesOfInterest.PossibleLocations.Residences.FirstOrDefault(x => x.Name == residenceID);
            if(foundResidence == null)
            {
                continue;
            }
            UIMenuItem PurchaseResidenceMenuItem = new UIMenuItem(foundResidence.Name, "Select to interact with this residence");
            PurchaseResidenceMenuItem.Activated += (sender, e) =>
            {
                HasTransitionedToResidence = true;
                sender.Visible = false;
                foundResidence.OnInteractFromMainBuilding(Player,ModItems,World,Settings,Weapons,Time, PlacesOfInterest, StoreCamera);
            };
            InteractionMenu.AddItem(PurchaseResidenceMenuItem);       
        }
    }
}

