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


public class CorruptCopInteraction
{
    private IContactInteractable Player;

    private MenuPool MenuPool;
    private UIMenu CopMenu;
    private iFruitContact LastAnsweredContact;
    private UIMenuItem PayoffCops;
    private UIMenuItem RequestCopWork;
    private IGangs Gangs;
    private IPlacesOfInterest PlacesOfInterest;
    private int CostToClearWanted
    {
        get
        {
            return Player.WantedLevel * 10000;
        }
    }
    public CorruptCopInteraction(IContactInteractable player, IGangs gangs, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        Gangs = gangs;
        PlacesOfInterest = placesOfInterest;
        MenuPool = new MenuPool();
    }
    public void Start(iFruitContact contact)
    {
        CopMenu = new UIMenu("", "Select an Option");
        CopMenu.RemoveBanner();
        MenuPool.Add(CopMenu);
        CopMenu.OnItemSelect += OnCopItemSelect;
        LastAnsweredContact = contact;
        PayoffCops = new UIMenuItem("Clear Wanted", "Ask your contact to have the cops forget about you") { RightLabel = CostToClearWanted.ToString("C0") };
        RequestCopWork = new UIMenuItem("Request Work", "Ask for some work from the cops");

        if (Player.IsWanted)
        {
            CopMenu.AddItem(PayoffCops);
        }
        else if (Player.IsNotWanted)
        {
            CopMenu.AddItem(RequestCopWork);
        }
        else
        {
            //CustomiFruit.Close();
            return;
        }
        CopMenu.Visible = true;
        GameFiber.StartNew(delegate
        {
            while (MenuPool.IsAnyMenuOpen())
            {
                GameFiber.Yield();
            }
            //CustomiFruit.Close();
        }, "CellPhone");
    }
    public void Update()
    {
        MenuPool.ProcessMenus();
    }
    private void OnCopItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == PayoffCops)
        {
            PayoffCop(LastAnsweredContact);
            CopMenu.Visible = false;
        }
        else if (selectedItem == RequestCopWork)
        {
            RequestWorkFromCop(LastAnsweredContact);
            CopMenu.Visible = false;
        }
    }
    private void RequestWorkFromCop(iFruitContact contact)
    {
        List<string> Replies = new List<string>() {
                "Nothing yet, I'll let you know",
                "I've got nothing for you yet",
                "Give me a few days",
                "Not a lot to be done right now",
                "We will let you know when you can do something for us",
                "Check back later.",
                };
        Player.CellPhone.AddPhoneResponse(contact.Name, contact.IconName, Replies.PickRandom());
        //CustomiFruit.Close(2000);
    }
    private void PayoffCop(iFruitContact contact)
    {

        EntryPoint.WriteToConsole($"Player.Money {Player.Money} CostToClearWanted {CostToClearWanted}");
        if (Player.WantedLevel > 4)
        {
            List<string> Replies = new List<string>() {
                $"Nothing I can do, you really fucked up.",
                $"Should have called me earlier",
                $"Too late now, you are on your own",
                $"Too many eyes on this one, should have let me known earlier",
                };
            Player.CellPhone.AddPhoneResponse(contact.Name, contact.IconName, Replies.PickRandom());

        }
        else if (Player.WantedLevel >= 3 && Player.PoliceResponse.HasHurtPolice)
        {
            List<string> Replies = new List<string>() {
                $"Shouldn't have messed with the cops so much, they really want you. It's outta my hands",
                $"They are out to get you since you fucked with the cops, nothing I can do.",
                $"Next time don't send so many cops to the hospital and maybe I can help",
                };
            Player.CellPhone.AddPhoneResponse(contact.Name, contact.IconName, Replies.PickRandom());
        }
        else if (Player.WantedLevel == 1)
        {
            List<string> Replies = new List<string>() {
                $"Why not just pay the fine?",
                $"Just stop and pay the fine",
                $"The fine is a lot cheaper than me",
                $"Why not pay the small fine instead of bothering me?",
                };
            Player.CellPhone.AddPhoneResponse(contact.Name, contact.IconName, Replies.PickRandom());
        }
        else if (Player.Money >= CostToClearWanted)
        {
            Player.GiveMoney(-1 * CostToClearWanted);

            GameFiber PayoffFiber = GameFiber.StartNew(delegate
            {
                int SleepTime = RandomItems.GetRandomNumberInt(5000, 10000);
                GameFiber.Sleep(SleepTime);
                Player.PayoffPolice();
                Player.SetWantedLevel(0, "Cop Payoff", true);

            }, "PayoffFiber");



            List<string> Replies = new List<string>() {
                $"Let me work my magic, hang on.",
                $"They should forget about you soon.",
                $"Let me make up some bullshit to distract them, wait a few.",
                $"Sending out an officer down across town, should get them off your tail for a while",
                };
            Player.CellPhone.AddPhoneResponse(contact.Name, contact.IconName, Replies.PickRandom());
        }
        else if (Player.Money < CostToClearWanted)
        {
            List<string> Replies = new List<string>() {
                $"Don't bother me unless you have some money",
                $"This shit isn't free you know",
                };
            Player.CellPhone.AddPhoneResponse(contact.Name, contact.IconName, Replies.PickRandom());
        }
        else
        {
            List<string> Replies = new List<string>() {
                $"Don't bother me",
                };
            Player.CellPhone.AddPhoneResponse(contact.Name, contact.IconName, Replies.PickRandom());
        }
    }
}

