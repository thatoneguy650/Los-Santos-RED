using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Mod;
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

public class Dealership : GameLocation, ILicensePlatePreviewable
{

    public Dealership() : base()
    {

    }
    public override string TypeName { get; set; } = "Dealership";
    public override int MapIcon { get; set; } = 811;//810;// (int)BlipSprite.GangVehicle;
    public override string ButtonPromptText { get; set; }
    public string LicensePlatePreviewText { get; set; } = "BUYMENOW";
    public override int RegisterCashMin { get; set; } = 1000;
    public override int RegisterCashMax { get; set; } = 3050;
    public override int RacketeeringAmountMin { get; set; } = 5000;
    public override int RacketeeringAmountMax { get; set; } = 10000;
    public Dealership(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
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
            DoEntranceCamera(true);
            Interior.Teleport(Player, this, StoreCamera);
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
        GameFiber.StartNew(delegate
        {
            try
            {
                SetupLocationCamera(locationCamera, isInside, true);
                CreateInteractionMenu();
                HandleVariableItems();
                Transaction = new Transaction(MenuPool, InteractionMenu, Menu, this);
                Transaction.LicensePlatePreviewable = this;
                Transaction.LocationCamera = StoreCamera;
                Transaction.VehicleDeliveryLocations = VehicleDeliveryLocations;
                Transaction.VehiclePreviewPosition = VehiclePreviewLocation;
                Transaction.CreateTransactionMenu(Player, ModItems, World, Settings, Weapons, Time);
                InteractionMenu.Visible = true;
                Transaction.ProcessTransactionMenu();
                Transaction.DisposeTransactionMenu();
                DisposeInteractionMenu();
                DisposeCamera(isInside);
                DisposeInterior();
                Player.ActivityManager.IsInteractingWithLocation = false;
                Player.IsTransacting = false;
                CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "CarDealershipInteract");
    }
    public override void AddDistanceOffset(Vector3 offsetToAdd)
    {
        foreach(SpawnPlace sp in VehicleDeliveryLocations)
        {
            sp.AddDistanceOffset(offsetToAdd);
        }
        VehiclePreviewLocation?.AddDistanceOffset(offsetToAdd);
        base.AddDistanceOffset(offsetToAdd);
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.CarDealerships.Add(this);
        base.AddLocation(possibleLocations);
    }
}

