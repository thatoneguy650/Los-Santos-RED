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
    private UIMenuItem PayoffCops;
    private UIMenuItem PayoffCopsInvestigation;
    private IGangs Gangs;
    private IPlacesOfInterest PlacesOfInterest;
    private ISettingsProvideable Settings;
    //private UIMenuListScrollerItem<Gang> StartGangHitMenu;
    private UIMenuItem TaskCancel;
    //private UIMenuItem StartWitnessEliminationMenu;
    //private UIMenuItem StartCopHitMenu;
    private UIMenu JobsSubMenu;
    private UIMenu ServicesSubMenu;
    private CorruptCopContact Contact;
    private UIMenuItem PayoffCopsAPB;
    private IAgencies Agencies;

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
    private int CostToClearAPB
    {
        get
        {
            if(Player.CriminalHistory.MaxWantedLevel == 0)
            {
                return Settings.SettingsManager.PlayerOtherSettings.CorruptCopAPBClearCost;
            }
            return Settings.SettingsManager.PlayerOtherSettings.CorruptCopAPBClearCost * Player.CriminalHistory.MaxWantedLevel;
        }
    }
    public CorruptCopInteraction(IContactInteractable player, IGangs gangs, IPlacesOfInterest placesOfInterest, ISettingsProvideable settings, CorruptCopContact contact, IAgencies agencies)
    {
        Player = player;
        Gangs = gangs;
        PlacesOfInterest = placesOfInterest;
        Settings = settings;
        Contact = contact;
        Agencies = agencies;
        MenuPool = new MenuPool();
    }
    public void Start(PhoneContact contact)
    {
        CopMenu = new UIMenu("", "Select an Option");
        CopMenu.RemoveBanner();
        MenuPool.Add(CopMenu);
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
            PayoffCop();
            sender.Visible = false;
        };
        PayoffCopsInvestigation = new UIMenuItem("Stop Investigation", "Ask your contact to have the cops forget about the current investigation") { RightLabel = "~r~" + CostToClearInvestigation.ToString("C0") + "~s~" };
        PayoffCopsInvestigation.Activated += (sender, e) =>
        {
            PayoffCopInvestigation();
            sender.Visible = false;
        };
        PayoffCopsAPB = new UIMenuItem("Clear APB/BOLO", "Ask your contact to have the cops forget about your APB/BOLO") { RightLabel = "~r~" + CostToClearAPB.ToString("C0") + "~s~" };
        PayoffCopsAPB.Activated += (sender, e) =>
        {
            PayoffCopAPB();
            sender.Visible = false;
        };
        ServicesSubMenu.AddItem(PayoffCops);
        ServicesSubMenu.AddItem(PayoffCopsInvestigation);
        ServicesSubMenu.AddItem(PayoffCopsAPB);
        PayoffCops.Enabled = Player.IsWanted;
        PayoffCopsInvestigation.Enabled = Player.Investigation.IsActive;
        PayoffCopsAPB.Enabled = Player.CriminalHistory.HasHistory;
    }
    private void AddJobs()
    {
        JobsSubMenu = MenuPool.AddSubMenu(CopMenu, "Jobs");
        JobsSubMenu.RemoveBanner();
        if (Player.PlayerTasks.HasTask(Contact.Name))
        {
            TaskCancel = new UIMenuItem("Cancel Task", "Tell the officer you can't complete the task.") { RightLabel = "~o~$?~s~" };
            TaskCancel.Activated += (sender, e) =>
            {
                Player.PlayerTasks.CancelTask(Contact);
                sender.Visible = false;
            };
            JobsSubMenu.AddItem(TaskCancel);
            return;
        }
        AddGangHitSubMenu();
        AddCopHitSubMenu();
        AddWitnessEliminationSubMenu();
    }
    private void AddGangHitSubMenu()
    {
        UIMenu gangHitSubMenu = MenuPool.AddSubMenu(JobsSubMenu, "Gang Hit");
        JobsSubMenu.MenuItems[JobsSubMenu.MenuItems.Count() - 1].Description = $"Do a hit on a gang for the cops.";
        JobsSubMenu.MenuItems[JobsSubMenu.MenuItems.Count() - 1].RightLabel = $"~HUD_COLOUR_GREENDARK~{Settings.SettingsManager.TaskSettings.OfficerFriendlyGangHitPaymentMin:C0}-{Settings.SettingsManager.TaskSettings.OfficerFriendlyGangHitPaymentMax:C0}~s~";
        gangHitSubMenu.RemoveBanner();
        UIMenuListScrollerItem<Gang> TargetMenu = new UIMenuListScrollerItem<Gang>("Target Gang", $"Choose a target gang.", Gangs.AllGangs.ToList());
        UIMenuNumericScrollerItem<int> TargetCountMenu = new UIMenuNumericScrollerItem<int>("Targets", $"Select the number of targets", 1, 3, 1) { Value = 1 };
        UIMenuItem StartTaskMenu = new UIMenuItem("Start", "Start the task.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{Settings.SettingsManager.TaskSettings.OfficerFriendlyGangHitPaymentMin:C0}-{Settings.SettingsManager.TaskSettings.OfficerFriendlyGangHitPaymentMax:C0}~s~" };
        StartTaskMenu.Activated += (sender, e) =>
        {
            Player.PlayerTasks.CorruptCopTasks.StartCopGangHitTask(Contact, TargetMenu.SelectedItem, TargetCountMenu.Value);
            sender.Visible = false;
        };
        TargetCountMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            StartTaskMenu.RightLabel = $"~HUD_COLOUR_GREENDARK~{Settings.SettingsManager.TaskSettings.OfficerFriendlyGangHitPaymentMin * TargetCountMenu.Value:C0}-{Settings.SettingsManager.TaskSettings.OfficerFriendlyGangHitPaymentMax * TargetCountMenu.Value:C0}~s~";
        };
        gangHitSubMenu.AddItem(TargetMenu);
        gangHitSubMenu.AddItem(TargetCountMenu);
        gangHitSubMenu.AddItem(StartTaskMenu);
    }
    private void AddWitnessEliminationSubMenu()
    {
        UIMenu witnessEliminationSubMenu = MenuPool.AddSubMenu(JobsSubMenu, "Witness Elimination");
        JobsSubMenu.MenuItems[JobsSubMenu.MenuItems.Count() - 1].Description = $"Probably some major federal indictment of somebody who majorly does not want to get indicted.";
        JobsSubMenu.MenuItems[JobsSubMenu.MenuItems.Count() - 1].RightLabel = $"~HUD_COLOUR_GREENDARK~{Settings.SettingsManager.TaskSettings.OfficerFriendlyWitnessEliminationPaymentMin:C0}-{Settings.SettingsManager.TaskSettings.OfficerFriendlyWitnessEliminationPaymentMax:C0}~s~";
        witnessEliminationSubMenu.RemoveBanner();
        UIMenuItem StartTaskMenu = new UIMenuItem("Start Task", "Start the task.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{Settings.SettingsManager.TaskSettings.OfficerFriendlyWitnessEliminationPaymentMin:C0}-{Settings.SettingsManager.TaskSettings.OfficerFriendlyWitnessEliminationPaymentMax:C0}~s~" };
        StartTaskMenu.Activated += (sender, e) =>
        {
            Player.PlayerTasks.CorruptCopTasks.StartWitnessEliminationTask(Contact);
            sender.Visible = false;
        };
        witnessEliminationSubMenu.AddItem(StartTaskMenu);
    }
    private void AddCopHitSubMenu()
    {
        UIMenu gangHitSubMenu = MenuPool.AddSubMenu(JobsSubMenu, "Cop Hit");
        JobsSubMenu.MenuItems[JobsSubMenu.MenuItems.Count() - 1].Description = $"Give the boys-in-blue a forced retirement.";// Force the retirement of some of the boys in blue.";
        JobsSubMenu.MenuItems[JobsSubMenu.MenuItems.Count() - 1].RightLabel = $"~HUD_COLOUR_GREENDARK~{Settings.SettingsManager.TaskSettings.OfficerFriendlyCopHitPaymentMin:C0}-{Settings.SettingsManager.TaskSettings.OfficerFriendlyCopHitPaymentMax:C0}~s~";
        gangHitSubMenu.RemoveBanner();
        UIMenuListScrollerItem<Agency> TargetMenu = new UIMenuListScrollerItem<Agency>("Target Agency", $"Choose a target agency.", Agencies.GetAgenciesByResponse(ResponseType.LawEnforcement).Where(x => x.ID != "UNK"));
        UIMenuNumericScrollerItem<int> TargetCountMenu = new UIMenuNumericScrollerItem<int>("Targets", $"Select the number of targets", 1, 3, 1) { Value = 1 };
        UIMenuItem StartTaskMenuItem = new UIMenuItem("Start", "Start the task.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{Settings.SettingsManager.TaskSettings.OfficerFriendlyCopHitPaymentMin:C0}-{Settings.SettingsManager.TaskSettings.OfficerFriendlyCopHitPaymentMax:C0}~s~" };
        StartTaskMenuItem.Activated += (sender, e) =>
        {
            Player.PlayerTasks.CorruptCopTasks.StartCopHitTask(Contact, TargetMenu.SelectedItem,TargetCountMenu.Value);
            sender.Visible = false;
        };
        TargetCountMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            StartTaskMenuItem.RightLabel = $"~HUD_COLOUR_GREENDARK~{Settings.SettingsManager.TaskSettings.OfficerFriendlyCopHitPaymentMin * TargetCountMenu.Value:C0}-{Settings.SettingsManager.TaskSettings.OfficerFriendlyCopHitPaymentMax * TargetCountMenu.Value:C0}~s~";
        };
        gangHitSubMenu.AddItem(TargetMenu);
        gangHitSubMenu.AddItem(TargetCountMenu);
        gangHitSubMenu.AddItem(StartTaskMenuItem);
    }

    public void Update()
    {
        MenuPool.ProcessMenus();
    }
    private void PayoffCop()
    {
        //EntryPoint.WriteToConsoleTestLong($"Player.Money {Player.BankAccounts.Money} CostToClearWanted {CostToClearWanted}");
        if (!Settings.SettingsManager.PlayerOtherSettings.CorruptCopClearCanAlwaysClearWanted && Player.WantedLevel > 4)
        {
            List<string> Replies = new List<string>() {
                $"Nothing I can do, you really fucked up.",
                $"Should have called me earlier",
                $"Too late now, you are on your own",
                $"Too many eyes on this one, should have let me known earlier",
                };
            Player.CellPhone.AddPhoneResponse(Contact.Name, Contact.IconName, Replies.PickRandom());

        }
        else if (!Settings.SettingsManager.PlayerOtherSettings.CorruptCopClearCanAlwaysClearWanted && Player.WantedLevel >= 3 && Player.PoliceResponse.HasHurtPolice)
        {
            List<string> Replies = new List<string>() {
                $"Shouldn't have messed with the cops so much, they really want you. It's outta my hands",
                $"They are out to get you since you fucked with the cops, nothing I can do.",
                $"Next time don't send so many cops to the hospital and maybe I can help",
                };
            Player.CellPhone.AddPhoneResponse(Contact.Name, Contact.IconName, Replies.PickRandom());
        }
        else if (!Settings.SettingsManager.PlayerOtherSettings.CorruptCopClearCanAlwaysClearWanted && Player.WantedLevel == 1)
        {
            List<string> Replies = new List<string>() {
                $"Why not just pay the fine?",
                $"Just stop and pay the fine",
                $"The fine is a lot cheaper than me",
                $"Why not pay the small fine instead of bothering me?",
                };
            Player.CellPhone.AddPhoneResponse(Contact.Name, Contact.IconName, Replies.PickRandom());
        }
        else if (Player.BankAccounts.GetMoney(false) >= CostToClearWanted)
        {
            Player.BankAccounts.GiveMoney(-1 * CostToClearWanted, false);

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
            Player.CellPhone.AddPhoneResponse(Contact.Name, Contact.IconName, Replies.PickRandom());
        }
        else if (Player.BankAccounts.GetMoney(false) < CostToClearWanted)
        {
            List<string> Replies = new List<string>() {
                $"Don't bother me unless you have some cash",
                $"This shit isn't free you know, make sure you've got the cash",
                };
            Player.CellPhone.AddPhoneResponse(Contact.Name, Contact.IconName, Replies.PickRandom());
        }
        else
        {
            List<string> Replies = new List<string>() {
                $"Don't bother me",
                };
            Player.CellPhone.AddPhoneResponse(Contact.Name, Contact.IconName, Replies.PickRandom());
        }
    }
    private void PayoffCopInvestigation()
    {
        if (Player.BankAccounts.GetMoney(false) >= CostToClearInvestigation)
        {
            Player.BankAccounts.GiveMoney(-1 * CostToClearInvestigation, false);

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
            Player.CellPhone.AddPhoneResponse(Contact.Name, Contact.IconName, Replies.PickRandom());
        }
        else if (Player.BankAccounts.GetMoney(false) < CostToClearInvestigation)
        {
            List<string> Replies = new List<string>() {
                $"Don't bother me unless you have some cash",
                $"This shit isn't free you know, make sure you've got the cash",
                };
            Player.CellPhone.AddPhoneResponse(Contact.Name, Contact.IconName, Replies.PickRandom());
        }
        else
        {
            List<string> Replies = new List<string>() {
                $"Don't bother me",
                };
            Player.CellPhone.AddPhoneResponse(Contact.Name, Contact.IconName, Replies.PickRandom());
        }
    }
    private void PayoffCopAPB()
    {
        if (Player.BankAccounts.GetMoney(false) >= CostToClearAPB)
        {
            Player.BankAccounts.GiveMoney(-1 * CostToClearAPB, false);

            GameFiber PayoffFiber = GameFiber.StartNew(delegate
            {
                try
                {
                    int SleepTime = RandomItems.GetRandomNumberInt(5000, 10000);
                    GameFiber.Sleep(SleepTime);
                    Player.CriminalHistory.Clear();
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
            Player.CellPhone.AddPhoneResponse(Contact.Name, Contact.IconName, Replies.PickRandom());
        }
        else if (Player.BankAccounts.GetMoney(false) < CostToClearInvestigation)
        {
            List<string> Replies = new List<string>() {
                $"Don't bother me unless you have some cash",
                $"This shit isn't free you know, make sure you've got the cash",
                };
            Player.CellPhone.AddPhoneResponse(Contact.Name, Contact.IconName, Replies.PickRandom());
        }
        else
        {
            List<string> Replies = new List<string>() {
                $"Don't bother me",
                };
            Player.CellPhone.AddPhoneResponse(Contact.Name, Contact.IconName, Replies.PickRandom());
        }
    }
}

