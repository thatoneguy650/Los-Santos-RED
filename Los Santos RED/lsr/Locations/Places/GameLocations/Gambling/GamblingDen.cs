using Blackjack;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GamblingDen : GameLocation
{
    private UIMenu GameChoiceSubMenu;

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

        if(GamblingParameters.HasBlackjack)
        {
            UIMenuItem playBlackjackMenuItem = new UIMenuItem("Play Blackjack", $"Also know as 'twenty-one'.~n~Limits: ~n~Min Bet: ${GamblingParameters.BlackJackMinBet}~n~Max Bet: ${GamblingParameters.BlackJackMaxBet} ~n~Surrender: {(GamblingParameters.BlackJackCanSurrender ? "Allowed" : "Unavailable")}");
            playBlackjackMenuItem.Activated += (sender, e) =>
            {
                sender.Visible = false;
                StartBlackjackGame();
                sender.Visible = true;
            };
            GameChoiceSubMenu.AddItem(playBlackjackMenuItem);
        }
        if (GamblingParameters.HasPoker)
        {
            UIMenuItem playPokerMenuItem = new UIMenuItem("Play Seven-Card Stud Poker", "Also known as Seven-Toed Pete or Down-The-River. Texas What Em? We Play poker here.");
            playPokerMenuItem.Activated += (sender, e) =>
            {
                sender.Visible = false;
            };
            playPokerMenuItem.Enabled = false;
            GameChoiceSubMenu.AddItem(playPokerMenuItem);
        }
        if (GamblingParameters.HasPoker)
        {
            UIMenuItem playrouletteMenuItem = new UIMenuItem("Play Roulette", "Means 'Little Wheel' in french. Enjoy watching balls? This is the game for you.");
            playrouletteMenuItem.Activated += (sender, e) =>
            {
                sender.Visible = false;
            };
            playrouletteMenuItem.Enabled = false;
            GameChoiceSubMenu.AddItem(playrouletteMenuItem);
        }



        GameChoiceSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Cash Loans");
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Just front me some cash, I'll catch it up on the backend!";
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            GameChoiceSubMenu.SetBannerType(BannerImage);
        }

    }
    private void StartBlackjackGame()
    {
        if(Player.BankAccounts.GetMoney(false) < GamblingParameters.BlackJackMinBet)
        {
            DisplayMessage("Error","You do not have enough cash on hand to play.");
            PlayErrorSound();
            return;
        }
        BlackJackGame blackJackGameInternal = new BlackJackGame(Player.CasinoGamePlayer, Settings, false, this, GamblingParameters);
        blackJackGameInternal.StartRound();
    }
}
