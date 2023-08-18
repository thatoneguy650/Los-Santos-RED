using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class GunStore : GameLocation
{
    private UIMenuItem completeTask;
    public GunStore() : base()
    {

    }

    public List<SpawnPlace> ParkingSpaces = new List<SpawnPlace>();
    public override bool ShowsOnDirectory { get; set; } = false;
    public override string TypeName { get; set; } = "Gun Store";
    public override int MapIcon { get; set; } = (int)BlipSprite.AmmuNation;
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
                    StoreCamera.SayGreeting = false;
                    StoreCamera.Setup();

                    CreateInteractionMenu();
                    Transaction = new Transaction(MenuPool, InteractionMenu, Menu, this);
                    Transaction.CreateTransactionMenu(Player, modItems, world, settings, weapons, time);
                    InteractionMenu.Visible = true;
                    Transaction.ProcessTransactionMenu();
                    if (ContactName == StaticStrings.UndergroundGunsContactName)
                    {
                        Player.RelationshipManager.GunDealerRelationship.AddMoneySpent(Transaction.MoneySpent);
                        player.RelationshipManager.GunDealerRelationship.SetReputation((Transaction.MoneySpent) / 5, false);
                    }
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
            }, "GangDenInteract");
        }
    }
    public override void AddDistanceOffset(Vector3 offsetToAdd)
    {
        foreach (SpawnPlace sp in ParkingSpaces)
        {
            sp.AddDistanceOffset(offsetToAdd);
        }
        base.AddDistanceOffset(offsetToAdd);
    }
}

