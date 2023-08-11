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


public class CorruptCopInteraction : IContactMenuInteraction
{
    private IContactInteractable Player;

    private MenuPool MenuPool;
    private UIMenu CopMenu;
    private PhoneContact LastAnsweredContact;
    private UIMenuItem PayoffCops;
    private UIMenuItem PayoffCopsInvestigation;
   // private UIMenuItem RequestCopWork;
    private IGangs Gangs;
    private IPlacesOfInterest PlacesOfInterest;
    private ISettingsProvideable Settings;
    private UIMenuItem GangHit;
    private UIMenuItem TaskCancel;
    private UIMenuItem WitnessElimination;
    private UIMenuItem CopHit;

    private int CostToClearWanted
    {
        get
        {
            return Player.WantedLevel * Settings.SettingsManager.PlayerOtherSettings.CorruptCopWantedClearCostScalar;
        }
    }
    private int CostToClearInvestigation
    {
        get
        {
            return Settings.SettingsManager.PlayerOtherSettings.CorruptCopInvestigationClearCost;
        }
    }
    public CorruptCopInteraction(IContactInteractable player, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings)
    {
        Player = player;
        Gangs = gangs;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        MenuPool = new MenuPool();
    }
    public void Start(PhoneContact contact)
    {
        CopMenu = new UIMenu("", "Select an Option");
        CopMenu.RemoveBanner();
        MenuPool.Add(CopMenu);
        CopMenu.OnItemSelect += OnCopItemSelect;
        LastAnsweredContact = contact;


        PayoffCops = new UIMenuItem("Clear Wanted", "Ask your contact to have the cops forget about you") { RightLabel = "~r~" + CostToClearWanted.ToString("C0") + "~s~" };
        PayoffCopsInvestigation = new UIMenuItem("Stop Investigation", "Ask your contact to have the cops forget about the current investigation") { RightLabel = "~r~" + CostToClearInvestigation.ToString("C0") + "~s~" };

        GangHit = new UIMenuItem("Gang Hit", "Do a hit on a gang for the cops.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{Settings.SettingsManager.TaskSettings.OfficerFriendlyGangHitPaymentMin:C0}-{Settings.SettingsManager.TaskSettings.OfficerFriendlyGangHitPaymentMax:C0}~s~" };
        WitnessElimination = new UIMenuItem("Witness Elimination", "Probably some major federal indictment of somebody who majorly does not want to get indicted.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{Settings.SettingsManager.TaskSettings.OfficerFriendlyWitnessEliminationPaymentMin:C0}-{Settings.SettingsManager.TaskSettings.OfficerFriendlyWitnessEliminationPaymentMax:C0}~s~" };
        CopHit = new UIMenuItem("Cop Hit", "Force the retirement of some of the LSPDs finest. ~r~WIP~s~") { RightLabel = $"~HUD_COLOUR_GREENDARK~{Settings.SettingsManager.TaskSettings.OfficerFriendlyCopHitPaymentMin:C0}-{Settings.SettingsManager.TaskSettings.OfficerFriendlyCopHitPaymentMax:C0}~s~" };

        if (Player.PlayerTasks.HasTask(StaticStrings.OfficerFriendlyContactName))
        {
            TaskCancel = new UIMenuItem("Cancel Task", "Tell the officer you can't complete the task.") { RightLabel = "~o~$?~s~" };
            CopMenu.AddItem(TaskCancel);
        }


        if (Player.IsWanted)
        {
            CopMenu.AddItem(PayoffCops);
        }
        else if (Player.Investigation.IsActive)
        {
            CopMenu.AddItem(PayoffCopsInvestigation);
        }
        else if (Player.IsNotWanted)
        {
            CopMenu.AddItem(GangHit);
            CopMenu.AddItem(WitnessElimination);
            //CopMenu.AddItem(CopHit);
        }
        else
        {
            //CustomiFruit.Close();
            return;
        }
        CopMenu.Visible = true;
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
    private void OnCopItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == PayoffCops)
        {
            PayoffCop(LastAnsweredContact);
            CopMenu.Visible = false;
        }
        if (selectedItem == PayoffCopsInvestigation)
        {
            PayoffCopInvestigation(LastAnsweredContact);
            CopMenu.Visible = false;
        }
        else if (selectedItem == TaskCancel)
        {
            Player.PlayerTasks.CancelTask(StaticStrings.OfficerFriendlyContactName);
            sender.Visible = false;
        }
        else if (selectedItem == GangHit)
        {
            Player.PlayerTasks.CorruptCopTasks.CopGangHitTask.Start();
            sender.Visible = false;
        }
        else if (selectedItem == WitnessElimination)
        {
            Player.PlayerTasks.CorruptCopTasks.WitnessEliminationTask.Start();
            sender.Visible = false;
        }
        else if (selectedItem == CopHit)
        {
            Player.PlayerTasks.CorruptCopTasks.CopHitTask.Start();
            sender.Visible = false;
        }
    }
    private void PayoffCop(PhoneContact contact)
    {

        //EntryPoint.WriteToConsoleTestLong($"Player.Money {Player.BankAccounts.Money} CostToClearWanted {CostToClearWanted}");
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
            Player.CellPhone.AddPhoneResponse(contact.Name,contact.IconName, Replies.PickRandom());
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
        else if (Player.BankAccounts.Money >= CostToClearWanted)
        {
            Player.BankAccounts.GiveMoney(-1 * CostToClearWanted);

            GameFiber PayoffFiber = GameFiber.StartNew(delegate
            {
                try
                {
                    int SleepTime = RandomItems.GetRandomNumberInt(5000, 10000);
                    GameFiber.Sleep(SleepTime);
                    Player.Respawning.PayoffPolice();
                    Player.SetWantedLevel(0, "Cop Payoff", true);
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "PayoffFiber");
            List<string> Replies = new List<string>() {
                $"Let me work my magic, hang on.",
                $"They should forget about you soon.",
                $"Let me make up some bullshit to distract them, wait a few.",
                $"Sending out an officer down across town, should get them off your tail for a while",
                };
            Player.CellPhone.AddPhoneResponse(contact.Name, contact.IconName, Replies.PickRandom());
        }
        else if (Player.BankAccounts.Money < CostToClearWanted)
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
    private void PayoffCopInvestigation(PhoneContact contact)
    {

        //EntryPoint.WriteToConsoleTestLong($"Player.Money {Player.BankAccounts.Money} CostToClearInvestigation {CostToClearInvestigation}");
        if (Player.BankAccounts.Money >= CostToClearInvestigation)
        {
            Player.BankAccounts.GiveMoney(-1 * CostToClearInvestigation);

            GameFiber PayoffFiber = GameFiber.StartNew(delegate
            {
                try
                {
                    int SleepTime = RandomItems.GetRandomNumberInt(5000, 10000);
                    GameFiber.Sleep(SleepTime);
                    Player.Respawning.PayoffPolice();
                    Player.SetWantedLevel(0, "Cop Payoff", true);
                    Player.Investigation.Expire();
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "PayoffFiber");
            List<string> Replies = new List<string>() {
                $"Let me work my magic, hang on.",
                $"They should forget about you soon.",
                $"Let me make up some bullshit to distract them, wait a few.",
                $"Sending out an officer down across town, should get them off your tail for a while",
                };
            Player.CellPhone.AddPhoneResponse(contact.Name, contact.IconName, Replies.PickRandom());
        }
        else if (Player.BankAccounts.Money < CostToClearInvestigation)
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

