/*  Blackjack Game Copyright (C) 2018

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see<https://www.gnu.org/licenses/>. */

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
        private uint notificationID;
        private Deck deck = new Deck();
        private CasinoPlayer CasinoPlayer;
        private Casino Casino;
        private Dealer Dealer;
        private MenuPool MenuPool;
        private BigMessageThread BigMessage;
        private bool isCompleted;
        private string selectedAction;
        private bool isCancelled;
        private int bet;
        private UIMenu BetMenu;
        private UIMenuNumericScrollerItem<int> betAmountScroller;
        private UIMenu ActionMenu;
        private ICasinoGamePlayable Player;
        private bool ShowNoStats;
        private ISettingsProvideable Settings;
        private bool ShowHands;
        private bool IsUICreated;
        private GameLocation GameLocation;
        private GamblingParameters GamblingParameters;
        private int LastBet;
        private bool isPaused;
        private uint lastGameTime;
        private UIMenuItem splitAction;

        public BlackJackGame(ICasinoGamePlayable player, ISettingsProvideable settings, bool enableAnimations, GameLocation gameLocation, GamblingParameters gamblingParameters)
        {
            Player = player;
            Settings = settings;
            EnableAnimations = enableAnimations;
            GameLocation = gameLocation;
            GamblingParameters = gamblingParameters;
        }
        public bool EnableAnimations { get; private set; }
        public bool IsActive { get; private set; }
        public int DelayTime { get; set; } = 2000;
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
            CasinoPlayer = new CasinoPlayer(Player.PlayerName, Player);
            Casino = new Casino(GamblingParameters.BlackJackMinBet,GamblingParameters.BlackJackMaxBet);
            Dealer = new Dealer(GamblingParameters.DealerName);
            MenuPool = new MenuPool();
            SetupTextures();
            BigMessage = new BigMessageThread(true);
            DoRound();
        }
        private void DoRound()
        {
            RemoveNotifications();
            if (!TakeBet())
            {
                ChooseErrorSound();
                if (isCancelled)
                {
                    DisplayCustomMessage("Result", "Cancelled.");
                    EndRound();
                }
                else
                {
                    DisplayCustomMessage("Alert", "~y~Invalid Bet.~s~");
                    EndRound();
                }
                return;
            }
            CreateUI();
            InitializeHands();
            HandleHandActions();
            if (isCancelled)
            {
                DisplayCustomMessage("Result", "Cancelled.");
                EndRound();
                return;
            }
            StartDelay();
            Card revealedCard = Dealer.RevealCard();
            CasinoPlayer.HandsCompleted++;
            if(!CheckHandValidConditions())
            {
                return;
            }
            DisplayCustomMessage(GamblingParameters.DealerName, $"Reveals {revealedCard.Description()}~n~Hand Value: {Dealer.GetHandValue()}");
            StartDelay();
            while (Dealer.GetHandValue() <= 16)
            {
                Card drawncard = deck.DrawCard();
                Dealer.RevealedCards.Add(drawncard);
                DisplayCustomMessage($"{GamblingParameters.DealerName} ", $"Draws {drawncard.Description()}~n~Hand Value: {Dealer.GetHandValue()}");
                StartDelay();
            }
            StartDelay();
            EvaluateHand(CasinoPlayer.PrimaryHand);
            StartDelay();
            if (CasinoPlayer.SplitHands != null && CasinoPlayer.SplitHands.Any())
            {
                foreach (Hand splitHand in CasinoPlayer.SplitHands)
                {
                    EvaluateHand(splitHand);
                    StartDelay();
                }
            }
            EndRound();
        }
        private void EvaluateHand(Hand hand)
        {
            if (hand.GetHandValue() > Dealer.GetHandValue())
            {
                CasinoPlayer.Wins++;
                if (hand.IsHandBlackjack())//    Casino.IsHandBlackjack(CasinoPlayer.PrimaryHand.Cards))
                {
                    BigMessage.MessageInstance.ShowColoredShard("~g~Player Wins $" + CasinoPlayer.WinBet(true) + "~s~ with ~o~Blackjack.~s~", "", HudColor.Black, HudColor.GreenDark, DelayTime);
                }
                else
                {
                    BigMessage.MessageInstance.ShowColoredShard("~g~Player Wins $" + CasinoPlayer.WinBet(false) + "~s~", "", HudColor.Black, HudColor.GreenDark, DelayTime);
                }
                ChooseSuccessSound();
            }
            else if (Dealer.GetHandValue() > 21)
            {
                CasinoPlayer.Wins++;
                BigMessage.MessageInstance.ShowColoredShard("~g~Player Wins $" + CasinoPlayer.WinBet(false) + "~s~", "", HudColor.Black, HudColor.GreenDark, DelayTime);
                ChooseSuccessSound();
            }
            else if (Dealer.GetHandValue() > hand.GetHandValue())
            {
                hand.HandBet = 0;
                BigMessage.MessageInstance.ShowColoredShard("~r~Dealer Wins.~s~", "", HudColor.Black, HudColor.RedDark, DelayTime);
                ChooseErrorSound();
            }
            else
            {
                CasinoPlayer.ReturnBet();
                BigMessage.MessageInstance.ShowColoredShard("Player and Dealer Push.", "", HudColor.Black, HudColor.Blue, DelayTime);
                ChooseErrorSound();
            }

        }
        private bool CheckHandValidConditions()
        {
            bool hasNonBust = false;
            if(CasinoPlayer.PrimaryHand.IsBust)
            {
                //CasinoPlayer.ClearBet();
                ChooseErrorSound();
                BigMessage.MessageInstance.ShowColoredShard("~r~Player Busts~s~", "", HudColor.Black, HudColor.RedDark, DelayTime);
                CasinoPlayer.PrimaryHand.Cards.Clear();
                StartDelay();
            }
            else if (CasinoPlayer.PrimaryHand.Cards.Count == 0)
            {
                BigMessage.MessageInstance.ShowColoredShard("~o~Player Surrenders $" + (CasinoPlayer.Bet / 2) + "~s~", "", HudColor.Black, HudColor.Orange, DelayTime);
                ChooseErrorSound();
                Player.BankAccounts.GiveMoney(CasinoPlayer.Bet / 2, false);
                //CasinoPlayer.ClearBet();
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
                        ChooseErrorSound();
                        BigMessage.MessageInstance.ShowColoredShard("~r~Player Busts~s~", "", HudColor.Black, HudColor.RedDark, DelayTime);
                        splitHand.Cards.Clear();
                        StartDelay();
                    }
                    else if (splitHand.Cards.Count == 0)
                    {
                        BigMessage.MessageInstance.ShowColoredShard("~o~Player Surrenders $" + (CasinoPlayer.Bet / 2) + "~s~", "", HudColor.Black, HudColor.Orange, DelayTime);
                        ChooseErrorSound();
                        Player.BankAccounts.GiveMoney(CasinoPlayer.Bet / 2, false);
                        //CasinoPlayer.ClearBet();
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
                StartDelay();
                //GameFiber.Sleep(DelayTime * 2);
                DisplayCustomMessage("Minimum Bet ", "You do not have the minimum required bet amount. You have completed " + (CasinoPlayer.HandsCompleted - 1) + " rounds.");
                isCancelled = true;
            }
            if (isCancelled)
            {
                IsActive = false;
                Player.IsTransacting = false;
                RemoveNotifications();
                BigMessage.Fiber?.Abort();
                if (BigMessage != null && BigMessage.MessageInstance != null)
                {
                    BigMessage.MessageInstance.Dispose();
                }
                ReleaseTextures();
                Game.RawFrameRender -= DrawSprites;
                return;
            }
            CasinoPlayer.ClearBet();
            ShowHands = false;
            StartDelay();
            DoRound();
        }
        private void HandleHandActions()
        {
            Hand selectedHand = CasinoPlayer.PrimaryHand;
            TakeActions(selectedHand);
            if(CasinoPlayer.SplitHands == null || !CasinoPlayer.SplitHands.Any())
            {
                return;
            }
            while(!isCancelled)
            {
                DisplayCustomMessage("Active Hand ", "Active hand has changed.");
                Hand selectedSplitHand = CasinoPlayer.SplitHands.FirstOrDefault(x => !x.HasTakenAction);
                if(selectedSplitHand == null)
                {
                    break;
                }
                else
                {
                    TakeActions(selectedSplitHand);
                }
            }
        }

        private void StartDelay()
        {
            uint GameTimeStarted = Game.GameTime;
            while(Game.GameTime - GameTimeStarted <= DelayTime)
            {
                GameFiber.Sleep(50);
                if(Player.IsMoveControlPressed)
                {
                    return;
                }
            }
            //GameFiber.Sleep(DelayTime);
        }
        private void RemoveNotifications()
        {
            //BigMessage.MessageInstance?.Dispose();

            if (GameLocation == null)
            {
                Game.RemoveNotification(notificationID);
            }
            else
            {
                GameLocation.RemoveMessage();
            }
        }
        private bool TakeBet()
        {
            if (Player.BankAccounts.GetMoney(false) < Casino.MinimumBet)
            {
                isCancelled = true;
                OnCannotMakeMinBet();
                return false;
            }
            isCompleted = false;
            isCancelled = false;
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
                    isCancelled = true;
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
            if (isCompleted && !isCancelled)
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
            ShowHands = true;
            DisplayCustomMessage(Player.PlayerName, $"Initial Hand: ~n~{CasinoPlayer.PrimaryHand.PrintCards()}");
            StartDelay();
            Card revealedCard = Dealer.RevealCard();
            DisplayCustomMessage(GamblingParameters.DealerName, $"Initial Card: {revealedCard.Description()}");
            StartDelay();
        }
        private void TakeActions(Hand hand)
        {
            do
            {
                CasinoPlayer.SetActiveHand(hand);

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
                    if (GamblingParameters.BlackJackCanSplit && hand.CanSplit())//if the hand has any DOUBLES
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
                    if (!GamblingParameters.BlackJackCanSurrender)
                    {
                        surrenderAction.Enabled = false;
                    }
                    ActionMenu.AddItem(surrenderAction);
                    
                    UIMenuItem CancelGame = new UIMenuItem("Cancel", "Cancel current Game");
                    CancelGame.Activated += (menu, item) =>
                    {
                        isCompleted = false;
                        isCancelled = true;
                        menu.Visible = false;
                    };
                    ActionMenu.AddItem(CancelGame);
                }
                else
                {
                    if (GamblingParameters.BlackJackCanSplit && hand.CanSplit())//if the hand has any DOUBLES
                    {
                        splitAction.Enabled = true;//no logic for this yet
                    }
                    else
                    {
                        splitAction.Enabled = false;
                    }

                }
                ActionMenu.Visible = true;
                ProcessMenuItems();
                if(isCancelled)
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
                        DisplayCustomMessage("Player ", $"Draws {drawnCard.Description()}~n~Hand Value: {hand.GetHandValue()}");
                        StartDelay();
                        //GameFiber.Sleep(DelayTime);
                        break;
                    case "STAND":
                        DisplayCustomMessage("Player ", $"Stand at {hand.GetHandValue()}");
                        //GameFiber.Sleep(DelayTime);
                        StartDelay();
                        break;
                    case "SURRENDER":
                        CasinoPlayer.OnSurrendered();
                        break;
                    case "SPLIT":
                        if (CasinoPlayer.OnSplitHands(hand))
                        {
                            Card drawnCard2 = deck.DrawCard();
                            hand.Cards.Add(drawnCard2);
                            hand.WasSplit = true;
                            DisplayCustomMessage("Player ", $"Draws {drawnCard2.Description()}~n~Hand Value: {hand.GetHandValue()}");
                            StartDelay();
                        }
                        else
                        {
                            DisplayCustomMessage("Player ", $"Could not split hand");
                            ChooseErrorSound();
                        }
                        break;
                    case "DOUBLE":
                        if (Player.BankAccounts.GetMoney(false) <= CasinoPlayer.Bet)
                        {
                            DisplayCustomMessage("Player ", $"You do not have enough cash to cover the bet.");
                            ChooseErrorSound();
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
            } while (!selectedAction.ToUpper().Equals("STAND") && !selectedAction.ToUpper().Equals("DOUBLE") && !selectedAction.ToUpper().Equals("SURRENDER") && hand.GetHandValue() <= 21 && !isCancelled);
            hand.HasTakenAction = true;
        }

        private void ReleaseTextures()
        {

        }
        private void ProcessMenuItems()
        {
            while (!isCompleted && !isCancelled && MenuPool.IsAnyMenuOpen())
            {
                MenuPool.ProcessMenus();
                GameFiber.Yield();
            }
        }
        private void OnCannotMakeMinBet()
        {
            DisplayCustomMessage("Alert", $"You do not have enough money to start a game");
        }
        private void DisplayCustomMessage(string header, string message)
        {
            if (GameLocation != null)
            {
                if (string.IsNullOrEmpty(header))
                {
                    header = "Message";
                }
                GameLocation.DisplayMessage(header, message);
            }
            else
            {
                Game.RemoveNotification(notificationID);
                if (string.IsNullOrEmpty(header))
                {
                    notificationID = Game.DisplayNotification($"{message}");
                }
                else
                {
                    notificationID = Game.DisplayNotification($"{header} {message}");
                }
            }
        }
        private void ChooseSuccessSound()
        {
            if(GameLocation != null)
            {
                GameLocation.PlaySuccessSound();
            }
            else
            {
                NativeHelper.PlaySuccessSound();
            }
        }
        private void ChooseErrorSound()
        {
            if (GameLocation != null)
            {
                GameLocation.PlayErrorSound();
            }
            else
            {
                NativeHelper.PlayErrorSound();
            }
        }
        private void CreateUI()
        {
            if(IsUICreated)
            {
                return;
            }
            GameFiber DoorWatcher = GameFiber.StartNew(delegate
            {
                IsUICreated = true;
                while (IsActive && !isCancelled)
                {
                    isPaused = lastGameTime == Game.GameTime;
                    lastGameTime = Game.GameTime;
                    DisplayGameStats();
                    GameFiber.Yield();
                }
                IsUICreated = false;
            }, "DoorWatcher");
        }
        private void DisplayGameStats()
        {
            if (ShowNoStats)
            {
                return;
            }
            float StartingPosition = Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayPositionX + Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayPositionXMediumOffset;// 0.2f;
            NativeHelper.DisplayTextOnScreen(CasinoPlayer.GetGameStatus(), StartingPosition, Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayScale, Color.White, (GTAFont)Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayFont, (GTATextJustification)2, true);
            if (!ShowHands)
            {
                return;
            }
            NativeHelper.DisplayTextOnScreen(Dealer.WriteHeader(), StartingPosition += Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplaySpacing, Settings.SettingsManager.LSRHUDSettings.TopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayScale, Color.White, (GTAFont)Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayFont, (GTATextJustification)2, true);
            NativeHelper.DisplayTextOnScreen(CasinoPlayer.PrimaryHand.WriteHeader(CasinoPlayer.Name), StartingPosition += Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplaySpacing, Settings.SettingsManager.LSRHUDSettings.TopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayScale, Color.White, (GTAFont)Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayFont, (GTATextJustification)2, true);
            if (CasinoPlayer.SplitHands == null)
            {
                return;
            }
            foreach (Hand splitHands in CasinoPlayer.SplitHands)
            {
                NativeHelper.DisplayTextOnScreen(splitHands.WriteHeader(CasinoPlayer.Name), StartingPosition += Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplaySpacing, Settings.SettingsManager.LSRHUDSettings.TopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayScale, Color.White, (GTAFont)Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayFont, (GTATextJustification)2, true);
            }
        }
        private void DrawSprites(object sender, GraphicsEventArgs args)
        {
            try
            {
                if (isPaused)
                {
                    return;
                }
                GetStuff(args);
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole($"UI: Draw ERROR {ex.Message} {ex.StackTrace} ", 0);
            }
        }
        private void GetStuff(GraphicsEventArgs args)
        {
            DisplayCardIcons(args);//, InitialPosX, InitialPosY, SecondPosY, );
        }
        private void DisplayCardIcons(GraphicsEventArgs args)//, float InitialPosX, float InitialPosY, float secondPosY, float Scale)
        {
            if (isPaused || !EntryPoint.ModController.IsRunning)
            {
                return;
            }
            if (!ShowHands)
            {
                return;
            }
            float ConsistencyScale = (float)Game.Resolution.Width / 2160f;
            float InitialPosX = Game.Resolution.Width * (Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconPositionX);//Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayPositionY;
            float InitialPosY = Game.Resolution.Height * (Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconPositionY);
            float SecondPosY = Game.Resolution.Height * (Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconPositionY + Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconSpacing);
            float Scale = ConsistencyScale * Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconScale;

            int DisplayedCards = 1;
            foreach (Card card in Dealer.HiddenCards)
            {
                DisplayIconSmall(args, Player.UnknownCardTexture, DisplayedCards, InitialPosX, InitialPosY, Scale);
                DisplayedCards++;
            }
            foreach (Card card in Dealer.RevealedCards)
            {
                DisplayIconSmall(args, GetTextureFromCard(card), DisplayedCards, InitialPosX, InitialPosY, Scale);
                DisplayedCards++;
            }
            DisplayedCards = 1;
            foreach (Card card in CasinoPlayer.PrimaryHand.Cards)
            {
                DisplayIconSmall(args, GetTextureFromCard(card), DisplayedCards, InitialPosX, SecondPosY, Scale);
                DisplayedCards++;
            }
            if(CasinoPlayer.SplitHands == null)
            {
                return;
            }
            int DisplayedSplits = 2;
            foreach (Hand splitHands in CasinoPlayer.SplitHands)
            {
                float SplitHandPos = Game.Resolution.Height * (Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconPositionY + (Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconSpacing * DisplayedSplits));
                DisplayedCards = 1;
                foreach (Card card in splitHands.Cards)
                {
                    DisplayIconSmall(args, GetTextureFromCard(card), DisplayedCards, InitialPosX, SplitHandPos, Scale);
                    DisplayedCards++;
                }
                DisplayedSplits++;
            }
        }
        private void DisplayIconSmall(GraphicsEventArgs args, Texture toShow, int Index, float XPosition, float YPosition, float Scale)
        {
            if (toShow == null || toShow.Size == null)
            {
                return;
            }
            float FinalPosX = XPosition - (Index * ((toShow.Size.Width - Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconSpacingPixelReduction) * Scale));//InitialPosX - (i * (toShow.Size.Width * Scale));
            float FinalPosY = YPosition;
            if (toShow == null || toShow.Size == null)
            {
                return;
            }
            RectangleF rectangleF = new RectangleF(FinalPosX, FinalPosY, toShow.Size.Width * Scale, toShow.Size.Height * Scale);
            args.Graphics.DrawTexture(toShow, rectangleF);
        }
        private Texture GetTextureFromCard(Card card)
        {
            Tuple<Card, Texture> returned = Player.CardIconList.Where(x => x.Item1.Face == card.Face && x.Item1.Suit == card.Suit).FirstOrDefault();
            if(returned == null)
            {
                EntryPoint.WriteToConsole($"{card.Face} {card.Suit} NOT FOUND");
                return null;
            }
            return returned.Item2;
        }
        private void SetupTextures()
        {
            Player.SetupSharedTextures();
            Game.RawFrameRender += DrawSprites;
        }



        //private void EndRound(RoundResult result)
        //{

        //    EntryPoint.WriteToConsole($"END ROUND RAN {result}");
        //    switch (result)
        //    {
        //        case RoundResult.PUSH:
        //            CasinoPlayer.ReturnBet();
        //            //DisplayCustomMessage("Result", "Player and Dealer Push.");
        //            BigMessage.MessageInstance.ShowColoredShard("Player and Dealer Push.", "", HudColor.Black, HudColor.Blue, DelayTime);
        //            break;
        //        case RoundResult.PLAYER_WIN:
        //            //DisplayCustomMessage("Result", "~g~Player Wins $" + CasinoPlayer.WinBet(false) + "~s~");
        //            ChooseSuccessSound();
        //            BigMessage.MessageInstance.ShowColoredShard("~g~Player Wins $" + CasinoPlayer.WinBet(false) + "~s~", "", HudColor.Black, HudColor.GreenDark, DelayTime);
        //            break;
        //        case RoundResult.PLAYER_BUST:
        //            //CasinoPlayer.ClearBet();
        //            ChooseErrorSound();
        //            //DisplayCustomMessage("Result", "~r~Player Busts~s~");
        //            BigMessage.MessageInstance.ShowColoredShard("~r~Player Busts~s~", "", HudColor.Black, HudColor.RedDark, DelayTime);
        //            break;
        //        case RoundResult.PLAYER_BLACKJACK:
        //            //DisplayCustomMessage("Result", "~g~Player Wins $" + CasinoPlayer.WinBet(true) + "~s~ with ~o~Blackjack.~s~");
        //            ChooseSuccessSound();
        //            BigMessage.MessageInstance.ShowColoredShard("~g~Player Wins $" + CasinoPlayer.WinBet(true) + "~s~ with ~o~Blackjack.~s~", "", HudColor.Black, HudColor.GreenDark, DelayTime);
        //            break;
        //        case RoundResult.DEALER_WIN:
        //            //CasinoPlayer.ClearBet();
        //            ChooseErrorSound();
        //            //DisplayCustomMessage("Result", "~r~Dealer Wins.~s~");
        //            BigMessage.MessageInstance.ShowColoredShard("~r~Dealer Wins.~s~", "", HudColor.Black, HudColor.RedDark, DelayTime);
        //            break;
        //        case RoundResult.SURRENDER:
        //            //DisplayCustomMessage("Result", "~o~Player Surrenders $" + (CasinoPlayer.Bet / 2) + "~s~");
        //            BigMessage.MessageInstance.ShowColoredShard("~o~Player Surrenders $" + (CasinoPlayer.Bet / 2) + "~s~", "", HudColor.Black, HudColor.Orange, DelayTime);
        //            ChooseErrorSound();
        //            Player.BankAccounts.GiveMoney(CasinoPlayer.Bet / 2, false);
        //            //CasinoPlayer.ClearBet();
        //            break;

        //        case RoundResult.CANCELLED:
        //            DisplayCustomMessage("Result", "Cancelled.");
        //            //CasinoPlayer.ClearBet();
        //            break;

        //        case RoundResult.INVALID_BET:
        //            DisplayCustomMessage("Alert", "~y~Invalid Bet.~s~");
        //            break;
        //    }
        //    if (Player.BankAccounts.GetMoney(false) <= Casino.MinimumBet)
        //    {
        //        StartDelay();
        //        //GameFiber.Sleep(DelayTime * 2);
        //        DisplayCustomMessage("Minimum Bet ", "You do not have the minimum required bet amount. You have completed " + (CasinoPlayer.HandsCompleted - 1) + " rounds.");
        //        isCancelled = true;
        //    }
        //    if (isCancelled)
        //    {
        //        IsActive = false;
        //        Player.IsTransacting = false;
        //        RemoveNotifications();
        //        BigMessage.Fiber?.Abort();
        //        if(BigMessage != null && BigMessage.MessageInstance != null)
        //        {
        //            BigMessage.MessageInstance.Dispose();
        //        }
        //        ReleaseTextures();
        //        Game.RawFrameRender -= DrawSprites;
        //        return;
        //    }
        //    ShowHands = false;
        //    StartDelay();
        //    DoRound();
        //}
        //private void TakeActions()
        //{
        //    do
        //    {
        //        isCompleted = false;
        //        selectedAction = "";
        //        if (ActionMenu == null)
        //        {
        //            ActionMenu = new UIMenu("Action", "Enter Action");
        //            SetupMenu(ActionMenu);
        //            MenuPool.Add(ActionMenu);
        //            UIMenuItem hitAction = new UIMenuItem("Hit", "Take another card.");
        //            hitAction.Activated += (menu, item) =>
        //            {
        //                isCompleted = true;
        //                selectedAction = "HIT";
        //                menu.Visible = false;
        //            };
        //            ActionMenu.AddItem(hitAction);
        //            UIMenuItem standAction = new UIMenuItem("Stand", "Take no more cards; also known as \"stand pat\", \"sit\", \"stick\", or \"stay\".");
        //            standAction.Activated += (menu, item) =>
        //            {
        //                isCompleted = true;
        //                selectedAction = "STAND";
        //                menu.Visible = false;
        //            };
        //            ActionMenu.AddItem(standAction);
        //            UIMenuItem doubleAction = new UIMenuItem("Double Down", "Increase the initial bet by 100% and take exactly one more card.");
        //            doubleAction.Activated += (menu, item) =>
        //            {
        //                isCompleted = true;
        //                selectedAction = "DOUBLE";
        //                menu.Visible = false;
        //            };
        //            ActionMenu.AddItem(doubleAction);
        //            splitAction = new UIMenuItem("Split", "Create two hands from a starting hand where both cards are the same value. Each new hand gets a second card resulting in two starting hands.");
        //            splitAction.Activated += (menu, item) =>
        //            {
        //                isCompleted = true;
        //                selectedAction = "SPLIT";
        //                menu.Visible = false;
        //            };
        //            splitAction.Enabled = false;
        //            if (GamblingParameters.BlackJackCanSplit && CasinoPlayer.CanSplit())//if the hand has any DOUBLES
        //            {
        //                splitAction.Enabled = true;//no logic for this yet
        //            }
        //            ActionMenu.AddItem(splitAction);
        //            UIMenuItem surrenderAction = new UIMenuItem("Surrender", "Forfeit half the bet and end the hand immediately.");
        //            surrenderAction.Activated += (menu, item) =>
        //            {
        //                isCompleted = true;
        //                selectedAction = "SURRENDER";
        //                menu.Visible = false;
        //            };
        //            if (!GamblingParameters.BlackJackCanSurrender)
        //            {
        //                surrenderAction.Enabled = false;
        //            }
        //            ActionMenu.AddItem(surrenderAction);

        //            UIMenuItem CancelGame = new UIMenuItem("Cancel", "Cancel current Game");
        //            CancelGame.Activated += (menu, item) =>
        //            {
        //                isCompleted = false;
        //                isCancelled = true;
        //                menu.Visible = false;
        //            };
        //            ActionMenu.AddItem(CancelGame);
        //        }
        //        else
        //        {
        //            if (GamblingParameters.BlackJackCanSplit && CasinoPlayer.CanSplit())//if the hand has any DOUBLES
        //            {
        //                splitAction.Enabled = true;//no logic for this yet
        //            }
        //            else
        //            {
        //                splitAction.Enabled = false;
        //            }

        //        }
        //        ActionMenu.Visible = true;
        //        ProcessMenuItems();
        //        if (isCancelled)
        //        {
        //            return;
        //        }
        //        switch (selectedAction.ToUpper())
        //        {
        //            case "HIT":
        //                Card drawnCard = deck.DrawCard();
        //                CasinoPlayer.PrimaryHand.Cards.Add(drawnCard);
        //                CasinoPlayer.OnHit();
        //                DisplayCustomMessage("Player ", $"Draws {drawnCard.Description()}~n~Hand Value: {CasinoPlayer.GetHandValue()}");
        //                StartDelay();
        //                //GameFiber.Sleep(DelayTime);
        //                break;
        //            case "STAND":
        //                DisplayCustomMessage("Player ", $"Stand at {CasinoPlayer.GetHandValue()}");
        //                //GameFiber.Sleep(DelayTime);
        //                StartDelay();
        //                break;
        //            case "SURRENDER":
        //                CasinoPlayer.OnSurrendered();
        //                break;
        //            case "SPLIT":
        //                if (CasinoPlayer.OnSplitHands())
        //                {
        //                    Card drawnCard2 = deck.DrawCard();
        //                    CasinoPlayer.PrimaryHand.Cards.Add(drawnCard2);
        //                    DisplayCustomMessage("Player ", $"Draws {drawnCard2.Description()}~n~Hand Value: {CasinoPlayer.GetHandValue()}");
        //                    StartDelay();
        //                }
        //                else
        //                {
        //                    DisplayCustomMessage("Player ", $"Could not split hand");
        //                    ChooseErrorSound();
        //                }
        //                break;
        //            case "DOUBLE":
        //                if (Player.BankAccounts.GetMoney(false) <= CasinoPlayer.Bet)
        //                {
        //                    CasinoPlayer.AddBet(Player.BankAccounts.GetMoney(false));
        //                }
        //                else
        //                {
        //                    CasinoPlayer.AddBet(CasinoPlayer.Bet);
        //                }
        //                CasinoPlayer.PrimaryHand.Cards.Add(deck.DrawCard());
        //                break;
        //            default:
        //                //DisplayCustomMessage("Alert", "Invalid Move");
        //                break;
        //        }

        //        if (CasinoPlayer.GetHandValue() > 21)
        //        {
        //            foreach (Card card in CasinoPlayer.PrimaryHand.Cards)
        //            {
        //                if (card.Value == 11) // Only a soft ace can have a value of 11
        //                {
        //                    card.Value = 1;
        //                    break;
        //                }
        //            }
        //        }
        //    } while (!selectedAction.ToUpper().Equals("STAND") && !selectedAction.ToUpper().Equals("DOUBLE") && !selectedAction.ToUpper().Equals("SURRENDER") && CasinoPlayer.GetHandValue() <= 21 && !isCancelled);
        //}


        //private void DisplayCardIcons(GraphicsEventArgs args, float InitialPosX, float InitialPosY, float secondPosY, float Scale)
        //{
        //    int DisplayedCards = 1;
        //    foreach (Card card in Dealer.HiddenCards)
        //    {
        //        Texture toShow = Player.UnknownCardTexture;
        //        if (toShow == null || toShow.Size == null)
        //        {
        //            continue;
        //        }
        //        float FinalPosX = InitialPosX - (DisplayedCards * ((toShow.Size.Width - Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconSpacingPixelReduction) * Scale));//InitialPosX - (i * (toShow.Size.Width * Scale));
        //        float FinalPosY = InitialPosY;
        //        if (toShow == null || toShow.Size == null)
        //        {
        //            continue;
        //        }
        //        RectangleF rectangleF = new RectangleF(FinalPosX, FinalPosY, toShow.Size.Width * Scale, toShow.Size.Height * Scale);
        //        args.Graphics.DrawTexture(toShow, rectangleF);
        //        DisplayedCards++;
        //    }
        //    foreach (Card card in Dealer.RevealedCards)
        //    {
        //        Texture toShow = GetTextureFromCard(card);
        //        if (toShow == null || toShow.Size == null)
        //        {
        //            continue;
        //        }
        //        float FinalPosX = InitialPosX - (DisplayedCards * ((toShow.Size.Width - Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconSpacingPixelReduction) * Scale));//InitialPosX - (i * (toShow.Size.Width * Scale));
        //        float FinalPosY = InitialPosY;
        //        if (toShow == null || toShow.Size == null)
        //        {
        //            continue;
        //        }
        //        RectangleF rectangleF = new RectangleF(FinalPosX, FinalPosY, toShow.Size.Width * Scale, toShow.Size.Height * Scale);
        //        args.Graphics.DrawTexture(toShow, rectangleF);
        //        DisplayedCards++;
        //    }
        //    DisplayedCards = 1;
        //    foreach (Card card in CasinoPlayer.Hand)
        //    {
        //        Texture toShow = GetTextureFromCard(card);
        //        if (toShow == null || toShow.Size == null)
        //        {
        //            continue;
        //        }
        //        float FinalPosX = InitialPosX - (DisplayedCards * ((toShow.Size.Width - Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconSpacingPixelReduction) * Scale));//InitialPosX - (i * (toShow.Size.Width * Scale));
        //        float FinalPosY = secondPosY;
        //        if (toShow == null || toShow.Size == null)
        //        {
        //            continue;
        //        }
        //        RectangleF rectangleF = new RectangleF(FinalPosX, FinalPosY, toShow.Size.Width * Scale, toShow.Size.Height * Scale);
        //        args.Graphics.DrawTexture(toShow, rectangleF);
        //        DisplayedCards++;
        //    }

        //    if (CasinoPlayer.SplitHands == null)
        //    {
        //        return;
        //    }
        //    foreach (List<Card> splitHands in CasinoPlayer.SplitHands)
        //    {
        //        DisplayedCards = 1;
        //        foreach (Card card in splitHands)
        //        {
        //            Texture toShow = GetTextureFromCard(card);
        //            if (toShow == null || toShow.Size == null)
        //            {
        //                continue;
        //            }
        //            float FinalPosX = InitialPosX - (DisplayedCards * ((toShow.Size.Width - Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconSpacingPixelReduction) * Scale));//InitialPosX - (i * (toShow.Size.Width * Scale));
        //            float FinalPosY = secondPosY;
        //            if (toShow == null || toShow.Size == null)
        //            {
        //                continue;
        //            }
        //            RectangleF rectangleF = new RectangleF(FinalPosX, FinalPosY, toShow.Size.Width * Scale, toShow.Size.Height * Scale);
        //            args.Graphics.DrawTexture(toShow, rectangleF);
        //            DisplayedCards++;
        //        }
        //    }

        //}
    }
}