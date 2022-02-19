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

public class GunStore : TransactableLocation
{
    private StoreCamera StoreCamera;
    private IActivityPerformable Player;
    private IModItems ModItems;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private ITimeControllable Time;
    private UIMenuItem completeTask;

    public GunStore() : base()
    {

    }
    public override int MapIcon { get; set; } = (int)BlipSprite.AmmuNation;
    public override Color MapIconColor { get; set; } = Color.White;
    public override float MapIconScale { get; set; } = 1.0f;
    public override string ButtonPromptText { get; set; }
    public bool IsIllegalShop { get; set; } = false;
    public int MoneyToUnlock { get; set; } = 0;
    public GunStore(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description, menuID)
    {
        ButtonPromptText = $"Shop at {Name}";
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

                InteractionMenu.Visible = true;
                InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
                ProcessTransactionMenu();

                if (IsIllegalShop)
                {
                    Player.GunDealerRelationship.AddMoneySpent(PurchaseMenu.MoneySpent);
                }


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
    }

}

