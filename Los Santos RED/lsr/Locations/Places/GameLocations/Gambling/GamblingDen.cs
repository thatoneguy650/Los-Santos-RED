using Blackjack;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using Roulette;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

public class GamblingDen : GameLocation, IRestableLocation
{
    private UIMenu GameChoiceSubMenu;
    private UIMenu LoanSubMenu;
    private bool KeepInteractionGoing;
    private UIMenuNumericScrollerItem<int> RestMenuItem;

    public GamblingDen(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public GamblingDen() : base()
    {

    }
    public override string TypeName { get; set; } = "Casino";
    public override int MapIcon { get; set; } = 680;//402 = car repair
    public override bool ShowsOnDirectory { get; set; } = false;
    public GamblingParameters GamblingParameters { get; set; } = new GamblingParameters();
    public bool IsRestrictedToFriendly { get; set; } = false;
    public bool IsRestrictedToMember { get; set; } = false;
    public int WinLimit { get; set; } = 10000;
    public int WinLimitResetHours { get; set; } = 24;
    public GameLocation GameLocation => this;
    [XmlIgnore]
    public Gang AssociatedGang { get; set; }
    [XmlIgnore]
    public GamblingDenInterior GamblingDenInterior { get; set; }
    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable names, ICrimes crimes, IPedGroups PedGroups, IEntityProvideable world,
IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations, IContacts contacts, IInteriors interiors,
ILocationInteractable player, IModItems modItems, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople, ModDataFileManager modDataFileManager)
    {
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, names, crimes, PedGroups, world, streets, locationTypes, settings, plateTypes, associations, contacts, interiors, player, modItems, weapons, time, placesOfInterest, issuableWeapons, heads, dispatchablePeople, modDataFileManager);
        //Menu = ShopMenus.GetSpecificMenu(MenuID);
        AssociatedGang = gangs.GetGang(AssignedAssociationID);
        if (HasInterior)
        {
            GamblingDenInterior = interiors.PossibleInteriors.GamblingDenInteriors.Where(x => x.LocalID == InteriorID).FirstOrDefault();
            interior = GamblingDenInterior;
            if (GamblingDenInterior != null)
            {
                GamblingDenInterior.SetGamblingDen(this);
            }
        }
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Enter {Name}";
        return true;
    }
    public override void OnInteract()
    {
        if (IsLocationClosed())
        {
            return;
        }

        if(!IsAvailableForPlayer())
        {
            return;
        }
        


        if (!CanInteract)
        {
            return;
        }
        if (Interior != null && Interior.IsTeleportEntry)
        {
            DoEntranceCamera(false);
            Interior.Teleport(Player, this, StoreCamera);
        }
        else
        {
            StandardInteract(null, false);
        }
    }
    public bool IsAvailableForPlayer()
    {
        if (AssociatedGang != null)
        {
            GangReputation currentReputation = Player.RelationshipManager.GangRelationships.GetReputation(AssociatedGang);
            GangRespect gangRespect = GangRespect.Hostile;
            if (currentReputation != null)
            {
                gangRespect = currentReputation.GangRelationship;
            }
            if (IsRestrictedToMember && gangRespect != GangRespect.Member)
            {
                Game.DisplayHelp($"{Name} is only available to members");
                PlayErrorSound();
                return false;
            }
            else if (IsRestrictedToFriendly && gangRespect != GangRespect.Member && gangRespect != GangRespect.Friendly)
            {
                Game.DisplayHelp($"{Name} is only available to associates");
                PlayErrorSound();
                return false;
            }
            else if (gangRespect == GangRespect.Hostile)
            {
                Game.DisplayHelp($"{Name} is not available to hostile gang members");
                PlayErrorSound();
                return false;
            }
        }
        return true;
    }
    public override void StandardInteract(LocationCamera locationCamera, bool isInside)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                SetupLocationCamera(locationCamera, isInside, false);
                CreateInteractionMenu();
                Transaction = new Transaction(MenuPool, InteractionMenu, Menu, this);
                InteractionMenu.Visible = true;
                Interact(true, true, true);
                ProcessInteractionMenu();
                DisposeInteractionMenu();
                DisposeCamera(isInside);
                DisposeInterior();
                Player.IsTransacting = false;
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "GamblingDenInteract");
    }



    public void CreateInteriorGameMenu(bool allowLoans, bool allowBlackjack, bool allowRoulette)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        //Player.IsTransacting = true;
        //GameFiber.StartNew(delegate
        //{
            try
            {
                //SetupLocationCamera(null, true, false);
                CreateInteractionMenu();
                //Transaction = new Transaction(MenuPool, InteractionMenu, Menu, this);
                InteractionMenu.Visible = true;
                Interact(allowLoans, allowBlackjack, allowRoulette);
                ProcessInteractionMenu();
                DisposeInteractionMenu();
                //DisposeCamera(true);
                //DisposeInterior();
                //Player.IsTransacting = false;
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        //}, "GamblingDenInteract");
    }
    private void Interact(bool allowLoans, bool allowBlackjack, bool allowRoulette)
    {
        if (allowBlackjack || allowRoulette)
        {
            GameChoiceSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Play Game");
            InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Choose one of the games to play.";
        }
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            GameChoiceSubMenu.SetBannerType(BannerImage);
        }

        if (allowBlackjack && GamblingParameters.BlackJackGameRulesList != null)
        {
            foreach (BlackJackGameRules blackJackGameRules in GamblingParameters.BlackJackGameRulesList)
            {
                UIMenuItem playBlackjackMenuItem = new UIMenuItem(blackJackGameRules.GameName, $"Also know as 'twenty-one'. {blackJackGameRules.Display}");
                playBlackjackMenuItem.Activated += (sender, e) =>
                {
                    sender.Visible = false;
                    StartBlackjackGame(blackJackGameRules);
                    sender.Visible = true;
                };
                GameChoiceSubMenu.AddItem(playBlackjackMenuItem);
            }
        }
        if (allowRoulette && GamblingParameters.RouletteGameRulesList != null)
        {
            foreach (RouletteGameRules rouletteGameRules in GamblingParameters.RouletteGameRulesList)
            {
                UIMenuItem playrouletteMenuItem = new UIMenuItem(rouletteGameRules.GameName, $"Means 'Little Wheel' in french. Enjoy watching balls? This is the game for you. {rouletteGameRules.Display}");
                playrouletteMenuItem.Activated += (sender, e) =>
                {
                    sender.Visible = false;
                    StartRouletteGame(rouletteGameRules);
                    sender.Visible = true;
                };
                GameChoiceSubMenu.AddItem(playrouletteMenuItem);
            }
        }
        if(AssociatedGang == null || !allowLoans)
        {
            EntryPoint.WriteToConsole("No gang, not adding loan options");
            return;
        }
        LoanSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Cash Loans");
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Just front me some cash, I'll catch it up on the backend!";
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            LoanSubMenu.SetBannerType(BannerImage);
        }
        AssociatedGang.AddLoanItems(Player,LoanSubMenu,this, Time);
    }
    private void StartRouletteGame(RouletteGameRules rouletteGameRules)
    {
        if (Player.BankAccounts.GetMoney(false) < rouletteGameRules.MinBet)
        {
            DisplayMessage("Error", "You do not have enough cash on hand to play.");
            PlayErrorSound();
            return;
        }
        if (Player.CasinoGamePlayer.GamblingManager.IsWinBanned(this))
        {
            DisplayMessage("Error", "You have been temporarily banned for winning too much.");
            PlayErrorSound();
            return;
        }
        if (AssociatedGang != null)
        {
            GangReputation gr = Player.RelationshipManager.GangRelationships.GetReputation(AssociatedGang);
            GangRespect currentRespect = GangRespect.Neutral;
            if (gr != null)
            {
                currentRespect = gr.GangRelationship;
            }
            if (rouletteGameRules.IsRestrictedToMember && currentRespect != GangRespect.Member)
            {
                DisplayMessage("Error", "This game is restricted to members.");
                PlayErrorSound();
                return;
            }
            else if (rouletteGameRules.IsRestrictedToFriendly && currentRespect != GangRespect.Friendly && currentRespect != GangRespect.Member)
            {
                DisplayMessage("Error", "You do not have enough rep to play in this game.");
                PlayErrorSound();
                return;
            }
        }
        RouletteGame rouletteGame = new RouletteGame(Player.CasinoGamePlayer, Settings, this, rouletteGameRules);
        rouletteGame.Setup();
        rouletteGame.StartRound();
    }
    private void StartBlackjackGame(BlackJackGameRules blackJackGameRules)
    {
        if(Player.BankAccounts.GetMoney(false) < blackJackGameRules.MinBet)
        {
            DisplayMessage("Error","You do not have enough cash on hand to play.");
            PlayErrorSound();
            return;
        }
        if(Player.CasinoGamePlayer.GamblingManager.IsWinBanned(this))
        {
            DisplayMessage("Error", "You have been temporarily banned for winning too much.");
            PlayErrorSound();
            return;
        }
        if (AssociatedGang != null)
        {
            GangReputation gr = Player.RelationshipManager.GangRelationships.GetReputation(AssociatedGang);
            GangRespect currentRespect = GangRespect.Neutral;
            if (gr != null)
            {
                currentRespect = gr.GangRelationship;
            }
            if (blackJackGameRules.IsRestrictedToMember && currentRespect != GangRespect.Member)
            {
                DisplayMessage("Error", "This game is restricted to members.");
                PlayErrorSound();
                return;
            }
            else if (blackJackGameRules.IsRestrictedToFriendly && currentRespect != GangRespect.Friendly && currentRespect != GangRespect.Member)
            {
                DisplayMessage("Error", "You do not have enough rep to play in this game.");
                PlayErrorSound();
                return;
            }
        }
        BlackJackGame blackJackGameInternal = new BlackJackGame(Player.CasinoGamePlayer, Settings, false, this, blackJackGameRules);
        blackJackGameInternal.StartRound();
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.GamblingDens.Add(this);
        base.AddLocation(possibleLocations);
    }






    //ALL THIS CRAP NEEDS TO GO !!!!
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
        // StoreCamera?.StopImmediately(false);
        Player.ActivityManager.IsInteractingWithLocation = false;
        Player.IsTransacting = false;
        if (Interior != null)
        {
            Interior.IsMenuInteracting = false;
        }
    }
    private void CreateRestInteractionMenu()
    {
        RestMenuItem = new UIMenuNumericScrollerItem<int>("Rest", "Rest at your business to recover health. Select up to 12 hours.", 1, 12, 1) { Formatter = v => v.ToString() + " hours" };
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
        Player.IsResting = true;
        Player.IsSleeping = true;
        Player.ButtonPrompts.AddPrompt("BusinessRest", "Cancel Rest", "BusinessRest", Settings.SettingsManager.KeySettings.InteractCancel, 99);





        GameFiber FastForwardWatcher = GameFiber.StartNew(delegate
        {
            try
            {
                while (Time.IsFastForwarding)
                {
                    if (!Settings.SettingsManager.NeedsSettings.ApplyNeeds)
                    {
                        Player.HealthManager.ChangeHealth(1);
                    }
                    if (Player.ButtonPrompts.IsPressed("BusinessRest"))
                    {
                        Time.StopFastForwarding();
                    }
                    GameFiber.Yield();
                }
                Player.ButtonPrompts.RemovePrompts("BusinessRest");
                Player.IsResting = false;
                Player.IsSleeping = false;
                InteractionMenu.Visible = true;
                KeepInteractionGoing = false;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "FastForwardWatcher");
        //EntryPoint.WriteToConsole($"PLAYER EVENT: START REST ACTIVITY AT BUSINESS");
    }




}
