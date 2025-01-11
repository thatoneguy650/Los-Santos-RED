using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public class GangInteraction : IContactMenuInteraction
{
    private IContactInteractable Player;
    private UIMenu GangMenu;
    private IAgencies Agencies;
    private MenuPool MenuPool;
    private UIMenuItem PayoffGang;
    private UIMenuItem PayoffGangNeutral;
    private UIMenuItem ApoligizeToGang;
    private UIMenuItem RequestGangDen;
    private UIMenuItem PayoffGangFriendly;
    private UIMenu GangWorkMenu;
    private UIMenu JobsSubMenu;
    private UIMenuItem GangMoneyPickup;
    private UIMenuItem GangPizza;
    private UIMenuItem GangRacketeering;
    private UIMenuItem GangArson;
    private UIMenuItem GangBribery;
    private UIMenuItem GangTaskCancel;
    private Gang ActiveGang;
    private GangReputation ActiveGangReputation;
    private IGangs Gangs;
    private IPlacesOfInterest PlacesOfInterest;
    private UIMenuItem PayoffDebt;
    private GangContact GangContact;
    private IEntityProvideable World;
    private UIMenuItem LeaveGangMenu;
    private ISettingsProvideable Settings;
    private UIMenuItem GangJoinMenu;
    private UIMenuItem GangImpoundTheft;
    private UIMenuItem GangBodyDisposal;
    private UIMenu WheelManSubMenu;
    private UIMenu CopHitSubMenu;
    private UIMenu GangHitSubMenu;
    private UIMenu GangAmbushSubMenu;
    private UIMenu GangTheftSubMenu;
    private UIMenu GangDeliverySubMenu;
    private IModItems ModItems;
    private UIMenuItem RequestBackupMenu;
    private UIMenu BackupSubMenu;
    public GangInteraction(IContactInteractable player, IGangs gangs, IPlacesOfInterest placesOfInterest, GangContact gangContact, IEntityProvideable world, ISettingsProvideable settings, IAgencies agencies, IModItems modItems)
    {
        Player = player;
        Gangs = gangs;
        PlacesOfInterest = placesOfInterest;
        MenuPool = new MenuPool();
        GangContact = gangContact;
        World = world;
        Settings = settings;
        Agencies = agencies;
        ModItems = modItems;
    }
    public void Start(PhoneContact phoneContact)
    {
        if(phoneContact == null)
        {
            EntryPoint.WriteToConsole("GangInteraction phoneContact IS NULL");
            return;
        }
        Start(Gangs.GetGangByContact(phoneContact.Name));
    }
    public void Start(Gang gang)
    {
        ActiveGang = gang;
        SetupInteraction();
        StartLoop();
    }
    private void SetupInteraction()
    {
        if (ActiveGang == null)
        {
            return;
        }
        ActiveGangReputation = Player.RelationshipManager.GangRelationships.GetReputation(ActiveGang);
        if(ActiveGangReputation == null)
        {
            return;
        }
        GangMenu = new UIMenu("", "Select an Option");
        GangMenu.RemoveBanner();
        MenuPool.Add(GangMenu);
        if(ActiveGangReputation.IsEnemy)
        {
            EnemyReply();
            return;
        }
        if (ActiveGangReputation.PlayerDebt > 0)
        {
            AddHasDebtItems();
        }
        else if (ActiveGangReputation.ReputationLevel < 0)
        {
            AddHostileRepItems();
        }
        else if (ActiveGangReputation.ReputationLevel >= ActiveGangReputation.FriendlyRepLevel)
        {
            AddFriendlyRepItems();        
        }
        else
        {
            AddNeutralRepItems();
        }
        GangMenu.Visible = true;
    }

    private void AddNeutralRepItems()
    {
        PayoffGangFriendly = new UIMenuItem("Payoff", "Payoff the gang to get a friendly relationship") { RightLabel = "~r~" + ActiveGangReputation.CostToPayoff.ToString("C0") + "~s~" };
        PayoffGangFriendly.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartPayoffGang(ActiveGang, GangContact);
            sender.Visible = false;
        };
        GangMenu.AddItem(PayoffGangFriendly);
    }
    private void AddFriendlyRepItems()
    {
        if (Player.PlayerTasks.HasTask(ActiveGang.ContactName))
        {
            GangTaskCancel = new UIMenuItem("Cancel Task", "Tell the gang you can't complete the task.") { RightLabel = "~o~$?~s~" };
            GangTaskCancel.Activated += (sender, selectedItem) =>
            {
                Player.PlayerTasks.CancelTask(ActiveGang?.Contact);
                sender.Visible = false;
            };
            GangMenu.AddItem(GangTaskCancel);
        }
        else
        {
            AddJobItems();
            if(ActiveGangReputation.CanAskToJoin)
            {
                GangJoinMenu = new UIMenuItem("Join gang", "Prove your worth and become a member of the gang");
                GangJoinMenu.Activated += (sender, selectedItem) =>
                {
                    Player.PlayerTasks.GangTasks.StartGangProveWorth(ActiveGang, 2, GangContact);
                    sender.Visible = false;
                };
                GangMenu.AddItem(GangJoinMenu);
            }
        }
        RequestGangDen = new UIMenuItem("Request Invite", "Request the location of the gang den");
        RequestGangDen.Activated += (sender, selectedItem) =>
        {
            RequestDenAddress();
            sender.Visible = false;
        };
        GangMenu.AddItem(RequestGangDen);
        if (ActiveGangReputation.IsMember)
        {

            BackupSubMenu = MenuPool.AddSubMenu(GangMenu, "Request Backup");
            BackupSubMenu.RemoveBanner();



            UIMenuNumericScrollerItem<int> backupCountMenu = new UIMenuNumericScrollerItem<int>("Requested Members", "Set the number of members you want to be dispatched to your location.", 1, 7, 1) { Value = 2 };
            BackupSubMenu.AddItem(backupCountMenu);

            List<VehicleNameSelect> vehicleNameList = new List<VehicleNameSelect>();
            vehicleNameList.Add(new VehicleNameSelect("") { VehicleModelName = "Random" });
            foreach (DispatchableVehicle dv in ActiveGang.Vehicles.Where(x => !x.RequiresDLC || Settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles))
            {
                VehicleNameSelect vns = new VehicleNameSelect(dv.ModelName);
                vns.UpdateItems();
                vehicleNameList.Add(vns);
            }
            UIMenuListScrollerItem<VehicleNameSelect> BackupVehiclesMenu = new UIMenuListScrollerItem<VehicleNameSelect>("Vehicle", $"Request a specific vehicle.", vehicleNameList);
            BackupSubMenu.AddItem(BackupVehiclesMenu);
            UIMenuItem StartBackupMenu = new UIMenuItem("Request Backup", "Request gang backup using the parameters.");
            StartBackupMenu.Activated += (sender, selectedItem) =>
            {
                RequestBackup(backupCountMenu.Value, BackupVehiclesMenu.SelectedItem.ModelName);
                sender.Visible = false;
            };
            BackupSubMenu.AddItem(StartBackupMenu);




            LeaveGangMenu = new UIMenuItem("Leave Gang", "Inform the gang that you no longer want to be a member");
            LeaveGangMenu.Activated += (sender, selectedItem) =>
            {
                if (LeaveGang())
                {
                    sender.Visible = false;
                }
            };
            GangMenu.AddItem(LeaveGangMenu);
        }
    }
    private void RequestBackup(int minMembers, string requiredModelName)
    {
        Player.GangBackupManager.RequestBackup(ActiveGang, minMembers, requiredModelName);
    }
    private void AddJobItems()
    {
        JobsSubMenu = MenuPool.AddSubMenu(GangMenu, "Jobs");
        JobsSubMenu.RemoveBanner();
        AddGangHitSubMenu();
        AddGangAmbushSubMenu();
        AddCopHitSubMenu();
        AddGangTheftSubMenu();
        GangMoneyPickup = new UIMenuItem("Money Pickup", "Pickup some cash from a dead drop for the gang and bring it back.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.PickupPaymentMin:C0}-{ActiveGang.PickupPaymentMax:C0}~s~" };
        GangMoneyPickup.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartGangPickup(ActiveGang, GangContact);
            sender.Visible = false;
        };
        AddGangDeliverySubMenu();
        AddWheelmanSubMenu();
        GangImpoundTheft = new UIMenuItem("Impound Theft", "Steal a gang car out of the impound lot. ~r~WIP~s~") { RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.ImpoundTheftPaymentMin:C0}-{ActiveGang.ImpoundTheftPaymentMax:C0}~s~" };
        GangImpoundTheft.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartImpoundTheft(ActiveGang, GangContact);
            sender.Visible = false;
        };
        GangBodyDisposal = new UIMenuItem("Vehicle Disposal", "Get rid of a dirty car. ~r~WIP~s~") { RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.BodyDisposalPaymentMin:C0}-{ActiveGang.BodyDisposalPaymentMax:C0}~s~" };
        GangBodyDisposal.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartGangBodyDisposal(ActiveGang, GangContact);
            sender.Visible = false;
        };
        GangPizza = new UIMenuItem("Pizza Man", "Pizza Time.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{100:C0}-{250:C0}~s~" };
        GangPizza.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartGangPizza(ActiveGang, GangContact);
            sender.Visible = false;
        };
        GangRacketeering = new UIMenuItem("Collect Protection", "Collect protection money from local shops. ~r~WIP~s~") { RightLabel = $"~HUD_COLOUR_GREENDARK~{100:C0}-{100000:C0}~s~" };
        GangRacketeering.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartGangRacketeering(ActiveGang, GangContact);
            sender.Visible = false;
        };
        GangBribery = new UIMenuItem("Pay Bribe", "Make a discreet payment to keep things smooth. ~r~WIP~s~") { RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.BriberyPaymentMin:C0}-{ActiveGang.BriberyPaymentMax:C0}~s~" };
        GangBribery.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartGangBribery(ActiveGang, GangContact);
            sender.Visible = false;
        };
        GangArson = new UIMenuItem("Arson", "Torch a building. ~r~WIP~s~") { RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.ArsonPaymentMin:C0}-{ActiveGang.ArsonPaymentMax:C0}~s~" };
        GangArson.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartGangArson(ActiveGang, GangContact);
            sender.Visible = false;
        };
        JobsSubMenu.AddItem(GangImpoundTheft);
        JobsSubMenu.AddItem(GangBodyDisposal);
        JobsSubMenu.AddItem(GangMoneyPickup);
        JobsSubMenu.AddItem(GangRacketeering);
        JobsSubMenu.AddItem(GangBribery);
        JobsSubMenu.AddItem(GangArson);
        //JobsSubMenu.AddItem(GangDelivery);  
        if (ActiveGang.GangClassification == GangClassification.Mafia)// == "Gambetti" || ActiveGang.ShortName == "Pavano" || ActiveGang.ShortName == "Lupisella" || ActiveGang.ShortName == "Messina" || ActiveGang.ShortName == "Ancelotti")
        {
            JobsSubMenu.AddItem(GangPizza);
        }
    }

    private void AddWheelmanSubMenu()
    {
        WheelManSubMenu = MenuPool.AddSubMenu(JobsSubMenu, "Wheelman");
        JobsSubMenu.MenuItems[JobsSubMenu.MenuItems.Count() - 1].Description = $"Be a wheelman for the gang.";
        JobsSubMenu.MenuItems[JobsSubMenu.MenuItems.Count() - 1].RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.WheelmanPaymentMin:C0}-{ActiveGang.WheelmanPaymentMax:C0}~s~";
        WheelManSubMenu.RemoveBanner();
        List<string> locationTypes = PlacesOfInterest.PossibleLocations.RobberyTaskLocations().Select(x => x.TypeName).Distinct().ToList();
        locationTypes.Add("Random");
        UIMenuListScrollerItem<string> LocationType = new UIMenuListScrollerItem<string>("Location Type", "Set the location type", locationTypes);
        UIMenuNumericScrollerItem<int> WheelManAccomplices = new UIMenuNumericScrollerItem<int>("Accomplices", $"Select the number of accomplices", 1, 3, 1) {Value = 1 };

        UIMenuCheckboxItem requireAllMembers = new UIMenuCheckboxItem("Completion Requires Accomplices",true,"If enabled, the robbery will fail if any or your accomplices are killed, busted, or left behind.");


        UIMenuItem GangWheelmanStart = new UIMenuItem("Start", $"Start the task.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.WheelmanPaymentMin:C0}-{ActiveGang.WheelmanPaymentMax:C0}~s~" };
        GangWheelmanStart.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartGangWheelman(ActiveGang, GangContact, WheelManAccomplices.Value, LocationType.SelectedItem, requireAllMembers.Checked);
            sender.Visible = false;
        };
        //WheelManAccomplices.IndexChanged += (sender, oldIndex, newIndex) =>
        //{
        //    GangWheelmanStart.RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.WheelmanPaymentMin * WheelManAccomplices.Value:C0}-{ActiveGang.WheelmanPaymentMax * WheelManAccomplices.Value:C0}~s~";
        //};//doesnt pay more for more poeple, you just get a cut
        WheelManSubMenu.AddItem(LocationType);
        WheelManSubMenu.AddItem(WheelManAccomplices);
        WheelManSubMenu.AddItem(requireAllMembers);
        WheelManSubMenu.AddItem(GangWheelmanStart);
    }

    private void AddCopHitSubMenu()
    {
        CopHitSubMenu = MenuPool.AddSubMenu(JobsSubMenu, "Cop Hit");
        JobsSubMenu.MenuItems[JobsSubMenu.MenuItems.Count() - 1].Description = $"Do a cop hit for the gang.";
        JobsSubMenu.MenuItems[JobsSubMenu.MenuItems.Count() - 1].RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.CopHitPaymentMin:C0}-{ActiveGang.CopHitPaymentMax:C0}~s~";
        CopHitSubMenu.RemoveBanner();
        UIMenuListScrollerItem<Agency> TargetMenu = new UIMenuListScrollerItem<Agency>("Target Agency", $"Choose a target agency.", Agencies.GetAgenciesByResponse(ResponseType.LawEnforcement).Where(x => x.ID != "UNK"));//.Where(x => x.ID != ActiveGang.ID).ToList());
        UIMenuNumericScrollerItem<int> TargetCountMenu = new UIMenuNumericScrollerItem<int>("Targets", $"Select the number of targets", 1, 3, 1) { Value = 1 };
        UIMenuItem StartTaskMenu = new UIMenuItem("Start", $"Start the task.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.CopHitPaymentMin:C0}-{ActiveGang.CopHitPaymentMax:C0}~s~" };
        StartTaskMenu.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartCopHit(ActiveGang, TargetCountMenu.Value, GangContact, TargetMenu.SelectedItem);
            sender.Visible = false;
        };
        TargetCountMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            StartTaskMenu.RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.CopHitPaymentMin * TargetCountMenu.Value:C0}-{ActiveGang.CopHitPaymentMax * TargetCountMenu.Value:C0}~s~";
        };
        CopHitSubMenu.AddItem(TargetMenu);
        CopHitSubMenu.AddItem(TargetCountMenu);
        CopHitSubMenu.AddItem(StartTaskMenu);
    }
    private void AddGangHitSubMenu()
    {  
        GangHitSubMenu = MenuPool.AddSubMenu(JobsSubMenu, "Gang Hit");
        JobsSubMenu.MenuItems[JobsSubMenu.MenuItems.Count() - 1].Description = $"Do a hit for the gang on a rival.";
        JobsSubMenu.MenuItems[JobsSubMenu.MenuItems.Count() - 1].RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.HitPaymentMin:C0}-{ActiveGang.HitPaymentMax:C0}~s~";
        GangHitSubMenu.RemoveBanner();
        //UIMenuListScrollerItem<Gang> TargetMenu = new UIMenuListScrollerItem<Gang>("Target Gang", $"Choose a target gang", Gangs.AllGangs.Where(x => x.ID != ActiveGang.ID && ActiveGang.EnemyGangs.Contains(x.ID)).ToList());

        UIMenuListScrollerItem<Gang> TargetMenu = new UIMenuListScrollerItem<Gang>("Target Gang", $"Choose a target gang", 
            Settings.SettingsManager.GangSettings.AllowNonEnemyTargets ? Gangs.AllGangs.Where(x => x.ID != ActiveGang.ID ).ToList()
             : Gangs.AllGangs.Where(x => x.ID != ActiveGang.ID && ActiveGang.EnemyGangs.Contains(x.ID)).ToList());




        UIMenuNumericScrollerItem<int> TargetCountMenu = new UIMenuNumericScrollerItem<int>("Targets", $"Select the number of targets", 1, 3, 1) { Value = 1 };
        UIMenuItem TaskStartMenu = new UIMenuItem("Start", $"Start the task.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.HitPaymentMin * TargetCountMenu.Value:C0}-{ActiveGang.HitPaymentMax * TargetCountMenu.Value:C0}~s~" };
        TaskStartMenu.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartGangHit(ActiveGang, TargetCountMenu.Value, GangContact, TargetMenu.SelectedItem);
            sender.Visible = false;
        };
        TargetCountMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            TaskStartMenu.RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.HitPaymentMin* TargetCountMenu.Value:C0}-{ActiveGang.HitPaymentMax* TargetCountMenu.Value:C0}~s~";
        };

        GangHitSubMenu.AddItem(TargetMenu);
        GangHitSubMenu.AddItem(TargetCountMenu);
        GangHitSubMenu.AddItem(TaskStartMenu);
    }
    private void AddGangAmbushSubMenu()
    {
        GangAmbushSubMenu = MenuPool.AddSubMenu(JobsSubMenu, "Ambush");
        JobsSubMenu.MenuItems[JobsSubMenu.MenuItems.Count() - 1].Description = $"Ambush any rival hoods seen in an area";
        JobsSubMenu.MenuItems[JobsSubMenu.MenuItems.Count() - 1].RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.HitPaymentMin:C0}-{ActiveGang.HitPaymentMax:C0}~s~";
        GangAmbushSubMenu.RemoveBanner();
        //UIMenuListScrollerItem<Gang> TargetMenu = new UIMenuListScrollerItem<Gang>("Target Gang", $"Choose a target gang", Gangs.AllGangs.Where(x => x.ID != ActiveGang.ID && ActiveGang.EnemyGangs.Contains(x.ID)).ToList());

        UIMenuListScrollerItem<Gang> TargetMenu = new UIMenuListScrollerItem<Gang>("Target Gang", $"Choose a target gang",
            Settings.SettingsManager.GangSettings.AllowNonEnemyTargets ? Gangs.AllGangs.Where(x => x.ID != ActiveGang.ID).ToList()
             : Gangs.AllGangs.Where(x => x.ID != ActiveGang.ID && ActiveGang.EnemyGangs.Contains(x.ID)).ToList());



        UIMenuNumericScrollerItem<int> TargetCountMenu = new UIMenuNumericScrollerItem<int>("Targets", $"Select the number of targets", 1, 3, 1) { Value = 1 };
        UIMenuItem TaskStartMenu = new UIMenuItem("Start", $"Start the task.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.HitPaymentMin * TargetCountMenu.Value:C0}-{ActiveGang.HitPaymentMax * TargetCountMenu.Value:C0}~s~" };
        TaskStartMenu.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartGangAmbush(ActiveGang, TargetCountMenu.Value, GangContact, TargetMenu.SelectedItem);
            sender.Visible = false;
        };
        TargetCountMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            TaskStartMenu.RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.AmbushPaymentMin * TargetCountMenu.Value:C0}-{ActiveGang.AmbushPaymentMax * TargetCountMenu.Value:C0}~s~";
        };

        GangAmbushSubMenu.AddItem(TargetMenu);
        GangAmbushSubMenu.AddItem(TargetCountMenu);
        GangAmbushSubMenu.AddItem(TaskStartMenu);
    }
    private void AddGangTheftSubMenu()
    {
        GangTheftSubMenu = MenuPool.AddSubMenu(JobsSubMenu, "Gang Theft");
        JobsSubMenu.MenuItems[JobsSubMenu.MenuItems.Count() - 1].Description = $"Steal an enemy gang car.";
        JobsSubMenu.MenuItems[JobsSubMenu.MenuItems.Count() - 1].RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.TheftPaymentMin:C0}-{ActiveGang.TheftPaymentMax:C0}~s~";
        GangTheftSubMenu.RemoveBanner();
        List<Gang> AllGangs = Gangs.AllGangs.Where(x => x.ID != ActiveGang.ID).ToList();
        List<VehicleNameSelect> vehicleNameList = new List<VehicleNameSelect>();
        foreach(DispatchableVehicle dv in AllGangs.FirstOrDefault().Vehicles.Where(x => !x.RequiresDLC || Settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles))
        {
            VehicleNameSelect vns = new VehicleNameSelect(dv.ModelName);
            vns.UpdateItems();
            vehicleNameList.Add(vns);
        }
        UIMenuListScrollerItem<VehicleNameSelect> GangTheftVehicles = new UIMenuListScrollerItem<VehicleNameSelect>("Target Vehicle", $"Choose a target vehicle.", vehicleNameList);
        UIMenuListScrollerItem<Gang> GangTheftTargets = new UIMenuListScrollerItem<Gang>("Target Gang", $"Choose a target gang.", AllGangs);
        GangTheftTargets.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            GangTheftVehicles.Items.Clear();
            List<VehicleNameSelect> vehicleNameList2 = new List<VehicleNameSelect>();
            foreach (DispatchableVehicle dv in GangTheftTargets.SelectedItem.Vehicles.Where(x=> !x.RequiresDLC || Settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles))
            {
                VehicleNameSelect vns = new VehicleNameSelect(dv.ModelName);
                vns.UpdateItems();
                vehicleNameList2.Add(vns);
            }
            GangTheftVehicles.Items = vehicleNameList2.ToList();
        };
        UIMenuItem GangTheftStart = new UIMenuItem("Start", $"Start the task.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.TheftPaymentMin:C0}-{ActiveGang.TheftPaymentMax:C0}~s~" };
        GangTheftStart.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartGangVehicleTheft(ActiveGang, GangContact, GangTheftTargets.SelectedItem, GangTheftVehicles.SelectedItem.ModelName, GangTheftVehicles.SelectedItem.ToString());
            sender.Visible = false;
        };
        GangTheftSubMenu.AddItem(GangTheftTargets);
        GangTheftSubMenu.AddItem(GangTheftVehicles);
        GangTheftSubMenu.AddItem(GangTheftStart);
    }

    private void AddGangDeliverySubMenu()
    {
        GangDeliverySubMenu = MenuPool.AddSubMenu(JobsSubMenu, "Delivery");
        JobsSubMenu.MenuItems[JobsSubMenu.MenuItems.Count() - 1].Description = $"Source some items for the gang.";
        JobsSubMenu.MenuItems[JobsSubMenu.MenuItems.Count() - 1].RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.DeliveryPaymentMin:C0}-{ActiveGang.DeliveryPaymentMax:C0}~s~";
        GangDeliverySubMenu.RemoveBanner();
        List<string> PossibleItems = ModItems.AllItems().Where(x => x.ItemSubType == ItemSubType.Narcotic).Select(x => x.Name).ToList();
        GangDen ActiveGangGangDen = PlacesOfInterest.GetMainDen(ActiveGang.ID, World.IsMPMapLoaded, Player.CurrentLocation);

        List<string> AvailableItems = new List<string>();
        foreach(string item in PossibleItems)
        {
            if(ActiveGangGangDen != null && (ActiveGangGangDen.Menu == null || ActiveGangGangDen.Menu.Items == null || !ActiveGangGangDen.Menu.Items.Any(x => x.Purchaseable && x.ModItemName == item)))
            {
                AvailableItems.Add(item);
            }
        }
        UIMenuListScrollerItem<string> GangDeliveryItems = new UIMenuListScrollerItem<string>("Item To Source", $"Choose an item to source for the gang.", AvailableItems);
        UIMenuItem GangDeliveryStart = new UIMenuItem("Start", "Start the task.") { RightLabel = $"~HUD_COLOUR_GREENDARK~{ActiveGang.DeliveryPaymentMin:C0}-{ActiveGang.DeliveryPaymentMax:C0}~s~" };
        GangDeliveryStart.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartGangDelivery(ActiveGang, GangContact, GangDeliveryItems.SelectedItem);
            sender.Visible = false;
        };
        GangDeliverySubMenu.AddItem(GangDeliveryItems);
        GangDeliverySubMenu.AddItem(GangDeliveryStart);
    }

    private void AddHostileRepItems()
    {
        PayoffGangNeutral = new UIMenuItem("Payoff", "Payoff the gang to return to a neutral relationship") { RightLabel = "~r~" + ActiveGangReputation.CostToPayoff.ToString("C0") + "~s~" };
        PayoffGangNeutral.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartPayoffGang(ActiveGang, GangContact);
            sender.Visible = false;
        };
        ApoligizeToGang = new UIMenuItem("Apologize", "Apologize to the gang for your actions");
        ApoligizeToGang.Activated += (sender, selectedItem) =>
        {
            ApologizeToGang();
            sender.Visible = false;
        };
        UIMenuItem InsultGangMenu = new UIMenuItem("Insult Gang", "Let them know what you ~r~REALLY~s~ think about them");
        InsultGangMenu.Activated += (sender, selectedItem) =>
        {
            InsultGang();
            sender.Visible = false;
        };
        GangMenu.AddItem(PayoffGangNeutral);
        GangMenu.AddItem(ApoligizeToGang);
        GangMenu.AddItem(InsultGangMenu);
    }

    private void InsultGang()
    {
        List<string> Insults = new List<string>() {
                    "Your mom raised you exactly like I expected",
                    "When your mother smiles, she looks like a horse",
                    "You are one ugly motherfucker",
                    "Fuck your wannabe crew",
                    "Gonna fuck you up prick",
                    "Eat any good books lately",
                    "Gonna send you and your mickey-mouse crew straight to hell",
                    "Aren't gangsters supposed to be tough?",
                    "Heard you were a fucking snitch",
                    "Gonna send your ass to the morgue",
                    };
        Game.DisplaySubtitle($"You: {Insults.PickRandom()}");
        Player.PlayerVoice.SayInsult();
        GameFiber.Sleep(4000);
        List<string> Replies = new List<string>() {
                    "Thats the best you got?",
                    "Keep talking asshole, see where it leads",
                    "Not wise to run your mouth to me",
                    "Lets see if you are as tough in person",
                    "You are a real telephone gangster, can't wait to meet in person",
                    "I'll remeber that when you are bleeding out",
                    };
        Player.CellPhone.AddPhoneResponse(ActiveGang.Contact.Name, ActiveGang.Contact.IconName, Replies.PickRandom());
        Player.RelationshipManager.GangRelationships.ChangeReputation(ActiveGang, -1000, false);
    }

    private void AddHasDebtItems()
    {
        PayoffDebt = new UIMenuItem("Payoff Debt", "Payoff your current debt to the gang") { RightLabel = "~r~" + ActiveGangReputation.PlayerDebt.ToString("C0") + "~s~" };
        PayoffDebt.Activated += (sender, selectedItem) =>
        {
            Player.PlayerTasks.GangTasks.StartPayoffGang(ActiveGang, GangContact);
            sender.Visible = false;
        };
        GangMenu.AddItem(PayoffDebt);
    }
    private void StartLoop()
    {
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
    private void ApologizeToGang()
    {
        List<string> Replies = new List<string>() {
                    "You think I give a shit?",
                    "Fuck off prick.",
                    "Go fuck yourself prick.",
                    "You are really starting to piss me off",
                    "(click)",
                    "I'm not even going to respond to this shit.",
                    "You are a stupid motherfucker aren't you?",
                    "Good luck dickhead.",
                    };
        Player.CellPhone.AddPhoneResponse(ActiveGang.Contact.Name, ActiveGang.Contact.IconName, Replies.PickRandom());
    }
    private void EnemyReply()
    {
        List<string> Replies = new List<string>() {
                    "Fuck off prick.",
                    "Go fuck yourself prick.",
                    "(click)",
                    };
        Player.CellPhone.AddPhoneResponse(ActiveGang.Contact.Name, ActiveGang.Contact.IconName, Replies.PickRandom());
    }
    private void RequestDenAddress()
    {
        GangDen myDen = PlacesOfInterest.GetMainDen(ActiveGang.ID, World.IsMPMapLoaded, Player.CurrentLocation);
        if (myDen != null)
        {
            Player.GPSManager.AddGPSRoute(myDen.Name, myDen.EntrancePosition);
            List<string> Replies = new List<string>() {
                    $"Our {ActiveGang.DenName} is located on {myDen.FullStreetAddress} come see us.",
                    $"Come check out our {ActiveGang.DenName} on {myDen.FullStreetAddress}.",
                    $"You can find our {ActiveGang.DenName} on {myDen.FullStreetAddress}.",
                    $"{myDen.FullStreetAddress}.",
                    $"It's on {myDen.FullStreetAddress} come see us.",
                    $"The {ActiveGang.DenName}? It's on {myDen.FullStreetAddress}.",

                    };
            Player.CellPhone.AddPhoneResponse(ActiveGang.Contact.Name, ActiveGang.Contact.IconName, Replies.PickRandom());
        }
    }
    private bool LeaveGang()
    {
        SimpleWarning popUpWarning = new SimpleWarning("Leave Gang", $"Are you sure you want to leave the gang {Player.RelationshipManager.GangRelationships.CurrentGang?.ShortName}", "", Player.ButtonPrompts, Settings);
        popUpWarning.Show();
        if (!popUpWarning.IsAccepted)
        {
            return false;
        }
        Player.RelationshipManager.GangRelationships.ResetGang(true);
        Player.RelationshipManager.GangRelationships.SetReputation(ActiveGang,ActiveGang.MinimumRep,true);

        return true;
    }
    
}

