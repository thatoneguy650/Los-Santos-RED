using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;


public class TransactionInteraction
{

    private ILocationInteractable Player;
    private IModItems ModItems;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private ITimeControllable Time;
    private IPlacesOfInterest PlacesOfInterest;

    private InteractableLocation InteractableLocation;
    private Transaction Transaction;

    public TransactionInteraction(InteractableLocation interactableLocation, ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        InteractableLocation = interactableLocation;
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;
        PlacesOfInterest = placesOfInterest;
    }

    public void Start()
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        InteractableLocation.CanInteract = false;
        Player.IsTransacting = true;
        //GameFiber.StartNew(delegate
        //{
        //    StoreCamera = new LocationCamera(this, Player);
        //    StoreCamera.Setup();

        //    CreateInteractionMenu();
        //    Transaction = new Transaction(MenuPool, InteractionMenu, Menu, this);
        //    Transaction.CreateTransactionMenu(Player, modItems, world, settings, weapons, time);

        //    InteractionMenu.Visible = true;
        //    InteractionMenu.OnItemSelect += (sender, eChaseBehaviorFlag) =>
        //    {
        //        if (selectedItem.Text == "Buy")
        //        {
        //            Transaction?.SellMenu?.Dispose();
        //            Transaction?.PurchaseMenu?.Show();
        //        }
        //        else if (selectedItem.Text == "Sell")
        //        {
        //            Transaction?.PurchaseMenu?.Dispose();
        //            Transaction?.SellMenu?.Show();
        //        }
        //    };
        //    Transaction.ProcessTransactionMenu();

        //    Transaction.DisposeTransactionMenu();
        //    DisposeInteractionMenu();

        //    StoreCamera.Dispose();

        //    Player.ActivityManager.IsInteractingWithLocation = false;
        //    Player.IsTransacting = false;
        //    InteractableLocation.CanInteract = true;
        //}, "RestaurantInteract");
    }
}

