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
        private HalfBetMenu HalfBetMenu;
        private SplitBetMenu SplitBetMenu;
        private CornerBetMenu CornerBetMenu;
        private StreetBetMenu StreetBetMenu;
        private DoubleStreetBetMenu DoubleStreetBetMenu;
        private List<RouletteBetMenu> RouletteBetMenus = new List<RouletteBetMenu>();
        //private RouletteRoundBet PreviousRouletteRoundBet;
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
            HalfBetMenu = new HalfBetMenu(player, settings, gameLocation, gamblingParameters, this);
            SplitBetMenu = new SplitBetMenu(player, settings, gameLocation, gamblingParameters, this);
            CornerBetMenu = new CornerBetMenu(player, settings, gameLocation, gamblingParameters, this);
            StreetBetMenu = new StreetBetMenu(player, settings, gameLocation, gamblingParameters, this);
            DoubleStreetBetMenu = new DoubleStreetBetMenu(player, settings, gameLocation, gamblingParameters, this);

            RouletteBetMenus.Add(SingleBetMenu);
            RouletteBetMenus.Add(RowBetMenu);
            RouletteBetMenus.Add(TopLineBetMenu);
            RouletteBetMenus.Add(ColumnBetMenu);
            RouletteBetMenus.Add(DozenBetMenu);
            RouletteBetMenus.Add(ColorBetMenu);
            RouletteBetMenus.Add(EvenOddBetMenu);
            RouletteBetMenus.Add(HalfBetMenu);
            RouletteBetMenus.Add(SplitBetMenu);
            RouletteBetMenus.Add(CornerBetMenu);
            RouletteBetMenus.Add(StreetBetMenu);
            RouletteBetMenus.Add(DoubleStreetBetMenu);
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
        public List<RoulettePocket> GetSplitBetAdjoiningPockets(RoulettePocket selectedPocket)
        {
            if(selectedPocket == null)
            {
                selectedPocket = new RoulettePocket(-1);
                selectedPocket.Setup();
            }
            List<AdjoiningLookup> SingleAdjoiningPockets = new List<AdjoiningLookup>()
            {
                new AdjoiningLookup(-1,new List<int>() { 0,1,2,3 }),
                new AdjoiningLookup(0,new List<int>() {-1,1,2 }),
                new AdjoiningLookup(1,new List<int>() { 0,2,4 }),
                new AdjoiningLookup(2,new List<int>() { -1,0,1,3,5 }),
                new AdjoiningLookup(3,new List<int>() { -1,2,6 }),
                new AdjoiningLookup(4,new List<int>() { 1,5,7 }),
                new AdjoiningLookup(5,new List<int>() { 2,4,6,8 }),
                new AdjoiningLookup(6,new List<int>() { 3,5,9 }),
                new AdjoiningLookup(7,new List<int>() { 4,8,10 }),
                new AdjoiningLookup(8,new List<int>() { 5,7,9,11 }),
                new AdjoiningLookup(9,new List<int>() {6,8,12  }),
                new AdjoiningLookup(10,new List<int>() { 7,11,13 }),
                new AdjoiningLookup(11,new List<int>() { 8,10,12,14 }),
                new AdjoiningLookup(12,new List<int>() { 9,11,15 }),
                new AdjoiningLookup(13,new List<int>() { 10,14,16 }),
                new AdjoiningLookup(14,new List<int>() { 11,13,15,17 }),
                new AdjoiningLookup(15,new List<int>() { 12,14,18 }),
                new AdjoiningLookup(16,new List<int>() { 13,17,19 }),
                new AdjoiningLookup(17,new List<int>() { 14,16,18,20 }),
                new AdjoiningLookup(18,new List<int>() { 15,17,21 }),
                new AdjoiningLookup(19,new List<int>() { 16,20,22 }),
                new AdjoiningLookup(20,new List<int>() { 17,19,21,23 }),
                new AdjoiningLookup(21,new List<int>() { 18,20,24 }),
                new AdjoiningLookup(22,new List<int>() { 19,23,25 }),
                new AdjoiningLookup(23,new List<int>() { 20,22,24,26 }),
                new AdjoiningLookup(24,new List<int>() { 21,23,27 }),
                new AdjoiningLookup(25,new List<int>() { 22,26,28 }),
                new AdjoiningLookup(26,new List<int>() { 23,25,27,29 }),
                new AdjoiningLookup(27,new List<int>() { 24,26,30 }),
                new AdjoiningLookup(28,new List<int>() { 25,29,31 }),
                new AdjoiningLookup(29,new List<int>() { 26,28,30,32 }),
                new AdjoiningLookup(30,new List<int>() { 27,29,33 }),
                new AdjoiningLookup(31,new List<int>() { 28,32,34 }),
                new AdjoiningLookup(32,new List<int>() { 29,31,33,35 }),
                new AdjoiningLookup(33,new List<int>() { 30,32,36 }),
                new AdjoiningLookup(34,new List<int>() { 31,35 }),
                new AdjoiningLookup(35,new List<int>() { 32,34,36 }),
                new AdjoiningLookup(36,new List<int>() { 33,35 }),
            };
            AdjoiningLookup al = SingleAdjoiningPockets.Where(x => x.MainPocketID == selectedPocket.PocketID).FirstOrDefault();
            return RouletteWheel.PocketsList.Where(x => al.AdjoiningPockets.Contains(x.PocketID)).ToList();
        }
        public void StartRound()
        {
            IsCancelled = false;
            IsCompleted = false;
            if (Player.BankAccounts.GetMoney(false) <= GamblingParameters.MinBet)
            {
                RouletteGameUI.DisplayGameEvent("Minimum Bet ", "You do not have the minimum required bet amount.", true);
                EndGame();
                return;
            }
            if (Player.GamblingManager.IsWinBanned(GameLocation))
            {
                RouletteGameUI.DisplayGameEvent("Banned ", "You have been temporarily banned for winning too much.", true);
                EndGame();
                return;
            }
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
            //PreviousRouletteRoundBet = RouletteRoundBet;
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
                    Player.GamblingManager.OnMoneyWon(GameLocation, winAmount);
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
            foreach(RouletteBetMenu rbm in RouletteBetMenus.OrderBy(x=> x.SortOrder))
            {
                rbm.AddMenuItems();
            }
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
            //UIMenuItem ReBet = new UIMenuItem("Re-Bet", "Select make the same bet as the last round.") { RightBadge = UIMenuItem.BadgeStyle.Alert };
            //ReBet.Activated += (menu, item) =>
            //{
            //    if(PreviousRouletteRoundBet == null)
            //    {
            //        return;
            //    }
            //    RouletteRoundBet = PreviousRouletteRoundBet;
            //    foreach (RouletteBetMenu rbm in RouletteBetMenus.OrderBy(x => x.SortOrder))
            //    {
            //        rbm.UpdateBetAmount();
            //    }
            //};
            //if (PreviousRouletteRoundBet != null)
            //{
            //    BetMenu.AddItem(ReBet);
            //}
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
        private class AdjoiningLookup
        {
            public AdjoiningLookup(int mainPocketID, List<int> adjoiningPockets)
            {
                MainPocketID = mainPocketID;
                AdjoiningPockets = adjoiningPockets;
            }
            public int MainPocketID { get; set; }
            public List<int> AdjoiningPockets { get; set; } = new List<int>();
        }
    }
}
