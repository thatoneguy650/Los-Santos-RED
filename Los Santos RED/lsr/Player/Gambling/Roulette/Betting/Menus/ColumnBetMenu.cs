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
    public class ColumnBetMenu : RouletteBetMenu
    {
        protected UIMenuListScrollerItem<int> BetTypeScroller;
        public override string MainTitle => "Column Bets";
        public override string MainDescription => "Make bets on the column of the pocket. ~n~Winning Pockets: ~n~1st Column: 1,4,7,10,13,16,19,22,25,28,31,34 ~n~2nd Column: 2,5,8,11,14,17,20,23,26,29,32,35 ~n~3rd Column: 3,6,9,12,15,18,21,24,27,30,33,36 ~n~Pays ~y~2 to 1~s~.";

        public override int SortOrder => 20;
        public ColumnBetMenu(ICasinoGamePlayable player, ISettingsProvideable settings, GamblingDen gameLocation, RouletteGameRules gamblingParameters, RouletteGame rouletteGame) : base(player, settings, gameLocation, gamblingParameters, rouletteGame)
        {

        }
        protected override void RefreshActiveBets()
        {
            RemoveBetsMenu.Clear();
            RemoveBetsMenu.RefreshIndex();
            foreach (ColumnBet sb in RouletteGame.RouletteRoundBet.ColumnBets)
            {
                UIMenuItem removeBet = new UIMenuItem(sb.BetName, "Select to remove this bet") { RightLabel = $"${sb.Amount}" };
                removeBet.Activated += (menu, item) =>
                {
                    Player.BankAccounts.GiveMoney(sb.Amount, false);
                    Player.GamblingManager.OnMoneyWon(GameLocation, sb.Amount);
                    RouletteGame.RouletteRoundBet.ColumnBets.Remove(sb);
                    UpdateBetAmount();
                    removeBet.Enabled = false;
                };
                RemoveBetsMenu.AddItem(removeBet);
            }
        }
        protected override void CreateBetTypeScroller()
        {
            BetTypeScroller = new UIMenuListScrollerItem<int>("Column", "Selected column", new List<int> { 1, 2, 3 }) { Formatter = v => $"{v}{(v == 1 ? "st" : v == 2 ? "nd" : v == 3 ? "rd" : "")} Column" };
            MakeNewBetMenu.AddItem(BetTypeScroller);
        }
        protected override void MakeBetActivated(UIMenu menu)
        {
            if (RouletteGame.RouletteRoundBet.ColumnBets != null && RouletteGame.RouletteRoundBet.ColumnBets.Any(x => x.Level == BetTypeScroller.SelectedItem))
            {
                Game.DisplaySubtitle("You already have a bet for this item");
            }
            else
            {
                RouletteGame.RouletteRoundBet.ColumnBets.Add(new ColumnBet(BetTypeScroller.SelectedItem, BetAmountScroller.Value));
                Player.BankAccounts.GiveMoney(-1 * BetAmountScroller.Value, false);
                Player.GamblingManager.OnMoneyWon(GameLocation, -1 * BetAmountScroller.Value);
                menu.Visible = false;
                UpdateBetAmount();
                MainBetsMenu.Visible = true;
            }
        }
        public override void UpdateBetAmount()
        {
            int totalBets = RouletteGame.RouletteRoundBet.ColumnBets.Sum(x => x.Amount);
            SetBetLabel(totalBets);
        }
    }
}
