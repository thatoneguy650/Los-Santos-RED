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

public class GamblingDen : GameLocation
{
    private UIMenu GameChoiceSubMenu;
    private UIMenu LoanSubMenu;
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
    [XmlIgnore]
    public Gang AssociatedGang { get; set; }
    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable names, ICrimes crimes, IPedGroups PedGroups, IEntityProvideable world,
IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations, IContacts contacts, IInteriors interiors,
ILocationInteractable player, IModItems modItems, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople)
    {
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, names, crimes, PedGroups, world, streets, locationTypes, settings, plateTypes, associations, contacts, interiors, player, modItems, weapons, time, placesOfInterest, issuableWeapons, heads, dispatchablePeople);
        Menu = ShopMenus.GetSpecificMenu(MenuID);
        AssociatedGang = gangs.GetGang(AssignedAssociationID);
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


        if(AssociatedGang != null)
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
                return;
            }
            else if (IsRestrictedToFriendly && gangRespect != GangRespect.Member && gangRespect != GangRespect.Friendly)
            {
                Game.DisplayHelp($"{Name} is only available to associates");
                PlayErrorSound();
                return;
            }
            else if (gangRespect == GangRespect.Hostile)
            {
                Game.DisplayHelp($"{Name} is not available to hostile gang members");
                PlayErrorSound();
                return;
            }
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
                Interact();
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
    private void Interact()
    {
        GameChoiceSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Play Game");
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Choose one of the games to play.";
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            GameChoiceSubMenu.SetBannerType(BannerImage);
        }

        if (GamblingParameters.BlackJackGameRulesList != null)
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
        if (GamblingParameters.RouletteGameRulesList != null)
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
        if(AssociatedGang == null)
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
}
