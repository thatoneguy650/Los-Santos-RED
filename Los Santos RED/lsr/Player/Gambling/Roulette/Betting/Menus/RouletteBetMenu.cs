using RAGENativeUI.Elements;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Helper;
using Rage;

namespace Roulette
{
    public class RouletteBetMenu
    {
        protected ICasinoGamePlayable Player;
        protected ISettingsProvideable Settings;
        protected GamblingDen GameLocation;
        protected RouletteGameRules GamblingParameters;

        protected UIMenuNumericScrollerItem<int> BetAmountScroller;
        protected UIMenu MainBetsMenu;
        protected UIMenuItem MainBetsMenuItem;
        protected UIMenu RemoveBetsMenu;
        protected RouletteGame RouletteGame;
        protected UIMenu MakeNewBetMenu;

        public virtual string MainTitle { get; set; } = "Generic Bet";
        public virtual string MainDescription { get; set; } = "Make Generic Bet";
        public virtual int SortOrder { get; set; } = 999;

        public RouletteBetMenu(ICasinoGamePlayable player, ISettingsProvideable settings, GamblingDen gameLocation, RouletteGameRules gamblingParameters, RouletteGame rouletteGame)
        {
            Player = player;
            Settings = settings;
            GameLocation = gameLocation;
            GamblingParameters = gamblingParameters;
            RouletteGame = rouletteGame;
        }
        public void AddMenuItems()
        {
            MainBetsMenu = RouletteGame.MenuPool.AddSubMenu(RouletteGame.BetMenu, MainTitle);
            MainBetsMenuItem = RouletteGame.BetMenu.MenuItems[RouletteGame.BetMenu.MenuItems.Count() - 1];
            MainBetsMenuItem.Description = MainDescription;
            MainBetsMenuItem.RightLabel = "";
            RouletteGame.SetupMenu(MainBetsMenu);
            MakeBetMenu();
            RemoveItemsMenu();
        }
        private void RemoveItemsMenu()
        {
            RemoveBetsMenu = RouletteGame.MenuPool.AddSubMenu(MainBetsMenu, "Current Bets");
            RouletteGame.SetupMenu(RemoveBetsMenu);
            RemoveBetsMenu.OnMenuOpen += (sender) =>
            {
                RefreshActiveBets();
            };
        }
        protected virtual void RefreshActiveBets()
        {

        }
        private void MakeBetMenu()
        {
            MakeNewBetMenu = RouletteGame.MenuPool.AddSubMenu(MainBetsMenu, "Make New Bet");
            UIMenuItem MakeSingleBetMenuItem = MainBetsMenu.MenuItems[MainBetsMenu.MenuItems.Count() - 1];
            RouletteGame.SetupMenu(MakeNewBetMenu);
            CreateBetTypeScroller();
            BetAmountScroller = new UIMenuNumericScrollerItem<int>("Bet Amount", "Enter Bet Amount", GamblingParameters.MinBet, GamblingParameters.MaxBet, 1){ Formatter = v => "$" + v + "", Value = GamblingParameters.MinBet };
            BetAmountScroller.Activated += (menu, item) =>
            {
                int newMaxValue = GamblingParameters.MaxBet;
                if (Player.BankAccounts.GetMoney(false) < newMaxValue)
                {
                    newMaxValue = Player.BankAccounts.GetMoney(false);
                }
                if (int.TryParse(NativeHelper.GetKeyboardInput(BetAmountScroller.Value.ToString()), out int enteredAmount))
                {
                    if (enteredAmount <= newMaxValue && enteredAmount > GamblingParameters.MinBet)
                    {
                        BetAmountScroller.Value = enteredAmount;
                    }
                }
            };
            MakeNewBetMenu.AddItem(BetAmountScroller);
            UIMenuItem MakeBet = new UIMenuItem("Make Bet", "Select to add the current bet.");
            MakeBet.Activated += (menu, item) =>
            {
                if (Player.BankAccounts.GetMoney(false) < BetAmountScroller.Value)
                {
                    Game.DisplaySubtitle("You do not have enough cash on hand to place this bet");
                }
                else
                {
                    MakeBetActivated(menu);
                }
            };
            MakeNewBetMenu.AddItem(MakeBet);
        }
        protected virtual void CreateBetTypeScroller()
        {

        }
        protected virtual void MakeBetActivated(UIMenu menu)
        {

        }
        public virtual void UpdateBetAmount()
        {

        }
        protected void SetBetLabel(int BetAmount)
        {
            if(BetAmount== 0)
            {
                MainBetsMenuItem.RightLabel = "";
                return;
            }
            MainBetsMenuItem.RightLabel = $"Bets: ${BetAmount}";
        }
    }
}
