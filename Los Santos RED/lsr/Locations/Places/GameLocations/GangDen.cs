using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class GangDen : GameLocation, IRestableLocation, IAssaultSpawnable
{
    private UIMenuItem pickupCashMenuItem;
    private UIMenuItem dropoffCashMenuItem;
    private UIMenuItem dropoffItemMenuItem;
    private UIMenuItem completeTask;
    private UIMenuNumericScrollerItem<int> RestMenuItem;
    private bool KeepInteractionGoing;
    private UIMenuItem LayLowMenuItem;
    private UIMenuItem dropoffKick;
    private UIMenu LoanSubMenu;
    


    public GangDen() : base()
    {

    }
    public override bool ShowsOnDirectory { get; set; } = false;
    public override string TypeName { get; set; } = "Gang Den";
    public override int MapIcon { get; set; } = 378;
    public override string ButtonPromptText { get; set; }
    public override string AssociationID => AssignedAssociationID;
    public bool IsPrimaryGangDen { get; set; } = false;
    public override string BlipName => AssociatedGang != null ? AssociatedGang.ShortName : base.BlipName;
    public GameLocation GameLocation => this;
    public bool DisableNearbyScenarios { get; set; } = false;
    public float DisableScenarioDistance { get; set; } = 50f;
    public int MaxAssaultSpawns { get; set; } = 15;
    public List<SpawnPlace> AssaultSpawnLocations { get; set; }
    public bool RestrictAssaultSpawningUsingPedSpawns { get; set; } = false;
    public float AssaultSpawnHeavyWeaponsPercent { get; set; } = 80f;
    [XmlIgnore]
    public GangDenInterior GangDenInterior { get; set; }
    [XmlIgnore]
    public bool IsAvailableForPlayer { get; set; } = false;
    [XmlIgnore]
    public int PickupMoney { get; set; }
    [XmlIgnore]
    public int ExpectedMoney { get; set; }
    [XmlIgnore]
    public ModItem ExpectedItem { get; set; }
    [XmlIgnore]
    public int ExpectedItemAmount { get; set; }
    [XmlIgnore]
    public Gang AssociatedGang { get; set; }
    [XmlIgnore]
    public Gang OriginalGang { get; set; }
    [XmlIgnore]
    public Blip TerritoryBlip { get; set; }
    public GangDen(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID, string assignedAssociationID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        AssignedAssociationID = assignedAssociationID;
        MenuID = menuID;
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Enter {AssociatedGang?.ShortName} {AssociatedGang?.DenName}";
        return true;
    }
    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable names, ICrimes crimes, IPedGroups PedGroups, IEntityProvideable world,
    IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations, IContacts contacts, IInteriors interiors,
    ILocationInteractable player, IModItems modItems, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople, ModDataFileManager modDataFileManager)
    {
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, names, crimes, PedGroups, world, streets, locationTypes, settings, plateTypes, associations, contacts, interiors, player, modItems, weapons, time, placesOfInterest, issuableWeapons, heads, dispatchablePeople, modDataFileManager);
        //Menu = ShopMenus.GetSpecificMenu(MenuID);
        AssociatedGang = gangs.GetGang(AssignedAssociationID);
        OriginalGang = AssociatedGang;
        ButtonPromptText = $"Enter {AssociatedGang?.ShortName} {AssociatedGang?.DenName}";
        if (HasInterior)
        {
            GangDenInterior = interiors.PossibleInteriors.GangDenInteriors.Where(x => x.LocalID == InteriorID).FirstOrDefault();
            interior = GangDenInterior;
            if (GangDenInterior != null)
            {
                GangDenInterior.SetGangDen(this);
            }
        }
    }
    public override void OnInteract()
    {
        if (IsLocationClosed())
        {
            return;
        }
        if (!IsAvailableForPlayer)
        {
            Game.DisplayHelp($"{Name} is only available to associates and members");
            return;
        }
        if (!CanInteract)
        {
            return;
        }
        if (GangDenInterior != null && GangDenInterior.IsTeleportEntry && IsAvailableForPlayer)
        {
            DoEntranceCamera(false);
            GangDenInterior.SetGangDen(this);
            GangDenInterior.Teleport(Player, this, StoreCamera);
        }
        else
        {
            StandardInteract(null, false);
        }
    }
    public override void StandardInteract(LocationCamera locationCamera, bool isInside)
    {
        if (!IsAvailableForPlayer)
        {
            Game.DisplayHelp($"{Name} is only available to associates and members");
            StoreCamera = locationCamera;
            DisposeCamera(isInside);
            DisposeInterior();
            return;
        }
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        GameFiber.StartNew(delegate
        {
            try
            {
                SetupLocationCamera(locationCamera, isInside, false);
                KeepInteractionGoing = false;
                CreateInteractionMenu();
                if (Player.IsWanted)
                {
                    CreateLayLowMenu();
                }
                CreateInteractionMenu();
                if (Player.IsNotWanted)
                {
                    KeepInteractionGoing = false;
                    CreateStandardMenuItems(isInside);
                    InteractionMenu.Visible = true;
                    while (IsAnyMenuVisible || Time.IsFastForwarding || KeepInteractionGoing)
                    {
                        MenuPool.ProcessMenus();
                        Transaction?.Update();
                        GameFiber.Yield();
                    }
                    Transaction.DisposeTransactionMenu();
                    Player.IsTransacting = false;
                }
                DisposeInteractionMenu();
                DisposeCamera(isInside);
                DisposeInterior();
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "GangDenInteract");
    }
    private void CreateTransactionMenuItems(bool isInside)
    {
        Player.IsTransacting = true;
        HandleVariableItems();
        Transaction = new Transaction(MenuPool, InteractionMenu, Menu, this);
        Transaction.UseAccounts = false;

        if (Player.RelationshipManager.GangRelationships.CurrentGang != null && Player.RelationshipManager.GangRelationships.CurrentGang.ID == AssignedAssociationID)
        {
            Transaction.IsFreeVehicles = Player.RelationshipManager.GangRelationships.CurrentGang.MembersGetFreeVehicles;// true;
            Transaction.IsFreeWeapons = Player.RelationshipManager.GangRelationships.CurrentGang.MembersGetFreeWeapons; //true;
        }
        Transaction.IsInteriorInteract = isInside;
        Transaction.CreateTransactionMenu(Player, ModItems, World, Settings, Weapons, Time);

        Transaction.VehicleDeliveryLocations = VehicleDeliveryLocations;
        Transaction.VehiclePreviewPosition = VehiclePreviewLocation;
    }
    private void CreateStandardMenuItems(bool isInside)
    {
        //KeepInteractionGoing = false;
        CreateTransactionMenuItems(isInside);
        CreateTaskMenuItems();
        CreateRequisitionMenuItems();
        AddKickUpItems();
        if (!isInside)
        {
            CreateRestInteractionMenu();
        }
        CreateLoanMenuItems();
    }

    /// <summary>
    /// The gang armoury: weapons, plates and a car, gated by rank and paid for in goodwill.
    ///
    /// Deliberately at the den rather than on the phone. Gear you have to drive home for
    /// pulls the player back toward their own turf, which is the whole point of a
    /// territory-shaped progression system; backup stays on the phone because needing a
    /// crew is the one thing that never happens while you are standing in the clubhouse.
    ///
    /// Follows this file's own idiom rather than the gang phone menu's: items stay VISIBLE
    /// and refuse at activation with a reason, the way DropoffKick does, instead of being
    /// hidden. A player who cannot see the shotgun does not know there is a shotgun to
    /// work toward.
    /// </summary>
    private void CreateRequisitionMenuItems()
    {
        GangRequisitionManager requisition = Player.GangRequisitionManager;
        if (requisition == null || !requisition.IsEnabled || AssociatedGang == null)
        {
            return;
        }
        if (Player.RelationshipManager.GangRelationships.CurrentGang?.ID != AssignedAssociationID)
        {
            return;
        }
        if (Player.RelationshipManager.GangRelationships.GetReputation(AssociatedGang)?.IsMember != true)
        {
            return;
        }

        UIMenu requisitionMenu = MenuPool.AddSubMenu(InteractionMenu, "The Stash");
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "See what they'll let you walk out with.";
        requisitionMenu.RemoveBanner();

        AddWeaponRequisitionItems(requisitionMenu, requisition);
        AddArmorRequisitionItem(requisitionMenu, requisition);
    }

    private void AddWeaponRequisitionItems(UIMenu requisitionMenu, GangRequisitionManager requisition)
    {
        List<IssuableWeapon> weapons = requisition.AvailableWeapons(AssociatedGang);
        if (!weapons.Any())
        {
            UIMenuItem none = new UIMenuItem("Piece", "Nothing here for you yet.") { Enabled = false };
            requisitionMenu.AddItem(none);
            return;
        }
        // Specific weapons rather than categories. A bat and a switchblade are both melee,
        // and offering "Melee" at one price made the whole category read as a single item
        // whose identity was decided by a dice roll.
        List<string> weaponNames = weapons.Select(x => requisition.DisplayNameFor(x)).ToList();
        UIMenuListScrollerItem<string> weaponScroller = new UIMenuListScrollerItem<string>("Weapon", "Pick what you're asking for.", weaponNames);
        requisitionMenu.AddItem(weaponScroller);

        UIMenuItem drawWeapon = new UIMenuItem("Take It", "Ask for it and see what they say.")
        {
            RightLabel = $"~o~{requisition.WeaponCostFor(weapons[0])} GW~s~"
        };
        weaponScroller.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            int index = weaponScroller.Index;
            if (index >= 0 && index < weapons.Count)
            {
                drawWeapon.RightLabel = $"~o~{requisition.WeaponCostFor(weapons[index])} GW~s~";
            }
        };
        drawWeapon.Activated += (sender, selectedItem) =>
        {
            int index = weaponScroller.Index;
            IssuableWeapon chosen = index >= 0 && index < weapons.Count ? weapons[index] : null;
            string refusal = requisition.RequestWeapon(AssociatedGang, chosen);
            if (refusal != null)
            {
                PlayErrorSound();
                DisplayMessage("~r~Reply", refusal);
                return;
            }
            PlaySuccessSound();
            DisplayMessage("~g~Reply", "Take it. Don't bring it back with a body on it.");
        };
        requisitionMenu.AddItem(drawWeapon);
    }

    private void AddArmorRequisitionItem(UIMenu requisitionMenu, GangRequisitionManager requisition)
    {
        string armorName = requisition.ArmorItemNameFor(AssociatedGang);
        if (string.IsNullOrWhiteSpace(armorName))
        {
            UIMenuItem none = new UIMenuItem("Vest", "Plates are for people who've earned them.") { Enabled = false };
            requisitionMenu.AddItem(none);
            return;
        }
        int cost = requisition.PrivilegeFor(AssociatedGang).ArmorGoodwill;
        UIMenuItem drawArmor = new UIMenuItem("Request Body Armor", $"Ask for a vest. Goes into your inventory.~n~{armorName}")
        {
            RightLabel = $"~o~{cost} GW~s~"
        };
        drawArmor.Activated += (sender, selectedItem) =>
        {
            string refusal = requisition.RequestArmor(AssociatedGang);
            if (refusal != null)
            {
                PlayErrorSound();
                DisplayMessage("~r~Reply", refusal);
                return;
            }
            PlaySuccessSound();
            DisplayMessage("~g~Reply", "It's in your bag. Put it on before you need it, not after.");
        };
        requisitionMenu.AddItem(drawArmor);
    }
    /// <summary>
    /// Putting the crew's own earnings to work.
    ///
    /// Deliberately NOT hung off CreateRequisitionMenuItems. Fencing was, once, and turning
    /// requisition off silently took fencing with it — the same coupling would hide this
    /// behind a feature it has nothing to do with, and this one has to survive on its own if
    /// the crew roster ships without the goodwill economy.
    ///
    /// The stake is a SHARE rather than a number. A fixed ladder of amounts would have to be
    /// rebuilt every time the player scrolls to a different man, because each man has a
    /// different pot; a percentage is correct for everyone and reads better besides.
    ///
    /// Scroller items render their own value on the right and throw if given a RightLabel -
    /// that mistake took down the whole den interaction once. Everything here goes in the
    /// title or the description.
    /// </summary>
    private void AddKickUpItems()
    {
        GangCrewManager crew = Player.GangCrewManager;
        GangKickUpManager kickUp = crew?.KickUp;
        if (crew == null || kickUp == null || !kickUp.IsEnabled || AssociatedGang == null)
        {
            return;
        }
        if (Player.RelationshipManager.GangRelationships.CurrentGang?.ID != AssignedAssociationID)
        {
            return;
        }

        List<GangCrewMember> earners = crew.MembersWithTribute();
        UIMenu kickUpMenu = MenuPool.AddSubMenu(InteractionMenu, "Put Somebody On It");
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description =
            "Your men have their own money. Let one of them run it up.";
        kickUpMenu.RemoveBanner();

        if (!earners.Any())
        {
            kickUpMenu.AddItem(new UIMenuItem("Nobody's holding", "Take them out and let them earn.") { Enabled = false });
            return;
        }

        List<string> names = earners.Select(x => $"{x.Name} ({x.Tribute})").ToList();
        UIMenuListScrollerItem<string> who = new UIMenuListScrollerItem<string>("Whose", "What he's sitting on.", names);
        kickUpMenu.AddItem(who);

        List<int> shares = new List<int> { 25, 50, 75, 100 };
        UIMenuListScrollerItem<int> share = new UIMenuListScrollerItem<int>("How much", "How much of his own he puts up.", shares);
        kickUpMenu.AddItem(share);

        Func<GangCrewMember> selectedMember = () =>
        {
            int index = who.Index;
            return index >= 0 && index < earners.Count ? earners[index] : null;
        };
        Func<int> selectedStake = () =>
        {
            GangCrewMember member = selectedMember();
            return member == null ? 0 : (int)Math.Round(member.Tribute * share.SelectedItem / 100.0);
        };

        List<UIMenuItem> tierItems = new List<UIMenuItem>();
        foreach (GangKickUpManager.RiskTier tier in kickUp.AllTiers)
        {
            GangKickUpManager.RiskTier captured = tier;
            UIMenuItem item = new UIMenuItem(kickUp.NameFor(captured), kickUp.BlurbFor(captured));
            item.Activated += (sender, selected) =>
            {
                GangCrewMember member = selectedMember();
                int stake = selectedStake();
                string result = kickUp.Attempt(member, stake, captured);
                Game.DisplaySubtitle(result);
                // The pot has changed, so every label that mentions it is now a lie. Rebuilding
                // the whole den menu mid-interaction is not safe, so close instead and let the
                // player walk back in to a menu built from current numbers.
                InteractionMenu.Visible = false;
                kickUpMenu.Visible = false;
            };
            tierItems.Add(item);
            kickUpMenu.AddItem(item);
        }

        Action refresh = () =>
        {
            GangCrewMember member = selectedMember();
            int stake = selectedStake();
            for (int i = 0; i < tierItems.Count; i++)
            {
                GangKickUpManager.RiskTier tier = kickUp.AllTiers[i];
                string stakeText = member == null ? "nothing"
                    : stake < kickUp.MinimumStake ? $"{stake}, under the {kickUp.MinimumStake} minimum"
                    : stake.ToString();
                tierItems[i].Description = $"Staking {stakeText}. {kickUp.BlurbFor(tier)}";
            }
        };
        who.IndexChanged += (sender, oldIndex, newIndex) => refresh();
        share.IndexChanged += (sender, oldIndex, newIndex) => refresh();
        refresh();
    }
    private void CreateLoanMenuItems()
    {
        LoanSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Cash Loans");
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Just front me some cash, I'll catch it up on the backend!";
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            LoanSubMenu.SetBannerType(BannerImage);
        }
        AssociatedGang.AddLoanItems(Player, LoanSubMenu, this, Time);
    }

    private void CreateTaskMenuItems()
    {
        if(!IsPrimaryGangDen)
        {
            return;
        }
        PlayerTask pt = Player.PlayerTasks.GetTask(AssociatedGang.ContactName);
        if (PickupMoney > 0)
        {
            pickupCashMenuItem = new UIMenuItem("Pick Up Cash", "Pick Up the expected amount of cash.") { RightLabel = $"${PickupMoney}" };
            pickupCashMenuItem.Activated += (sender, selectedItem) =>
            {
                PickupCash();
            };
            InteractionMenu.AddItem(pickupCashMenuItem);
        }
        else if (ExpectedMoney > 0 && pt.IsReadyForPayment)
        {
            dropoffCashMenuItem = new UIMenuItem("Drop Cash", "Drop off the expected amount of cash.") { RightLabel = $"${ExpectedMoney}" };
            dropoffCashMenuItem.Activated += (sender, selectedItem) =>
            {
                DropoffCash();
            };
            InteractionMenu.AddItem(dropoffCashMenuItem);
        }
        else if (ExpectedItem != null && pt.IsReadyForPayment)
        {
            dropoffItemMenuItem = new UIMenuItem($"Drop off item", $"Drop off {ExpectedItem.Name} - {ExpectedItemAmount} {ExpectedItem.MeasurementName}(s).") { RightLabel = $"{ExpectedItem.Name} - {ExpectedItemAmount} {ExpectedItem.MeasurementName}(s)" };
            dropoffItemMenuItem.Activated += (sender, selectedItem) =>
            {
                DropoffItem();
            };
            InteractionMenu.AddItem(dropoffItemMenuItem);
        }
        else if (pt != null && pt.IsActive && pt.IsReadyForPayment)
        {
            completeTask = new UIMenuItem($"Collect Money", $"Inform the higher ups that you have completed the assigment and collect your payment.") { RightLabel = $"${pt.PaymentAmountOnCompletion}" };
            completeTask.Activated += (sender, selectedItem) =>
            {
                SetCompleteTask();
            };
            InteractionMenu.AddItem(completeTask);
        }
        if (Player.RelationshipManager.GangRelationships.GetReputation(AssociatedGang)?.IsMember == true && Player.RelationshipManager.GangRelationships.CurrentGangKickUp != null)
        {
            dropoffKick = new UIMenuItem("Pay Dues", $"Drop off your member dues.~n~{Player.RelationshipManager.GangRelationships.CurrentGangKickUp}") { RightLabel = $"${Player.RelationshipManager.GangRelationships.CurrentGangKickUp.DueAmount}" };
            dropoffKick.Activated += (sender, selectedItem) =>
            {
                DropoffKick();
            };
            InteractionMenu.AddItem(dropoffKick);
        }
    }

    private void CreateLayLowMenu()
    {
        LayLowMenuItem = new UIMenuItem("Lay Low", "Wait out the cops.");
        LayLowMenuItem.Activated += (sender, selectedItem) =>
        {
            LayLow();
        };
        InteractionMenu.AddItem(LayLowMenuItem);
        InteractionMenu.Visible = true;
        while (IsAnyMenuVisible || Time.IsFastForwarding || KeepInteractionGoing)// || Player.IsWanted)
        {
            MenuPool.ProcessMenus();
            GameFiber.Yield();
        }
    }
    protected override void LoadInterior(bool isOpen)
    {
        if(GangDenInterior != null)
        {
            GangDenInterior.Load(isOpen);
        }
        else if (HasInterior && interior != null)
        {
            interior.Load(isOpen);
        }
    }

    private void SetCompleteTask()
    {
        PlaySuccessSound();
        DisplayMessage("~g~Reply", "Thanks for taking care of that thing. Here's your share.");
        ExpectedMoney = 0;
        Player.PlayerTasks.CompleteTask(AssociatedGang.Contact, true);
        completeTask.Enabled = false;
    }
    private void DropoffKick()
    {
        if (Player.RelationshipManager.GangRelationships.CurrentGangKickUp != null)
        {
            if (Player.BankAccounts.GetMoney(false) >= Player.RelationshipManager.GangRelationships.CurrentGangKickUp.DueAmount)
            {
                if (Player.RelationshipManager.GangRelationships.CurrentGangKickUp.CanPay)
                {
                    Player.BankAccounts.GiveMoney(-1 * Player.RelationshipManager.GangRelationships.CurrentGangKickUp.DueAmount, false);
                    Player.RelationshipManager.GangRelationships.CurrentGangKickUp.PayDue();
                    InteractionMenu.Visible = false;
                    PlaySuccessSound();
                    DisplayMessage("~g~Reply", "Thanks for the kick.");
                }
                else
                {
                    PlayErrorSound();
                    DisplayMessage("~r~Reply", "Not time yet, come back closer to the due date.");
                }
            }
            else
            {
                PlayErrorSound();
                DisplayMessage("~r~Reply", "Come back when you actually have the cash.");
            }
        }
    }
    private void PickupCash()
    {
        Player.BankAccounts.GiveMoney(PickupMoney, false);
        PickupMoney = 0;
        pickupCashMenuItem.Enabled = false;
        PlaySuccessSound();
        DisplayMessage("~g~Reply", "Here's the cash. Don't lose it.");
    }
    private void DropoffCash()
    {
        if (Player.BankAccounts.GetMoney(false) >= ExpectedMoney)
        {
            Player.BankAccounts.GiveMoney(-1 * ExpectedMoney, false);
            ExpectedMoney = 0;
            Player.PlayerTasks.CompleteTask(AssociatedGang.Contact, true);
            //InteractionMenu.Visible = false;
            dropoffCashMenuItem.Enabled = false;
            PlaySuccessSound();
            DisplayMessage("~g~Reply", "Thanks for the cash. Here's your cut.");
        }
        else
        {
            PlayErrorSound();
            DisplayMessage("~r~Reply", "Come back when you actually have the cash.");
        }
    }
    private void DropoffItem()
    {
        if (Player.Inventory.Get(ExpectedItem)?.Amount >= ExpectedItemAmount)
        {
            Player.Inventory.Remove(ExpectedItem, ExpectedItemAmount);
            PlaySuccessSound();
            DisplayMessage("~g~Reply", $"Thanks for bringing us {ExpectedItemAmount} {ExpectedItem.MeasurementName}(s) of {ExpectedItem.Name}. Have something for your time.");
            ExpectedItem = null;
            ExpectedItemAmount = 0;
            Player.PlayerTasks.CompleteTask(AssociatedGang.Contact, true);
            dropoffItemMenuItem.Enabled = false;
            //InteractionMenu.Visible = false;
        }
        else
        {
            PlayErrorSound();
            DisplayMessage("~r~Reply", $"Come back when you actually have {ExpectedItemAmount} {ExpectedItem.MeasurementName}(s) of {ExpectedItem.Name}.");
        }
    }

    public void ResetItems()
    {
        ExpectedMoney = 0;
        ExpectedItem = null;
        ExpectedItemAmount = 0;
    }

    private void LayLow()
    {
        int TimeToWait = RandomItems.GetRandomNumberInt(3, 6);
        Time.FastForward(Time.CurrentDateTime.AddHours(TimeToWait));//  new DateTime(Time.CurrentYear, Time.CurrentMonth, Time.CurrentDay, 11, 0, 0));
        InteractionMenu.Visible = false;
        KeepInteractionGoing = true;
        GameFiber FastForwardWatcher = GameFiber.StartNew(delegate
        {
            while (Time.IsFastForwarding)
            {
                GameFiber.Yield();
            }
            Player.SetWantedLevel(0, "Gang Lay Low", true);
            LayLowMenuItem.Enabled = false;
            KeepInteractionGoing = false;
        }, "LayLowWatcher");
    }
    public void CreateRestMenu(bool removeBanner)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        Player.IsTransacting = true;
        CreateInteractionMenu();
        InteractionMenu.Visible = true;
        if (removeBanner)
        {
            InteractionMenu.RemoveBanner();
        }
        else if (!HasBannerImage)
        {
            InteractionMenu.SetBannerType(EntryPoint.LSRedColor);
        }
        InteractionMenu.Clear();
        CreateRestInteractionMenu();
        while (IsAnyMenuVisible || Time.IsFastForwarding || KeepInteractionGoing)
        {
            MenuPool.ProcessMenus();
            GameFiber.Yield();
        }
        DisposeInteractionMenu();
        Player.ActivityManager.IsInteractingWithLocation = false;
        Player.IsTransacting = false;
        if (Interior != null)
        {
            Interior.IsMenuInteracting = false;
        }
    }
    private void CreateRestInteractionMenu()
    {
        RestMenuItem = new UIMenuNumericScrollerItem<int>("Relax", $"Relax at the {AssociatedGang?.DenName}. Recover ~g~health~s~ and increase ~s~rep~s~ a small amount. Select up to 12 hours.", 1, 12, 1)
        {
            Formatter = v => v.ToString() + " hours"
        };
        RestMenuItem.Activated += (sender, selectedItem) =>
        {
            Rest(RestMenuItem.Value);
        };

        InteractionMenu.AddItem(RestMenuItem);
    }
    private void Rest(int Hours)
    {
        Time.FastForward(Time.CurrentDateTime.AddHours(Hours));//  new DateTime(Time.CurrentYear, Time.CurrentMonth, Time.CurrentDay, 11, 0, 0));
        InteractionMenu.Visible = false;
        KeepInteractionGoing = true;
        Player.ButtonPrompts.AddPrompt("GangDenRest", "Cancel Rest", "GangDenRest", Settings.SettingsManager.KeySettings.InteractCancel, 99);
        DateTime TimeLastAddedItems = Time.CurrentDateTime;
        GameFiber FastForwardWatcher = GameFiber.StartNew(delegate
        {
            while (Time.IsFastForwarding)
            {
                Player.IsResting = true;
                Player.IsSleeping = true;
                if (DateTime.Compare(Time.CurrentDateTime, TimeLastAddedItems) >= 0)
                {
                    if (!Settings.SettingsManager.NeedsSettings.ApplyNeeds)
                    {
                        Player.HealthManager.ChangeHealth(1);
                    }
                    Player.RelationshipManager.GangRelationships.ChangeReputation(AssociatedGang, 2, false);
                    TimeLastAddedItems = TimeLastAddedItems.AddMinutes(30);
                }
                if (Player.ButtonPrompts.IsPressed("GangDenRest"))
                {
                    Time.StopFastForwarding();
                }
                GameFiber.Yield();
            }
            Player.ButtonPrompts.RemovePrompts("GangDenRest");
            Player.IsResting = false;
            Player.IsSleeping = false;
            InteractionMenu.Visible = true;
            KeepInteractionGoing = false;
        }, "RestWatcher");
    }
    public override void DisplayMessage(string header, string message)
    {
        Game.RemoveNotification(NotificationHandle);
        NotificationHandle = Game.DisplayNotification(AssociatedGang.Contact.IconName, AssociatedGang.Contact.IconName, AssociatedGang.Contact.Name, header, message);
    }
    //public override void AddDistanceOffset(Vector3 offsetToAdd)
    //{
    //    foreach (SpawnPlace sp in VehicleDeliveryLocations)
    //    {
    //        sp.AddDistanceOffset(offsetToAdd);
    //    }
    //    VehiclePreviewLocation?.AddDistanceOffset(offsetToAdd);
    //    base.AddDistanceOffset(offsetToAdd);
    //}
    private void CreateGangTerritoryBlip(IEntityProvideable world)
    {
        if (!Settings.SettingsManager.GangSettings.ShowGangTerritoryBlip || AssociatedGang == null)
        {
            return;
        }
        if(TerritoryBlip.Exists())
        {
            return;
        }
        TerritoryBlip = new Blip(EntrancePosition, Settings.SettingsManager.GangSettings.GangTerritoryBlipSize) 
        { 
            Color = AssociatedGang.Color
        };
        TerritoryBlip.Color = AssociatedGang.Color;// currentBlipColor;
        TerritoryBlip.Alpha = Settings.SettingsManager.GangSettings.GangTerritoryBlipAlpha;/// currentblipAlpha;
        NativeFunction.CallByName<bool>("SET_BLIP_AS_SHORT_RANGE", (uint)TerritoryBlip.Handle, true);   
        NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
        NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(BlipName);
        NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(TerritoryBlip);
        world.AddBlip(TerritoryBlip);
    }
    protected override float GetCurrentIconAlpha(ITimeReportable time)
    {
        if (!IsAvailableForPlayer)
        {
            return MapClosedIconAlpha;
        }
        return base.GetCurrentIconAlpha(time);
    }
    public override void ActivateBlip(ITimeReportable time, IEntityProvideable world)
    {
        // EntryPoint.WriteToConsole($"GANG DEN ActivateBlip FOR {Name}");
        if (AssociatedGang == null)
        {
            EntryPoint.WriteToConsole($"GANG DEN ASSOCIATED GANG IS NULL {Name}");
            return;
        }
        if (!TerritoryBlip.Exists())
        {
            CreateGangTerritoryBlip(world);
        }
        base.ActivateBlip(time, world);
    }
    public override void DeactivateBlip()
    {
        if (TerritoryBlip.Exists())
        {
            TerritoryBlip.Delete();
        }
        base.DeactivateBlip();
    }
    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        MaxAssaultSpawns = 15;
        AssaultSpawnHeavyWeaponsPercent = 80f;
    }
    public override bool HasAssociation(Gang gang)
    {
        if(gang == null)
        {
            return false;
        }
        if(AssociatedGang == null)
        {
            return false;
        }
        return AssociatedGang.ID == gang.ID;
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.GangDens.Add(this);
        base.AddLocation(possibleLocations);
    }

    public override void SetTakeoverGang(Gang currentGang, Gang gangToReplace)
    {
        if(currentGang == null || gangToReplace == null)
        {
            return;
        }
        AssociatedGang = currentGang;
        AssignedAssociationID = currentGang.ID;
        IsAvailableForPlayer = false;
        DeactivateBlip();
        ActivateBlip(Time, World);




        base.SetTakeoverGang(currentGang, gangToReplace);

    }
    public override void ResetGangTakeover()
    {
        if (OriginalGang != null)
        {
            AssociatedGang = OriginalGang;
            AssignedAssociationID = OriginalGang.ID;
        }
        IsAvailableForPlayer = true;
        DeactivateBlip();
        ActivateBlip(Time, World);





        base.ResetGangTakeover();

    }
}

