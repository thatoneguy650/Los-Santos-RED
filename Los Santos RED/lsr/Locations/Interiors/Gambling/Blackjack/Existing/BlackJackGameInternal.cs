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

namespace Blackjack
{
    public class BlackJackGameInternal
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

        public BlackJackGameInternal(ICasinoGamePlayable player, ISettingsProvideable settings, bool enableAnimations, GameLocation gameLocation, GamblingParameters gamblingParameters)
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

            BigMessage = new BigMessageThread(true);
            DoRound();
        }
        private void DoRound()
        {
            RemoveNotifications();
            if (!TakeBet())
            {
                if (isCancelled)
                {
                    EndRound(RoundResult.CANCELLED);
                }
                else
                {
                    EndRound(RoundResult.INVALID_BET);
                }
                ChooseErrorSound();
                return;
            }
            CreateUI();
            InitializeHands();

            TakeActions();
            if (isCancelled)
            {
                EndRound(RoundResult.CANCELLED);
                return;
            }


            StartDelay();

            Card revealedCard = Dealer.RevealCard();

            CasinoPlayer.HandsCompleted++;
            if (CasinoPlayer.Hand.Count == 0)
            {
                EndRound(RoundResult.SURRENDER);
                ChooseErrorSound();
                return;
            }
            else if (CasinoPlayer.GetHandValue() > 21)
            {
                EndRound(RoundResult.PLAYER_BUST);
                ChooseErrorSound();
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


            if (CasinoPlayer.GetHandValue() > Dealer.GetHandValue())
            {
                CasinoPlayer.Wins++;
                if (Casino.IsHandBlackjack(CasinoPlayer.Hand))
                {
                    EndRound(RoundResult.PLAYER_BLACKJACK);
                }
                else
                {
                    EndRound(RoundResult.PLAYER_WIN);
                }
                ChooseSuccessSound();
            }
            else if (Dealer.GetHandValue() > 21)
            {
                CasinoPlayer.Wins++;
                EndRound(RoundResult.PLAYER_WIN);
                ChooseSuccessSound();
            }
            else if (Dealer.GetHandValue() > CasinoPlayer.GetHandValue())
            {
                EndRound(RoundResult.DEALER_WIN);
                ChooseErrorSound();
            }
            else
            {
                EndRound(RoundResult.PUSH);
                ChooseErrorSound();
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
                    //bet = betAmountScroller.Value;
                    //isCompleted = true;
                    //menu.Visible = false;


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
                            //ChooseSuccessSound();
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
            CasinoPlayer.Hand = deck.DealHand();


            Dealer.HiddenCards = deck.DealHand();
            Dealer.RevealedCards = new List<Card>();
            // If hand contains two aces, make one Hard.
            if (CasinoPlayer.Hand[0].Face == Face.Ace && CasinoPlayer.Hand[1].Face == Face.Ace)
            {
                CasinoPlayer.Hand[1].Value = 1;
            }

            if (Dealer.HiddenCards[0].Face == Face.Ace && Dealer.HiddenCards[1].Face == Face.Ace)
            {
                Dealer.HiddenCards[1].Value = 1;
            }
            ShowHands = true;
            DisplayCustomMessage(Player.PlayerName, $"Initial Hand: ~n~{CasinoPlayer.PrintCards()}");
            StartDelay();
            Card revealedCard = Dealer.RevealCard();
            DisplayCustomMessage(GamblingParameters.DealerName, $"Initial Card: {revealedCard.Description()}");
            StartDelay();

        }
        private void TakeActions()
        {
            do
            {
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
                    UIMenuItem splitAction = new UIMenuItem("Split", "Create two hands from a starting hand where both cards are the same value. Each new hand gets a second card resulting in two starting hands.");
                    splitAction.Activated += (menu, item) =>
                    {
                        isCompleted = true;
                        selectedAction = "SPLIT";
                        menu.Visible = false;
                    };
                    if(!GamblingParameters.BlackJackCanSplit)
                    {
                        splitAction.Enabled = false;
                    }
                    splitAction.Enabled = false;//no logic for this yet

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
                ActionMenu.Visible = true;
                ProcessMenuItems();
                if(isCancelled)
                {
                    return;
                }
                switch (selectedAction.ToUpper())
                {
                    case "HIT":
                        Card drawnCard = deck.DrawCard();
                        CasinoPlayer.Hand.Add(drawnCard);
                        DisplayCustomMessage("Player ", $"Draws {drawnCard.Description()}~n~Hand Value: {CasinoPlayer.GetHandValue()}");
                        StartDelay();
                        //GameFiber.Sleep(DelayTime);
                        break;
                    case "STAND":
                        DisplayCustomMessage("Player ", $"Stand at {CasinoPlayer.GetHandValue()}");
                        //GameFiber.Sleep(DelayTime);
                        StartDelay();
                        break;
                    case "SURRENDER":
                        CasinoPlayer.Hand.Clear();
                        break;
                    case "DOUBLE":
                        if (Player.BankAccounts.GetMoney(false) <= CasinoPlayer.Bet)
                        {
                            CasinoPlayer.AddBet(Player.BankAccounts.GetMoney(false));
                        }
                        else
                        {
                            CasinoPlayer.AddBet(CasinoPlayer.Bet);
                        }
                        CasinoPlayer.Hand.Add(deck.DrawCard());
                        break;
                    default:
                        //DisplayCustomMessage("Alert", "Invalid Move");
                        break;
                }

                if (CasinoPlayer.GetHandValue() > 21)
                {
                    foreach (Card card in CasinoPlayer.Hand)
                    {
                        if (card.Value == 11) // Only a soft ace can have a value of 11
                        {
                            card.Value = 1;
                            break;
                        }
                    }
                }
            } while (!selectedAction.ToUpper().Equals("STAND") && !selectedAction.ToUpper().Equals("DOUBLE") && !selectedAction.ToUpper().Equals("SURRENDER") && CasinoPlayer.GetHandValue() <= 21 && !isCancelled);
        }
        private void EndRound(RoundResult result)
        {

            EntryPoint.WriteToConsole($"END ROUND RAN {result}");
            switch (result)
            {
                case RoundResult.PUSH:
                    CasinoPlayer.ReturnBet();
                    //DisplayCustomMessage("Result", "Player and Dealer Push.");
                    BigMessage.MessageInstance.ShowColoredShard("Player and Dealer Push.", "", HudColor.Black, HudColor.Blue, DelayTime);
                    break;
                case RoundResult.PLAYER_WIN:
                    //DisplayCustomMessage("Result", "~g~Player Wins $" + CasinoPlayer.WinBet(false) + "~s~");
                    ChooseSuccessSound();
                    BigMessage.MessageInstance.ShowColoredShard("~g~Player Wins $" + CasinoPlayer.WinBet(false) + "~s~", "", HudColor.Black, HudColor.GreenDark, DelayTime);
                    break;
                case RoundResult.PLAYER_BUST:
                    CasinoPlayer.ClearBet();
                    ChooseErrorSound();
                    //DisplayCustomMessage("Result", "~r~Player Busts~s~");
                    BigMessage.MessageInstance.ShowColoredShard("~r~Player Busts~s~", "", HudColor.Black, HudColor.RedDark, DelayTime);
                    break;
                case RoundResult.PLAYER_BLACKJACK:
                    //DisplayCustomMessage("Result", "~g~Player Wins $" + CasinoPlayer.WinBet(true) + "~s~ with ~o~Blackjack.~s~");
                    ChooseSuccessSound();
                    BigMessage.MessageInstance.ShowColoredShard("~g~Player Wins $" + CasinoPlayer.WinBet(true) + "~s~ with ~o~Blackjack.~s~", "", HudColor.Black, HudColor.GreenDark, DelayTime);
                    break;
                case RoundResult.DEALER_WIN:
                    CasinoPlayer.ClearBet();
                    ChooseErrorSound();
                    //DisplayCustomMessage("Result", "~r~Dealer Wins.~s~");
                    BigMessage.MessageInstance.ShowColoredShard("~r~Dealer Wins.~s~", "", HudColor.Black, HudColor.RedDark, DelayTime);
                    break;
                case RoundResult.SURRENDER:
                    //DisplayCustomMessage("Result", "~o~Player Surrenders $" + (CasinoPlayer.Bet / 2) + "~s~");
                    BigMessage.MessageInstance.ShowColoredShard("~o~Player Surrenders $" + (CasinoPlayer.Bet / 2) + "~s~", "", HudColor.Black, HudColor.Orange, DelayTime);
                    ChooseErrorSound();
                    Player.BankAccounts.GiveMoney(CasinoPlayer.Bet / 2, false);
                    CasinoPlayer.ClearBet();
                    break;

                case RoundResult.CANCELLED:
                    DisplayCustomMessage("Result", "Cancelled.");
                    CasinoPlayer.ClearBet();
                    break;

                case RoundResult.INVALID_BET:
                    DisplayCustomMessage("Alert", "~y~Invalid Bet.~s~");
                    break;
            }
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
                return;
            }
            ShowHands = false;
            StartDelay();
            //GameFiber.Sleep(DelayTime * 2);
            DoRound();
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
            NativeHelper.DisplayTextOnScreen($"Current Bet: ${CasinoPlayer.Bet} Wins: ~g~{CasinoPlayer.Wins}~s~ Hands: {CasinoPlayer.HandsCompleted}", StartingPosition, Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayScale, Color.White, (GTAFont)Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayFont, (GTATextJustification)2, true);
            if (!ShowHands)
            {
                return;
            }
            NativeHelper.DisplayTextOnScreen(CasinoPlayer.WriteHand(), StartingPosition += Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplaySpacing, Settings.SettingsManager.LSRHUDSettings.TopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayScale, Color.White, (GTAFont)Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayFont, (GTATextJustification)2, true);
            NativeHelper.DisplayTextOnScreen(Dealer.WriteHand(), StartingPosition += Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplaySpacing, Settings.SettingsManager.LSRHUDSettings.TopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayScale, Color.White, (GTAFont)Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayFont, (GTATextJustification)2, true);
        }       
    }
}