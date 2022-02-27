using ExtensionsMethods;
using iFruitAddon2;
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
    private iFruitContact AnsweredContact;
    public GunDealerInteraction(IContactInteractable player, IGangs gangs, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        Gangs = gangs;
        PlacesOfInterest = placesOfInterest;
        MenuPool = new MenuPool();
    }
    public void Start(iFruitContact contact)
    {
        AnsweredContact = contact;
        GunDealerMenu = new UIMenu("", "Select an Option");
        GunDealerMenu.RemoveBanner();
        MenuPool.Add(GunDealerMenu);
        GunDealerMenu.OnItemSelect += OnTopItemSelect;
        RequestWork = new UIMenuItem("Request Work", "Ask for some work from the gun dealers, better be strapped");
        GunDealerMenu.AddItem(RequestWork);
        foreach (GunStore gl in PlacesOfInterest.PossibleLocations.GunStores)
        {
            if (gl.IsIllegalShop && gl.IsEnabled)
            {
                GunDealerMenu.AddItem(new UIMenuItem(gl.Name, gl.Description + "~n~Address: " + gl.StreetAddress));
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
        if(selectedItem == RequestWork)
        {
            List<string> Replies = new List<string>() {
                "Nothing yet, I'll let you know",
                "I've got nothing for you yet",
                "Give me a few days",
                "Not a lot to be done right now",
                "We will let you know when you can do something for us",
                "Check back later.",
                };
            Player.CellPhone.AddPhoneResponse(AnsweredContact.Name, AnsweredContact.IconName, Replies.PickRandom());
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
            Player.AddGPSRoute(gunStore.Name, gunStore.EntrancePosition);
            List<string> Replies = new List<string>() {
                    $"Our shop is located on {gunStore.StreetAddress} come see us.",
                    $"Come check out our shop on {gunStore.StreetAddress}.",
                    $"You can find our shop on {gunStore.StreetAddress}.",
                    $"{gunStore.StreetAddress}.",
                    $"It's on {gunStore.StreetAddress} come see us.",
                    $"The shop? It's on {gunStore.StreetAddress}.",

                    };
            Player.CellPhone.AddPhoneResponse(AnsweredContact.Name, AnsweredContact.IconName, Replies.PickRandom());
        }
    }
}

