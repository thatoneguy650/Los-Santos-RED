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

public class Dealership : GameLocation, ILicensePlatePreviewable
{

    public Dealership() : base()
    {

    }
    public override string TypeName { get; set; } = "Dealership";
    public override int MapIcon { get; set; } = 810;// (int)BlipSprite.GangVehicle;
    public override string ButtonPromptText { get; set; }
    public string LicensePlatePreviewText { get; set; } = "BUYMENOW";
    public Dealership(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Shop At {Name}";
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
            Player.ActivityManager.IsInteractingWithLocation = true;
            CanInteract = false;
            Player.IsTransacting = true;
            GameFiber.StartNew(delegate
            {
                try
                {
                    StoreCamera = new LocationCamera(this, Player, Settings);
                    StoreCamera.Setup();
                    CreateInteractionMenu();
                    Transaction = new Transaction(MenuPool, InteractionMenu, Menu, this);
                    Transaction.LicensePlatePreviewable = this;
                    Transaction.VehicleDeliveryLocations = VehicleDeliveryLocations;
                    Transaction.VehiclePreviewPosition = VehiclePreviewLocation;
                    Transaction.CreateTransactionMenu(Player, modItems, world, settings, weapons, time);
                    InteractionMenu.Visible = true;
                    Transaction.ProcessTransactionMenu();
                    Transaction.DisposeTransactionMenu();
                    DisposeInteractionMenu();
                    StoreCamera.Dispose();
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
}

