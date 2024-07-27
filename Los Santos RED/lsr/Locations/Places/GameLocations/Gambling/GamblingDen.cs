﻿using Blackjack;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
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
    public override bool ShowsOnDirectory => false;
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
        GangReputation currentReputation = Player.RelationshipManager.GangRelationships.GetReputation(AssociatedGang);
        GangRespect gangRespect = GangRespect.Hostile;
        if(currentReputation != null)
        {
            gangRespect = currentReputation.GangRelationship;
        }
        if(IsRestrictedToMember && gangRespect != GangRespect.Member)
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
        else if(gangRespect == GangRespect.Hostile)
        {
            Game.DisplayHelp($"{Name} is not available to hostile gang members");
            PlayErrorSound();
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
        if(GamblingParameters.BlackjackGamblingParameters.IsEnabled)
        {
            UIMenuItem playBlackjackMenuItem = new UIMenuItem("Play Blackjack", $"Also know as 'twenty-one'.~n~Limits: ~n~Min Bet: ${GamblingParameters.BlackjackGamblingParameters.MinBet}~n~Max Bet: ${GamblingParameters.BlackjackGamblingParameters.MaxBet} ~n~Surrender: {(GamblingParameters.BlackjackGamblingParameters.CanSurrender ? "Allowed" : "Unavailable")}");
            playBlackjackMenuItem.Activated += (sender, e) =>
            {
                sender.Visible = false;
                StartBlackjackGame();
                sender.Visible = true;
            };
            GameChoiceSubMenu.AddItem(playBlackjackMenuItem);
        }
        //if (GamblingParameters.BlackjackGamblingParameters.HasPoker)
        //{
        //    UIMenuItem playPokerMenuItem = new UIMenuItem("Play Seven-Card Stud Poker", "Also known as Seven-Toed Pete or Down-The-River. Texas What Em? We Play poker here.");
        //    playPokerMenuItem.Activated += (sender, e) =>
        //    {
        //        sender.Visible = false;
        //    };
        //    playPokerMenuItem.Enabled = false;
        //    GameChoiceSubMenu.AddItem(playPokerMenuItem);
        //}
        if (GamblingParameters.RouletteGamblingParameters.IsEnabled)
        {
            UIMenuItem playrouletteMenuItem = new UIMenuItem("Play Roulette", "Means 'Little Wheel' in french. Enjoy watching balls? This is the game for you.");
            playrouletteMenuItem.Activated += (sender, e) =>
            {
                sender.Visible = false;
            };
            playrouletteMenuItem.Enabled = false;
            GameChoiceSubMenu.AddItem(playrouletteMenuItem);
        }
        if(AssociatedGang == null)
        {
            EntryPoint.WriteToConsole("SETUP IS NULL");
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

    private void StartBlackjackGame()
    {
        if(Player.BankAccounts.GetMoney(false) < GamblingParameters.BlackjackGamblingParameters.MinBet)
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
        BlackJackGame blackJackGameInternal = new BlackJackGame(Player.CasinoGamePlayer, Settings, false, this, GamblingParameters);
        blackJackGameInternal.StartRound();
    }
}