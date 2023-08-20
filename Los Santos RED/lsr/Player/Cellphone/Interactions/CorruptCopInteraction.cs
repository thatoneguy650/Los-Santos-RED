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
    private UIMenu JobsSubMenu;
    private UIMenu ServicesSubMenu;

    private int CostToClearWanted
    {
        get
        {
            return Player.WantedLevel == 0 ? Settings.SettingsManager.PlayerOtherSettings.CorruptCopWantedClearCostScalar : Player.WantedLevel * Settings.SettingsManager.PlayerOtherSettings.CorruptCopWantedClearCostScalar;
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
        LastAnsweredContact = contact;
        AddJobs();
        AddServices();    
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

    private void AddServices()
    {
        ServicesSubMenu = MenuPool.AddSubMenu(CopMenu, "Services");
        ServicesSubMenu.RemoveBanner();
        PayoffCops = new UIMenuItem("Clear Wanted", "Ask your contact to have the cops forget about you") { RightLabel = "~r~" + CostToClearWanted.ToString("C0") + "~s~" };
        PayoffCops.Activated += (sender,e) =>
        {
            PayoffCop(LastAnsweredContact);
            sender.Visible = false;
        };
        PayoffCopsInvestigation = new UIMenuItem("Stop Investigation", "Ask your contact to have the cops forget about the current investigation") { RightLabel = "~r~" + CostToClearInvestigation.ToString("C0") + "~s~" };
        PayoffCopsInvestigation.Activated += (sender, e) =>
        {
            PayoffCopInvestigation(LastAnsweredContact);
            sender.Visible = false;
        };
        ServicesSubMenu.AddItem(PayoffCops);
        ServicesSubMenu.AddItem(PayoffCopsInvestigation);
        PayoffCops.Enabled = Player.IsWanted;
        PayoffCopsInvestigation.Enabled = Player.Investigation.IsActive;
    }

    private void AddJobs()
    {
        JobsSubMenu = MenuPool.AddSubMenu(CopMenu, "Jobs");
        JobsSubMenu.RemoveBanner();
        if (Player.PlayerTasks.HasTask(StaticStrings.OfficerFriendlyContactName))
        {
            TaskCancel = new UIMenuItem("Cancel Task", "Tell the officer you can't complete the task.") { RightLabel = "~o~$?~s~" };
            TaskCancel.Activated += (sender, e) =>
            {
                Player.PlayerTasks.CancelTask(StaticStrings.OfficerFriendlyContactName);
                sender.Visible = false;
            };
            JobsSubMenu.AddItem(TaskCancel);
            return;
        }
        GangHit = new UIMenuItem("Gang Hit", "Do a hit on a gang for the cops.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{Settings.SettingsManager.TaskSettings.OfficerFriendlyGangHitPaymentMin:C0}-{Settings.SettingsManager.TaskSettings.OfficerFriendlyGangHitPaymentMax:C0}~s~" };
        GangHit.Activated += (sender, e) =>
        {
            Player.PlayerTasks.CorruptCopTasks.CopGangHitTask.Start();
            sender.Visible = false;
        };
        WitnessElimination = new UIMenuItem("Witness Elimination", "Probably some major federal indictment of somebody who majorly does not want to get indicted.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{Settings.SettingsManager.TaskSettings.OfficerFriendlyWitnessEliminationPaymentMin:C0}-{Settings.SettingsManager.TaskSettings.OfficerFriendlyWitnessEliminationPaymentMax:C0}~s~" };
        WitnessElimination.Activated += (sender, e) =>
        {
            Player.PlayerTasks.CorruptCopTasks.WitnessEliminationTask.Start();
            sender.Visible = false;
        };
        CopHit = new UIMenuItem("Cop Hit", "Force the retirement of some of the LSPDs finest. ~r~WIP~s~") { RightLabel = $"~HUD_COLOUR_GREENDARK~{Settings.SettingsManager.TaskSettings.OfficerFriendlyCopHitPaymentMin:C0}-{Settings.SettingsManager.TaskSettings.OfficerFriendlyCopHitPaymentMax:C0}~s~" };
        CopHit.Activated += (sender, e) =>
        {
            Player.PlayerTasks.CorruptCopTasks.CopHitTask.Start();
            sender.Visible = false;
        };
        JobsSubMenu.AddItem(GangHit);
        JobsSubMenu.AddItem(WitnessElimination);
        //CopMenu.AddItem(CopHit);
    }

    public void Update()
    {
        MenuPool.ProcessMenus();
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

