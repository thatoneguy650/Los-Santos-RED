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
using System.Text;
using System.Threading;

using Blackjack;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
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
        private Casino Casino = new Casino();
        private Dealer Dealer = new Dealer();
        private MenuPool MenuPool;
        private bool isCompleted;
        private string selectedAction;
        private bool isCancelled;
        private int bet;
        private UIMenu BetMenu;
        private UIMenuNumericScrollerItem<int> betAmountScroller;
        private UIMenu ActionMenu;
        private ICasinoGamePlayable ModPlayer;
        private bool ShowNoStats;
        private ISettingsProvideable Settings;
        private bool ShowHands;

        public BlackJackGameInternal(ICasinoGamePlayable modPlayer, ISettingsProvideable settings)
        {
            ModPlayer = modPlayer;
            Settings = settings;
        }

        public bool IsActive { get; private set; }

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
        /// <summary>
        /// Initialize Deck, deal the player and dealer hands, and display them.
        /// </summary>
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
            Dealer.RevealCard();
            ShowHands = true;
            DisplayHands();
        }

        private void DoRound()
        {
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
                NativeHelper.PlayErrorSound();
                return;
            }

            InitializeHands();

            CreateUI();
            TakeActions();
            if(isCancelled)
            {
                EndRound(RoundResult.CANCELLED);
                NativeHelper.PlayErrorSound();
                return;
            }
            GameFiber.Sleep(1500);
            Dealer.RevealCard();
            DisplayHands();
            CasinoPlayer.HandsCompleted++;
            if (CasinoPlayer.Hand.Count == 0)
            {
                EndRound(RoundResult.SURRENDER);
                NativeHelper.PlayErrorSound();
                return;
            }
            else if (CasinoPlayer.GetHandValue() > 21)
            {
                EndRound(RoundResult.PLAYER_BUST);
                NativeHelper.PlayErrorSound();
                return;
            }

            while (Dealer.GetHandValue() <= 16)
            {

                Card drawncard = deck.DrawCard();
                Dealer.RevealedCards.Add(drawncard);
                Game.RemoveNotification(notificationID);
                notificationID = Game.DisplayNotification($"Dealer Draws: {drawncard.Description()}");

                NativeHelper.PlaySuccessSound();
                GameFiber.Sleep(1500);
                //DisplayHands();
            }
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
                NativeHelper.PlaySuccessSound();
            }
            else if (Dealer.GetHandValue() > 21)
            {
                CasinoPlayer.Wins++;
                EndRound(RoundResult.PLAYER_WIN);
                NativeHelper.PlaySuccessSound();
            }
            else if (Dealer.GetHandValue() > CasinoPlayer.GetHandValue())
            {
                EndRound(RoundResult.DEALER_WIN);
                NativeHelper.PlayErrorSound();
            }
            else
            {
                EndRound(RoundResult.PUSH);
                NativeHelper.PlayErrorSound();
            }
        }


        /// <summary>
        /// Handles everything for the round.
        /// </summary>
        public void StartRound()
        {
            ModPlayer.IsTransacting = true;
            IsActive = true;
            CasinoPlayer = new CasinoPlayer(ModPlayer);
            Casino = new Casino();
            Dealer = new Dealer();
            MenuPool = new MenuPool();
            DoRound();
        }

        private void CreateUI()
        {
            GameFiber DoorWatcher = GameFiber.StartNew(delegate
            {
                while (IsActive && !isCancelled)
                {
                    DisplayGameStats();
                    GameFiber.Yield();
                }
            }, "DoorWatcher");
        }

    

        /// <summary>
        /// Ask user for action and perform that action until they stand, double, or bust.
        /// </summary>
        private void TakeActions()
        {

            do
            {
                DisplayHands();
                isCompleted = false;
                selectedAction = "";
                if (ActionMenu == null)
                {
                    ActionMenu = new UIMenu("Action", "Enter Action");
                    MenuPool.Add(ActionMenu);
                    //List<string> actions = new List<string>() { "HIT", "STAND", "SURRENDER", "DOUBLE" };
                    //UIMenuListScrollerItem<string> actionScroller = new UIMenuListScrollerItem<string>("Action", "Enter action", actions);//  { Value = Casino.MinimumBet };
                    //actionScroller.Activated += (menu, item) =>
                    //{
                    //    action = actionScroller.SelectedItem;
                    //    isCompleted = true;
                    //    menu.Visible = false;
                    //};
                    //ActionMenu.AddItem(actionScroller);
                    UIMenuItem hitAction = new UIMenuItem("Hit", "Do the hit action");
                    hitAction.Activated += (menu, item) =>
                    {
                        isCompleted = true;
                        selectedAction = "HIT";
                        menu.Visible = false;
                        EntryPoint.WriteToConsole($"HIT ACTION RAN {selectedAction}");
                    };
                    ActionMenu.AddItem(hitAction);
                    UIMenuItem standAction = new UIMenuItem("Stand", "Do the stand action");
                    standAction.Activated += (menu, item) =>
                    {
                        isCompleted = true;
                        selectedAction = "STAND";
                        menu.Visible = false;
                        EntryPoint.WriteToConsole($"STAND ACTION RAN {selectedAction}");
                    };
                    ActionMenu.AddItem(standAction);
                    UIMenuItem surrenderAction = new UIMenuItem("Surrender", "Do the surrender action");
                    surrenderAction.Activated += (menu, item) =>
                    {
                        isCompleted = true;
                        selectedAction = "SURRENDER";
                        menu.Visible = false;
                        EntryPoint.WriteToConsole($"SURRENDER ACTION RAN {selectedAction}");
                    };
                    ActionMenu.AddItem(surrenderAction);
                    UIMenuItem doubleAction = new UIMenuItem("Double", "Do the double action");
                    doubleAction.Activated += (menu, item) =>
                    {
                        isCompleted = true;
                        selectedAction = "DOUBLE";
                        menu.Visible = false;
                        EntryPoint.WriteToConsole($"DOUBLE ACTION RAN {selectedAction}");
                    };
                    ActionMenu.AddItem(doubleAction);
                    UIMenuItem CancelGame = new UIMenuItem("Cancel", "Cancel current Game");
                    CancelGame.Activated += (menu, item) =>
                    {
                        isCompleted = false;
                        isCancelled = true;
                        menu.Visible = false;
                        EntryPoint.WriteToConsole($"CANCEL RAN {selectedAction}");
                    };
                    ActionMenu.AddItem(CancelGame);
                }
                ActionMenu.Visible = true;
                while (!isCompleted && !isCancelled && MenuPool.IsAnyMenuOpen())
                {
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }
                EntryPoint.WriteToConsole($"ACTION: {selectedAction}");

                if(isCancelled)
                {
                    return;
                }

                switch (selectedAction.ToUpper())
                {
                    case "HIT":
                        CasinoPlayer.Hand.Add(deck.DrawCard());
                        break;
                    case "STAND":
                        break;
                    case "SURRENDER":
                        CasinoPlayer.Hand.Clear();
                        break;
                    case "DOUBLE":
                        if (ModPlayer.BankAccounts.GetMoney(false) <= CasinoPlayer.Bet)
                        {
                            CasinoPlayer.AddBet(ModPlayer.BankAccounts.GetMoney(false));
                        }
                        else
                        {
                            CasinoPlayer.AddBet(CasinoPlayer.Bet);
                        }
                        CasinoPlayer.Hand.Add(deck.DrawCard());
                        break;
                    default:
                        Game.RemoveNotification(notificationID);
                        notificationID = Game.DisplayNotification("INVALID MOVE.");
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
            } while (!selectedAction.ToUpper().Equals("STAND") && !selectedAction.ToUpper().Equals("DOUBLE")
                && !selectedAction.ToUpper().Equals("SURRENDER") && CasinoPlayer.GetHandValue() <= 21 && !isCancelled);
        }

        private void DisplayHands()
        {
           // Game.DisplayNotification($"{player.WriteHand()} ~n~~n~{Dealer.WriteHand()}");
        }

        /// <summary>
        /// Take player's bet
        /// </summary>
        /// <returns>Was the bet valid</returns>
        private bool TakeBet()
        {
            if(ModPlayer.BankAccounts.GetMoney(false) < Casino.MinimumBet)
            {
                isCancelled = true;
                Game.RemoveNotification(notificationID);
                notificationID = Game.DisplayNotification($"You do not have enough money to start a game");
                return false;
            }

            isCompleted = false;
            isCancelled = false;
            bet = 0;
            if (BetMenu == null)
            {
                BetMenu = new UIMenu("Bet", "Enter Bet Amount");
                MenuPool.Add(BetMenu);

                int maxValue = Casino.MaximumBet;
                if(ModPlayer.BankAccounts.GetMoney(false) < maxValue)
                {
                    maxValue = ModPlayer.BankAccounts.GetMoney(false);
                }

                betAmountScroller = new UIMenuNumericScrollerItem<int>("Bet Amount", "Enter Bet Amount for Hand", Casino.MinimumBet, maxValue, 1) { Value = Casino.MinimumBet };
                betAmountScroller.Activated += (menu, item) =>
                {
                    bet = betAmountScroller.Value;
                    isCompleted = true;
                    menu.Visible = false;
                    EntryPoint.WriteToConsole("BET AMOUNT RAN");
                };
                BetMenu.AddItem(betAmountScroller);
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
                if (ModPlayer.BankAccounts.GetMoney(false) < maxValue)
                {
                    maxValue = ModPlayer.BankAccounts.GetMoney(false);
                }
                betAmountScroller.Maximum = maxValue;
                betAmountScroller.Value = Casino.MinimumBet;
            }
            BetMenu.Visible = true;
            while(!isCompleted && !isCancelled && MenuPool.IsAnyMenuOpen())
            {
                MenuPool.ProcessMenus();
                GameFiber.Yield();
            }
            if(isCompleted && !isCancelled)
            {
                EntryPoint.WriteToConsole("ADD BET RAN");
                CasinoPlayer.AddBet(bet);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Perform action based on result of round and start next round.
        /// </summary>
        /// <param name="result">The result of the round</param>
        private void EndRound(RoundResult result)
        {
            switch (result)
            {
                case RoundResult.PUSH:
                    CasinoPlayer.ReturnBet();
                    Game.RemoveNotification(notificationID);
                    notificationID = Game.DisplayNotification("Player and Dealer Push.");
                    break;
                case RoundResult.PLAYER_WIN:
                    Game.RemoveNotification(notificationID);
                    notificationID = Game.DisplayNotification("Player Wins $" + CasinoPlayer.WinBet(false));
                    break;
                case RoundResult.PLAYER_BUST:
                    CasinoPlayer.ClearBet();
                    Game.RemoveNotification(notificationID);
                    notificationID = Game.DisplayNotification("Player Busts");
                    break;
                case RoundResult.PLAYER_BLACKJACK:
                    Game.RemoveNotification(notificationID);
                    notificationID = Game.DisplayNotification("Player Wins $" + CasinoPlayer.WinBet(true) + " with Blackjack.");
                    break;
                case RoundResult.DEALER_WIN:
                    CasinoPlayer.ClearBet();
                    Game.RemoveNotification(notificationID);
                    notificationID = Game.DisplayNotification("Dealer Wins.");
                    break;
                case RoundResult.SURRENDER:
                    Game.RemoveNotification(notificationID);
                    notificationID = Game.DisplayNotification("Player Surrenders $" + (CasinoPlayer.Bet / 2) + "");
                    ModPlayer.BankAccounts.GiveMoney(CasinoPlayer.Bet / 2, false);
                    CasinoPlayer.ClearBet();
                    break;

                case RoundResult.CANCELLED:
                    Game.RemoveNotification(notificationID);
                    notificationID = Game.DisplayNotification("Player Cancelled");
                    CasinoPlayer.ClearBet();
                    break;

                case RoundResult.INVALID_BET:
                    Game.RemoveNotification(notificationID);
                    notificationID = Game.DisplayNotification("Invalid Bet.");
                    break;
            }

            if (ModPlayer.BankAccounts.GetMoney(false) <= Casino.MinimumBet)
            {
                GameFiber.Sleep(2500);
                Game.RemoveNotification(notificationID);
                notificationID = Game.DisplayNotification("You do not have the minimum required bet amount. You have completed " + (CasinoPlayer.HandsCompleted - 1) + " rounds.");
                isCancelled = true;
            }
            if(isCancelled)
            {
                IsActive = false;
                ModPlayer.IsTransacting = false;
                return;
            }
            ShowHands = false;
            GameFiber.Sleep(2000);
            DoRound();
        }



        private void DisplayGameStats()
        {
            if (ShowNoStats)
            {
                return;
            }

            float StartingPosition = Settings.SettingsManager.LSRHUDSettings.TopDisplayPositionX + 0.2f;
            DrawMainStats(StartingPosition += Settings.SettingsManager.LSRHUDSettings.TopDisplaySpacing);

            if(!ShowHands)
            {
                return;
            }

            DrawPlayerHand(StartingPosition += Settings.SettingsManager.LSRHUDSettings.TopDisplaySpacing);
            DrawDealerHand(StartingPosition += Settings.SettingsManager.LSRHUDSettings.TopDisplaySpacing);
        }

        private void DrawDealerHand(float pos)
        {
            DisplayTextOnScreen(Dealer.WriteHand(), pos, Settings.SettingsManager.LSRHUDSettings.TopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.TopDisplayScale, Color.White, GTAFont.FontPricedown, (GTATextJustification)2, false);
        }

        private void DrawPlayerHand(float pos)
        {
            DisplayTextOnScreen(CasinoPlayer.WriteHand(), pos, Settings.SettingsManager.LSRHUDSettings.TopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.TopDisplayScale, Color.White, GTAFont.FontPricedown, (GTATextJustification)2, false);
        }

        private void DrawMainStats(float pos)
        {
            string MainStats = $"Bet: {CasinoPlayer.Bet} Wins: {CasinoPlayer.Wins} Hands: {CasinoPlayer.HandsCompleted}";
            DisplayTextOnScreen(MainStats, pos, Settings.SettingsManager.LSRHUDSettings.TopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.TopDisplayScale, Color.White, GTAFont.FontPricedown, (GTATextJustification)2, false);
        }

        private void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification, bool outline)
        {
            DisplayTextOnScreen(TextToShow, X, Y, Scale, TextColor, Font, Justification, outline, 255);
        }
        private void DisplayTextOnScreen(string TextToShow, float X, float Y, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification, bool outline, int alpha)
        {
            try
            {
                if (TextToShow == "" || alpha == 0 || TextToShow is null)
                {
                    return;
                }
                NativeFunction.Natives.SET_TEXT_FONT((int)Font);
                NativeFunction.Natives.SET_TEXT_SCALE(Scale, Scale);
                NativeFunction.Natives.SET_TEXT_COLOUR((int)TextColor.R, (int)TextColor.G, (int)TextColor.B, alpha);

                NativeFunction.Natives.SetTextJustification((int)Justification);

                NativeFunction.Natives.SET_TEXT_DROP_SHADOW();

                if (outline)
                {
                    NativeFunction.Natives.SET_TEXT_OUTLINE(true);


                    NativeFunction.Natives.SET_TEXT_EDGE(1, 0, 0, 0, 255);
                }
                NativeFunction.Natives.SET_TEXT_DROP_SHADOW();
                //NativeFunction.Natives.SetTextDropshadow(20, 255, 255, 255, 255);//NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);
                //NativeFunction.Natives.SetTextJustification((int)GTATextJustification.Center);
                if (Justification == GTATextJustification.Right)
                {
                    NativeFunction.Natives.SET_TEXT_WRAP(0f, Y);
                }
                else
                {
                    NativeFunction.Natives.SET_TEXT_WRAP(0f, 1f);
                }
                NativeFunction.Natives.x25fbb336df1804cb("STRING"); //NativeFunction.Natives.x25fbb336df1804cb("STRING");
                                                                    //NativeFunction.Natives.x25FBB336DF1804CB(TextToShow);
                NativeFunction.Natives.x6C188BE134E074AA(TextToShow);
                NativeFunction.Natives.xCD015E5BB0D96A57(Y, X);
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole($"UI ERROR {ex.Message} {ex.StackTrace}", 0);
            }
            //return;
        }
    }
}