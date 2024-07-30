using Blackjack;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class RouletteGame
    {
        private ICasinoGamePlayable Player;
        private ISettingsProvideable Settings;
        private GamblingDen GameLocation;
        private RouletteGameRules GamblingParameters;
        private RouletteGameUI RouletteGameUI;
        private SingleBetMenu SingleBetMenu;
        private ColorBetMenu ColorBetMenu;
        private EvenOddBetMenu EvenOddBetMenu;
        private ColumnBetMenu ColumnBetMenu;
        private DozenBetMenu DozenBetMenu;
        private RowBetMenu RowBetMenu;
        private TopLineBetMenu TopLineBetMenu;
        public bool IsCancelled { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsActive { get; private set; }

        public MenuPool MenuPool { get; private set; }
        public UIMenu BetMenu { get; private set; }
        public RouletteRoundBet RouletteRoundBet { get; private set; }
        public RouletteWheel RouletteWheel { get; private set; }
        public RouletteGame(ICasinoGamePlayable player, ISettingsProvideable settings, GamblingDen gameLocation, RouletteGameRules gamblingParameters)
        {
            Player = player;
            Settings = settings;
            GameLocation = gameLocation;
            GamblingParameters = gamblingParameters;
            RouletteWheel = new RouletteWheel(this);
            RouletteGameUI = new RouletteGameUI(player, settings, this, GameLocation);
            RouletteGameUI.ShowLosingBets = true;
            SingleBetMenu = new SingleBetMenu(player,settings,gameLocation,gamblingParameters, this);
            RowBetMenu = new RowBetMenu(player, settings, gameLocation, gamblingParameters, this);
            TopLineBetMenu = new TopLineBetMenu(player, settings, gameLocation, gamblingParameters, this);
            ColumnBetMenu = new ColumnBetMenu(player, settings, gameLocation, gamblingParameters, this);
            DozenBetMenu = new DozenBetMenu(player, settings, gameLocation, gamblingParameters, this);
            ColorBetMenu = new ColorBetMenu(player, settings, gameLocation, gamblingParameters, this);
            EvenOddBetMenu = new EvenOddBetMenu(player, settings, gameLocation, gamblingParameters, this);
        }
        public void Setup()
        {
            Player.IsTransacting = true;
            IsActive = true;
            MenuPool = new MenuPool();
            RouletteWheel.Setup();
            RouletteGameUI.SetupBigMessages();
        }
        private void EndGame()
        {
            IsActive = false;
            Player.IsTransacting = false;
            RouletteGameUI.Disable();
        }
        public void StartRound()
        {
            IsCancelled = false;
            IsCompleted = false;
            RouletteGameUI.DisplayGameEvent("Alert", "Place your bets.", true);
            if (!TakeBets())
            {
                RouletteGameUI.DisplayGameEvent("Alert","Game has been cancelled", true);
                EndGame();
                return;
            }
            RouletteWheel.Spin();
            RouletteGameUI.OnWheelSpun($"Selected: {RouletteWheel.SelectedPocket.FullDisplay}");
            CheckWins();
            StartRound();
        }
        private void CheckWins()
        {
            foreach(RouletteBet rb in RouletteRoundBet.RouletteBets)
            {
                if(rb.IsWinner(RouletteWheel.SelectedPocket))
                {
                    int winAmount = rb.WinAmount();
                    RouletteGameUI.OnWonBet(rb.BetName, winAmount);
                    Player.BankAccounts.GiveMoney(winAmount, false);
                }
                else
                {
                    RouletteGameUI.OnLostBet(rb.BetName,rb.Amount);
                }
            }
        }
        private bool TakeBets()
        {
            RouletteRoundBet = new RouletteRoundBet();
            CreateBetMenu();
            SingleBetMenu.AddMenuItems();
            ColorBetMenu.AddMenuItems();
            EvenOddBetMenu.AddMenuItems();
            ColumnBetMenu.AddMenuItems();
            DozenBetMenu.AddMenuItems();
            RowBetMenu.AddMenuItems();
            TopLineBetMenu.AddMenuItems();
            AddGeneralRoundItems();
            BetMenu.Visible = true;
            ProcessMenuItems();
            if (IsCompleted && !IsCancelled)
            {
                return true;
            }
            return false;
        }
        private void AddGeneralRoundItems()
        {
            UIMenuItem MakeBet = new UIMenuItem("Spin Wheel", "Select lock in bets and spin the wheel.") { RightBadge = UIMenuItem.BadgeStyle.Alert };
            MakeBet.Activated += (menu, item) =>
            {
                IsCompleted = true;
                menu.Visible = false;
            };
            BetMenu.AddItem(MakeBet);
            UIMenuItem CancelMenuItem = new UIMenuItem("Cancel", "Select cancel all bets and stop playing.") { RightBadge = UIMenuItem.BadgeStyle.Star };
            CancelMenuItem.Activated += (menu, item) =>
            {
                IsCancelled = true;
                menu.Visible = false;
            };
            BetMenu.AddItem(CancelMenuItem);

        }
        private void ProcessMenuItems()
        {
            while (!IsCompleted && !IsCancelled && MenuPool.IsAnyMenuOpen())
            {
                MenuPool.ProcessMenus();
                GameFiber.Yield();
            }
        }
        private void CreateBetMenu()
        {
            BetMenu = new UIMenu("Bets", "Enter Bets");
            SetupMenu(BetMenu);
            MenuPool.Add(BetMenu);
        }
        public void SetupMenu(UIMenu uIMenu)
        {
            if (GameLocation != null && uIMenu != null && GameLocation.HasBannerImage)
            {
                uIMenu.SetBannerType(Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{GameLocation.BannerImagePath}"));
            }
        }

    }
}
