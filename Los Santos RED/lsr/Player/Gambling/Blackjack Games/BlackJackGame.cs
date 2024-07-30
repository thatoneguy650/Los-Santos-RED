using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;

using Blackjack;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using NAudio.Gui;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using static System.Windows.Forms.AxHost;

namespace Blackjack
{
    public class BlackJackGame
    {
        private ICasinoGamePlayable Player;
        private ISettingsProvideable Settings;
        private Casino Casino;
        private MenuPool MenuPool;
        private UIMenu BetMenu;
        private UIMenuNumericScrollerItem<int> betAmountScroller;
        private UIMenu ActionMenu;
        private UIMenuItem splitAction;

        private GamblingDen GameLocation;
        private BlackJackGameRules GamblingParameters;
        private BlackjackGameUI BlackjackGameUI;
        private int LastBet;
        private uint lastGameTime;
        private bool isCompleted;
        private string selectedAction;
        private int bet;

        private Deck deck = new Deck();
        public BlackJackGame(ICasinoGamePlayable player, ISettingsProvideable settings, bool enableAnimations, GamblingDen gameLocation, BlackJackGameRules gamblingParameters)
        {
            Player = player;
            Settings = settings;
            EnableAnimations = enableAnimations;
            GameLocation = gameLocation;
            GamblingParameters = gamblingParameters;
            BlackjackGameUI = new BlackjackGameUI(player, settings, this, GameLocation);
        }
        public bool EnableAnimations { get; private set; }
        public bool IsActive { get; private set; }
        public int DelayTime { get; set; } = 2000;
        public bool isPaused { get; private set; }
        public bool IsCancelled { get; private set; } //=> isCancelled;
        public bool ShowNoGameStats { get; private set; } //=> ShowNoStats;
        public bool ShowHandUI { get; private set; } //=> ShowHands;
        public CasinoPlayer CasinoPlayer { get; private set; }
        public Dealer Dealer { get; private set; }
        private enum RoundResult
        {
            PUSH,
            PLAYER_WIN,
            PLAYER_BUST,
            PLAYER_BLACKJACK,
            DEALER_WIN,
            SURRENDER,
            INVALID_BET,
            CANCELLED
        }
        public void StartRound()
        {
            Player.IsTransacting = true;
            IsActive = true;
            CasinoPlayer = new CasinoPlayer(Player.PlayerName, Player, GameLocation);
            Casino = new Casino(GamblingParameters.MinBet,GamblingParameters.MaxBet);
            Dealer = new Dealer(GamblingParameters.DealerName);
            MenuPool = new MenuPool();
            BlackjackGameUI.StartRound();
            DoRound();
        }
        private void DoRound()
        {
            BlackjackGameUI.RemoveNotifications();
            if (!TakeBet())
            {
                if (IsCancelled)
                {
                    BlackjackGameUI.OnCancelled();
                    EndRound();
                }
                else
                {
                    BlackjackGameUI.OnInvalidBet();
                    EndRound();
                }
                return;
            }
            BlackjackGameUI.CreateOrDisplayGameUI();
            InitializeHands();
            HandleHandActions();
            if (IsCancelled)
            {
                BlackjackGameUI.OnCancelled();
                EndRound();
                return;
            }
            if (!CheckHandValidConditions())
            {
                return;
            }
            Card revealedCard = Dealer.RevealCard();
            CasinoPlayer.HandsCompleted++;
            BlackjackGameUI.DisplayGameEvent(GamblingParameters.DealerName, $"Reveals {revealedCard.Description()}~n~Hand Value: {Dealer.GetHandValue()}", true);
            while (Dealer.GetHandValue() <= 16)
            {
                Card drawncard = deck.DrawCard();
                Dealer.RevealedCards.Add(drawncard);
                BlackjackGameUI.DisplayGameEvent($"{GamblingParameters.DealerName} ", $"Draws {drawncard.Description()}~n~Hand Value: {Dealer.GetHandValue()}", true);
            }
            EvaluateHand(CasinoPlayer.PrimaryHand);
            if (CasinoPlayer.SplitHands != null && CasinoPlayer.SplitHands.Any())
            {
                foreach (Hand splitHand in CasinoPlayer.SplitHands)
                {
                    EvaluateHand(splitHand);
                }
            }
            EndRound();
        }
        private void EvaluateHand(Hand hand)
        {
            if(hand.IsSurrendered)
            {
                CasinoPlayer.OnSurrendered(hand);
                BlackjackGameUI.OnHandSurrendered("Surrendered", hand.GetHandName(CasinoPlayer.Name));
            }
            else if (hand.IsBust)
            {
                BlackjackGameUI.OnHandBusted(hand.GetHandName(CasinoPlayer.Name));
            }
            else if (hand.GetHandValue() > Dealer.GetHandValue())
            {
                CasinoPlayer.Wins++;
                bool isBlackJack = hand.IsHandBlackjack();
                int winAmount = CasinoPlayer.WinBet(isBlackJack);
                BlackjackGameUI.OnPlayerWon(winAmount, hand.GetHandName(CasinoPlayer.Name), isBlackJack);
            }
            else if (Dealer.GetHandValue() > 21)
            {
                CasinoPlayer.Wins++;
                BlackjackGameUI.OnPlayerWon(CasinoPlayer.WinBet(false), hand.GetHandName(CasinoPlayer.Name), false);
            }
            else if (Dealer.GetHandValue() > hand.GetHandValue())
            {
                hand.HandBet = 0;
                BlackjackGameUI.OnDealerWins(hand.GetHandName(CasinoPlayer.Name));
            }
            else
            {
                CasinoPlayer.ReturnBet();
                BlackjackGameUI.OnPlayerDealerPush(hand.GetHandName(CasinoPlayer.Name));
            }
        }
        private bool CheckHandValidConditions()
        {
            bool hasNonBust = false;
            if(CasinoPlayer.PrimaryHand.IsBust)
            {
                BlackjackGameUI.OnHandBusted(CasinoPlayer.PrimaryHand.GetHandName(CasinoPlayer.Name));
            }
            else if (CasinoPlayer.PrimaryHand.Cards.Count == 0)
            {
                Player.BankAccounts.GiveMoney(CasinoPlayer.Bet / 2, false);
                BlackjackGameUI.OnHandSurrendered("~o~Player Surrenders $" + (CasinoPlayer.Bet / 2) + "~s~", CasinoPlayer.PrimaryHand.GetHandName(CasinoPlayer.Name));
            }
            else
            {
                hasNonBust = true;
            }
            if(CasinoPlayer.SplitHands != null && CasinoPlayer.SplitHands.Any())
            {
                foreach(Hand splitHand in CasinoPlayer.SplitHands)
                {
                    if(splitHand.IsBust)
                    {
                        BlackjackGameUI.OnHandBusted(splitHand.GetHandName(CasinoPlayer.Name));
                    }
                    else if (splitHand.Cards.Count == 0)
                    {
                        Player.BankAccounts.GiveMoney(CasinoPlayer.Bet / 2, false);
                        BlackjackGameUI.OnHandSurrendered("~o~Player Surrenders $" + (CasinoPlayer.Bet / 2) + "~s~", splitHand.GetHandName(CasinoPlayer.Name));
                    }
                    else
                    {
                        hasNonBust = true;
                    }
                }
            }
            if(!hasNonBust)
            {
                EndRound();
                return false;
            }
            return true;
        }
        private void EndRound()
        {
            if (Player.BankAccounts.GetMoney(false) <= Casino.MinimumBet)
            {
                BlackjackGameUI.OnCannotMeetMinimumBet();
                IsCancelled = true;
            }
            if(Player.GamblingManager.IsWinBanned(GameLocation))
            {
                BlackjackGameUI.DisplayGameEvent("Banned ", "You have been temporarily banned for winning too much.", true);
                IsCancelled = true;
            }
            if (IsCancelled)
            {
                IsActive = false;
                Player.IsTransacting = false;
                BlackjackGameUI.Disable();
                return;
            }
            CasinoPlayer.ClearBet();
            ShowHandUI = false;
            BlackjackGameUI.StartDelay();
            DoRound();
        }
        private void HandleHandActions()
        {
            Hand selectedHand = CasinoPlayer.PrimaryHand;
            TakeActions(selectedHand);
            if (CasinoPlayer.SplitHands == null || !CasinoPlayer.SplitHands.Any())
            {
                return;
            }
            while(!IsCancelled)
            {
                Hand selectedSplitHand = CasinoPlayer.SplitHands.FirstOrDefault(x => !x.HasTakenAction);
                if(selectedSplitHand == null)
                {
                    break;
                }
                else
                {
                    CasinoPlayer.SetActiveHand(selectedSplitHand);
                    BlackjackGameUI.DisplayGameEvent("Active Hand ", "Active hand has changed.", true);
                    if (selectedSplitHand.Cards.Count() == 1 && selectedSplitHand.Cards[0].Face == Face.Ace)//required ace hit
                    {
                        Card drawnCard = deck.DrawCard();
                        selectedSplitHand.Cards.Add(drawnCard);
                        CasinoPlayer.OnHit();
                        BlackjackGameUI.DisplayGameEvent("Player ", $"Ace Split Force Draws {drawnCard.Description()}~n~Hand Value: {selectedSplitHand.GetHandValue()}", true);
                        selectedSplitHand.HasTakenAction = true;
                    }
                    else
                    {
                        Card drawnCard = deck.DrawCard();
                        selectedSplitHand.Cards.Add(drawnCard);
                        CasinoPlayer.OnHit();
                        BlackjackGameUI.DisplayGameEvent("Player ", $"Draws {drawnCard.Description()}~n~Hand Value: {selectedSplitHand.GetHandValue()}", true);
                        TakeActions(selectedSplitHand);
                    }
                }
            }
        }
        private bool TakeBet()
        {
            if (Player.BankAccounts.GetMoney(false) < Casino.MinimumBet)
            {
                IsCancelled = true;
                BlackjackGameUI.OnCannotMeetMinimumBet();
                return false;
            }
            isCompleted = false;
            IsCancelled = false;
            bet = 0;
            if (BetMenu == null)
            {
                BetMenu = new UIMenu("Bet", "Enter Bet Amount");
                SetupMenu(BetMenu);
                MenuPool.Add(BetMenu);
                int maxValue = Casino.MaximumBet;
                if (Player.BankAccounts.GetMoney(false) < maxValue)
                {
                    maxValue = Player.BankAccounts.GetMoney(false);
                }
                betAmountScroller = new UIMenuNumericScrollerItem<int>("Bet Amount", "Enter Bet Amount for Hand", Casino.MinimumBet, maxValue, 1) { Formatter = v => "$" + v + "",Value = Casino.MinimumBet };
                betAmountScroller.Activated += (menu, item) =>
                {
                    int maxValue2 = Casino.MaximumBet;
                    if (Player.BankAccounts.GetMoney(false) < maxValue2)
                    {
                        maxValue2 = Player.BankAccounts.GetMoney(false);
                    }
                    if (int.TryParse(NativeHelper.GetKeyboardInput(betAmountScroller.Value.ToString()), out int eneteredAmount))
                    {
                        if(eneteredAmount <= maxValue2 && eneteredAmount > Casino.MinimumBet)
                        {
                            betAmountScroller.Value = eneteredAmount;
                        }
                    }
                };
                BetMenu.AddItem(betAmountScroller);
                UIMenuItem PlaceBetMenu = new UIMenuItem("Place Bet", "Place the current bet amount");
                PlaceBetMenu.Activated += (menu, item) =>
                {
                    bet = betAmountScroller.Value;
                    isCompleted = true;
                    menu.Visible = false;
                };
                BetMenu.AddItem(PlaceBetMenu);

                UIMenuItem CancelGame = new UIMenuItem("Cancel", "Cancel current Game");
                CancelGame.Activated += (menu, item) =>
                {
                    IsCancelled = true;
                    menu.Visible = false;
                };
                BetMenu.AddItem(CancelGame);
            }
            else
            {
                int maxValue = Casino.MaximumBet;
                if (Player.BankAccounts.GetMoney(false) < maxValue)
                {
                    maxValue = Player.BankAccounts.GetMoney(false);
                }
                betAmountScroller.Maximum = maxValue;

                if (LastBet >= betAmountScroller.Minimum && LastBet <= betAmountScroller.Maximum)
                {
                    betAmountScroller.Value = LastBet;
                }
                else
                {
                    betAmountScroller.Value = Casino.MinimumBet;
                }
            }
            BetMenu.Visible = true;
            ProcessMenuItems();
            if (isCompleted && !IsCancelled)
            {
                CasinoPlayer.AddBet(bet);
                LastBet = bet;
                return true;
            }
            return false;
        }
        private void SetupMenu(UIMenu uIMenu)
        {
            if (GameLocation != null && uIMenu != null && GameLocation.HasBannerImage)
            {
                uIMenu.SetBannerType(Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{GameLocation.BannerImagePath}"));
            }
        }
        private void InitializeHands()
        {
            deck.Initialize();
            CasinoPlayer.ClearAllHands();
            CasinoPlayer.PrimaryHand.Cards = deck.DealHand();
            CasinoPlayer.PrimaryHand.HandBet = CasinoPlayer.Bet;
            Dealer.HiddenCards = deck.DealHand();
            Dealer.RevealedCards = new List<Card>();
            // If hand contains two aces, make one Hard.
            if (CasinoPlayer.PrimaryHand.Cards[0].Face == Face.Ace && CasinoPlayer.PrimaryHand.Cards[1].Face == Face.Ace)
            {
                CasinoPlayer.PrimaryHand.Cards[1].Value = 1;
            }

            if (Dealer.HiddenCards[0].Face == Face.Ace && Dealer.HiddenCards[1].Face == Face.Ace)
            {
                Dealer.HiddenCards[1].Value = 1;
            }
            ShowHandUI = true;
            BlackjackGameUI.DisplayGameEvent(Player.PlayerName, $"Initial Hand: ~n~{CasinoPlayer.PrimaryHand.PrintCards()}", true);
            Card revealedCard = Dealer.RevealCard();
            BlackjackGameUI.DisplayGameEvent(GamblingParameters.DealerName, $"Initial Card: {revealedCard.Description()}", true);
        }
        private void TakeActions(Hand hand)
        {
            do
            {
                CasinoPlayer.SetActiveHand(hand);
                if (hand.IsBust)
                {
                    hand.HasTakenAction = true;
                    return;
                }
                EntryPoint.WriteToConsole($"SET ACTIVE HAND TO: {hand.PrintCards()} CanSplit:{hand.CanSplit()}");
                isCompleted = false;
                selectedAction = "";
                if (ActionMenu == null)
                {
                    ActionMenu = new UIMenu("Action", "Enter Action");
                    SetupMenu(ActionMenu);
                    MenuPool.Add(ActionMenu);
                    UIMenuItem hitAction = new UIMenuItem("Hit", "Take another card.");
                    hitAction.Activated += (menu, item) =>
                    {
                        isCompleted = true;
                        selectedAction = "HIT";
                        menu.Visible = false;
                    };
                    ActionMenu.AddItem(hitAction);
                    UIMenuItem standAction = new UIMenuItem("Stand", "Take no more cards; also known as \"stand pat\", \"sit\", \"stick\", or \"stay\".");
                    standAction.Activated += (menu, item) =>
                    {
                        isCompleted = true;
                        selectedAction = "STAND";
                        menu.Visible = false;
                    };
                    ActionMenu.AddItem(standAction);
                    UIMenuItem doubleAction = new UIMenuItem("Double Down", "Increase the initial bet by 100% and take exactly one more card.");
                    doubleAction.Activated += (menu, item) =>
                    {
                        isCompleted = true;
                        selectedAction = "DOUBLE";
                        menu.Visible = false;
                    };
                    ActionMenu.AddItem(doubleAction);
                    splitAction = new UIMenuItem("Split", "Create two hands from a starting hand where both cards are the same value. Each new hand gets a second card resulting in two starting hands.");
                    splitAction.Activated += (menu, item) =>
                    {
                        isCompleted = true;
                        selectedAction = "SPLIT";
                        menu.Visible = false;
                    };
                    splitAction.Enabled = false;
                    if (GamblingParameters.CanSplit && hand.CanSplit())//if the hand has any DOUBLES
                    {
                        splitAction.Enabled = true;//no logic for this yet
                    }
                    ActionMenu.AddItem(splitAction);     
                    UIMenuItem surrenderAction = new UIMenuItem("Surrender", "Forfeit half the bet and end the hand immediately.");
                    surrenderAction.Activated += (menu, item) =>
                    {
                        isCompleted = true;
                        selectedAction = "SURRENDER";
                        menu.Visible = false;
                    };
                    if (!GamblingParameters.CanSurrender)
                    {
                        surrenderAction.Enabled = false;
                    }
                    ActionMenu.AddItem(surrenderAction);
                    
                    UIMenuItem CancelGame = new UIMenuItem("Cancel", "Cancel current Game");
                    CancelGame.Activated += (menu, item) =>
                    {
                        isCompleted = false;
                        IsCancelled = true;
                        menu.Visible = false;
                    };
                    ActionMenu.AddItem(CancelGame);
                }
                else
                {
                    if (GamblingParameters.CanSplit && hand.CanSplit())//if the hand has any DOUBLES
                    {
                        splitAction.Enabled = true;
                    }
                    else
                    {
                        splitAction.Enabled = false;
                    }
                }
                ActionMenu.Visible = true;
                ProcessMenuItems();
                if(IsCancelled)
                {
                    hand.HasTakenAction = true;
                    return;
                }
                switch (selectedAction.ToUpper())
                {
                    case "HIT":
                        Card drawnCard = deck.DrawCard();
                        hand.Cards.Add(drawnCard);
                        CasinoPlayer.OnHit();
                        BlackjackGameUI.DisplayGameEvent("Player ", $"Draws {drawnCard.Description()}~n~Hand Value: {hand.GetHandValue()}", true);
                        break;
                    case "STAND":
                        BlackjackGameUI.DisplayGameEvent("Player ", $"Stand at {hand.GetHandValue()}", true);
                        break;
                    case "SURRENDER":
                        hand.OnSurrendered();
                        BlackjackGameUI.DisplayGameEvent("Player ", $"Surrendered Hand", true);
                        break;
                    case "SPLIT":
                        if (CasinoPlayer.OnSplitHands(hand))
                        {
                            BlackjackGameUI.DisplayGameEvent("Player ", $"Hand has been split", true);
                            Card drawnCard2 = deck.DrawCard();
                            hand.Cards.Add(drawnCard2);
                            hand.WasSplit = true;
                            BlackjackGameUI.DisplayGameEvent("Player ", $"Draws {drawnCard2.Description()}~n~Hand Value: {hand.GetHandValue()}", true);
                        }
                        else
                        {
                            BlackjackGameUI.DisplayGameEvent("Player ", $"Could not split hand", true, false, true, false);
                        }
                        break;
                    case "DOUBLE":
                        if (Player.BankAccounts.GetMoney(false) <= CasinoPlayer.Bet)
                        {
                            BlackjackGameUI.DisplayGameEvent("Player ", $"You do not have enough cash to cover the bet.", true, false, true, false);
                        }
                        else
                        {
                            CasinoPlayer.AddBet(CasinoPlayer.Bet);
                            hand.Cards.Add(deck.DrawCard());
                        }
                        break;
                    default:
                        //DisplayCustomMessage("Alert", "Invalid Move");
                        break;
                }

                if (hand.GetHandValue() > 21)
                {
                    foreach (Card card in hand.Cards)
                    {
                        if (card.Value == 11) // Only a soft ace can have a value of 11
                        {
                            card.Value = 1;
                            break;
                        }
                    }
                }
            } while (!selectedAction.ToUpper().Equals("STAND") && !selectedAction.ToUpper().Equals("DOUBLE") && !selectedAction.ToUpper().Equals("SURRENDER") && hand.GetHandValue() <= 21 && !IsCancelled);
            hand.HasTakenAction = true;
        }
        private void ProcessMenuItems()
        {
            while (!isCompleted && !IsCancelled && MenuPool.IsAnyMenuOpen())
            {
                MenuPool.ProcessMenus();
                GameFiber.Yield();
            }
        }
    }
}