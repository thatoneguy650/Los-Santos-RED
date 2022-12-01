using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GunDealerInteraction
{
    private IContactInteractable Player;
    private UIMenu GunDealerMenu;
    private MenuPool MenuPool;
    private IGangs Gangs;
    private IPlacesOfInterest PlacesOfInterest;
    private UIMenuItem RequestLocation;
    private UIMenuItem RequestWork;
    private UIMenu LocationSubMenu;
    private PhoneContact AnsweredContact;
    private UIMenuItem TaskCancel;
    private UIMenuItem GunPickup;
    private ISettingsProvideable Settings;

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
        GunDealerMenu = new UIMenu("", "Select an Option");
        GunDealerMenu.RemoveBanner();
        MenuPool.Add(GunDealerMenu);
        GunDealerMenu.OnItemSelect += OnTopItemSelect;


        GunPickup = new UIMenuItem("Gun Pickup", "Pickup some guns and bring them to a shop. ~r~WIP~s~") { RightLabel = $"~HUD_COLOUR_GREENDARK~{Settings.SettingsManager.TaskSettings.UndergroundGunsGunPickupPaymentMin:C0}-{Settings.SettingsManager.TaskSettings.UndergroundGunsGunPickupPaymentMax:C0}~s~" };

        if (Player.PlayerTasks.HasTask(StaticStrings.UndergroundGunsContactName))
        {
            TaskCancel = new UIMenuItem("Cancel Task", "Tell the gun dealer you can't complete the task.") { RightLabel = "~o~$?~s~" };
            GunDealerMenu.AddItem(TaskCancel);
        }
        else
        {
            GunDealerMenu.AddItem(GunPickup);
        }
        foreach (GunStore gl in PlacesOfInterest.PossibleLocations.GunStores)
        {
            if (gl.ContactName == StaticStrings.UndergroundGunsContactName && gl.IsEnabled)
            {
                GunDealerMenu.AddItem(new UIMenuItem(gl.Name, gl.Description + "~n~Address: " + gl.FullStreetAddress));
            }
        }
        GunDealerMenu.Visible = true;
        GameFiber.StartNew(delegate
        {
            while (MenuPool.IsAnyMenuOpen())
            {
                GameFiber.Yield();
            }
            Player.CellPhone.Close(250);
        }, "CellPhone");
    }
    private void OnTopItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == TaskCancel)
        {
            Player.PlayerTasks.CancelTask(StaticStrings.UndergroundGunsContactName);
            sender.Visible = false;
        }
        else if (selectedItem == GunPickup)
        {
            Player.PlayerTasks.UndergroundGunsTasks.GunPickupTask.Start();
            sender.Visible = false;
        }
        else
        {
            RequestLocations(selectedItem.Text);
            sender.Visible = false;
        }
    }

    public void Update()
    {
        MenuPool.ProcessMenus();
    }
    private void RequestLocations(string name)
    {
        GunStore gunStore = PlacesOfInterest.PossibleLocations.GunStores.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
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

