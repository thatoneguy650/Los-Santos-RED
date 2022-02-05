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

public class GangDen : TransactableLocation
{
    private StoreCamera StoreCamera;
    private UIMenuItem dropoffCash;
    private UIMenuItem dropoffItem;

    private IActivityPerformable Player;
    private IModItems ModItems;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private ITimeControllable Time;


    public GangDen() : base()
    {

    }
    public override BlipSprite MapIcon { get; set; } = BlipSprite.Shrink;
    public override Color MapIconColor { get; set; } = Color.White;
    public override float MapIconScale { get; set; } = 1.0f;
    public override string ButtonPromptText { get; set; }

    [XmlIgnore]
    public int ExpectedMoney { get; set; }
    [XmlIgnore]
    public int RepOnDropOff { get; set; }
    [XmlIgnore]
    public ModItem ExpectedItem { get; set; }
    [XmlIgnore]
    public Gang AssociatedGang { get; set; }


    public GangDen(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, ShopMenu shopMenu, Gang _gang) : base(_EntrancePosition, _EntranceHeading, _Name, _Description, shopMenu)
    {
        AssociatedGang = _gang;
        ButtonPromptText = $"Enter {AssociatedGang.ShortName} {AssociatedGang.DenName}";
    }
    public override void OnInteract(IActivityPerformable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;

        if (CanInteract)
        {
            Player.IsInteractingWithLocation = true;
            CanInteract = false;

            GameFiber.StartNew(delegate
            {
                StoreCamera = new StoreCamera(this, Player);
                StoreCamera.Setup();

                CreateInteractionMenu();
                CreateTransactionMenu(Player, modItems, world, settings, weapons, time);
                if (ExpectedMoney > 0)
                {
                    dropoffCash = new UIMenuItem("Drop Cash", "Drop off the expected amount of cash.") {RightLabel = $"${ExpectedMoney}" };
                    InteractionMenu.AddItem(dropoffCash);
                }
                if (ExpectedItem != null)
                {
                    dropoffItem = new UIMenuItem($"Drop off item", $"Drop off the {ExpectedItem.Name}.") { RightLabel = $"{ExpectedItem.Name}" };
                    InteractionMenu.AddItem(dropoffItem);
                }
                InteractionMenu.Visible = true;
                InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
                ProcessTransactionMenu();

                DisposeTransactionMenu();
                DisposeInteractionMenu();

                StoreCamera.Dispose();

                Player.IsInteractingWithLocation = false;
                CanInteract = true;
            }, "GangDenInteract");       
        }
    }
    private void InteractionMenu_OnItemSelect(RAGENativeUI.UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem.Text == "Buy")
        {
            SellMenu?.Dispose();
            PurchaseMenu?.Show();
        }
        else if (selectedItem.Text == "Sell")
        {
            PurchaseMenu?.Dispose();
            SellMenu?.Show();
        }
        else if (selectedItem == dropoffCash)
        {
            if(Player.Money >= ExpectedMoney)
            {
                Player.GiveMoney(-1*ExpectedMoney);
                Game.DisplayNotification(AssociatedGang.ContactIcon, AssociatedGang.ContactIcon, AssociatedGang.ContactName, "~g~Reply", "Thanks for the cash.");
                if(RepOnDropOff > 0)
                {
                    Player.GangRelationships.ChangeReputation(AssociatedGang, RepOnDropOff,false);
                }
                RepOnDropOff = 0;
                ExpectedMoney = 0;
                Player.PlayerTasks.CompletedTask(AssociatedGang.ContactName);
                InteractionMenu.Visible = false;
            }
            else
            {
                Game.DisplayNotification(AssociatedGang.ContactIcon,AssociatedGang.ContactIcon,AssociatedGang.ContactName,"~r~Reply","Come back when you actually have the cash.");
            }
        }
        else if (selectedItem == dropoffItem)
        {
            if(Player.Inventory.HasItem(ExpectedItem.Name))
            {
                Player.Inventory.Remove(ExpectedItem,1);
                Game.DisplayNotification(AssociatedGang.ContactIcon, AssociatedGang.ContactIcon, AssociatedGang.ContactName, "~g~Reply", $"Thanks for bringing us {ExpectedItem.Name}.");
                if (RepOnDropOff > 0)
                {
                    Player.GangRelationships.ChangeReputation(AssociatedGang, RepOnDropOff, false);
                }
                RepOnDropOff = 0;
                ExpectedItem = null;
                Player.PlayerTasks.CompletedTask(AssociatedGang.ContactName);
                InteractionMenu.Visible = false;
            }
            else
            {
                Game.DisplayNotification(AssociatedGang.ContactIcon, AssociatedGang.ContactIcon, AssociatedGang.ContactName, "~r~Reply", $"Come back when you actually have the {ExpectedItem.Name}.");
            }

        }
    }

}

