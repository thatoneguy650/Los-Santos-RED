﻿using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class GunStore : InteractableLocation
{
    private LocationCamera StoreCamera;
    private ILocationInteractable Player;
    private IModItems ModItems;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private ITimeControllable Time;
    private UIMenuItem completeTask;
    private Transaction Transaction;
    public GunStore() : base()
    {

    }


    //[XmlIgnore]
    //public ShopMenu Menu { get; set; }
    //public string MenuID { get; set; }


    //public Vector3 ParkingSpot { get; set; }
    //public float ParkingHeading { get; set; }
    public List<SpawnPlace> ParkingSpaces = new List<SpawnPlace>();
    public override bool ShowsOnDirectory { get; set; } = false;
    public override string TypeName { get; set; } = "Gun Store";
    public override int MapIcon { get; set; } = (int)BlipSprite.AmmuNation;
    public override Color MapIconColor { get; set; } = Color.White;
    public override float MapIconScale { get; set; } = 1.0f;
    public override string ButtonPromptText { get; set; }
    public int MoneyToUnlock { get; set; } = 0;
    public string ContactName { get; set; } = "";

    public GunStore(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
        ButtonPromptText = $"Shop at {Name}";
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        return true;
    }
    public override void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;

        if (CanInteract)
        {
            Player.ActivityManager.IsInteractingWithLocation = true;
            CanInteract = false;
            Player.IsTransacting = true;
            GameFiber.StartNew(delegate
            {
                StoreCamera = new LocationCamera(this, Player);
                StoreCamera.SayGreeting = false;
                StoreCamera.Setup();

                CreateInteractionMenu();
                Transaction = new Transaction(MenuPool, InteractionMenu, Menu, this);
                Transaction.CreateTransactionMenu(Player, modItems, world, settings, weapons, time);

                InteractionMenu.Visible = true;
                InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
                Transaction.ProcessTransactionMenu();

                if (ContactName == EntryPoint.UndergroundGunsContactName)
                {
                    Player.RelationshipManager.GunDealerRelationship.AddMoneySpent(Transaction.PurchaseMenu.MoneySpent);
                    Player.RelationshipManager.GunDealerRelationship.AddMoneySpent(Transaction.SellMenu.MoneySpent);


                    player.RelationshipManager.GunDealerRelationship.SetReputation((Transaction.PurchaseMenu.MoneySpent + Transaction.SellMenu.MoneySpent) / 5, false);
                }

                Transaction.DisposeTransactionMenu();
                DisposeInteractionMenu();

                StoreCamera.Dispose();

                Player.ActivityManager.IsInteractingWithLocation = false;
                Player.IsTransacting = false;
                CanInteract = true;
            }, "GangDenInteract");
        }
    }
    private void InteractionMenu_OnItemSelect(RAGENativeUI.UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem.Text == "Buy")
        {
            Transaction?.SellMenu?.Dispose();
            Transaction?.PurchaseMenu?.Show();
        }
        else if (selectedItem.Text == "Sell")
        {
            Transaction?.PurchaseMenu?.Dispose();
            Transaction?.SellMenu?.Show();
        }
    }

}

