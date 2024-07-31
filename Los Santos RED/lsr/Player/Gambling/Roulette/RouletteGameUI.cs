using Blackjack;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class RouletteGameUI
    {
        private ICasinoGamePlayable Player;
        private ISettingsProvideable Settings;
        private RouletteGame RouletteGame;
        private GamblingDen GameLocation;
        public BigMessageThread BigMessage { get; private set; }
        private bool IsUICreated;
        private bool isPaused;
        private uint lastGameTime;
        private uint notificationID;
        public int DelayTime { get; set; } = 2000;
        public bool ShowLosingBets { get; set; } = false;

        public RouletteGameUI(ICasinoGamePlayable player, ISettingsProvideable settings, RouletteGame rouletteGame, GamblingDen gameLocation)
        {
            Player = player;
            Settings = settings;
            RouletteGame = rouletteGame;
            GameLocation = gameLocation;
        }
        private void RemoveBigMessages()
        {
            BigMessage.Fiber?.Abort();
            if (BigMessage != null && BigMessage.MessageInstance != null)
            {
                BigMessage.MessageInstance.Dispose();
            }
        }
        public void Disable()
        {
            RemoveBigMessages();
            RemoveNotifications();
        }
        public void RemoveNotifications()
        {
            if (GameLocation == null)
            {
                Game.RemoveNotification(notificationID);
            }
            else
            {
                GameLocation.RemoveMessage();
            }
        }
        public void DisplayGameEvent(string header, string message, bool delay)
        {
            DisplayCustomMessage(header, message);
            if (delay)
            {
                StartDelay();
            }
        }
        public void SetupBigMessages()
        {
            BigMessage = new BigMessageThread(true);
        }
        public void ChooseSuccessSound()
        {
            if (GameLocation != null)
            {
                GameLocation.PlaySuccessSound();
            }
            else
            {
                NativeHelper.PlaySuccessSound();
            }
        }
        public void ChooseErrorSound()
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
        public void OnWheelSpun(string v)
        {
            DisplayCustomMessage("Alert", "No More Bets");
            StartDelay();
            BigMessage.MessageInstance.ShowColoredShard(v, "", HudColor.Black, HudColor.Blue, DelayTime);
            StartDelay();
        }
        public void StartDelay()
        {
            Player.ButtonPrompts.AttemptAddPrompt("casinoGame", "Skip", "casinoGameSkip", GameControl.Attack, 9999);
            uint GameTimeStarted = Game.GameTime;
            while (Game.GameTime - GameTimeStarted <= DelayTime)
            {
                GameFiber.Yield();
                if (Player.ButtonPrompts.IsPressed("casinoGameSkip"))
                {
                    Player.ButtonPrompts.RemovePrompts("casinoGame");
                    return;
                }
            }
            Player.ButtonPrompts.RemovePrompts("casinoGame");
        }

        public void OnWonBet(string message, int winAmount)
        {
            ChooseSuccessSound();
            BigMessage.MessageInstance.ShowColoredShard($"Won ${winAmount}", message, HudColor.Black, HudColor.Green, DelayTime);
            StartDelay();
        }

        public void OnLostBet(string message, int betAmount)
        {
            if (!ShowLosingBets)
            {
                return;
            }
            ChooseErrorSound();
            BigMessage.MessageInstance.ShowColoredShard($"Lost Bet Of ${betAmount}", message, HudColor.Black, HudColor.RedDark, DelayTime);
            StartDelay();
        }
    }
}
