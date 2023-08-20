﻿using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

public class GunDealerInteraction : IContactMenuInteraction
{
    private IContactInteractable Player;
    private UIMenu GunDealerMenu;
    private MenuPool MenuPool;
    private IGangs Gangs;
    private IPlacesOfInterest PlacesOfInterest;
    private PhoneContact AnsweredContact;
    private ISettingsProvideable Settings;
    private UIMenu LocationsSubMenu;
    private UIMenu JobsSubMenu;

    public GunDealerInteraction(IContactInteractable player, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings)
    {
        Player = player;
        Gangs = gangs;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        MenuPool = new MenuPool();
    }
    public void Start(PhoneContact contact)
    {
        AnsweredContact = contact;
        if(contact == null)
        {
            return;
        }
        GunDealerMenu = new UIMenu("", "Select an Option");
        GunDealerMenu.RemoveBanner();
        MenuPool.Add(GunDealerMenu);


        AddJobItems();
        AddLocationItems();

        GunDealerMenu.Visible = true;
        InteractionLoop();
    }
    private void InteractionLoop()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (MenuPool.IsAnyMenuOpen())
                {
                    GameFiber.Yield();
                }
                Player.CellPhone.Close(250);
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "CellPhone");
    }
    public void Update()
    {
        MenuPool.ProcessMenus();
    }

    private void AddLocationItems()
    {
        LocationsSubMenu = MenuPool.AddSubMenu(GunDealerMenu, "Locations");
        LocationsSubMenu.RemoveBanner();
        foreach (GunStore gl in PlacesOfInterest.PossibleLocations.GunStores.Where(x => x.ContactName == AnsweredContact.Name))
        {
            if (!gl.IsEnabled)
            {
                continue;
            }
            UIMenu locationsubMenu = MenuPool.AddSubMenu(LocationsSubMenu, gl.Name);
            locationsubMenu.RemoveBanner();

            UIMenuItem storeAddressRequest = new UIMenuItem("Request Directions", gl.Name + "~n~" + gl.Description + "~n~Address: " + gl.FullStreetAddress);
            storeAddressRequest.Activated += (sender, selectedItem) =>
            {
                RequestLocations(gl);
                sender.Visible = false;
            };
            locationsubMenu.AddItem(storeAddressRequest);
        }
    }

    private void AddJobItems()
    {
        JobsSubMenu = MenuPool.AddSubMenu(GunDealerMenu, "Jobs");
        JobsSubMenu.RemoveBanner();

        UIMenuItem TaskCancel = new UIMenuItem("Cancel Task", "Tell the gun dealer you can't complete the task.") { RightLabel = "~o~$?~s~" };
        TaskCancel.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.CancelTask(AnsweredContact.Name);
            sender.Visible = false;
        };
        if (Player.PlayerTasks.HasTask(AnsweredContact.Name))
        {
            JobsSubMenu.AddItem(TaskCancel);
            return;
        }
        UIMenuItem GunPickup = new UIMenuItem("Gun Pickup", "Pickup some guns and bring them to a shop.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{Settings.SettingsManager.TaskSettings.UndergroundGunsGunPickupPaymentMin:C0}-{Settings.SettingsManager.TaskSettings.UndergroundGunsGunPickupPaymentMax:C0}~s~" };
        GunPickup.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.UndergroundGunsTasks.GunPickupTask.Start();
            sender.Visible = false;
        };
        JobsSubMenu.AddItem(GunPickup);
    }
    private void RequestLocations(GunStore gunStore)
    {
        if (gunStore != null)
        {
            Player.GPSManager.AddGPSRoute(gunStore.Name, gunStore.EntrancePosition);
            List<string> Replies = new List<string>() {
                    $"Our shop is located on {gunStore.FullStreetAddress} come see us.",
                    $"Come check out our shop on {gunStore.FullStreetAddress}.",
                    $"You can find our shop on {gunStore.FullStreetAddress}.",
                    $"{gunStore.FullStreetAddress}.",
                    $"It's on {gunStore.FullStreetAddress} come see us.",
                    $"The shop? It's on {gunStore.FullStreetAddress}.",

                    };
            Player.CellPhone.AddPhoneResponse(AnsweredContact.Name, AnsweredContact.IconName, Replies.PickRandom());
        }
    }
}

